using System.Collections.Generic;
using System.Data;

namespace SAHL.Core.Data
{
    public abstract class SqlRepositoryBase : IReadOnlySqlRepository
    {
        private IDbConnection connection;

        public IDbConnection DbConnection
        {
            get { return connection; }
            protected set { connection = value; }
        }

        public void UseConnection(IDbConnection connection)
        {
            this.DbConnection = connection;
        }

        public abstract T GetByKey<T, K>(K key);

        public abstract void Insert<T>(T modelToInsert);

        public abstract void InsertNoLog<T>(T modelToInsert);

        public abstract void InsertNoLog<T>(IEnumerable<T> modelsToInsert);

        public abstract void Insert<T>(IEnumerable<T> modelsToInsert);

        public abstract void Insert<T>(ISqlStatement<T> insertStatement);

        public abstract void Update<T>(T modelToUpdate);

        public abstract void Update<T>(IEnumerable<T> modelsToUpdate);

        public abstract void Update<T>(ISqlStatement<T> updateStatement);

        public abstract void DeleteByKey<T, K>(K key);

        public abstract void Delete<T>(ISqlStatement<T> deleteStatement);

        public abstract void DeleteWhere<T>(string whereClause, object param = null);

        public abstract IEnumerable<T> Select<T>(string sqlQuery, object param = null);

        public abstract IEnumerable<T> Select<T>(ISqlStatement<T> sqlStatementObject);

        public abstract T SelectOne<T>(string sqlQuery, object param = null);

        public abstract T SelectOne<T>(ISqlStatement<T> sqlStatementObject);

        public abstract void SelectOneInto<T>(string sqlQuery, T instanceToPopulate, object param = null);

        public abstract void SelectOneInto<T>(ISqlStatement<T> sqlStatementObject, T instanceToPopulate);

        public abstract IEnumerable<T> SelectWhere<T>(string whereClause, object param = null);

        public abstract T SelectOneWhere<T>(string whereClause, object param = null);

        public abstract void ExecuteNonQuery<T>(ISqlStatement<T> statementToExecute);

        public void Dispose()
        {
        }
    }
}