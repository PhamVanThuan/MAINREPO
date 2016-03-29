using Mono.Cecil;

namespace SAHL.Tools.Capitec.CSJsonifier.Templates.Shared
{
    public partial class ParameterDefinitionTemplate : ITemplate, ITemplateForModel<ParameterDefinition>
    {
        public ParameterDefinition Model { get; protected set; }

        public string Process(dynamic model)
        {
            this.Model = model;
            return this.TransformText();
        }
    }
}