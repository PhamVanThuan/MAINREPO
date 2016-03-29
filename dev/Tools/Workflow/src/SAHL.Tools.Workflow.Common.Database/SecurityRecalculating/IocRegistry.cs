using EasyNetQ;
using SAHL.Config.Services.Core.Conventions;
using SAHL.Core.Caching;
using SAHL.Core.Configuration;
using SAHL.Core.IoC;
using SAHL.Core.Messaging;
using SAHL.Core.Messaging.EasyNetQ;
using SAHL.Core.Messaging.RabbitMQ;
using SAHL.Core.Services;
using SAHL.Core.X2.AppDomain;
using SAHL.Core.X2.Factories;
using SAHL.Core.X2.Logging;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Communication.EasyNetQ;
using SAHL.X2Engine2.Communication.RabbitMQ;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Node;
using SAHL.X2Engine2.Node.AppDomain;
using SAHL.X2Engine2.Providers;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;
using System;
using System.IO.Abstractions;

namespace SAHL.Tools.Workflow.Common.Database.SecurityRecalculating
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            Scan(scan =>
            {
                scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                scan.WithDefaultConventions();
                scan.Convention<CommandHandlerDecoratorConvention>();
                scan.ConnectImplementationsToTypesClosing(typeof(ICacheKeyGeneratorFactory<>));
                scan.LookForRegistries();

                For<IEasyNetQMessageBusSettings>().Singleton().Use<EasyNetQMessageBusSettingsWithEmptyLogger>();
                For<IMessageBusConfigurationProvider>().Singleton().Use<MessageBusConfigurationProvider>();
                For<IMessageBusAdvanced>().Use<MessageBus>();

                //Node
                For<IX2ServiceCommandRouter>().LifecycleIs(new ThreadLocalStorageLifecycle()).Use<X2ServiceCommandRouter>();
                For<IServiceCommandHandlerProvider>().Use<StructureMapServiceHandlerProvider>();

                For<IRabbitMQConnectionFactoryFactory>().Singleton().Use<RabbitMQConnectionFactoryFactory>();
                For<IRabbitMQConnectionFactory>().Singleton().Use<RabbitMQConnectionFactory>();
                For<IQueueConsumerManager>().Singleton().Use<QueueConsumerManager>();

                For<IX2RequestSubscriber>().Singleton().Use<X2NodeRequestConsumerConfigurationProvider>();
                For<IX2ConsumerConfigurationProvider>().Use((context) => context.GetInstance<IX2RequestSubscriber>());

                For<IX2ResponseSubscriber>().Singleton().Use<X2ResponseConsumerConfigurationProvider>();
                For<IX2ConsumerConfigurationProvider>().Use((context) => context.GetInstance<IX2ResponseSubscriber>());

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
                For<ICacheKeyGenerator>().Use<CacheKeyGenerator>();
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

                For<IMessageRouteNameBuilder>().Singleton().Use<DefaultRouteNameBuilder<X2RouteEndpoint>>();
                For<IMessageProcessor<IX2Request>>().Use<X2NodeRequestMessageProcessor>();
                //Common
                For<ISerializationProvider>().Singleton().Use<JsonSerializationProvider>();

                For<IStartableService>().Use((context) => context.GetInstance<IX2EngineNodeHost>("X2Node"));
                For<IStoppableService>().Use((context) => context.GetInstance<IX2EngineNodeHost>("X2Node"));
            });
        }

        private class EasyNetQNullLogger : IEasyNetQLogger
        {
            public void DebugWrite(string format, params object[] args)
            {
            }

            public void ErrorWrite(Exception exception)
            {
            }

            public void ErrorWrite(string format, params object[] args)
            {
            }

            public void InfoWrite(string format, params object[] args)
            {
            }
        }

        private class EasyNetQMessageBusSettingsWithEmptyLogger : IEasyNetQMessageBusSettings
        {
            public void RegisterServices(IServiceRegister serviceRegister)
            {
                serviceRegister.Register<ISerializer>(y => { return new SAHL.Core.Messaging.EasyNetQ.JsonSerializer(); });
                serviceRegister.Register<IEasyNetQLogger>(y => { return new EasyNetQNullLogger(); });
            }
        }
    }
}