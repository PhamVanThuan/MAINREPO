using System;

namespace SAHL.Core.Data
{
    public interface IReadWriteSqlRepository : IReadOnlySqlRepository, IReadWriteRepository, IDisposable
    {
        void ExecuteSqlStatement(string statementToExecute, dynamic param = null);
        void DeleteWhere<T>(string whereClause, object param = null);
    }
}