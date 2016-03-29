using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator.Model
{
    public class AttributeModel
    {
        public string HtmlAttribute
        {
            get;
            protected set;
        }

        public string NGErrorTag
        {
            get;
            protected set;
        }

        public string NGErrorMessage
        {
            get;
            protected set;
        }

        public AttributeModel(Mono.Cecil.CustomAttribute attribute)
        {
            string className = attribute.AttributeType.Name;
            switch (className)
            {
                case "RequiredAttribute":
                    HtmlAttribute = "ng-required='true'";
                    NGErrorMessage = "input is required";
                    NGErrorTag = "required";
                    break;
                case "RangeAttribute":
                    HtmlAttribute = string.Format("sahl-range='from:{0},to:{1}'", attribute.ConstructorArguments[0].Value, attribute.ConstructorArguments[1].Value);
                    NGErrorMessage = string.Format("input must be within the range of {0} and {1}", attribute.ConstructorArguments[0].Value, attribute.ConstructorArguments[1].Value);
                    NGErrorTag = "range";
                    break;
                case "RegularExpressionAttribute":
                    HtmlAttribute = string.Format("ng-pattern='/{0}/'", attribute.ConstructorArguments[0].Value);
                    NGErrorMessage = attribute.Properties[0].Argument.Value + "";
                    NGErrorTag = "pattern";
                    break;
                case "EmailAddressAttribute":
                    HtmlAttribute = "ng-pattern='/(^$)|([A-Za-z0-9_\\-\\.])+\\@([A-Za-z0-9_\\-\\.])+\\.([A-Za-z]{2,4})/'";
                    NGErrorMessage = "Input must be in the form of an email address";
                    NGErrorTag = "pattern";
                    break;
                case "PhoneAttribute":
                    HtmlAttribute = "ng-pattern='/^$|0[0-8]d{8}/'";
                    NGErrorMessage = "Input must be in the form of a phone number";
                    NGErrorTag = "pattern";
                    break;
                case "MaxLengthAttribute":
                    HtmlAttribute = string.Format("ng-maxlength='{0}'", attribute.ConstructorArguments[0].Value);
                    NGErrorMessage = string.Format("Input has max length of {0} characters", attribute.ConstructorArguments[0].Value);
                    NGErrorTag = "maxlength";
                    break;
                case "MinLengthAttribute":
                    HtmlAttribute = string.Format("ng-minlength='{0}'", attribute.ConstructorArguments[0].Value);
                    NGErrorMessage = string.Format("Input has min length of {0} characters", attribute.ConstructorArguments[0].Value);
                    NGErrorTag = "minlength";
                    break;
                default:
                    break;
            }
        }
    }
}
