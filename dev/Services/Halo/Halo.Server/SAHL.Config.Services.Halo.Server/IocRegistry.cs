using StructureMap.Configuration.DSL;

using SAHL.Core.IoC;
using SAHL.Services.Halo.Server;

namespace SAHL.Config.Services.Halo.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            this.For<IStartable>().Use<MapProfileStartup>();
        }
    }
}
