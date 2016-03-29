namespace SAHL.Core.Data.Context.Builders
{
    public class TransactionScopeReadOnlyDbContextBuilder
    {
        private readonly TransactionScopeTypeEnum scopeType;
        private readonly System.Transactions.IsolationLevel isolationLevel;
        private readonly ISqlRepositoryFactory sqlRepositoryFactory;
        private readonly IDbConnectionProviderFactory dbConnectionProviderFactory;
        private readonly IDbConnectionProviderStorage dbConnectionProviderStorage;

        public TransactionScopeReadOnlyDbContextBuilder(ISqlRepositoryFactory sqlRepositoryFactory, IDbConnectionProviderFactory dbConnectionProviderFactory, 
                                                        IDbConnectionProviderStorage dbConnectionProviderStorage, TransactionScopeTypeEnum scopeType, 
                                                        System.Transactions.IsolationLevel isolationLevel)
        {
            this.scopeType                   = scopeType;
            this.isolationLevel              = isolationLevel;
            this.sqlRepositoryFactory        = sqlRepositoryFactory;
            this.dbConnectionProviderFactory = dbConnectionProviderFactory;
            this.dbConnectionProviderStorage = dbConnectionProviderStorage;
        }

        public IReadOnlyDbContext InAppContext()
        {
            return new DbContextBuilder(this.sqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage)
                .BuildReadOnlyContext(Strings.DBCONTEXT_APP, this.scopeType, this.isolationLevel);
        }

        public IReadOnlyDbContext InWorkflowContext()
        {
            return new DbContextBuilder(this.sqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage)
                .BuildReadOnlyContext(Strings.DBCONTEXT_WORKFLOW, this.scopeType, this.isolationLevel);
        }
    }
}
