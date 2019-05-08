using NPOI.SS.UserModel;
using System;
using System.Text.RegularExpressions;
using TinyJSON;

namespace AtDb
{
    public static class TableUtilities
    {
        public static bool IsTableStart(IRow row, out TableMetadata metadata)
        {
            ICell metaDataCell = row.GetCell(0);

            try
            {
                metadata = JSON.Load(metaDataCell.StringCellValue).Make<TableMetadata>();
            }
            catch
            {
                metadata = null;
            }

            return metadata != null;
        }

        public static bool IsTableEnd(IRow row)
        {
            ICell firstCell = row.GetCell(0);
            if(firstCell.CellType != CellType.String)
            {
                return false;
            }

            string marker = firstCell.StringCellValue;
            Regex regex = new Regex(Constants.TABLE_END_REGEX, RegexOptions.IgnoreCase);
            return regex.IsMatch(marker);
        }
    }
}
