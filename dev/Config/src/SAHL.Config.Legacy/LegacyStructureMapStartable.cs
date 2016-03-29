using SAHL.Config.Core;
using SAHL.Core;
using SAHL.Core.Configuration;
using SAHL.Core.Data;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Dapper;
using SAHL.Core.Logging;
using SAHL.Core.Logging.Loggers.Database;
using SAHL.Core.Messaging;
using SAHL.Core.Messaging.EasyNetQ;
using SAHL.Core.X2.Logging;
using SAHL.Core.X2.Shared;
using StructureMap;

namespace SAHL.Config.Legacy
{
    public class LegacyStructureMapStartable : IDomainServiceStartable
    {
        public LegacyStructureMapStartable()
        {
            var processPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var processFolder = System.IO.Path.GetDirectoryName(processPath);

            ObjectFactory.Configure((configuration) =>
            {
                configuration.Scan(x =>
                {
                    x.AssembliesFromPath(processFolder, y => y.FullName.StartsWith("SAHL"));
                    x.AddAllTypesOf<IUIStatementsProvider>();
                    x.WithDefaultConventions();
                });
                configuration.For<IIocContainer>().Use<StructureMapIocContainer>();
                configuration.For<IRawLogger>().Singleton().Use<DatabaseLogger>();
                configuration.For<IRawMetricsLogger>().Singleton().Use<DatabaseMetricsLogger>();
                configuration.For<ILoggerSource>().Use(new LoggerSource("SqlRepositoryFactory", LogLevel.Error, true)).Named("SqlRepositoryFactoryLogSource");
                configuration.For<IDatabaseLoggerDataManager>().Singleton().Use<DatabaseLoggerDataManager>();
                configuration.For<IDatabaseLoggerUtils>().Singleton().Use<DatabaseLoggerUtils>();
                configuration.For<IDatabaseMetricsLoggerDataManager>().Singleton().Use<DatabaseMetricsLoggerDataManager>();
                configuration.For<IApplicationConfigurationProvider>().Singleton().Use<ApplicationConfigurationProvider>();
                configuration.For<IMetricTimerFactory>().Singleton().Use<MetricTimerFactory>();
                configuration.For<ILoggerAppSource>().Singleton().Use<LoggerAppSourceFromConfiguration>();
                configuration.For<ILogger>().Singleton().Use<Logger>();
                configuration.For<IX2Logging>().Singleton().Use<X2Logging>();
                configuration.For<IMessageBus>().Singleton().Use<MessageBus>();
                configuration.For<IMessageBusAdvanced>().Singleton().Use<MessageBus>();
                configuration.For<IMessageBusConfigurationProvider>().Singleton().Use<MessageBusConfigurationProvider>();
                configuration.For<IMessageRoute>().Use<MessageRoute>();
                configuration.For<IEasyNetQMessageBusSettings>().Singleton().Use<DefaultEasyNetQMessageBusSettings>();
                configuration.For<IMessageRouteNameBuilder>().Singleton().Use<DefaultRouteNameBuilder<IMessageRoute>>();
                configuration.For<IUIStatementProvider>().Singleton().Use<AssemblyUIStatementProvider>();

                configuration.For<IDbConfigurationProvider>().Use<DefaultDbConfigurationProvider>();

                configuration.For<IDbConnectionProviderStorage>().Singleton().Use<DefaultDbConnectionProviderStorage>();

                configuration.For<IDbConnectionProviderFactory>().Use<DefaultDbConnectionProviderFactory>();

                // Dapper Repository
                configuration.For<IReadOnlySqlRepository>().Use<DapperRepository>().Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("SqlRepositoryFactoryLogSource"));

                configuration.For<IReadWriteSqlRepository>().Use<DapperRepository>().Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("SqlRepositoryFactoryLogSource"));

                configuration.For<ISqlRepositoryFactory>().Use<DapperSqlRepositoryFactory>().Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("SqlRepositoryFactoryLogSource"));
            });

            var statementProvider = ObjectFactory.GetInstance<AssemblyUIStatementProvider>();
        }

        public void Start(string processName)
        {
            DbContextConfiguration.Instance.RepositoryFactory = ObjectFactory.GetInstance<ISqlRepositoryFactory>();
        }
    }
}
