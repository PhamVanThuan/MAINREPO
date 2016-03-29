using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;

namespace SAHL.Tools.RestServiceRoutenator
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
            Type type = model.GetType();
            var template = container.ForGenericType(typeof(ITemplateForModel<>)).WithParameters(type).GetInstanceAs<ITemplate>();
            return template;
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