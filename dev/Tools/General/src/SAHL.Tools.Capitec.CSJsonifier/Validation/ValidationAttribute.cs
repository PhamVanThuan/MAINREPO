using System;
using System.Text.RegularExpressions;

namespace SAHL.Tools.Capitec.CSJsonifier.Validation
{
    public class ValidationAttribute
    {
        public string Name { get; set; }

        public string PropertyName { get; set; }

        public string PropertyNameText { get; set; }

        public string ErrorMessage { get; set; }

        public string[] Args { get; set; }

        public ValidationAttribute(string name, string propertyName, string errorMessage, string[] args)
        {
            this.Name = name;
            this.PropertyName = propertyName;
            this.ErrorMessage = errorMessage;
            this.PropertyNameText = ConvertPropertyNameToSpaced(propertyName);
            this.Args = args;
        }

        private string ConvertPropertyNameToSpaced(string propName)
        {
            var regex = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z]) |
                                (?<=[^A-Z])(?=[A-Z]) |
                                (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            string spacedName = regex.Replace(propName, " ");
            spacedName = Char.ToUpper(spacedName[0]) + spacedName.Substring(1, spacedName.Length - 1);
            return spacedName;
        }
    }

    public class ArrayValidationAttribute : ValidationAttribute
    {
        public bool IsComplex;
        public string FullName;

        public ArrayValidationAttribute(string name, string propertyName, string errorMessage, string[] args, string fullName)
            : base(name, propertyName, errorMessage, args)
        {
            this.IsComplex = true;
            this.FullName = fullName;
        }
    }

    public class ComplexValidationAttribute : ValidationAttribute
    {
        public string FullName;
        public ComplexValidationAttribute(string name, string propertyName, string errorMessage, string[] args,string fullName)
            : base(name, propertyName, errorMessage, args)
        {
            this.FullName = fullName;
        }
    }

    public class RangeValidationAttribute : ValidationAttribute
    {
        public bool numberCheck;
        public string min;
        public string max;

        public RangeValidationAttribute(string name, string propertyName, string errorMessage, bool numberCheck, string min, string max, string[] args)
            : base(name, propertyName, errorMessage, args)
        {
            this.numberCheck = numberCheck;
            this.min = min;
            this.max = max;
        }
    }
}