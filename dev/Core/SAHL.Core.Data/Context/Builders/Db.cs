using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Context.Builders;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Core.Data
{
    public class Db : IDb
    {
        private readonly IDbConnectionProviderFactory dbConnectionProviderFactory;
        private readonly IDbConnectionProviderStorage dbConnectionProviderStorage;
        private readonly ISqlRepositoryFactory dbSqlRepositoryFactory;

        public Db(ISqlRepositoryFactory dbSqlRepositoryFactory, IDbConnectionProviderStorage dbConnectionProviderStorage, IDbConnectionProviderFactory dbConnectionProviderFactory)
        {
            this.dbConnectionProviderFactory = dbConnectionProviderFactory;
            this.dbConnectionProviderStorage = dbConnectionProviderStorage;
            this.dbSqlRepositoryFactory = dbSqlRepositoryFactory;
        }

        public Db()
        {
            this.dbConnectionProviderFactory = DbContextConfiguration.Instance.DbConnectionProviderFactory ?? 
                                                            new DefaultDbConnectionProviderFactory(DbContextConfiguration.Instance.DbConfigurationProvider ?? 
                                                            new DefaultDbConfigurationProvider());
            this.dbConnectionProviderStorage = DbContextConfiguration.Instance.DbConnectionProviderStorage ?? new DefaultDbConnectionProviderStorage();
            this.dbSqlRepositoryFactory = DbContextConfiguration.Instance.RepositoryFactory;
        }

        public IReadOnlyDbContextBuilder ForReadOnly()
        {
            return new ReadOnlyDbContextBuilder(this.dbSqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage);
        }

        public IReadWriteDbContextBuilder ForReadWrite()
        {
            return new ReadWriteDbContextBuilder(this.dbSqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage);
        }

        public IDbContext InAppContext()
        {
            return new DbContextBuilder(this.dbSqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage)
                .BuildContext(Strings.DBCONTEXT_APP, TransactionScopeTypeEnum.Inherited, System.Transactions.IsolationLevel.ReadCommitted);
        }

        public IReadOnlyDbContext InReadOnlyAppContext()
        {
            return new ReadOnlyDbContext(this.dbConnectionProviderStorage,
                                         this.dbConnectionProviderFactory,
                                         this.dbSqlRepositoryFactory.GetNewReadOnlyRepository(),
                                         Strings.DBCONTEXT_APP,
                                         TransactionScopeTypeEnum.None,
                                         System.Transactions.IsolationLevel.ReadCommitted);
        }

        public IDbContext InWorkflowContext()
        {
            return new DbContextBuilder(this.dbSqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage)
                .BuildContext(Strings.DBCONTEXT_WORKFLOW, TransactionScopeTypeEnum.Inherited, System.Transactions.IsolationLevel.ReadCommitted);
        }

        public IReadOnlyDbContext InReadOnlyWorkflowContext()
        {
            return new ReadOnlyDbContext(this.dbConnectionProviderStorage, this.dbConnectionProviderFactory, this.dbSqlRepositoryFactory.GetNewReadOnlyRepository(),
                Strings.DBCONTEXT_WORKFLOW, TransactionScopeTypeEnum.None, System.Transactions.IsolationLevel.ReadCommitted);
        }

        public IDdlDbContext DDLInAppContext()
        {
            return new DdlDbContext(this.dbSqlRepositoryFactory.GetNewDdlRepository(),
                                    this.dbConnectionProviderStorage,
                                    this.dbConnectionProviderFactory,
                                    Strings.DBCONTEXT_APP);
        }

        public IDdlDbContext DDLInWorkflowContext()
        {
            return new DdlDbContext(this.dbSqlRepositoryFactory.GetNewDdlRepository(),
                                    this.dbConnectionProviderStorage,
                                    this.dbConnectionProviderFactory,
                                    Strings.DBCONTEXT_WORKFLOW);
        }

        public ITransactionScopeDbContextBuilder WithNoTransactionScope()
        {
            return new TransactionScopeDbContextBuilder(this.dbSqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage, 
                                                        TransactionScopeTypeEnum.None, System.Transactions.IsolationLevel.ReadCommitted);
        }

        public ITransactionScopeDbContextBuilder WithNoTransactionScope(System.Transactions.IsolationLevel isolationLevel)
        {
            return new TransactionScopeDbContextBuilder(this.dbSqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage, 
                                                        TransactionScopeTypeEnum.None, isolationLevel);
        }

        public ITransactionScopeDbContextBuilder WithInheritedTransactionScope()
        {
            return new TransactionScopeDbContextBuilder(this.dbSqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage, 
                                                        TransactionScopeTypeEnum.Inherited, System.Transactions.IsolationLevel.ReadCommitted);
        }

        public ITransactionScopeDbContextBuilder WithInheritedTransactionScope(System.Transactions.IsolationLevel isolationLevel)
        {
            return new TransactionScopeDbContextBuilder(this.dbSqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage, 
                                                        TransactionScopeTypeEnum.Inherited, isolationLevel);
        }

        public ITransactionScopeDbContextBuilder WithNewTransactionScope()
        {
            return new TransactionScopeDbContextBuilder(this.dbSqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage, 
                                                        TransactionScopeTypeEnum.New, System.Transactions.IsolationLevel.ReadCommitted);
        }

        public ITransactionScopeDbContextBuilder WithNewTransactionScope(System.Transactions.IsolationLevel isolationLevel)
        {
            return new TransactionScopeDbContextBuilder(this.dbSqlRepositoryFactory, this.dbConnectionProviderFactory, this.dbConnectionProviderStorage, 
                                                        TransactionScopeTypeEnum.New, isolationLevel);
        }
    }
}