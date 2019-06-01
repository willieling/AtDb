using System;
using TinyJSON;

namespace AtDb
{
    public enum DataStyle
    {
        Direct,
        List,
        Dictionary
    }

    [Serializable]
    public class TableMetadata
    {
        [Include]
        public string TableName { get; private set; }
        [Include]
        public string ClassName { get; private set; }
        [Include]
        public bool Compress { get; private set; }
        [Include]
        public bool Export { get; private set; }
        [Include]
        public DataStyle Style { get; private set; }

        public TableMetadata()
        {
            TableName = string.Empty;
            ClassName = string.Empty;
            Compress = true;
            Export = true;
            Style = DataStyle.Dictionary;
        }

        public TableMetadata(TableMetadata original)
        {
            TableName = original.TableName;
            ClassName = original.ClassName;
            Compress = original.Compress;
            Export = original.Export;
            Style = original.Style;

        }
    }
}