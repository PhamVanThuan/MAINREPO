using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using SAHL.Tools.Stringifier;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Helpers
{
    public static class PropertyUtility
    {

        private static Dictionary<string, Action<Property, CustomAttribute>> functionLookup =
            new Dictionary<string, Action<Property, CustomAttribute>>();

        static PropertyUtility()
        {
            functionLookup.Add("RequiredAttribute", new Action<Property, CustomAttribute>(HandelRequiredValidation));
            functionLookup.Add("StringLengthAttribute", new Action<Property, CustomAttribute>(HandleStringLengthValidation));
            functionLookup.Add("RegularExpressionAttribute", new Action<Property, CustomAttribute>(HandleRegularExpressionValidation));
            functionLookup.Add("MinLengthAttribute", new Action<Property, CustomAttribute>(HandleMinLengthValidation));
            functionLookup.Add("MaxLengthAttribute", new Action<Property, CustomAttribute>(HandleMaxLengthValidation));
            functionLookup.Add("RangeAttribute", new Action<Property, CustomAttribute>(HandleRangeValidation));
        }

        public static void GetPropertyValidation(PropertyDefinition property, Property prop)
        {
            // check if this property has a validation
            if (property.HasCustomAttributes)
            {
                foreach (var validationAttr in property.CustomAttributes)
                {

                    if (functionLookup.ContainsKey(validationAttr.AttributeType.Name))
                    {
                        Action<Property, CustomAttribute> action = functionLookup[validationAttr.AttributeType.Name];
                        action.DynamicInvoke(prop, validationAttr);
                    }
                    else
                    {
                        HandleCustomValidation(prop, validationAttr);
                    }
                }
            }
        }

        private static void HandleCustomValidation(Property prop, CustomAttribute validationAttr)
        {
            var attributeName = validationAttr.AttributeType.Name.Replace("Attribute", string.Empty);

            var validation = new ValidationModel();
            validation.FullType = validationAttr.AttributeType.FullName;
            validation.Name = Sentencer.InsertWhiteSpaceBetweenWords(attributeName);

            //get attribute arguments
            string arguments = "";
            foreach (var property in validationAttr.Properties)
            {
                if (property.Name != null & property.Argument.Value != null)
                {
                    arguments += Sentencer.InsertWhiteSpaceBetweenWords(property.Name) + " : " + property.Argument.Value + ", ";
                }
            }

            if (arguments.Length > 0)
            {
                validation.Name += " [" + arguments.TrimEnd().Substring(0, arguments.Length - 2) + "]";
            }

            prop.Validations.Add(validation);
        }

        private static void HandleMaxLengthValidation(Property prop, CustomAttribute validationAttr)
        {
            var validation = new ValidationModel();
            if (validationAttr.ConstructorArguments.Count == 1)
            {
                validation.Name = string.Format("Maximum Length [{0}]", validationAttr.ConstructorArguments[0].Value);
                HandleErrorMessaage(validationAttr, validation);
                validation.FullType = "System.ComponentModel.DataAnnotations.MaxLengthAttribute";
                prop.Validations.Add(validation);
            }
        }

        private static void HandleMinLengthValidation(Property prop, CustomAttribute validationAttr)
        {
            var validation = new ValidationModel();
            if (validationAttr.ConstructorArguments.Count == 1)
            {
                validation.Name = string.Format("Minimum Length [{0}]", validationAttr.ConstructorArguments[0].Value);
                HandleErrorMessaage(validationAttr, validation);
                validation.FullType = "System.ComponentModel.DataAnnotations.MinLengthAttribute";
                prop.Validations.Add(validation);
            }
        }

        private static void HandleRegularExpressionValidation(Property prop, CustomAttribute validationAttr)
        {
            var validation = new ValidationModel();
            if (validationAttr.ConstructorArguments.Count == 1)
            {
                validation.Name = string.Format("Regular Expression [{0}]", validationAttr.ConstructorArguments[0].Value);
                HandleErrorMessaage(validationAttr, validation);
                validation.FullType = "System.ComponentModel.DataAnnotations.RegularExpressionAttribute";
                prop.Validations.Add(validation);
            }
        }

        private static void HandleStringLengthValidation(Property prop, CustomAttribute validationAttr)
        {
            var validation = new ValidationModel();
            if (validationAttr.ConstructorArguments.Count == 1)
            {
                validation.Name = string.Format("Length [{0}]", validationAttr.ConstructorArguments[0].Value);
                HandleErrorMessaage(validationAttr, validation);
                validation.FullType = "System.ComponentModel.DataAnnotations.StringLengthAttribute";
                prop.Validations.Add(validation);
            }
        }

        private static void HandleRangeValidation(Property prop, CustomAttribute validationAttr)
        {
            var validation = new ValidationModel();
            if (validationAttr.ConstructorArguments.Count == 2)
            {
                validation.Name = string.Format("Range [{0} - {1}]", validationAttr.ConstructorArguments[0].Value, validationAttr.ConstructorArguments[1].Value);
                HandleErrorMessaage(validationAttr, validation);
                validation.FullType = "System.ComponentModel.DataAnnotations.RangeAttribute";
                prop.Validations.Add(validation);
            }
        }

        private static void HandelRequiredValidation(Property prop, CustomAttribute validationAttr)
        {
            var validation = new ValidationModel()
            {
                Name = "Required",
                FullType = "System.ComponentModel.DataAnnotations.RequiredAttribute"
            };
            prop.Validations.Add(validation);
        }

        private static void HandleErrorMessaage(CustomAttribute validationAttr, ValidationModel validation)
        {
            if (validationAttr.Properties.Any(x => x.Name == "ErrorMessage"))
            {
                var msg = validationAttr.Properties.Where(x => x.Name == "ErrorMessage").Single().Argument;
                validation.ErrorMessage = msg.Value == null ? "" : msg.Value.ToString();
            }
        }
    }
}