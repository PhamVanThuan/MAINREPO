using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.ITC;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.ITC.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("ITCUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("ITCService");

            For<IItcServiceClient>().Use<ItcServiceClient>()
                                    .Ctor<IServiceUrlConfigurationProvider>()
                                        .Is(x => x.TheInstanceNamed("ITCUrlConfiguration"))
                                    .Ctor<IJsonActivator>("jsonActivator")
                                        .Is<JsonActivator>();
        }
    }
}