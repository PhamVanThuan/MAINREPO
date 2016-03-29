using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using SAHL.Tools.RestServiceRoutenator.Validation;

namespace SAHL.Tools.RestServiceRoutenator.Templates.Partials
{
    public partial class TypeDefinitionTemplate : ITemplate, ITemplateForModel<TypeDefinition>
    {
        private MethodReference constructor;
        private IList<ValidationAttribute> validationAttributes;

        internal ITemplateManager TemplateManager { get; set; }

        internal MethodReference Constructor
        {
            get
            {
                if (constructor == null)
                {
                    constructor = TemplateManager.GetConstructor(this.Model);
                }
                return constructor;
            }
        }


        internal IEnumerable<string> GetMethods()
        {
            return this.Model.Methods.Where(x => x.RouteableMethod()).Select(method => TemplateManager.GetTemplate(method).Process(method));
        }

        internal IList<ValidationAttribute> ValidationAttributes
        {
            get
            {
                if (validationAttributes == null)
                {
                    validationAttributes = TemplateManager.GetValidationAttributes(this.Model);
                }
                return validationAttributes;
            }
        }

        public TypeDefinition Model
        {
            get;
            protected set;
        }

        public TypeDefinitionTemplate(ITemplateManager templateManager)
        {
            this.TemplateManager = templateManager;
        }

        public string Process(dynamic model)
        {
            this.Model = model;
            return this.TransformText();
        }
    }
}