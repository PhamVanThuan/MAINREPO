using StructureMap;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.Capitec.CSJsonifier
{
    public class TemplateManager : ITemplateManager
    {
        private IContainer container;
        private IValidationAttributeScanner validationScanner;

        public TemplateManager(IContainer container, IValidationAttributeScanner validationScanner)
        {
            this.container = container;
            this.validationScanner = validationScanner;
        }

        public ITemplate GetTemplate(dynamic model)
        {
            return container.ForGenericType(typeof(ITemplateForModel<>)).WithParameters(model.GetType()).GetInstanceAs<ITemplate>();
        }

        public Mono.Cecil.MethodReference GetConstructor(Mono.Cecil.TypeDefinition typeDefintion)
        {
            var constructors = typeDefintion.Methods.Where(x => x.IsConstructor);
            if (constructors.Count() > 1)
            {
                return typeDefintion.Methods.FirstOrDefault(x => x.IsConstructor && x.HasParameters);
            }
            return typeDefintion.Methods.SingleOrDefault(x => x.IsConstructor);
        }

        public IList<Validation.ValidationAttribute> GetValidationAttributes(Mono.Cecil.TypeDefinition typeDefintion)
        {
            return this.validationScanner.ScanValidationAttributes(typeDefintion);
        }

        
    }
}