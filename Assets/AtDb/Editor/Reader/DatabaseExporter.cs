using AtDb.Enums;
using AtDb.ErrorSystem;
using AtDb.Reader.Container;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

namespace AtDb.Reader
{
    public class DatabaseExporter : IErrorLogger
    {
        private readonly AttributesParser attributesParser;
        private readonly ClassMaker classMaker;
        private readonly BookReference bookReference;
        private readonly ModelContainerFactoryFactory modelContainerFactoryFactory;
        private readonly EnumCacher enumCacher;
        private readonly DatabaseFileWriter databaseWriter;
        private readonly ErrorReporter errorReporter;

        private ModelContainerFactory modelContainerFactory;

        private bool isExporting;

        public ErrorLogger ErrorLogger { get; private set; }

        public DatabaseExporter()
        {
            attributesParser = new AttributesParser();
            classMaker = new ClassMaker();
            bookReference = new BookReference();
            modelContainerFactoryFactory = new ModelContainerFactoryFactory(classMaker);
            enumCacher = new EnumCacher(classMaker);
            databaseWriter = new DatabaseFileWriter(enumCacher);
            errorReporter = new ErrorReporter(bookReference);

            ErrorLogger = new ErrorLogger();
        }

        public string DatabaseSourcePath
        {
            get { return databaseWriter.DatabaseSourcePath; }
            set { databaseWriter.DatabaseSourcePath = value; }
        }
        public string DatabaseExportPath
        {
            get { return databaseWriter.DatabaseExportPath; }
            set { databaseWriter.DatabaseExportPath = value; }
        }
        public string GeneratedEnumsPath
        {
            get { return databaseWriter.GeneratedEnumsPath; }
            set { databaseWriter.GeneratedEnumsPath = value; }
        }

        public void Initialize(DatabaseExporterConfiguration configuration)
        {
            databaseWriter.Initialize(configuration);
        }

        public void Export()
        {
            if(isExporting)
            {
                return;
            }

            Reset();

            isExporting = true;

            string[] excelFiles = GetExcelFiles(DatabaseSourcePath);
            IEnumerable<TableDataContainer> tableDataContainers = GetTableDataContainers(excelFiles);
            IEnumerable<ModelDataContainer> modelContainers = FillModelsWithData(tableDataContainers);
            databaseWriter.ExportAllData(modelContainers);
            errorReporter.PrintNotices();

            isExporting = false;
        }

        private void Reset()
        {
            errorReporter.Clear();
            enumCacher.Clear();
        }

        private string[] GetExcelFiles(string folderPath)
        {
            const string ONLY_EXCEL = "*.xlsx";
            string[] files = Directory.GetFiles(folderPath, ONLY_EXCEL);
            if (files.Length == 0)
            {
                ErrorLogger.AddError("Could not find any excels files at {0}", folderPath);
            }

            return files;
        }

        private IEnumerable<TableDataContainer> GetTableDataContainers(string[] files)
        {
            if (errorReporter.HasErrors)
            {
                FlagStopExport();
                return null;
            }

            List<TableDataContainer> containers = new List<TableDataContainer>();
            foreach (string file in files)
            {
                IEnumerable<TableDataContainer> fileContainers = GetTableDataContainersFromExcelFile(file);
                containers.AddRange(fileContainers);
            }

            return containers;
        }

        private IEnumerable<TableDataContainer> GetTableDataContainersFromExcelFile(string filePath)
        {
            if (errorReporter.HasErrors)
            {
                FlagStopExport();
                return null;
            }

            XSSFWorkbook workbook = GetWorkbook(filePath);
            bookReference.AddBookWithPath(workbook, filePath);

            IEnumerable<TableDataContainer> containers = GetTableDatacontainersFromWorkbook(workbook);
            return containers;
        }

        private void FlagStopExport()
        {
            errorReporter.PrintNotices();
            isExporting = false;
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

            ErrorLogger.CopyNotices(attributesParser);

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
                ErrorLogger.AddError("Could not find ending for table {0}", metadata.TableName);
            }

            ErrorLogger.CopyNotices(attributesParser);

            return container;
        }

        private IEnumerable<ModelDataContainer> FillModelsWithData(IEnumerable<TableDataContainer> tableContainers)
        {
            if (errorReporter.HasErrors)
            {
                FlagStopExport();
                return null;
            }

            modelContainerFactory = modelContainerFactoryFactory.Create();
            List<ModelDataContainer> modelContainers = new List<ModelDataContainer>();

            CacheEnumValues(tableContainers);

            foreach (TableDataContainer tableContainer in tableContainers)
            {
                if (!tableContainer.metadata.IsEnum)
                {
                    ModelDataContainer modelContainer = modelContainerFactory.Create(tableContainer);
                    modelContainers.Add(modelContainer);
                }
            }

            errorReporter.LogNotices(modelContainerFactory);


            return modelContainers;
        }

        private void CacheEnumValues(IEnumerable<TableDataContainer> tableContainers)
        {
            EnumGatherer enumGatherer = enumCacher.GetGatherer();
            foreach (TableDataContainer tableContainer in tableContainers)
            {
                if (tableContainer.metadata.IsEnum)
                {
                    enumGatherer.CacheEnumValues(tableContainer);
                }
            }

            errorReporter.LogNotices(enumCacher);
        }
    }
}