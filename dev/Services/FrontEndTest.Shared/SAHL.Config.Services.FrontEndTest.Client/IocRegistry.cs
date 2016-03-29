using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.FrontEndTest;
using StructureMap.Configuration.DSL;
using System;
using System.Linq;

namespace SAHL.Config.Services.FrontEndTest.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("FrontEndTestUrlConfiguration")
                .Ctor<string>("serviceName").Is("FrontEndTestService");
            For<IFrontEndTestServiceClient>().Use<FrontEndTestServiceClient>()
                           .Ctor<IServiceUrlConfigurationProvider>()
                                .Is(x => x.TheInstanceNamed("FrontEndTestUrlConfiguration"))
                           .Ctor<IJsonActivator>("jsonActivator")
                                .Is<JsonActivator>();
        }
    }
}