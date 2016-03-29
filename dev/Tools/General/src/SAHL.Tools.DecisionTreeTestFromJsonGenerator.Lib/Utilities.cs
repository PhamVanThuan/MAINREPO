using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SAHL.Tools.DecisionTreeTestFromJsonGenerator.Lib
{
    public static class Utilities
    {
        public static string UppercaseFirstLetter(string original)
        {
            if (String.IsNullOrEmpty(original)) { return String.Empty; }
            return String.Format("{0}{1}", original.Substring(0, 1).ToUpper(), original.Substring(1, original.Length - 1));
        }

        public static string StripBadChars(string inString)
        {
            inString = inString.Replace("(", string.Empty);
            inString = inString.Replace(")", string.Empty);
            inString = inString.Replace("-", string.Empty);
            Regex rgx = new Regex("[^a-zA-Z0-9_]");
            return rgx.Replace(inString, string.Empty);
        }

        public static string EscapeQuotes(string original)
        {
            if (String.IsNullOrEmpty(original)) { return String.Empty; }
            return original.Replace("\"", "\\\"");
        }
    }
}