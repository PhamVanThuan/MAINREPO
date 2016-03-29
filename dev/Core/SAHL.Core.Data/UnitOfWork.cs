using SAHL.Core.Data.Context;
using System;
using System.Transactions;

namespace SAHL.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Complete();
    }

    internal class UnitOfWork : IUnitOfWork
    {
        private TransactionScope _scope;
        private IDbConnectionProviderStorage _dbConnectionProviderStorage;
        private string contextName;

        public UnitOfWork(IDbConnectionProviderStorage dbConnectionProviderStorage)
        {
            if (dbConnectionProviderStorage == null) { throw new ArgumentNullException("dbConnectionProviderStorage"); }
            if (!dbConnectionProviderStorage.HasConnectionProvider)
            {
                throw new ArgumentException("DbConnectionProviderStorage does not contain a DbConnectionProvider");
            }

            var transactionOptions = new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                };

            _scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions);
            _dbConnectionProviderStorage = dbConnectionProviderStorage ?? new DefaultDbConnectionProviderStorage();

            this.RegisterContext();
        }

        public void Complete()
        {
            if (_scope == null) { return; }
            // can't complete if the current scope is not active
            if (System.Transactions.Transaction.Current != null && System.Transactions.Transaction.Current.TransactionInformation.Status == TransactionStatus.Active)
            {
                _scope.Complete();
            }
        }

        public void Dispose()
        {
            this.UnRegisterContext();

            if (_scope == null) { return; }
            _scope.Dispose();
        }

        private void RegisterContext()
        {
            contextName = Guid.NewGuid().ToString();
            _dbConnectionProviderStorage.ConnectionProvider.RegisterUnitOfWorkContext(contextName);
        }

        private void UnRegisterContext()
        {
            if (_dbConnectionProviderStorage == null) { return; }
            _dbConnectionProviderStorage.ConnectionProvider.UnregisterUnitOfWorkContext(contextName);
        }

        private string GetDbConnectionContextName()
        {
            return Strings.DBCONTEXT_APP;
        }
    }
}