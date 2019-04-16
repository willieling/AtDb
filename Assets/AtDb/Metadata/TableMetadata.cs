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

        public TableMetadata(bool encrypted, bool export)
        {
            Encrypted = encrypted;
            Export = export;
        }

        public TableMetadata(TableMetadata original)
        {
            Encrypted = original.Encrypted;
            Export = original.Export;
        }
    }
}