using SAHL.Core.Data;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Dapper;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Core.Logging.Loggers.Database;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.FileConventions;
using SAHL.Core.Validation;
using StructureMap;
using StructureMap.Configuration.DSL;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SAHL.Core.Testing.Ioc.Registration;

namespace SAHL.Core.Testing.Ioc
{
    public class TestingRegistry : Registry
    {
        public TestingRegistry()
        {
            //Dapper
            For<IUIStatementProvider>().Singleton().Use<AssemblyUIStatementProvider>();
            For<IUIStatementProvider>().Use<DapperUIStatementProvider>();
            For<ILoggerSource>().Singleton().Use(new LoggerSource("SqlRepositoryFactory", LogLevel.Error, true)).Named("SqlRepositoryFactoryLogSource");
            For<IDbConfigurationProvider>().Use<DefaultDbConfigurationProvider>();
            For<IDbConnectionProviderStorage>().Singleton().Use<DefaultDbConnectionProviderStorage>();
            For<IDbConnectionProviderFactory>().Use<DefaultDbConnectionProviderFactory>();
            For<IDbFactory>().Use<DbFactory>();

            // Dapper Repository
            For<IReadOnlySqlRepository>().Use<DapperRepository>()
                .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("SqlRepositoryFactoryLogSource"));
            For<IReadWriteSqlRepository>().Use<DapperRepository>()
                .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("SqlRepositoryFactoryLogSource"));
            For<ISqlRepositoryFactory>().Use<DapperSqlRepositoryFactory>()
                .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("SqlRepositoryFactoryLogSource"));
            For<IStartable>().Use<SqlRepositoryFactoryConfig>();

            //Core
            For<ITypeMetaDataLookup>().Singleton().Use<TypeMetaDataLookup>();

            //Logging
            For<IRawLogger>().Singleton().Use<DatabaseLogger>();
            For<IRawMetricsLogger>().Singleton().Use<DatabaseMetricsLogger>();
            For<ILoggerSource>().Singleton().Use(new LoggerSource("DefaultLogSource", LogLevel.Error, false));
            For<ILoggerSource>().Singleton().Use(new LoggerSource("AppStartUp", LogLevel.Error, true)).Named("AppStartUpLogSource");
            For<ILoggerAppSource>().Use<LoggerAppSourceFromConfiguration>();

            //Old way (specs still use it)
            var uiStatementProvider = ObjectFactory.GetInstance<AssemblyUIStatementProvider>();
            var testLoggerAppSource = new LoggerAppSourceFromImplicitSource("TestLoggerAppSource");
            DbContextConfiguration.Instance.RepositoryFactory = new DapperSqlRepositoryFactory(uiStatementProvider,
                new Logger(new NullRawLogger(), new NullMetricsRawLogger(), testLoggerAppSource, new MetricTimerFactory()),
                new LoggerSource("StatementTests", LogLevel.Error, false));
        }
    }

    public class SqlRepositoryFactoryConfig : IStartable
    {
        public void Start()
        {
            DbContextConfiguration.Instance.RepositoryFactory = ObjectFactory.GetInstance<ISqlRepositoryFactory>();
        }
    }
}
