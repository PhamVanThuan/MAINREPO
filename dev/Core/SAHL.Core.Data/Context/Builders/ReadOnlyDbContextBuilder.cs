namespace SAHL.Core.Data.Context.Builders
{
    public class ReadOnlyDbContextBuilder : SAHL.Core.Data.Context.Builders.IReadOnlyDbContextBuilder
    {
        private ISqlRepositoryFactory sqlRepositoryFactory;
        private IDbConnectionProviderFactory dbConnectionProviderFactory;
        private IDbConnectionProviderStorage dbConnectionProviderStorage;

        public ReadOnlyDbContextBuilder(ISqlRepositoryFactory sqlRepositoryFactory, IDbConnectionProviderFactory dbConnectionProviderFactory, IDbConnectionProviderStorage dbConnectionProviderStorage)
        {
            this.sqlRepositoryFactory = sqlRepositoryFactory;
            this.dbConnectionProviderFactory = dbConnectionProviderFactory;
            this.dbConnectionProviderStorage = dbConnectionProviderStorage;
        }

        public IReadOnlyDbContext InAppContext()
        {
            return new DbContextBuilder(this.sqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage)
                .BuildReadOnlyContext(Strings.DBCONTEXT_APP, TransactionScopeTypeEnum.None, System.Transactions.IsolationLevel.ReadCommitted);
        }

        public IReadOnlyDbContext InWorkflowContext()
        {
            return new DbContextBuilder(this.sqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage)
                .BuildReadOnlyContext(Strings.DBCONTEXT_WORKFLOW, TransactionScopeTypeEnum.None, System.Transactions.IsolationLevel.ReadCommitted);
        }
    }
}