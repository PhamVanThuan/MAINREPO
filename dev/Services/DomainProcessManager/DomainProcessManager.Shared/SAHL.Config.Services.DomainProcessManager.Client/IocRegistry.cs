using SAHL.Services.Interfaces.DomainProcessManager;
using StructureMap.Configuration.DSL;
using System.Collections.Specialized;
using System.Configuration;

namespace SAHL.Config.Services.DomainProcessManager.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IDomainProcessManagerClient>()
               .Use<DomainProcessManagerClient>()
               .Ctor<NameValueCollection>("nameValueCollection")
               .Is(ConfigurationManager.AppSettings);
        }
    }
}