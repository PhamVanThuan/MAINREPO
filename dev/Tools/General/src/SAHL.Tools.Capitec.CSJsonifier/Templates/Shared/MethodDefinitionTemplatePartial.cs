using Mono.Cecil;

namespace SAHL.Tools.Capitec.CSJsonifier.Templates.Shared
{
    public partial class MethodDefinitionTemplate : ITemplate, ITemplateForModel<MethodDefinition>
    {
        public MethodDefinition Model { get; protected set; }

        public string Process(dynamic model)
        {
            this.Model = model;
            return this.TransformText();
        }
    }
}