using NPOI.SS.UserModel;
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
            if(firstCell == null || firstCell.CellType != CellType.String)
            {
                return false;
            }

            string marker = firstCell.StringCellValue;
            bool isMatch = Constants.endTableRegex.IsMatch(marker);
            return isMatch;
        }

        public static bool HasIncludeMarker(string value)
        {
            bool isMatch = Constants.attributeMarkerRegex.IsMatch(value);
            return isMatch;
        }
    }
}
