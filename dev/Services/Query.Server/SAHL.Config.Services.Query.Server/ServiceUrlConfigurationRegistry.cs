using SAHL.Core.Services;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.Query.Server
{
    public class ServiceUrlConfigurationRegistry : Registry
    {
        public const string ServiceUrlConfigurationName = "QueryServiceUrlConfiguration";

        public ServiceUrlConfigurationRegistry()
        {
            this.For<IServiceUrlConfigurationProvider>()
                .Use<ServiceUrlConfigurationProvider>()
                .Named(ServiceUrlConfigurationName)
                .Ctor<string>("serviceName").Is("QueryService");
        }
    }
}