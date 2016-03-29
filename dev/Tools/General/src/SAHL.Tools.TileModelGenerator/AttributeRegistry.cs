using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator
{
    public static class AttributeRegistry
    {
        internal static string GetAttribute(Mono.Cecil.CustomAttribute attribute)
        {
            string className = attribute.AttributeType.Name;
            string result = "";

            switch (className)
            {
                case "RequiredAttribute":
                    result = "ng-required";
                    break;
                case "RangeAttribute":
                    result = string.Format("sahl-range='from:{0},to:{1}'",attribute.ConstructorArguments[0].Value,attribute.ConstructorArguments[1].Value);
                    break;
                case "RegularExpressionAttribute":
                    result = string.Format("ng-pattern='/{0}/'",attribute.ConstructorArguments[0].Value);
                    break;
                case "StringLengthAttribute":
                    result = string.Format("ng-maxlength='{0}'", attribute.ConstructorArguments[0].Value);
                    break;
                case "EmailAddressAttribute":
                    result = "ng-pattern='/(^$)|([A-Za-z0-9_\\-\\.])+\\@([A-Za-z0-9_\\-\\.])+\\.([A-Za-z]{2,4})/'";
                    break;
                case "PhoneAttribute":
                    result = "ng-pattern='/^$|0[0-8]d{8}/'";
                    break;
                case "MaxLengthAttribute":
                    result = string.Format("ng-maxlength='{0}'", attribute.ConstructorArguments[0].Value);
                    break;
                case "MinLengthAttribute":
                    result = string.Format("ng-minlength='{0}'", attribute.ConstructorArguments[0].Value);
                    break;
                default:
                     break;
            }
            return result;
        }
    }
}
