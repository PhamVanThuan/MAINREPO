using System;
using System.Text;

namespace SAHL.Tools.RestServiceRoutenator
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
            foreach (string word 
                in words)
                sb.Append(string.Format("{0}{1}", word.Substring(0, 1).ToLower(), word.Substring(1)));
            return sb.ToString();
        }


        public static string MakeFirstLetterLowerCase(this string source)
        {
            return char.ToLower(source[0]) + source.Substring(1);
        }

        public static string MakeFirstLetterCapitalCase(this string source)
        {
            return char.ToUpper(source[0]) + source.Substring(1);
        }

        public static string GetCleanControllerName(this string incomingName)
        {
            return incomingName.Replace("Controller", "");
        }

        public static string CleanOperandText(string input)
        {
            if (input.StartsWith("\"") && input.EndsWith("\""))
                return input.Substring(1, input.Length - 2).Replace("\"", "\\\"");
            return input;
        }
    }
}