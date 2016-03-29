using System.Threading;

namespace SAHL.Core.Data.Context
{
    public class DefaultDbConnectionProviderStorage : IDbConnectionProviderStorage
    {
        private ThreadLocal<IDbConnectionProvider> dbConnectionProvider = new ThreadLocal<IDbConnectionProvider>();
        private ReaderWriterLockSlim readerWriterLock;

        public DefaultDbConnectionProviderStorage()
        {
            this.readerWriterLock = new ReaderWriterLockSlim();
        }

        public bool HasConnectionProvider
        {
            get
            {
                try
                {
                    this.readerWriterLock.EnterReadLock();
                    return this.dbConnectionProvider.Value != null;
                }
                finally
                {
                    this.readerWriterLock.ExitReadLock();
                }
            }
        }

        public IDbConnectionProvider ConnectionProvider
        {
            get
            {
                return this.dbConnectionProvider.Value;
            }
        }

        public void RegisterConnectionProvider(IDbConnectionProvider connectionProvider)
        {
            try
            {
                this.readerWriterLock.EnterWriteLock();
                this.dbConnectionProvider.Value = connectionProvider;
            }
            finally
            {
                this.readerWriterLock.ExitWriteLock();
            }
        }

        public void UnRegisterConnectionProvider()
        {
            try
            {
                this.readerWriterLock.EnterWriteLock();
                this.dbConnectionProvider.Value = null;
            }
            finally
            {
                this.readerWriterLock.ExitWriteLock();
            }
        }
    }
}