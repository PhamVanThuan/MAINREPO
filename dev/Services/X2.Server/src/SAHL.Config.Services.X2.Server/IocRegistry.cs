using EasyNetQ;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SAHL.Core.Caching;
using SAHL.Core.Data;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Dapper;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Core.Messaging;
using SAHL.Core.Messaging.EasyNetQ;
using SAHL.Core.Messaging.RabbitMQ;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Core.X2.Logging;
using SAHL.Core.X2.Messages.Management;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Communication.EasyNetQ;
using SAHL.X2Engine2.Communication.RabbitMQ;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Hosts;
using SAHL.X2Engine2.MessageHandlers;
using SAHL.X2Engine2.Node.Providers;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.Services;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace SAHL.Config.Services.X2.Server
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

            For<IX2ServiceCommandRouter>().LifecycleIs(new ThreadLocalStorageLifecycle()).Use<X2ServiceCommandRouter>();
            For<IServiceCommandHandlerProvider>().Use<StructureMapServiceHandlerProvider>();
            For<IWorkflowDataProvider>().Use<WorkflowDataProvider>();

            For<IX2ProcessProvider>().Singleton().Use<X2ProcessProvider>();
            For<IX2NodeConfigurationProvider>().Singleton().Use<X2NodeConfigurationProvider>();

            For<IRabbitMQConnectionFactory>().Singleton().Use<RabbitMQConnectionFactory>();
            For<IRabbitMQConnectionFactoryFactory>().Singleton().Use<RabbitMQConnectionFactoryFactory>();
            For<IQueueConnectionFactory>().Singleton().Use<QueueConnectionFactory>();
            For<IQueueConsumerManager>().Singleton().Use<QueueConsumerManager>();
            For<IQueuePublisher>().Singleton().Use<QueuePublisher>();
            
            For<IQueuePublisher>().Singleton().Use<QueuePublisher>();
            For<IX2RequestPublisher>().Singleton().Use<RabbitMQRequestPublisher>();
            For<IMessageBusAdvanced>().Singleton().Add<MessageBus>();
            For<IX2EngineConfigurationProvider>().Singleton().Use<X2EngineConfigurationProvider>();
            For<IX2NodeManagementPublisher>().Singleton().Add<EasyNetQNodeManagementPublisher>();
            
            For<IX2EngineHost>().Singleton().Use<X2EngineHost>().Named("X2Engine");
            For<ITimeoutServiceFactory>().Singleton().Use<TimeoutServiceFactory>();
            For<IX2RequestRouter>().Singleton().Use<X2RequestRouter>();
            For<IX2RequestStore>().Singleton().Use<X2RequestStore>();
            For<IX2RequestInterrogator>().Singleton().Use<X2RequestInterrogator>();
            For<IX2RoutePlanner>().Singleton().Use<X2RoutePlanner>();
            For<ITimerFactory>().Singleton().Use<TimerFactory>();
            For<IMessageBusManagementClient>().Singleton().Use<EasyNetQManagementClient>();
            For<IX2ConsumerMonitor>().Singleton().Use<X2ConsumerMonitor>();
            For<IX2ResponseSubscriber>().Singleton().Use<X2ResponseConsumerConfigurationProvider>();
            For<IX2ManagementSubscriber>().Singleton().Use<X2ManagementConsumerConfigurationProvider>();

            For<IX2ConsumerConfigurationProvider>().Use((context) => context.GetInstance<IX2ResponseSubscriber>());
            For<IX2ConsumerConfigurationProvider>().Use((context) => context.GetInstance<IX2ManagementSubscriber>());

            For<IX2ConsumerManager>().Singleton().Use<RabbitMQConsumerManager>();
            For<IX2RequestMonitorFactory>().Singleton().Use<X2RequestMonitorFactory>();
            For<IResponseThreadWaiterManager>().Singleton().Use<ResponseThreadWaiterManager>();

    		For<IX2Logging>().Singleton().Use<X2Logging>();

            For<IX2ResponseFactory>().Singleton().Use<X2ResponseFactory>();
            For<ISerializationProvider>().Singleton().Use<JsonSerializationProvider>();
            For<IX2Engine>().Singleton().Use<X2Engine>();
            For<IX2EngineHost>().Singleton().Use<X2EngineHost>();
            For<IMessageRouteNameBuilder>().Singleton().Use<DefaultRouteNameBuilder<X2RouteEndpoint>>();

            // Common
            For<ICache>().Singleton().Use<InMemoryCache>();
            For<IEasyNetQMessageBusSettings>().Singleton().Use<DefaultEasyNetQMessageBusSettings>();

            For<IStartableService>().Use((context) => context.GetInstance<IX2EngineHost>());
            For<IStoppableService>().Use((context) => context.GetInstance<IX2EngineHost>());
            
            For<IMessageBusAdvanced>().Singleton().Use<MessageBus>().Named("HealthMessageBus");
            For<IMessageBusAdvanced>().Singleton().Use<MessageBus>();
        }
    }
}

