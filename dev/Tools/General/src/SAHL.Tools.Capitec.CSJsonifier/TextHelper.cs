using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Capitec.CSJsonifier
{
    public static class TextHelper
    {
        public static string PascalCase(string source)
        {
            StringBuilder sb = new StringBuilder(source.Length);
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException("A null or empty value cannot be converted", source);
            if (source.Length < 2)
                return source.ToUpper();
            string[] words = source.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
                sb.Append(string.Format("{0}{1}", word.Substring(0, 1).ToLower(), word.Substring(1)));
            return sb.ToString();
        }

        public static string CleanOperandText(string input)
        {
            if (input.StartsWith("\"") && input.EndsWith("\""))
                return input.Substring(1, input.Length - 2).Replace("\"", "\\\"");
            return input;
        }
    }
}
