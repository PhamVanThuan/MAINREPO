using SAHL.Core.IoC;
using SAHL.Core.Messaging.EasyNetQ;
using SAHL.Services.Interfaces.CapitecFailedMessageProcessor;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.X2NodeServer
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IEasyNetQMessageBusSettings>().Singleton().Use<DefaultEasyNetQMessageBusSettings>();
            For<ISerializationProvider>().Singleton().Use<JsonSerializationProvider>();

            For<ICapitecFailedMessageProcessor>().Singleton().Use<CapitecFailedMessageProcessor>();

            For<IStartableService>().Use((context) => context.GetInstance<ICapitecFailedMessageProcessor>());
            For<IStoppableService>().Use((context) => context.GetInstance<ICapitecFailedMessageProcessor>());
        }
    }
}