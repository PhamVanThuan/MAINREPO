using System;
using System.Collections.Generic;

namespace SAHL.Core.Data
{
    public interface IReadOnlySqlRepository : ISqlRepository, IReadOnlyRepository, IDisposable
    {
        IEnumerable<T> Select<T>(string sqlQuery, object param = null);

        IEnumerable<T> Select<T>(ISqlStatement<T> sqlStatementObject);

        T SelectOne<T>(string sqlQuery, object param = null);

        T SelectOne<T>(ISqlStatement<T> sqlStatementObject);

        void SelectOneInto<T>(string sqlQuery, T instanceToPopulate, object param = null);

        void SelectOneInto<T>(ISqlStatement<T> sqlStatementObject, T instanceToPopulate);

        IEnumerable<T> SelectWhere<T>(string whereClause, object param = null);

        T SelectOneWhere<T>(string whereClause, object param = null);
    }
}