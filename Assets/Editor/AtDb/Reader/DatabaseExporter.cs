using AtDb.Reader.Container;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using TinyJSON;

namespace AtDb.Reader
{
    public class DatabaseExporter
    {
        private readonly AttributesParser attributesParser = new AttributesParser();
        public void Export(string folderPath)
        {
            string[] excelFiles = GetExcelFiles(folderPath);
            IEnumerable<TableDataContainer> containers = GetTableDataContainers(excelFiles);
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
            ++rowIndex;

            List<AttributeDefinition> attributes = attributesParser.GetAttributes(nameRow, typeRow);
            int startIndex = rowIndex;
            int endIndex = NO_INDEX;
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
            }
            while (rowIndex < sheet.LastRowNum);

            if (endIndex == NO_INDEX)
            {
                //todo error logging
            }

            TableDataContainer container = new TableDataContainer(metadata, attributes, startIndex, endIndex);
            return container;
        }

        private TableMetadata GetMetaData(IRow row)
        {
            const int SECOND_CELL_INDEX = 1;

            ICell metaDataCell = row.GetCell(SECOND_CELL_INDEX);
            TableMetadata metaData = JSON.Load(metaDataCell.StringCellValue).Make<TableMetadata>();
            return metaData;
        }
    }
}