using SAHL.Config.Data.Dapper;
using SAHL.Core.Data;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Dapper;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Data.Dapper
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL.Core.Data"));
                x.AddAllTypesOf<IUIStatementsProvider>();
            });

            For<IUIStatementProvider>().Singleton().Use<AssemblyUIStatementProvider>();
            For<ILoggerSource>().Singleton().Use(new LoggerSource("SqlRepositoryFactory", LogLevel.Error, true)).Named("SqlRepositoryFactoryLogSource");

            For<IDbConfigurationProvider>().Use<DefaultDbConfigurationProvider>();

            For<IDbConnectionProviderStorage>().Singleton().Use<DefaultDbConnectionProviderStorage>();

            For<IDbConnectionProviderFactory>().Use<DefaultDbConnectionProviderFactory>();

            // Dapper Repository
            For<IReadOnlySqlRepository>().Use<DapperRepository>()
                .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("SqlRepositoryFactoryLogSource"));
            For<IReadWriteSqlRepository>().Use<DapperRepository>()
                .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("SqlRepositoryFactoryLogSource"));
            For<ISqlRepositoryFactory>().Use<DapperSqlRepositoryFactory>()
                .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("SqlRepositoryFactoryLogSource"));

            For<IStartable>().Use<SqlRepositoryFactoryConfig>();
        }
    }
}