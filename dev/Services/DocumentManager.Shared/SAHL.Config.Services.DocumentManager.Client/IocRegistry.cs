using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.DocumentManager;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.DocumentManager.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("DocumentManagerUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("DocumentManagerService");

            For<IDocumentManagerServiceClient>().Use<DocumentManagerServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("DocumentManagerUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}