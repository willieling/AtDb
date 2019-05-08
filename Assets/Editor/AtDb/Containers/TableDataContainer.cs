using System.Collections.Generic;

namespace AtDb.Reader.Container
{
    public class TableDataContainer
    {
        public TableMetadata Metadata { get; set; }
        public List<AttributeDefinition> Attributes { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public TableDataContainer(TableMetadata metadata, List<AttributeDefinition> attributes, int startIndex, int endIndex)
        {
            Metadata = metadata;
            Attributes = attributes;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }
}
