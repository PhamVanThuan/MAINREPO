using System.Collections.Generic;
using Mono.Cecil;
using SAHL.Tools.RestServiceRoutenator.Validation;

namespace SAHL.Tools.RestServiceRoutenator
{
    public interface ITemplateManager
    {
        ITemplate GetTemplate(dynamic model);

        MethodReference GetConstructor(TypeDefinition typeDefintion);

        IList<ValidationAttribute> GetValidationAttributes(TypeDefinition typeDefintion);
    }
}