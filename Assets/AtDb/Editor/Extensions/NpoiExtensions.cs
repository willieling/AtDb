//using AtDb.Reader;
//using NPOI.SS.UserModel;

//namespace AtDb.Extensions
//{
//    public static class NpoiExtensions
//    {
        

//        public static string GetLocation(this ICell cell)
//        {
//            IRow row = cell.Row;
//            ISheet sheet = row.Sheet;
//            IWorkbook workbook = sheet.Workbook;

//            char columnName = cell.ColumnIndex.ToExcelColumn();
//            int rowNumber = cell.RowIndex + 1;  //rowIndex is 0 based

//            string location = string.Format("Book {0} - Sheet {1} - Cell{2}{3}",
//                bookReference.GetBookName(workbook), sheet.SheetName, columnName, rowNumber);
//            return location;
//        }

//        public static string GetLocation(this IRow row)
//        {
//            ISheet sheet = row.Sheet;
//            IWorkbook workbook = sheet.Workbook;

//            string location = string.Format("Book {0} - Sheet {1}",
//                bookReference.GetBookName(workbook), sheet.SheetName);
//            return location;
//        }
//    }
//}