using AtDb.ErrorSystem;
using System.Text.RegularExpressions;

namespace AtDb.Extensions
{
    public static class RegexExtensions
    {
        public static string GetFirstMatch(this Match match)
        {
            const int FIRST_GROUPING = 1;

            string value = match.Groups[FIRST_GROUPING].Value;
            return value;
        }
    }
}