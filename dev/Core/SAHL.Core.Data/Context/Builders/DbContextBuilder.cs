using System.Transactions;

namespace SAHL.Core.Data.Context.Builders
{
    public class DbContextBuilder
    {
        private readonly ISqlRepositoryFactory sqlRepositoryFactory;
        private readonly IDbConnectionProviderFactory dbConnectionProviderFactory;
        private readonly IDbConnectionProviderStorage dbconnectionProviderStorage;

        public DbContextBuilder(ISqlRepositoryFactory sqlRepositoryFactory, IDbConnectionProviderFactory dbConnectionProviderFactory, IDbConnectionProviderStorage dbConnectionProviderStorage)
        {
            this.sqlRepositoryFactory        = sqlRepositoryFactory;
            this.dbConnectionProviderFactory = dbConnectionProviderFactory;
            this.dbconnectionProviderStorage = dbConnectionProviderStorage;
        }

        public IDbContext BuildContext(string connectionContextName, TransactionScopeTypeEnum scopeType, IsolationLevel isolationLevel)
        {
            return new DbContext(this.dbconnectionProviderStorage, this.dbConnectionProviderFactory, this.sqlRepositoryFactory.GetNewReadWriteRepository(), 
                                 connectionContextName, scopeType, isolationLevel);
        }

        public IReadOnlyDbContext BuildReadOnlyContext(string connectionContextName, TransactionScopeTypeEnum scopeType, IsolationLevel isolationLevel)
        {
            return new ReadOnlyDbContext(this.dbconnectionProviderStorage, this.dbConnectionProviderFactory, this.sqlRepositoryFactory.GetNewReadOnlyRepository(), 
                                         connectionContextName, scopeType, isolationLevel);
        }
    }
}
