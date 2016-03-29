namespace SAHL.Core.Data.Context.Builders
{
    public class TransactionScopeDbContextBuilder : ITransactionScopeDbContextBuilder
    {
        private readonly TransactionScopeTypeEnum scopeType;
        private readonly System.Transactions.IsolationLevel isolationLevel;
        private readonly ISqlRepositoryFactory sqlRepositoryFactory;
        private readonly IDbConnectionProviderFactory dbConnectionProviderFactory;
        private readonly IDbConnectionProviderStorage dbConnectionProviderStorage;

        public TransactionScopeDbContextBuilder(ISqlRepositoryFactory sqlRepositoryFactory, IDbConnectionProviderFactory dbConnectionProviderFactory, 
                                                IDbConnectionProviderStorage dbConnectionProviderStorage, TransactionScopeTypeEnum scopeType, 
                                                System.Transactions.IsolationLevel isolationLevel)
        {
            this.scopeType                   = scopeType;
            this.isolationLevel              = isolationLevel;
            this.sqlRepositoryFactory        = sqlRepositoryFactory;
            this.dbConnectionProviderFactory = dbConnectionProviderFactory;
            this.dbConnectionProviderStorage = dbConnectionProviderStorage;
        }

        public virtual IDbContext InAppContext()
        {
            return new DbContextBuilder(this.sqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage)
                .BuildContext(Strings.DBCONTEXT_APP, this.scopeType, this.isolationLevel);
        }

        public virtual IDbContext InWorkflowContext()
        {
            return new DbContextBuilder(this.sqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage)
                .BuildContext(Strings.DBCONTEXT_WORKFLOW, this.scopeType, this.isolationLevel);
        }
    }
}
