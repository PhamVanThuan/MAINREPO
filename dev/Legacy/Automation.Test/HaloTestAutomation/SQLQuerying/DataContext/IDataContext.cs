using Automation.DataModels;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System;

namespace Automation.DataAccess.Interfaces
{
    public interface IDataContext
    {
        QueryResults ExecuteSQLQuery(SQLStatement statement);

        QueryResults ExecuteSQLScalar(SQLStatement statement);

        bool ExecuteNonSQLQuery(SQLStatement statement);

        QueryResults ExecuteStoredProcedureWithResults(SQLStoredProcedure procedure);

        void ExecuteStoredProcedure(SQLStoredProcedure procedure);

        IEnumerable<T> Query<T>(string sqlString, DynamicParameters parameters = null, CommandType? commandtype = null);

        void Execute(string sqlString, DynamicParameters parameters = null, CommandType? commandtype = null);

        IEnumerable<T3> Query<T1, T2, T3>(string sqlString, Func<T1, T2, T3> map, string splitOn, CommandType commandtype, DynamicParameters parameters = null);

        IEnumerable<T4> Query<T1, T2, T3, T4>(string sqlString, Func<T1, T2, T3, T4> map, string splitOn, CommandType commandtype, DynamicParameters parameters = null);

        IEnumerable<T5> Query<T1, T2, T3, T4, T5>(string sqlString, Func<T1, T2, T3, T4, T5> map, string splitOn, CommandType commandtype, DynamicParameters parameters = null);

        IEnumerable<T6> Query<T1, T2, T3, T4, T5, T6>(string sqlString, Func<T1, T2, T3, T4, T5, T6> map, string splitOn, CommandType commandtype, DynamicParameters parameters = null);
    }
}