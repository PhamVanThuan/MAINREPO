using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.Query;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.Query.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            const string serviceUrlConfigurationName = "QueryServiceUrlConfiguration";

            this.For<IServiceUrlConfigurationProvider>()
                .Use<ServiceUrlConfigurationProvider>().Named(serviceUrlConfigurationName)
                .Ctor<string>("serviceName").Is("QueryService");

            this.For<IQueryServiceClient>()
                .Use<QueryServiceClient>()
                .Ctor<IServiceUrlConfigurationProvider>().Is(a => a.TheInstanceNamed(serviceUrlConfigurationName))
                .Ctor<IJsonActivator>("jsonActivator").Is<JsonActivator>();
        }
    }
}
