using SAHL.Tools.Capitec.CSJsonifier.Validation;
using Mono.Cecil;
using System.Collections.Generic;

namespace SAHL.Tools.Capitec.CSJsonifier
{
    public interface ITemplateManager
    {
        ITemplate GetTemplate(dynamic model);

        MethodReference GetConstructor(TypeDefinition typeDefintion);

        IList<ValidationAttribute> GetValidationAttributes(TypeDefinition typeDefintion);
    }
}