using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Core.Testing.Providers
{
    public class SubstituteRepositoryProvider
    {
        public static IReadOnlySqlRepository GetReadOnlyRepository()
        {
            var sqlRepositoryFactory = Substitute.For<ISqlRepositoryFactory>();
            var readonlyRepository = Substitute.For<IReadOnlySqlRepository>();

            DbContextConfiguration.Instance.RepositoryFactory = sqlRepositoryFactory;
            DbContextConfiguration.Instance.DbConfigurationProvider = Substitute.For<IDbConfigurationProvider>();
            DbContextConfiguration.Instance.DbConnectionProviderFactory = Substitute.For<IDbConnectionProviderFactory>();
            DbContextConfiguration.Instance.DbConnectionProviderStorage = Substitute.For<IDbConnectionProviderStorage>();

            sqlRepositoryFactory.GetNewReadOnlyRepository().Returns(readonlyRepository);
            return readonlyRepository;
        }

        public static IReadWriteSqlRepository GetReadWriteRepository()
        {
            var sqlRepositoryFactory = Substitute.For<ISqlRepositoryFactory>();
            var readWriteRepository = Substitute.For<IReadWriteSqlRepository>();

            DbContextConfiguration.Instance.RepositoryFactory = sqlRepositoryFactory;
            DbContextConfiguration.Instance.DbConfigurationProvider = Substitute.For<IDbConfigurationProvider>();
            DbContextConfiguration.Instance.DbConnectionProviderFactory = Substitute.For<IDbConnectionProviderFactory>();
            DbContextConfiguration.Instance.DbConnectionProviderStorage = Substitute.For<IDbConnectionProviderStorage>();

            sqlRepositoryFactory.GetNewReadWriteRepository().Returns(readWriteRepository);
            return readWriteRepository;
        }
    }
}