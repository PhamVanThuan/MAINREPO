using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib
{
    public static class Utilities
    {
        public static string StripInvalidChars(string original)
        {
            original = original.Replace("(", string.Empty);
            original = original.Replace(")", string.Empty);
            original = original.Replace("-", string.Empty);
            Regex rgx = new Regex("[^a-zA-Z0-9_]");
            return rgx.Replace(original, string.Empty);
        }

        public static string LowerFirstLetter(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return String.Empty;
            }
            return String.Format("{0}{1}", original.Substring(0, 1).ToLower(), original.Substring(1, original.Length - 1));
        }

        public static string CapitaliseFirstLetter(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return String.Empty;
            }
            return String.Format("{0}{1}", original.Substring(0, 1).ToUpper(), original.Substring(1, original.Length - 1));
        }

        public static dynamic ToDynamic(object value)
        {
            if (value.GetType().Equals(typeof(ExpandoObject)))
            {
                return value;
            }
            IDictionary<string, object> expando = new ExpandoObject();

            Type type = value.GetType();
            foreach (var fieldInfo in type.GetFields())
                expando.Add(fieldInfo.Name, fieldInfo.GetValue(value));

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }

        public static bool IsMessageOfSeverity(ISystemMessage message, string expectedSeverity)
        {
            expectedSeverity = expectedSeverity.Replace(" Messages", String.Empty);
            SystemMessageSeverityEnum expectedSeverityEnum = SystemMessageSeverityEnum.Debug;
            if (Enum.TryParse(expectedSeverity, true, out expectedSeverityEnum))
            {
                return message.Severity == expectedSeverityEnum;
            }
            else return false;
        }

        public static string GetEnumerationValueForRubyEnumString(string rubyEnum, dynamic enumerations)
        {
            string enumerationValue = "";
            if (rubyEnum.StartsWith("Enumerations::"))
            {
                rubyEnum = rubyEnum.Replace("::", ".");
                var enumerationParts = rubyEnum.Split('.');

                ExpandoObject currentEnumerationGroup = Utilities.ToDynamic(enumerations);
                foreach (var enumerationPart in enumerationParts.Skip(1))
                {
                    var currentGroup = ((IDictionary<string, object>)currentEnumerationGroup)[enumerationPart];
                    if (enumerationPart != enumerationParts.Last())
                    {
                        currentEnumerationGroup = Utilities.ToDynamic(currentGroup);
                    }
                    else
                    {
                        enumerationValue = currentGroup.ToString();
                    }
                }
            }
            return enumerationValue;
        }
    }
}
