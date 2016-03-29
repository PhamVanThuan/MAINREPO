using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using StructureMap.Configuration.DSL;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.EventProjectionStreamer;

namespace SAHL.Config.Services.EventProjectionStreamer.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>()
                                                   .Named("EventProjectionStreamerUrlConfiguration")
                                                   .Ctor<string>().Is(x => "EventProjectionStreamerService");

            For<IEventProjectionStreamerServiceClient>().Use<EventProjectionStreamerServiceClient>()
                                                  .Ctor<IServiceUrlConfigurationProvider>()
                                                        .Is(x => x.TheInstanceNamed("EventProjectionStreamerUrlConfiguration"))
                                                  .Ctor<IJsonActivator>("jsonActivator")
                                                        .Is<JsonActivator>();
        }
    }
}
