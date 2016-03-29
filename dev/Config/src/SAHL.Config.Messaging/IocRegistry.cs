using SAHL.Core.Messaging;
using SAHL.Core.Messaging.EasyNetQ;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Messaging
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IMessageBusAdvanced>().Singleton().Use<MessageBus>();
            For<IMessageBus>().Use(x => x.GetInstance<IMessageBusAdvanced>());
            For<IMessageBusConfigurationProvider>().Singleton().Use<MessageBusConfigurationProvider>();
            For<IEasyNetQMessageBusSettings>().Singleton().Use<ShortNameEasyNetQMessageBusSettings>();
            For<IMessageRouteNameBuilder>().Singleton().Use<DefaultRouteNameBuilder<IMessageRoute>>();
        }
    }
}