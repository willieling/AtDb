using AtDb.Reader.Container;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AtDb.Reader
{
    public class AttributesParser
    {
        private IRow nameRow;
        private IRow typeRow;
        private readonly List<AttributeDefinition> attributes = new List<AttributeDefinition>();

        public List<AttributeDefinition> GetAttributes(IRow nameRow, IRow typeRow)
        {
            this.nameRow = nameRow;
            this.typeRow = typeRow;
            attributes.Clear();

            for (int i = 0; i < this.nameRow.LastCellNum; ++i)
            {
                if (IsArray(i))
                {
                    i = CreateArrayAttribute(i);
                }
                else
                {
                    CreateSingleAttribute(i);
                }
            }
            return attributes;
        }

        private bool IsArray(int i)
        {
            ICell type = typeRow.GetCell(i);
            string typeValue = type.StringCellValue;
            return typeValue.Contains(Constants.ARRAY_MARKER);
        }

        private int CreateArrayAttribute(int index)
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
            const string BEFORE_UNDERSCORE = "(.+)_.+";
            const int EXPECTED_GROUPS = 2;
            const int FIRST_GROUP = 1;

            Regex regex = new Regex(BEFORE_UNDERSCORE);
            Match match = regex.Match(typeString);

            if(match.Groups.Count != EXPECTED_GROUPS)
            {
                //todo error loggin
            }
            return match.Groups[FIRST_GROUP].Value;
        }

        private void CreateSingleAttribute(int i)
        {
            AttributeDefinition attribute = CreateAttributeWithIndex(i);
            attributes.Add(attribute);
        }

        private AttributeDefinition CreateAttributeWithIndex(int i)
        {
            ICell name = nameRow.GetCell(i);
            ICell type = typeRow.GetCell(i);

            AttributeDefinition attribute = new AttributeDefinition(i, name.StringCellValue, type.StringCellValue);
            return attribute;
        }
    }
}
