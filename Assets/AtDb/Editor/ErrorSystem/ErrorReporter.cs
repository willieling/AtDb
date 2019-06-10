using AtDb.Extensions;
using AtDb.Reader;
using NPOI.SS.UserModel;
using System.Collections.Concurrent;
using System.Text;
using UnityEngine;

namespace AtDb.ErrorSystem
{
    public class ErrorReporter
    {
        private const string SHEET_LOCATION = "Book '{0}' - Sheet '{1}'";
        private const string CELL_LOCATION = SHEET_LOCATION + " - Cell {2}{3}";

        private ConcurrentBag<NoticeContainer> warnings = new ConcurrentBag<NoticeContainer>();
        private ConcurrentBag<NoticeContainer> errors = new ConcurrentBag<NoticeContainer>();

        private readonly StringBuilder stringBuilder = new StringBuilder();

        private readonly BookReference bookReference;

        public ErrorReporter(BookReference bookReference)
        {
            this.bookReference = bookReference;
        }

        public bool HasWarnings
        {
            get { return warnings.Count > 0; }
        }

        public bool HasErrors
        {
            get { return errors.Count > 0; }
        }

        public void PrintNotices()
        {
            foreach (NoticeContainer error in errors)
            {
                string message = FormatAndAddLocation(error);
                Debug.LogError(message);
            }

            foreach (NoticeContainer warning in warnings)
            {
                string message = FormatAndAddLocation(warning);
                Debug.LogError(message);
            }

            Clear();
        }

        public void LogNotices(params IErrorLogger[] loggers)
        {
            foreach (IErrorLogger logger in loggers)
            {
                foreach (NoticeContainer error in logger.ErrorLogger.errors)
                {
                    errors.Add(error);
                }

                foreach (NoticeContainer warning in logger.ErrorLogger.warnings)
                {
                    warnings.Add(warning);
                }
            }
        }

        public void Clear()
        {
            warnings = new ConcurrentBag<NoticeContainer>();
            errors = new ConcurrentBag<NoticeContainer>();
        }

        private string FormatAndAddLocation(NoticeContainer notice)
        {
            string format = notice.baseMessage;
            object[] args = notice.args;
            string location = GetLocation(notice);
            stringBuilder.Clear();
            stringBuilder.AppendFormat(format, args);

            stringBuilder.Append(location);
            string item = stringBuilder.ToString();
            return item;
        }

        private string GetLocation(NoticeContainer notice)
        {
            string location = string.Empty;
            if (notice.cell != null)
            {
                location = GetLocation(notice.cell);
            }
            else if (notice.row != null)
            {
                location = GetLocation(notice.row);
            }

            return location;
        }

        private string GetLocation(ICell cell)
        {
            IRow row = cell.Row;
            ISheet sheet = row.Sheet;
            IWorkbook workbook = sheet.Workbook;

            char columnName = cell.ColumnIndex.ToExcelColumn();
            int rowNumber = cell.RowIndex;

            string location = string.Format(CELL_LOCATION,
                bookReference.GetBookName(workbook), sheet.SheetName, columnName, rowNumber);
            return location;
        }

        private string GetLocation(IRow row)
        {
            ISheet sheet = row.Sheet;
            IWorkbook workbook = sheet.Workbook;

            string location = string.Format(SHEET_LOCATION,
                bookReference.GetBookName(workbook), sheet.SheetName);
            return location;
        }
    }
}