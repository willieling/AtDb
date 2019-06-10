using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace AtDb.Reader.Container
{
    public class TableDataContainer
    {
        public readonly TableMetadata metadata;
        public readonly List<AttributeDefinition> attributes;
        public readonly List<IRow> rawData;

        public bool IsExported { get; private set; }

        public TableDataContainer(TableMetadata metadata, List<AttributeDefinition> attributes, List<IRow> rawData)
        {
            this.metadata = metadata;
            this.attributes = attributes;
            this.rawData = rawData;
        }

        public void MarkAsExported()
        {
            IsExported = true;
        }
    }
}
