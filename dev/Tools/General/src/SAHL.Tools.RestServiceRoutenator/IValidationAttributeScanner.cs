using System.Collections.Generic;
using SAHL.Tools.RestServiceRoutenator.Validation;

namespace SAHL.Tools.RestServiceRoutenator
{
    public interface IValidationAttributeScanner
    {
        IList<ValidationAttribute> ScanValidationAttributes(Mono.Cecil.TypeDefinition type);
    }
}