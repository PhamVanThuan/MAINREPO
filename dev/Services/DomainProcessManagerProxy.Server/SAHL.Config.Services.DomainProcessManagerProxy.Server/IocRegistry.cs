using SAHL.Core.Services.CommandPersistence;
using SAHL.Services.DomainProcessManagerProxy.DpmServiceReference;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.DomainProcessManagerProxy.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IDomainProcessManagerService>().Use(new DomainProcessManagerServiceClient());
        }
    }
}
