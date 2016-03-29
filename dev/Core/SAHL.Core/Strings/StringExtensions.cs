using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SAHL.Core.Strings
{
    public static class StringExtensions
    {
        //Field is wrapped as Lazy<> so that we only create the replacer if it is called
        private static readonly Lazy<StringReplacer> stringReplacer = new Lazy<StringReplacer>();

        private static readonly IDictionary<string, string> keywords = new Dictionary<string, string>()
        {
            {".Core.",".C."},
            {".Interfaces.",".I."},
            {".Services.",".SVS."},
            {".Projections.",".P."},
            {".EventProjection.",".EP."},
            {".Events.",".E."},
            {".LegacyEventGenerator.",".LEG."},
            {".WorkFlow.",".WF."},
            {"WrappedEvent`","WE`"}
        };

        private static readonly Regex spaceAndCharacterRegex = new Regex("[^a-zA-Z0-9_]");

        public static string ConvertToTitleCase(this string source)
        {
            CultureInfo info = System.Threading.Thread.CurrentThread.CurrentCulture;

            TextInfo textInfo = info.TextInfo;

            string returnString = textInfo.ToTitleCase(source);

            return returnString;
        }

        public static string RemoveSpaceAndCharacters(string input)
        {
            return spaceAndCharacterRegex.Replace(input, string.Empty);
        }

        public static bool IsDigitsOnly(this string input)
        {
            return input.All(a => a >= '0' && a <= '9');
        }

        //for reference
        //http://msdn.microsoft.com/en-us/library/x2dbyw72(v=vs.71).aspx

        public static string CamelCase(string input)
        {
            var cleanedInput = RemoveSpaceAndCharacters(input);
            return Char.ToLowerInvariant(cleanedInput[0]) + cleanedInput.Substring(1);
        }

        public static string PascalCase(string input)
        {
            var cleanedInput = RemoveSpaceAndCharacters(input);
            return Char.ToUpperInvariant(cleanedInput[0]) + cleanedInput.Substring(1);
        }

        public static string Shorten(this string source)
        {
            var result = KeywordReplacer(source, (kvp) => kvp.Key, (kvp) => kvp.Value);
            return result.Replace("SAHL.", "S.");
        }

        public static string Lengthen(this string source)
        {
            var result = KeywordReplacer(source, (kvp) => kvp.Value, (kvp) => kvp.Key);
            Regex reg = new Regex(@"^S\.|\sS\.|(?<=:)S\.|(?<=\[)S\.");
            return reg.Replace(result, "SAHL.");
        }

        public static string TryFormat(string format, object source)
        {
            return TryFormat(CultureInfo.InvariantCulture, format, source);
        }

        public static string TryFormat(IFormatProvider formatProvider, string format, object source)
        {
            var formatProviderToUse = formatProvider ?? CultureInfo.InvariantCulture;
            if (format == null)
            {
                return source == null ? null : source.ToString();
            }
            try
            {
                return string.Format(formatProviderToUse, format, source);
            }
            catch (Exception)
            {
                return source.ToString();
            }
        }

        private static string KeywordReplacer(string contextString, Func<KeyValuePair<string, string>, string> key, Func<KeyValuePair<string, string>, string> value)
        {
            return keywords.Aggregate(contextString, (current, kvp) => current.Replace(key(kvp), value(kvp)));
        }
    }
}
