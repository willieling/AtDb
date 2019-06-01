using AtDb.Reader.Container;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace AtDb.Reader
{
    public class DatabaseExporter
    {
        private readonly AttributesParser attributesParser = new AttributesParser();
        private readonly ClassMaker classMaker = new ClassMaker();

        private bool isExporting;
        private Func<object, string> serializationFunction;
        private Func<string, string> compressionFunction;

        public string DatabaseSourcePath { get; set; }
        public string DatabaseExportPath { get; set; }
        public string GeneratedEnumsPath { get; set; }

        public void Initialize(DatabaseExporterConfiguration configuration)
        {
            serializationFunction = configuration.SerializationFunction;
            compressionFunction = configuration.CompressionFunction;
        }

        public void Export()
        {
            if(isExporting)
            {
                return;
            }

            isExporting = true;

            string[] excelFiles = GetExcelFiles(DatabaseSourcePath);
            IEnumerable<TableDataContainer> tableDataContainers = GetTableDataContainers(excelFiles);
            IEnumerable<ModelDataContainer> modelContainers = FillModelsWithData(tableDataContainers);
            WriteDataToFiles(modelContainers);
        }

        private string[] GetExcelFiles(string folderPath)
        {
            const string ONLY_EXCEL = "*.xlsx";
            string[] files = Directory.GetFiles(folderPath, ONLY_EXCEL);
            return files;
        }

        private IEnumerable<TableDataContainer> GetTableDataContainers(string[] files)
        {
            List<TableDataContainer> containers = new List<TableDataContainer>();
            foreach (string file in files)
            {
                IEnumerable<TableDataContainer> fileContainers = GetTableDataContainersFromExcelFile(file);
                containers.AddRange(fileContainers);
            }

            return containers;
        }

        private IEnumerable<TableDataContainer> GetTableDataContainersFromExcelFile(string file)
        {
            XSSFWorkbook workbook = GetWorkbook(file);
            IEnumerable<TableDataContainer> containers = GetTableDatacontainersFromWorkbook(workbook);
            return containers;
        }

        private XSSFWorkbook GetWorkbook(string file)
        {
            XSSFWorkbook workbook;
            using (FileStream filestream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(filestream);
            }

            return workbook;
        }

        private IEnumerable<TableDataContainer> GetTableDatacontainersFromWorkbook(XSSFWorkbook workbook)
        {
            List<TableDataContainer> containers = new List<TableDataContainer>();

            for (int i = 0; i < workbook.Count; ++i)
            {
                ISheet sheet = workbook.GetSheetAt(i);
                IEnumerable<TableDataContainer> sheetcontainers = GetTableDatacontainersFromSheet(sheet);
                containers.AddRange(sheetcontainers);
            }

            return containers;
        }

        private IEnumerable<TableDataContainer> GetTableDatacontainersFromSheet(ISheet sheet)
        {
            List<TableDataContainer> containers = new List<TableDataContainer>();
            for (int rowIndex = 0; rowIndex < sheet.LastRowNum; ++rowIndex)
            {
                IRow row = sheet.GetRow(rowIndex);
                if (row == null)
                {
                    continue;
                }

                TableMetadata metadata;
                if (TableUtilities.IsTableStart(row, out metadata))
                {
                    TableDataContainer container = CreateTableDataContainer(metadata, sheet, ref rowIndex);
                    containers.Add(container);
                }
            }

            return containers;
        }

        private TableDataContainer CreateTableDataContainer(TableMetadata metadata, ISheet sheet, ref int rowIndex)
        {
            const int NO_INDEX = -1;

            ++rowIndex;
            IRow nameRow = sheet.GetRow(rowIndex);
            ++rowIndex;
            IRow typeRow = sheet.GetRow(rowIndex);

            List<AttributeDefinition> attributes = attributesParser.GetAttributes(nameRow, typeRow);
            int endIndex = NO_INDEX;

            List<IRow> rawData = new List<IRow>();
            do
            {
                ++rowIndex;
                IRow row = sheet.GetRow(rowIndex);

                if (row == null)
                {
                    continue;
                }

                if (TableUtilities.IsTableEnd(row))
                {
                    endIndex = rowIndex;
                    break;
                }

                rawData.Add(row);
            }
            while (rowIndex < sheet.LastRowNum);

            TableDataContainer container = new TableDataContainer(metadata, attributes, rawData);
            if (endIndex == NO_INDEX)
            {
                container.MarkHasDataError();
                //todo error logging
            }

            return container;
        }

        private IEnumerable<ModelDataContainer> FillModelsWithData(IEnumerable<TableDataContainer> tableContainers)
        {
            List<ModelDataContainer> modelContainers = new List<ModelDataContainer>();
            foreach (TableDataContainer tableContainer in tableContainers)
            {
                ModelDataContainer modelContainer = ConvertTableDataToModelData(tableContainer);
                modelContainers.Add(modelContainer);
            }
            return modelContainers;
        }

        private ModelDataContainer ConvertTableDataToModelData(TableDataContainer tableData)
        {
            object model = classMaker.MakeClass(tableData.metadata.ClassName);

            ModelDataContainer modelContainer = new ModelDataContainer(classMaker, model, tableData);
            return modelContainer;
        }

        private void WriteDataToFiles(IEnumerable<ModelDataContainer> modelContainers)
        {
            DirectorUtilities.CreateDirectoryIfNeeded(DatabaseExportPath);

            foreach (ModelDataContainer container in modelContainers)
            {
                WriteDataToFile(container);
            }

            isExporting = false;
            UnityEditor.EditorUtility.DisplayDialog("Export", "Export complete!", "ok");
        }

        private void WriteDataToFile(ModelDataContainer container)
        {
            const string FULL_FILE_PATH = "{0}/{1}";

            string serializedData = SerializeModel(container);
            string filename = container.MetaData.TableName;
            string fullPath = string.Format(FULL_FILE_PATH, DatabaseExportPath, filename);
            File.WriteAllText(fullPath, serializedData);
        }

        private string SerializeModel(ModelDataContainer container)
        {
            string serializedData = serializationFunction.Invoke(container.model);
            if (container.MetaData.Compress)
            {
                serializedData = compressionFunction.Invoke(serializedData);
            }
            return serializedData;
        }
    }
}