using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.Capitec.CSJsonifier.Validation
{
    public class ValidationAttributeScanner : IValidationAttributeScanner
    {
        public IList<ValidationAttribute> ScanValidationAttributes(Mono.Cecil.TypeDefinition type)
        {
            List<ValidationAttribute> results = new List<ValidationAttribute>();

            results.AddRange(GetPropertyValidationAttributes(type));
            results.AddRange(GetLinkedCommandValidationAttributes(type));

            return results;
        }

        private List<ValidationAttribute> GetPropertyValidationAttributes(Mono.Cecil.TypeDefinition type)
        {
            List<ValidationAttribute> results = new List<ValidationAttribute>();

            foreach (var property in type.Properties.Where(p => p.CustomAttributes.Any(attr => attr.AttributeType.Namespace == "System.ComponentModel.DataAnnotations")).ToList())
            {
                string propertyName = Char.ToLowerInvariant(property.Name[0]) + property.Name.Substring(1);

                var attributes = property.CustomAttributes.Where(attr => attr.AttributeType.Namespace == "System.ComponentModel.DataAnnotations").ToList();
                foreach (var attribute in attributes)
                {
                    results.Add(CreateValidationAttribute(attribute, propertyName, property.PropertyType));
                }
            }

            return results;
        }

        private List<ValidationAttribute> GetLinkedCommandValidationAttributes(Mono.Cecil.TypeDefinition type)
        {
            List<ValidationAttribute> results = new List<ValidationAttribute>();

            //Get all interfaces that implement interfaces.
            var possibleValidations = type.Module.Types.Where(x => x.IsInterface && x.HasInterfaces);
            foreach (var possibleValidation in possibleValidations)
            {
                var customValidations = possibleValidation.Interfaces.Where(x => x.Name == "ICommandValidation`2" || x.Name == "IQueryValidation`2");
                foreach (var customValidation in customValidations)
                {
                    var genericType = (Mono.Cecil.GenericInstanceType)customValidation;
                    if (genericType.GenericArguments.Any(x => x.Name == type.Name))
                    {
                        var validation = genericType.GenericArguments.Where(x => x.Name != type.Name).First();
                        var validationType = validation.Module.Types.First(x => x.Name == validation.Name);
                        var customAttributes = GetPropertyValidationAttributes(validationType);

                        var propertyNames = new List<string>();
                        foreach (var prop in type.Properties)
                        {
                            propertyNames.Add(prop.Name);
                        }
                        results.AddRange(customAttributes.Where(attr => propertyNames.Any(propName => String.Compare(propName, attr.PropertyName, true) == 0)));
                    }
                }
            }

            return results;
        }

        private string GetValidationErrorMessage(Mono.Cecil.CustomAttribute valAttr)
        {
            string errorMessage = "";
            var errorArgument = valAttr.Properties.FirstOrDefault(x => x.Name == "ErrorMessage");
            if (errorArgument.Argument.Value != null)
                errorMessage = errorArgument.Argument.Value.ToString();
            return errorMessage;
        }

        private ValidationAttribute CreateValidationAttribute(Mono.Cecil.CustomAttribute attribute, string propertyName,Mono.Cecil.TypeReference type)
        {
            Mono.Cecil.TypeDefinition defined = type.Resolve();
            string name = attribute.AttributeType.Name.Replace("Attribute", "");
            var errorMessage = GetValidationErrorMessage(attribute);
            List<string> args = new List<string>();
            foreach (var cArgs in attribute.ConstructorArguments)
            {
                args.Add(cArgs.Value.ToString());
            }
            if (name == "Range")
            {
                return GetRangeValidationAttribute(new ValidationAttribute(name, propertyName, errorMessage, args.ToArray()), attribute);
            }
            if (type.Namespace == "System.Collections.Generic")
            {
                Mono.Cecil.TypeDefinition propDefinition = ((Mono.Cecil.GenericInstanceType)type).GenericArguments[0].Resolve();
                bool isComplex = (propDefinition.IsClass && !propDefinition.IsAbstract && !propDefinition.Namespace.StartsWith("System") && !propDefinition.IsArray);
                if (isComplex)
                {

                    return new ArrayValidationAttribute(name, propertyName, errorMessage, args.ToArray(), string.Format("{0},{1}", propDefinition.FullName, propDefinition.Module.Assembly.Name.Name));
                }
            }
            if (type.IsArray)
            {
                Mono.Cecil.TypeDefinition propDefinition = type.GetElementType().Resolve();
                bool isComplex = (propDefinition.IsClass && !propDefinition.IsAbstract && !propDefinition.Namespace.StartsWith("System") && !propDefinition.IsArray);
                if (isComplex)
                {
                    return new ArrayValidationAttribute(name, propertyName, errorMessage, args.ToArray(), string.Format("{0},{1}", propDefinition.FullName, propDefinition.Module.Assembly.Name.Name));
                }
            }
            if (defined.IsClass && !defined.IsAbstract && !defined.Namespace.StartsWith("System") && !type.IsArray)
            {
                return new ComplexValidationAttribute(name, propertyName, errorMessage, args.ToArray(), string.Format("{0},{1}",defined.FullName,defined.Module.Assembly.Name.Name));
            }
            return new ValidationAttribute(name, propertyName, errorMessage, args.ToArray());
        }

        private RangeValidationAttribute GetRangeValidationAttribute(ValidationAttribute valAttr, Mono.Cecil.CustomAttribute attribute)
        {
            var firstArg = attribute.ConstructorArguments[0];
            var args = valAttr.Args;
            string min = "";
            string max = "";
            bool numberCheck = false;
            if (firstArg.Type.FullName == "System.Type")
            {
                var typeName = ((Mono.Cecil.TypeReference)firstArg.Value).Name;
                numberCheck = (typeName == "Decimal" || typeName.Contains("Int") || typeName == "Float" || typeName == "Double");
                min = args[1];
                max = args[2];
            }
            else
            {
                numberCheck = true;
                min = args[0];
                max = args[1];
            }
            return new RangeValidationAttribute(valAttr.Name, valAttr.PropertyName, valAttr.ErrorMessage, numberCheck, min, max, valAttr.Args);
        }
    }
}