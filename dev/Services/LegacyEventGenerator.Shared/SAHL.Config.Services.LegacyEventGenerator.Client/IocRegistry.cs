using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.LegacyEventGenerator;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Config.Services.LegacyEventGenerator.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("LegacyEventGeneratorUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("LegacyEventGeneratorService");

            For<ILegacyEventGeneratorServiceClient>().Use<LegacyEventGeneratorServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("LegacyEventGeneratorUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}
