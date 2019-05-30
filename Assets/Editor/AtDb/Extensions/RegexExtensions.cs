using System.Text.RegularExpressions;

namespace AtDb.Extensions
{
    public static class RegexExtensions
    {
        public static string GetFirstMatch(this Match match)
        {
            const int EXPECTED_GROUPS = 2;
            const int FIRST_GROUP = 1;

            string value = string.Empty;
            if (match.Groups.Count == EXPECTED_GROUPS)
            {
                value = match.Groups[FIRST_GROUP].Value;
            }
            else
            {
                //todo error logging
            }

            return value;
        }
    }
}