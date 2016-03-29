using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.Communications;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.Communications.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>()
                                                   .Named("CommunicationsUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is(x => "CommunicationsService");

            For<ICommunicationsServiceClient>().Use<CommunicationsServiceClient>()
                                                  .Ctor<IServiceUrlConfigurationProvider>()
                                                        .Is(x => x.TheInstanceNamed("CommunicationsUrlConfiguration"))
                                                  .Ctor<IJsonActivator>("jsonActivator")
                                                        .Is<JsonActivator>();
        }
    }
}