using SAHL.Core.Messaging.EasyNetQ;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.EventPublisher.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IEasyNetQMessageBusSettings>().Use<ShortNameEasyNetQMessageBusSettings>();
        }
    }
}