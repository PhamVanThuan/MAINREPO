using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace Common.Extensions
{
    public static class EnumerableExtensions
    {
        private static Random random = new Random();

        public static T SelectRandom<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException();
            if (!sequence.Any())
                return sequence.FirstOrDefault();
            int count = sequence.Count();
            int index = random.Next(0, count);
            return sequence.ElementAtOrDefault(index);
        }
    }

    public static class StringExtensions
    {
        /// <summary>
        /// This method will take a currency string in the format of "R 710,000.00" to either "710000" or "710000.00".
        /// This is required when retrieving a label from a screen and wanting to compare it a string value from a test
        /// </summary>
        /// <param name="removeDecimals">Option to remove the decimals</param>
        /// <param name="str">string to be cleaned</param>
        public static string CleanCurrencyString(this String str, bool removeDecimals)
        {
            int index = 0;
            string cleanedString;
            if (str.IndexOf('R') >= 0)
            {
                str = str.Remove(0, 2);
            }

            while (str.IndexOf(',') > 0)
            {
                index = str.IndexOf(',');
                str = str.Remove(index, 1);
            }
            while (str.IndexOf(" ") > 0)
            {
                index = str.IndexOf(" ");
                str = str.Remove(index, 1);
            }
            if (!removeDecimals)
            {
                cleanedString = str;
            }
            else
            {
                index = str.IndexOf('.');
                str = str.Remove(index, 3);
                cleanedString = str;
            }

            cleanedString = str;
            return cleanedString;
        }

        /// <summary>
        /// This method will remove the double space from a string and replace it with a single space.
        /// </summary>
        /// <param name="str">String to clean</param>
        public static string RemoveDoubleSpace(this String str)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            while (str.Contains("  "))
            {
                str = regex.Replace(str, @" ");
            }
            return str;
        }

        /// <summary>
        /// Removes percentages from strings
        /// </summary>
        /// <param name="removeDecimals"></param>
        /// <param name="str">string to clean</param>
        public static string CleanPercentageString(this String str, bool removeDecimals)
        {
            if (str.IndexOf('%') > 0)
            {
                int indexer = str.IndexOf('%');
                str = str.Remove(indexer, 1);
            }
            if (removeDecimals)
            {
                int decimalIndexer = str.IndexOf('.');
                str = str.Remove(decimalIndexer, 3);
            }
            return str;
        }

        /// <summary>
        /// This method will escape '+', '^' and '$' characters from a string by placing the escape character '\' before them.
        /// </summary>
        /// <param name="str">String to clean</param>
        public static string EscapeCharactersBeforeRegexProcessing(this String str)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            string[] characters = new string[] { "+", "^", "$" };

            foreach (string c in characters)
                str = str.Replace(c, @"\" + c);

            return str;
        }

        public static string RemoveDoubleSpaces(this String str)
        {
            string[] split = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string result = string.Empty;

            foreach (string s in split)
                result += s + " ";
            return result.TrimEnd(' ');
        }

        public static string RemoveDomainPrefix(this String str)
        {
            return str.Replace(@"SAHL\", string.Empty);
        }

        public static string RemoveWhiteSpace(this String str)
        {
            return str.Replace(" ", string.Empty);
        }

        public static string ToNullableSQLString(this string str)
        {
            return str = string.IsNullOrEmpty(str) ? "null" : string.Concat("'", str, "'");
        }
    }

    public static class DictionaryExtensions
    {
        public static bool LastActivitySucceeded(this Dictionary<int, Common.Models.WorkflowReturnData> dictionary)
        {
            return dictionary.OrderByDescending(x => x.Key).Last().Value.ActivityCompleted;
        }

        public static Common.Models.WorkflowReturnData LastStep(this Dictionary<int, Common.Models.WorkflowReturnData> dictionary)
        {
            return dictionary.OrderByDescending(x => x.Key).Last().Value;
        }
    }

    public static class DateExtensions
    {
        public static string ToNullableSQLString(this DateTime? date)
        {
            return (date == null) ? "null" : Convert.ToDateTime(date).ToString(Common.Constants.Formats.DateTimeFormatSQL);
        }
    }

    public static class TextFieldExtensions
    {
        public static void ClearExistingValueAndSetTo(this TextField textField, string value)
        {
            textField.Clear();
            textField.Value = value;
        }

        public static void SetNewValueIfNotTheSameAsCurrent(this TextField textField, string value)
        {
            var currentValue = textField.Value;
            if (currentValue != value)
                textField.TypeText(value);
        }
    }

    public static class SelectListExtensions
    {
        public static string SelectNewOptionReturningNewSelection(this SelectList selectList)
        {
            if (selectList.Options.Count() == 1)
                return selectList.SelectedOption.Text;
            Option selectedItem = selectList.SelectedOption;
            var option = (from options in selectList.Options where options.Value != selectedItem.Value && !options.Value.Contains("select") select options).SelectRandom();
            selectList.SelectByValue(option.Value);
            return selectList.SelectedOption.Text;
        }
    }
}