using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SAHL.Common.Utils
{
    /// <summary>
    /// Contains various utility functions pertaining to strings.
    /// </summary>
    public sealed class StringUtils
    {
        /// <summary>
        /// Helper function to concatenate a bunch of strings together where we only want strings that have a length
        /// greater than 0.
        /// </summary>
        /// <param name="nullableStrings"></param>
        /// <returns></returns>
        public static string JoinNullableStrings(params string[] nullableStrings)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in nullableStrings)
            {
                if (!String.IsNullOrEmpty(s))
                    sb.Append(s);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Capitalises only the first letter of a word - the rest will be in lowercase
        /// Note that any whitespace at the beginning or end of the string will be trimmed.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string CapitaliseFirstLetter(string word)
        {
            if (String.IsNullOrEmpty(word))
                return "";

            if (!char.IsLetter(word[0]))
                return word;

            string temp = word.Trim().ToLower();
            char capital = char.ToUpper(temp[0]);
            temp = temp.Remove(0, 1).Insert(0, capital.ToString());

            return temp;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="c"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int[] CountChar(char c, string s)
        {
            int pos = 0, count = 0;
            int[] output = { };

            while ((pos = s.IndexOf(c, pos)) != -1)
            {
                Array.Resize<int>(ref output, count + 1);
                output[count] = pos;
                count++;
                pos++;
            }
            return output;
        }

        /// <summary>
        /// Returns a list of enumeration values as a delimited string.  This is useful, for example, with
        /// sql IN queries.
        /// </summary>
        /// <example>
        /// <code>
        /// public enum MyEnum
        /// {
        ///     A = 1,
        ///     B = 2,
        ///     C = 3
        /// }
        ///
        /// string s = StringUtils.JoinEnumValues(",", MyEnum.A, MyEnum.C);
        /// </code>
        /// The value of <c>s</c> in the above code with be "1,3".
        /// </example>
        /// <param name="delimiter">The string delimiter.</param>
        /// <param name="enumValues">A list of enumeration values.</param>
        /// <returns>A string containing a list of values delimited by <c>delimiter</c>.</returns>
        public static string JoinEnumValues(string delimiter, params object[] enumValues)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object enumValue in enumValues)
            {
                int i = (int)Enum.Parse(enumValue.GetType(), enumValue.ToString());
                if (sb.Length > 0)
                    sb.Append(delimiter);
                sb.Append(i.ToString());
            }
            return sb.ToString();
        }

        public static string Delimit(string delimiter, string[] Strings)
        {
            string Result = "";
            for (int i = 0; i < Strings.Length - 1; i++)
            {
                Result += Strings[i] + " " + delimiter + " ";
            }
            Result += Strings[Strings.Length - 1];

            return Result;
        }

        public static bool IsNullOrEmptyTrimmed(string stringToCheck)
        {
            if (stringToCheck == null)
                return true;

            if (String.IsNullOrEmpty(stringToCheck.Trim()))
                return true;

            return false;
        }

        /// <summary>
        /// This method will removed everything but letters, numbers and spaces. It will also remove any ' or " followed by the character s.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

    }
}