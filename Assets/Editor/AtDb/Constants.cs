using TinyJSON;

namespace AtDb
{
    public static class Constants
    {
        public const EncodeOptions ENCODE_OPTIONS = EncodeOptions.EnumsAsInts | EncodeOptions.NoTypeHints;

        public const string TABLE_END_REGEX = "@@end";

        public const string ARRAY_MARKER = "_";
    }

}