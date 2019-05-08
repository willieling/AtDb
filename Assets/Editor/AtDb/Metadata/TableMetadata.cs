using System;
using TinyJSON;

namespace AtDb
{
    [Serializable]
    public class TableMetadata
    {
        [Include]
        public bool Encrypted { get; private set; }
        [Include]
        public bool Export { get; private set; }
        [Include]
        public bool LookupTable { get; private set; }
        [Include]
        public bool IntermediateTable { get; private set; }

        public TableMetadata()
        {
            Encrypted = true;
            Export = true;
            LookupTable = true;
            IntermediateTable = false;
        }

        public TableMetadata(TableMetadata original)
        {
            Encrypted = original.Encrypted;
            Export = original.Export;
            LookupTable = original.LookupTable;
            IntermediateTable = original.IntermediateTable;

        }
    }
}