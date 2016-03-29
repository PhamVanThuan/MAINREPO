using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SAHL.Tools.Stringifier
{
    public static class Sentencer
    {
        private static readonly Regex genericTypeArgumentRegex = new Regex("`[0-9]+", RegexOptions.Compiled);
        private static readonly Regex underscoreRegex = new Regex("_+", RegexOptions.Compiled);
        private static readonly Regex interfaceTypeDescriptorRegex = new Regex("I[A-Z][a-z]+|^(?!ID)I[A-Z]+", RegexOptions.Compiled);
        private static readonly Regex sentenceRegularCaseRegex = new Regex("[A-Z][a-z]+", RegexOptions.Compiled);
        private static readonly Regex sentenceUpperCaseRegex = new Regex("[A-Z][A-Z]+[a-z]", RegexOptions.Compiled);
        private static readonly Regex numericCaseRegex = new Regex("[0-9]+[a-z]+|[0-9]+", RegexOptions.Compiled);

        public static string ToSentence(this string source)
        {
            return InsertWhiteSpaceBetweenWords(source).TrimEnd();
        }

        public static string ToSentenceFromTypeName(string input)
        {
            var output = input;

            output = RemoveInterfaceTypeDescriptor(output);
            output = HandleGenericTypeArgument(output);
            output = HandleUnderscores(output);

            output = InsertWhiteSpaceBetweenWords(output);

            return output.TrimEnd();
        }

        public static string InsertWhiteSpaceBetweenWords(string input)
        {
            string output = input;

            output = HandleUpperCasesCase(output);
            output = HandleSentenceRegularCase(output);
            output = HandleNumericCase(output);

            return output.TrimEnd();
        }

        private static string HandleGenericTypeArgument(string input)
        {
            var output = genericTypeArgumentRegex.Replace(input, string.Empty);
            return output;
        }

        public static string HandleUnderscores(string input)
        {
            var output = underscoreRegex.Replace(input, string.Empty);
            return output;
        }

        private static string RemoveInterfaceTypeDescriptor(string input)
        {
            return interfaceTypeDescriptorRegex.IsMatch(input) ? input.Substring(1, input.Length - 1) : input;
        }

        private static string HandleSentenceRegularCase(string input)
        {
            var output = sentenceRegularCaseRegex.Replace(input, AddSpaceIfNotAlreadyEndingWithSpace);
            return output;
        }

        public static string HandleUpperCasesCase(string input)
        {
            var output = sentenceUpperCaseRegex.Replace(input, AddSpaceBeforeLastCapitals);
            return output;
        }

        private static string AddSpaceBeforeLastCapitals(Match match)
        {
            return match.Value.Substring(0, match.Value.Length - 2) + " " + match.Value.Substring(match.Value.Length - 2, 2);
        }

        private static string HandleNumericCase(string input)
        {
            var output = numericCaseRegex.Replace(input, AddSpaceIfNotAlreadyEndingWithSpace);
            return output;
        }

        private static string AddSpaceIfNotAlreadyEndingWithSpace(Match match)
        {
            var value = match.Value.EndsWith(" ") ? match.Value : match.Value + " ";
            return value;
        }
    }
}
