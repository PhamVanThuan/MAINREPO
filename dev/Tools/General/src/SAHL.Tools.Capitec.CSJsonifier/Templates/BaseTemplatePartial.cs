using Mono.Cecil;
using StructureMap;
using System.Text;
namespace SAHL.Tools.Capitec.CSJsonifier.Templates
{
    public partial class BaseTemplate : IBaseTemplate
    {
        internal INamespaceProvider NamespaceProvider;
        private IContainer container;
        private ITemplateManager templateManager;
        public IScanResult Result { get; protected set; }

        internal string generatedCode;

        public BaseTemplate(IContainer container,ITemplateManager templateManager,INamespaceProvider namespaceProvider)
        {
            this.container = container;
            this.templateManager = templateManager;
            this.NamespaceProvider = namespaceProvider;
        }

        public string Process(IScanResult result)
        {
            this.Result = result;
            ITemplate template = templateManager.GetTemplate(result);
            this.generatedCode = template.Process(result);
            return this.TransformText();
        }


    }
}