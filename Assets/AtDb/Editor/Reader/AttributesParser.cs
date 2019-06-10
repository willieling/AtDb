using AtDb.ErrorSystem;
using AtDb.Extensions;
using AtDb.Reader.Container;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AtDb.Reader
{
    public class AttributesParser : IErrorLogger
    {
        private IRow nameRow;
        private IRow typeRow;
        private List<AttributeDefinition> attributes;

        public ErrorLogger ErrorLogger { get; private set; }

        public AttributesParser()
        {
            ErrorLogger = new ErrorLogger();
        }

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
            bool contains = typeValue.Contains(Constants.ARRAY_MARKER);
            return contains;
        }

        private int AddArrayAttribute(int index)
        {
            AttributeDefinition attribute = CreateAttributeWithIndex(index);
            attributes.Add(attribute);

            string startingName = attribute.Name;

            int peekIndex = index + 1;
            while(peekIndex < nameRow.LastCellNum)
            {
                ICell nameCell = nameRow.GetCell(peekIndex);
                string name = ExtractName(nameCell);

                bool sameName = string.Compare(name, startingName, true) == 0;
                if (sameName)
                {
                    attribute.IncrementEndIndex();
                }
                else
                {
                    break;
                }

                ++index;
                peekIndex = index + 1;
            }

            return index;
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

            string extractedName = ExtractName(name);
            string extractedType = ExtractType(type);

            AttributeDefinition attribute = new AttributeDefinition(i, extractedName, extractedType);
            return attribute;
        }

        private string ExtractName(ICell nameCell)
        {
            string name = nameCell.StringCellValue;
            Match match = Constants.attributeMarkerRegex.Match(name);

            string extractedName = string.Empty;
            if (match.Groups.Count >= 2)
            {
                extractedName = match.GetFirstMatch();
            }
            else
            {
                ErrorLogger.AddError(nameCell, "Could not extract name from {0}", name);
            }

            if (string.IsNullOrEmpty(extractedName))
            {
                ErrorLogger.AddError(nameCell, "Extracted name is empty");
            }

            return extractedName;
        }

        private string ExtractType(ICell cellType)
        {
            string type = cellType.StringCellValue;
            int index = type.IndexOf(Constants.ARRAY_MARKER);
            int length = index == -1 ? type.Length : index;
            string extracted = type.Substring(0, length);
            if(string.IsNullOrEmpty(extracted))
            {
                ErrorLogger.AddError(cellType, "Could not extract type from '{0}'", type);
            }

            return extracted;
        }
    }
}
