using SAHL.Core.Data;
using SAHL.Core.Events;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Events
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IEventStore>().Use((y) => new EventStore(y.TryGetInstance<IEventSerialiser>(), y.TryGetInstance<IDbFactory>()));
        }
    }
}