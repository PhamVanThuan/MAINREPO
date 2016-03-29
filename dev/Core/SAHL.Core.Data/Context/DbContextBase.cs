using System.Transactions;

namespace SAHL.Core.Data.Context
{
    public abstract class DbContextBase
    {
        private readonly TransactionScope transactionScope;
        private readonly string connectionContextName;
        private readonly IDbConnectionProviderStorage connectionProviderStorage;

        protected DbContextBase(IDbConnectionProviderStorage connectionProviderStorage, IDbConnectionProviderFactory connectionProviderFactory,
                                ISqlRepository repository, string connectionContextName, TransactionScopeTypeEnum scopeType)
            : this(connectionProviderStorage, connectionProviderFactory, repository, connectionContextName, scopeType, System.Transactions.IsolationLevel.ReadCommitted)
        {
        }

        protected DbContextBase(IDbConnectionProviderStorage connectionProviderStorage, IDbConnectionProviderFactory connectionProviderFactory,
                                ISqlRepository repository, string connectionContextName, TransactionScopeTypeEnum scopeType, System.Transactions.IsolationLevel isolationLevel)
        {
            this.connectionContextName     = connectionContextName;
            this.connectionProviderStorage = connectionProviderStorage;

            switch (scopeType)
            {
                case TransactionScopeTypeEnum.None:
                    this.transactionScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = isolationLevel });
                    break;

                case TransactionScopeTypeEnum.Inherited:
                    this.transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = isolationLevel });
                    break;

                case TransactionScopeTypeEnum.New:
                    this.transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = isolationLevel });
                    break;

                default:
                    //use default scope that was provided - no action
                    break;
            }

            if (!this.connectionProviderStorage.HasConnectionProvider)
            {
                this.connectionProviderStorage.RegisterConnectionProvider(connectionProviderFactory.GetNewConnectionProvider());
            }

            var connection = this.connectionProviderStorage.ConnectionProvider.RegisterContext(connectionContextName);

            repository.UseConnection(connection);
        }

        public virtual void Dispose()
        {
            this.connectionProviderStorage.ConnectionProvider.UnRegisterContext(this.connectionContextName);

            if (this.transactionScope != null)
            {
                this.transactionScope.Dispose();
            }
        }

        public virtual void Complete()
        {
            this.transactionScope.Complete();
        }
    }
}