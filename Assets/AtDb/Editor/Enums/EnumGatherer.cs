using AtDb.Reader.Container;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;

namespace AtDb.Enums
{
    public class EnumGatherer
    {
        private readonly EnumCacher enumCacher;

        private int columnIndex;
        private List<IRow> rows;

        public EnumGatherer(EnumCacher enumCacher)
        {
            this.enumCacher = enumCacher;
        }

        public void CacheEnumValues(TableDataContainer tableContainer)
        {
            rows = tableContainer.rawData;

            for (int i = 0; i < tableContainer.attributes.Count; ++i)
            {
                columnIndex = i;
                AttributeDefinition attribute = tableContainer.attributes[columnIndex];
                CacheEnumValues(attribute);
            }
        }

        private void CacheEnumValues(AttributeDefinition attribute)
        {
            string[] values = GetValues();

            EnumContainer.EnumStyle style;
            bool parsed = Enum.TryParse(attribute.Type, out style);
            if(!parsed)
            {
                //todo error logging
            }
            else
            {
                enumCacher.CacheEnum(attribute.Name, values, style);
            }
        }

        private string[] GetValues()
        {
            List<string> values = new List<string>(rows.Count);

            foreach (IRow row in rows)
            {
                ICell cell = row.GetCell(columnIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL);
                if (cell != null)
                {
                    values.Add(cell.StringCellValue);
                }
            }

            return values.ToArray();
        }
    }
}