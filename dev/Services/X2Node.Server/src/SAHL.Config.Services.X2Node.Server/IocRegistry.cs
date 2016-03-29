using SAHL.Config.Services.X2.NodeServer;
using SAHL.Core.Caching;
using SAHL.Core.Configuration;
using SAHL.Core.Data;
using SAHL.Core.Data.Dapper;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Core.Messaging;
using SAHL.Core.Messaging.EasyNetQ;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.AppDomain;
using SAHL.Core.X2.Factories;
using SAHL.Core.X2.Logging;
using SAHL.Core.X2.Messages;
using SAHL.Config.Messaging;
using SAHL.X2Engine2;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Communication.EasyNetQ;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Node;
using SAHL.X2Engine2.Node.AppDomain;
using SAHL.X2Engine2.Providers;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;
using System.IO.Abstractions;
using SAHL.X2Engine2.Communication.RabbitMQ;
using SAHL.Core.Messaging.RabbitMQ;
using StructureMap;

namespace SAHL.Config.Services.X2NodeServer
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                x.WithDefaultConventions();
                x.ConnectImplementationsToTypesClosing(typeof(ICacheKeyGeneratorFactory<>));
                x.LookForRegistries();
            });


            For<IX2EngineConfigurationProvider>().Singleton().Use<X2EngineConfigurationProvider>();

            // Node
            For<IX2ServiceCommandRouter>().LifecycleIs(new ThreadLocalStorageLifecycle()).Use<X2ServiceCommandRouter>();
            For<IServiceCommandHandlerProvider>().Use<StructureMapServiceHandlerProvider>();

            For<IRabbitMQConnectionFactoryFactory>().Singleton().Use<RabbitMQConnectionFactoryFactory>();
            For<IRabbitMQConnectionFactory>().Singleton().Use<RabbitMQConnectionFactory>();
            For<IQueueConsumerManager>().Singleton().Use<QueueConsumerManager>();
            For<IX2RequestSubscriber>().Singleton().Use<X2NodeRequestConsumerConfigurationProvider>();
            For<IX2ConsumerConfigurationProvider>().Use((context) => context.GetInstance<IX2RequestSubscriber>());
            For<IX2ConsumerManager>().Singleton().Use<RabbitMQConsumerManager>();
            For<IX2RequestPublisher>().Singleton().Use<RabbitMQRequestPublisher>();
            For<IX2ResponsePublisher>().Singleton().Use<RabbitMQResponsePublisher>();
            For<IX2WorkflowRequestHandler>().Singleton().Use<X2WorkflowRequestHandler>();
            For<IProcessInstantiator>().Singleton().Use<ProcessInstantiator>();
            For<IAppDomainFactory>().Singleton().Use<AppDomainFactory>();
            For<IAppDomainProcessProxyCache>().Singleton().Use<AppDomainProcessProxyCache>();
            For<IAppDomainProcessProxyFactory>().Singleton().Use<AppDomainProcessProxyFactory>();
            For<IMessageCollectionFactory>().Use<MessageCollectionFactory>();
            For<ICache>().Singleton().Use<InMemoryCache>();
            For<IHashGenerator>().Use<DefaultHashGenerator>();
            For<IFileSystem>().Singleton().Use<FileSystem>();
            For<IConfigurationProvider>().Singleton().Use<ConfigurationProvider>();
            For<INugetRetriever>().Singleton().Use<NugetRetriever>();
            For<IAppDomainFileManager>().Singleton().Use<AppDomainFileManager>();
            For<IServiceCommandHandler<HandleMapReturnCommand>>().Use<HandleMapReturnCommandHandler>();
            For<IX2EngineNodeHost>().Singleton().Use<X2EngineNodeHost>().Named("X2Node");
            For<IX2EngineNode>().Singleton().Use<X2EngineNode>();
            For<IX2Logging>().Singleton().Use<X2Logging>();
            For<IMessageProcessor<IX2Request>>().Use<X2NodeRequestMessageProcessor>();
            For<IMessageProcessorProvider>().Singleton().Use<X2NodeRequestProcessorProvider>();

            For<IMessageRouteNameBuilder>().Singleton().Use<DefaultRouteNameBuilder<X2RouteEndpoint>>();
            // Common
            For<IEasyNetQMessageBusSettings>().Singleton().Use<DefaultEasyNetQMessageBusSettings>();
            For<ISerializationProvider>().Singleton().Use<JsonSerializationProvider>();
            For<IMessageBusAdvanced>().Singleton().Add<MessageBus>();
            For<IX2NodeManagementMessageProcessor>().Singleton().Add<X2NodeManagementMessageProcessor>();
            For<IX2NodeManagementSubscriber>().Singleton().Add<EasyNetQNodeManagementSubscriber>();

            For<IStartableService>().Use((context) => context.GetInstance<IX2EngineNodeHost>("X2Node"));
            For<IStoppableService>().Use((context) => context.GetInstance<IX2EngineNodeHost>("X2Node"));
        }
    }
}