using Machine.Fakes;
using SAHL.Core.Data;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Core.Testing.Fakes
{
    public class MockRepositoryProvider : WithFakes
    {
        public static IReadOnlySqlRepository GetReadOnlyRepository()
        {
            var sqlRepositoryFactory = An<ISqlRepositoryFactory>();
            var readonlyRepository = An<IReadOnlySqlRepository>();
            DbContextConfiguration.Instance.RepositoryFactory = sqlRepositoryFactory;
            DbContextConfiguration.Instance.DbConfigurationProvider = An<IDbConfigurationProvider>();
            DbContextConfiguration.Instance.DbConnectionProviderFactory = An<IDbConnectionProviderFactory>();
            DbContextConfiguration.Instance.DbConnectionProviderStorage = An<IDbConnectionProviderStorage>();
            sqlRepositoryFactory.WhenToldTo(x => x.GetNewReadOnlyRepository()).Return(readonlyRepository);
            return readonlyRepository;
        }

        public static IReadWriteSqlRepository GetReadWriteRepository()
        {
            var sqlRepositoryFactory = An<ISqlRepositoryFactory>();
            var readWriteRepository = An<IReadWriteSqlRepository>();
            DbContextConfiguration.Instance.RepositoryFactory = sqlRepositoryFactory;
            DbContextConfiguration.Instance.DbConfigurationProvider = An<IDbConfigurationProvider>();
            DbContextConfiguration.Instance.DbConnectionProviderFactory = An<IDbConnectionProviderFactory>();
            DbContextConfiguration.Instance.DbConnectionProviderStorage = An<IDbConnectionProviderStorage>();
            sqlRepositoryFactory.WhenToldTo(x => x.GetNewReadWriteRepository()).Return(readWriteRepository);
            return readWriteRepository;
        }
    }
}