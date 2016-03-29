using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.X2;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace SAHL.Config.Services.X2.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            this.For<IServiceUrlConfiguration>().Use<ServiceUrlConfiguration>()
                                                .Ctor<string>().Is(ConfigurationManager.AppSettings["X2WebHost_Url"]).Named("X2ServiceClientConfiguration");
            this.For<IX2Service>().Use<X2ServiceClient>()
                                  .Ctor<IServiceUrlConfiguration>().Is(x => x.GetInstance<IServiceUrlConfiguration>("X2ServiceClientConfiguration"));
        }
    }
}
