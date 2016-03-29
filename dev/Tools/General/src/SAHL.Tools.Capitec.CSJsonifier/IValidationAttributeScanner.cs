using SAHL.Tools.Capitec.CSJsonifier.Validation;
using System.Collections.Generic;

namespace SAHL.Tools.Capitec.CSJsonifier
{
    public interface IValidationAttributeScanner
    {
        IList<ValidationAttribute> ScanValidationAttributes(Mono.Cecil.TypeDefinition type);
    }
}