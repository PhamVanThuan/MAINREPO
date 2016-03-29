using System.IO.Abstractions;
using SAHL.Core.Caching;
using SAHL.Core.Configuration;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Core.Logging.Loggers.Database;
using SAHL.Core.Messaging;
using SAHL.Core.Messaging.EasyNetQ;
using SAHL.Core.Services;
using SAHL.Core.X2.AppDomain;
using SAHL.Core.X2.Factories;
using SAHL.Core.X2.Logging;
using SAHL.Core.X2.Messages;
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

namespace SAHL.X2Engine2.ServiceHost.Node
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                x.WithDefaultConventions();
                //x.ConnectImplementationsToTypesClosing(typeof(ICacheKeyGeneratorFactory<>));
                x.LookForRegistries();
            });

            // Node
            For<IX2ServiceCommandRouter>().LifecycleIs(new ThreadLocalStorageLifecycle()).Use<X2ServiceCommandRouter>();
            For<IServiceCommandHandlerProvider>().Use<StructureMapServiceHandlerProvider>();

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

            For<IMessageProcessorProvider>().Singleton().Use<X2NodeRequestProcessorProvider>();

            For<IEasyNetQMessageBusSettings>().Singleton().Use<DefaultEasyNetQMessageBusSettings2>();

            For<IMessageBusConfigurationProvider>().Singleton().Use<MessageBusConfigurationProvider>();

            For<IMessageRouteNameBuilder>().Singleton().Use<DefaultRouteNameBuilder<X2RouteEndpoint>>();

            For<IMessageProcessor<IX2Request>>().Use<X2NodeRequestMessageProcessor>();

            // Common
            For<ISerializationProvider>().Singleton().Use<JsonSerializationProvider>();
            For<IMessageBusAdvanced>().Singleton().Add<MessageBus>();

            For<IStartableService>().Use((context) => context.GetInstance<IX2EngineNodeHost>("X2Node"));
            For<IStoppableService>().Use((context) => context.GetInstance<IX2EngineNodeHost>("X2Node"));

            For<IRawLogger>().Singleton().Use<DatabaseLogger>();
            For<IRawMetricsLogger>().Singleton().Use<DatabaseMetricsLogger>();
        }
    }
}