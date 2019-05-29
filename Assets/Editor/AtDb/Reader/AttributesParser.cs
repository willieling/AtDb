using AtDb.Extensions;
using AtDb.Reader.Container;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AtDb.Reader
{
    public class AttributesParser
    {
        private IRow nameRow;
        private IRow typeRow;
        private List<AttributeDefinition> attributes;

        public List<AttributeDefinition> GetAttributes(IRow nameRow, IRow typeRow)
        {
            this.nameRow = nameRow;
            this.typeRow = typeRow;
            attributes = new List<AttributeDefinition>();

            for (int i = 0; i < this.nameRow.LastCellNum; ++i)
            {
                if (ShouldInclude(i))
                {
                    if (IsArray(i))
                    {
                        i = AddArrayAttribute(i);
                    }
                    else
                    {
                        AddSingleAttribute(i);
                    }
                }
            }
            return attributes;
        }

        private bool ShouldInclude(int i)
        {
            ICell type = nameRow.GetCell(i);
            bool hasMarker = TableUtilities.HasIncludeMarker(type.StringCellValue);
            return hasMarker;
        }

        private bool IsArray(int i)
        {
            ICell type = typeRow.GetCell(i);
            string typeValue = type.StringCellValue;
            return typeValue.Contains(Constants.ARRAY_MARKER);
        }

        private int AddArrayAttribute(int index)
        {
            AttributeDefinition attribute = CreateAttributeWithIndex(index);
            string startingName = attribute.Name;

            int peekIndex = index + 1;
            for (; peekIndex < nameRow.LastCellNum; ++index)
            {
                ICell name = nameRow.GetCell(peekIndex);
                bool sameName = string.Compare(name.StringCellValue, startingName, true) == 0;
                if (sameName)
                {
                    attribute.IncrementEndIndex();
                }
                else
                {
                    break;
                }

                peekIndex = index + 1;
            }

            return index;
        }

        private string GetType(string typeString)
        {
            Match match = Constants.beforeUnderscoreRegex.Match(typeString);
            string type = match.GetFirstMatch();
            return type;
        }

        private void AddSingleAttribute(int i)
        {
            AttributeDefinition attribute = CreateAttributeWithIndex(i);
            attributes.Add(attribute);
        }

        private AttributeDefinition CreateAttributeWithIndex(int i)
        {
            ICell name = nameRow.GetCell(i);
            ICell type = typeRow.GetCell(i);

            string matchedName = ExtractName(name);

            AttributeDefinition attribute = new AttributeDefinition(i, matchedName, type.StringCellValue);
            return attribute;
        }

        private string ExtractName(ICell name)
        {
            Match match = Constants.attributeMarkerRegex.Match(name.StringCellValue);
            string extractedName = match.GetFirstMatch();

            if (string.IsNullOrEmpty(extractedName))
            {
                //todo error logging
            }

            return extractedName;
        }
    }
}
