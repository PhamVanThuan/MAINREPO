using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SAHL.Tools.DecisionTreeModelsFromJsonGenerator.Lib
{
    public class Utilities
    {
        public static string ToPascalCase(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                throw new ArgumentNullException("original");
            }
            
            var words = original.Split(new char[] { ' ', '_' });
            var result = string.Empty;
            foreach (var word in words)
            {
                result += string.Format("{0}{1}", word.Substring(0, 1).ToUpper(), word.Substring(1, word.Length - 1));
            }
            return StripBadChars(result);
        }

        public static string StripSpacesAndUpperCaseFirst(string original)
        {
            if (string.IsNullOrWhiteSpace(original))
            {
                throw new ArgumentNullException("original");
            }
            var words = original.Split(' ');
            var result = String.Empty;
            foreach (var word in words)
            {
                result += word;
            }
            result = String.Format("{0}{1}", result.Substring(0, 1).ToUpper(), result.Substring(1, result.Length - 1));
            return result;
        }

        public static string StripBadChars(string inString)
        {
            inString = inString.Replace("(", string.Empty);
            inString = inString.Replace(")", string.Empty);
            inString = inString.Replace("-", string.Empty);
            Regex rgx = new Regex("[^a-zA-Z0-9_]");
            return rgx.Replace(inString, string.Empty);
        }

        public static string ToCamelCase(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                throw new ArgumentNullException("original");
            }
            original = StripBadChars(original);
            return string.Format("{0}{1}", original.Substring(0, 1).ToLower(), original.Substring(1, original.Length - 1));
        }
    }
}
