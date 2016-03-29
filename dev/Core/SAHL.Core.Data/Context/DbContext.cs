using System.Transactions;
using System.Collections.Generic;

namespace SAHL.Core.Data.Context
{
    public class DbContext : ReadOnlyDbContext, IDbContext
    {
        private readonly IReadWriteSqlRepository readWriteSqlRepository;

        internal DbContext(IDbConnectionProviderStorage connectionProviderStorage, IDbConnectionProviderFactory connectionProviderFactory, 
                           IReadWriteSqlRepository readWriteSqlRepository, string connectionContextName, TransactionScopeTypeEnum scopeType)
            : base(connectionProviderStorage, connectionProviderFactory, readWriteSqlRepository, connectionContextName, scopeType)
        {
            this.readWriteSqlRepository = readWriteSqlRepository;
        }

        internal DbContext(IDbConnectionProviderStorage connectionProviderStorage, IDbConnectionProviderFactory connectionProviderFactory, 
                           IReadWriteSqlRepository readWriteSqlRepository, string connectionContextName, TransactionScopeTypeEnum scopeType, 
                           IsolationLevel isolationLevel)
            : base(connectionProviderStorage, connectionProviderFactory, readWriteSqlRepository, connectionContextName, scopeType, isolationLevel)
        {
            this.readWriteSqlRepository = readWriteSqlRepository;
        }

        public void Insert<T>(T modelToInsert)
        {
            this.readWriteSqlRepository.Insert(modelToInsert);
        }

        public void InsertNoLog<T>(T modelToInsert)
        {
            this.readWriteSqlRepository.InsertNoLog(modelToInsert);
        }

        public void Insert<T>(IEnumerable<T> modelsToInsert)
        {
            this.readWriteSqlRepository.Insert(modelsToInsert);
        }

        public void Insert<T>(ISqlStatement<T> insertStatement)
        {
            this.readWriteSqlRepository.Insert(insertStatement);
        }

        public void Update<T>(T modelToUpdate)
        {
            this.readWriteSqlRepository.Update(modelToUpdate);
        }

        public void Update<T>(IEnumerable<T> modelsToUpdate)
        {
            this.readWriteSqlRepository.Update(modelsToUpdate);
        }

        public void Update<T>(ISqlStatement<T> updateStatement)
        {
            this.readWriteSqlRepository.Update(updateStatement);
        }

        public void DeleteByKey<T, TKey>(TKey key)
        {
            this.readWriteSqlRepository.DeleteByKey<T, TKey>(key);
        }

        public void ExecuteSqlStatement(string statementToExecute, dynamic param = null)
        {
            this.readWriteSqlRepository.ExecuteSqlStatement(statementToExecute, param);
        }

        public void DeleteWhere<T>(string whereClause, object param = null)
        {
            this.readWriteSqlRepository.DeleteWhere<T>(whereClause, param);
        }

        public void Delete<T>(ISqlStatement<T> deleteStatement)
        {
            this.readWriteSqlRepository.Delete(deleteStatement);
        }

        public void ExecuteNonQuery<T>(ISqlStatement<T> statementToExecute)
        {
            this.readWriteSqlRepository.ExecuteNonQuery(statementToExecute);
        }

        public void InsertNoLog<T>(IEnumerable<T> modelsToInsert)
        {
            this.readWriteSqlRepository.InsertNoLog(modelsToInsert);
        }
    }
}
