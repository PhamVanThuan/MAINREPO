using SAHL.Core.Data.Context;

namespace SAHL.Core.Data
{
    public class DbFactory : IDbFactory
    {
        private IDbConnectionProviderFactory dbConnectionProviderFactory;
        private IDbConnectionProviderStorage dbConnectionProviderStorage;
        private ISqlRepositoryFactory dbSqlRepositoryFactory;

        public DbFactory(ISqlRepositoryFactory dbSqlRepositoryFactory, IDbConnectionProviderStorage dbConnectionProviderStorage, IDbConnectionProviderFactory dbConnectionProviderFactory)
        {
            this.dbConnectionProviderFactory = dbConnectionProviderFactory;
            this.dbConnectionProviderStorage = dbConnectionProviderStorage;
            this.dbSqlRepositoryFactory = dbSqlRepositoryFactory;
        }

        public IDb NewDb()
        {
            return new Db(this.dbSqlRepositoryFactory, this.dbConnectionProviderStorage, this.dbConnectionProviderFactory);
        }
    }
}