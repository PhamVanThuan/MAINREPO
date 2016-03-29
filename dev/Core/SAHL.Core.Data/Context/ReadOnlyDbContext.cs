using System.Collections.Generic;
using System.Transactions;

namespace SAHL.Core.Data.Context
{
    public class ReadOnlyDbContext : DbContextBase, IReadOnlyDbContext
    {
        private IReadOnlySqlRepository readOnlySqlRepository;

        internal ReadOnlyDbContext(IDbConnectionProviderStorage connectionProviderStorage, IDbConnectionProviderFactory connectionProviderFactory,
                                   IReadOnlySqlRepository readOnlySqlRepository, string connectionContextName, TransactionScopeTypeEnum scopeType)
            : base(connectionProviderStorage, connectionProviderFactory, readOnlySqlRepository, connectionContextName, scopeType)
        {
            this.readOnlySqlRepository = readOnlySqlRepository;
        }

        internal ReadOnlyDbContext(IDbConnectionProviderStorage connectionProviderStorage, IDbConnectionProviderFactory connectionProviderFactory,
                                   IReadOnlySqlRepository readOnlySqlRepository, string connectionContextName, TransactionScopeTypeEnum scopeType, IsolationLevel isolationLevel)
            : base(connectionProviderStorage, connectionProviderFactory, readOnlySqlRepository, connectionContextName, scopeType, isolationLevel)
        {
            this.readOnlySqlRepository = readOnlySqlRepository;
        }

        public T GetByKey<T, K>(K key)
        {
            return this.readOnlySqlRepository.GetByKey<T, K>(key);
        }

        public IEnumerable<T> Select<T>(string sqlQuery, object param = null)
        {
            return this.readOnlySqlRepository.Select<T>(sqlQuery, param);
        }

        public IEnumerable<T> Select<T>(ISqlStatement<T> sqlStatmentObject)
        {
            return this.readOnlySqlRepository.Select<T>(sqlStatmentObject);
        }

        public T SelectOne<T>(string sqlQuery, object param = null)
        {
            return this.readOnlySqlRepository.SelectOne<T>(sqlQuery, param);
        }

        public T SelectOne<T>(ISqlStatement<T> sqlStatmentObject)
        {
            return this.readOnlySqlRepository.SelectOne<T>(sqlStatmentObject);
        }

        public virtual void UseConnection(System.Data.IDbConnection dbConnection)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> SelectWhere<T>(string whereClause, object param = null)
        {
            return this.readOnlySqlRepository.SelectWhere<T>(whereClause, param);
        }

        public T SelectOneWhere<T>(string whereClause, object param = null)
        {
            return this.readOnlySqlRepository.SelectOneWhere<T>(whereClause, param);
        }

        public void SelectOneInto<T>(string sqlQuery, T instanceToPopulate, object param = null)
        {
            this.readOnlySqlRepository.SelectOneInto<T>(sqlQuery, instanceToPopulate, param);
        }

        public void SelectOneInto<T>(ISqlStatement<T> sqlStatementObject, T instanceToPopulate)
        {
            this.readOnlySqlRepository.SelectOneInto<T>(sqlStatementObject, instanceToPopulate);
        }
    }
}