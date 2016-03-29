using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SAHL.Tools.RestServiceRoutenator.Reflection;
using StructureMap;

namespace SAHL.Tools.RestServiceRoutenator.Templates
{
    public partial class RestBaseTemplate : IRestBaseTemplate
    {
        internal INamespaceProvider NamespaceProvider;
        private IContainer container;
        private ITemplateManager templateManager;
        public IScanResult Result { get; protected set; }

        internal string generatedCode;

        public RestBaseTemplate(IContainer container, ITemplateManager templateManager, INamespaceProvider namespaceProvider)
        {
            this.container = container;
            this.templateManager = templateManager;
            this.NamespaceProvider = namespaceProvider;
        }

        public string Process(IScanResult result)
        {
            this.Result = result;
            var routes = result.FoundTypes.GetAllRoutesForGivenTypes();
            routes = routes.Concat(result.AdditionalData).Distinct();
            
            RouteFinder finder = new RouteFinder(new Tokeniser());
            finder.Initialise(routes.ToArray());
            Routes theRoots = finder.AllRoutes;

            ITemplate template = templateManager.GetTemplate(theRoots);
            this.generatedCode =
                theRoots.ChildRoutes.Select(template.Process).Aggregate((current, next) => current + next);// template.Process(theRoots);
            return this.TransformText();
        }


    }
}