using System;
using System.Text.RegularExpressions;

namespace SAHL.DecisionTree.Shared.Helpers
{
    public static class Utilities
    {
        public static string rubyFunctions = @" class Numeric                                                    
                                                    def truncate_to( decimals=0 )
                                                        factor = 10.0**decimals
                                                        (self*factor).floor / factor
                                                    end
                                                end";

        public static string StripInvalidChars(string original)
        {
            original = original.Replace("(", string.Empty);
            original = original.Replace(")", string.Empty);
            original = original.Replace("-", string.Empty);
            Regex rgx = new Regex("[^a-zA-Z0-9_]");
            return rgx.Replace(original, string.Empty);
        }

        public static string CapitaliseFirstLetter(string original)
        {
            return String.Format("{0}{1}", original.Substring(0, 1).ToUpper(), original.Substring(1, original.Length - 1));
        }

        public static string LowerFirstLetter(string original)
        {
            return String.Format("{0}{1}", original.Substring(0, 1).ToLower(), original.Substring(1, original.Length - 1));
        }

        public static string ToPascalCase(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                throw new ArgumentNullException("original");
            }

            original = original.Replace("(", string.Empty);
            original = original.Replace(")", string.Empty);
            var words = original.Split(new char[] { ' ', '_' });
            var result = string.Empty;
            foreach (var word in words)
            {
                result += string.Format("{0}{1}", word.Substring(0, 1).ToUpper(), word.Substring(1, word.Length - 1));
            }
            return result;
        }
    }
}