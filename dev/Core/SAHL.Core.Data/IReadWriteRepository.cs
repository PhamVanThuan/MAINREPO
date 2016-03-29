using System.Collections.Generic;

namespace SAHL.Core.Data
{
    public interface IReadWriteRepository : IReadOnlyRepository
    {
        void Insert<T>(T modelToInsert);

        void InsertNoLog<T>(T modelToInsert);

        void InsertNoLog<T>(IEnumerable<T> modelsToInsert);

        void Insert<T>(IEnumerable<T> modelsToInsert);

        void Insert<T>(ISqlStatement<T> insertStatement);

        void Update<T>(T modelToUpdate);

        void Update<T>(IEnumerable<T> modelsToUpdate);

        void Update<T>(ISqlStatement<T> updateStatement);

        void DeleteByKey<T, K>(K key);

        void Delete<T>(ISqlStatement<T> deleteStatement);

        void ExecuteNonQuery<T>(ISqlStatement<T> statementToExecute);
    }
}