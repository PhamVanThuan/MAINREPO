using System;
using System.Text;
namespace SAHL.Tools.Capitec.CSJsonifier.Templates
{
    public partial class FakeBaseTemplate : IFakeBaseTemplate
    {
        public IScanResult Result { get; protected set; }
        internal INamespaceProvider NamespaceProvider;


        public FakeBaseTemplate(INamespaceProvider namespaceProvider)
        {
            this.NamespaceProvider = namespaceProvider;
        }

        public string Process(IScanResult result)
        {
            this.Result = result;
            return this.TransformText();
        }
    }
}