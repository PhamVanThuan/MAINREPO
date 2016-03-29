namespace SAHL.Core.Data.Context.Builders
{
    public class ReadWriteDbContextBuilder : SAHL.Core.Data.Context.Builders.IReadWriteDbContextBuilder
    {
        private readonly ISqlRepositoryFactory sqlRepositoryFactory;
        private readonly IDbConnectionProviderFactory dbConnectionProviderFactory;
        private readonly IDbConnectionProviderStorage dbConnectionProviderStorage;

        public ReadWriteDbContextBuilder(ISqlRepositoryFactory sqlRepositoryFactory, IDbConnectionProviderFactory dbConnectionProviderFactory, 
                                         IDbConnectionProviderStorage dbConnectionProviderStorage)
        {
            this.sqlRepositoryFactory        = sqlRepositoryFactory;
            this.dbConnectionProviderFactory = dbConnectionProviderFactory;
            this.dbConnectionProviderStorage = dbConnectionProviderStorage;
        }

        public ITransactionScopeDbContextBuilder WithNoTransactionScope()
        {
            return new TransactionScopeDbContextBuilder(this.sqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage, 
                                                        TransactionScopeTypeEnum.None, System.Transactions.IsolationLevel.ReadCommitted);
        }

        public ITransactionScopeDbContextBuilder WithNoTransactionScope(System.Transactions.IsolationLevel isolationLevel)
        {
            return new TransactionScopeDbContextBuilder(this.sqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage, 
                                                        TransactionScopeTypeEnum.None, isolationLevel);
        }

        public ITransactionScopeDbContextBuilder WithInheritedTransactionScope()
        {
            return new TransactionScopeDbContextBuilder(this.sqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage, 
                                                        TransactionScopeTypeEnum.Inherited, System.Transactions.IsolationLevel.ReadCommitted);
        }

        public ITransactionScopeDbContextBuilder WithInheritedTransactionScope(System.Transactions.IsolationLevel isolationLevel)
        {
            return new TransactionScopeDbContextBuilder(this.sqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage, 
                                                        TransactionScopeTypeEnum.Inherited, isolationLevel);
        }

        public ITransactionScopeDbContextBuilder WithNewTransactionScope()
        {
            return new TransactionScopeDbContextBuilder(this.sqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage, 
                                                        TransactionScopeTypeEnum.New, System.Transactions.IsolationLevel.ReadCommitted);
        }

        public ITransactionScopeDbContextBuilder WithNewTransactionScope(System.Transactions.IsolationLevel isolationLevel)
        {
            return new TransactionScopeDbContextBuilder(this.sqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage, 
                                                        TransactionScopeTypeEnum.New, isolationLevel);
        }
    }
}