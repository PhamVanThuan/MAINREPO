using SAHL.Tools.Capitec.CSJsonifier.Validation;
using Mono.Cecil;
using System.Collections.Generic;

namespace SAHL.Tools.Capitec.CSJsonifier.Templates.Shared
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