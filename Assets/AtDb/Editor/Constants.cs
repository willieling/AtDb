using System.Reflection;
using System.Text.RegularExpressions;
using TinyJSON;

namespace AtDb
{
    public static class Constants
    {
        public const EncodeOptions ENCODE_OPTIONS = EncodeOptions.EnumsAsInts | EncodeOptions.NoTypeHints;
        public const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public;

        public const string TABLE_END_REGEX = "@@end";
        public const string ARRAY_MARKER = "_";
        public const string ATTRIBUTE_MARKER_PATTERN = @"\s+@(.+)";

        public const int INPUT_OFFSET = 125;
        public const int THREADS = 8;

        public static readonly Regex beforeUnderscoreRegex = new Regex(BEFORE_UNDERSCORE_PATTERN);
        public static readonly Regex attributeMarkerRegex = new Regex(ATTRIBUTE_MARKER_PATTERN);
        public static readonly Regex endTableRegex = new Regex(TABLE_END_REGEX, RegexOptions.IgnoreCase);

        private const string BEFORE_UNDERSCORE_PATTERN = "(.+)_.+";
        private const string ATTRIBUTE_NAME_PATTERN = "(.+)_.+";
    }

}