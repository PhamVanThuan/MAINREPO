using SAHL.Tools.Capitec.CSJsonifier.Validation;

namespace SAHL.Tools.Capitec.CSJsonifier.Templates.Shared
{
    public partial class ValidationAttributeTemplate : ITemplate, ITemplateForModel<ValidationAttribute>
    {
        public ValidationAttribute Model { get; protected set; }

        public string Process(dynamic model)
        {
            this.Model = model;
            return this.TransformText();
        }
    }
}