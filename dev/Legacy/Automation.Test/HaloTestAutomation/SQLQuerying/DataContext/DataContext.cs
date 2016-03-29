using Automation.DataAccess.Interfaces;
using Automation.DataModels;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System;
using System.Configuration;
using Common.Enums;

namespace Automation.DataAccess
{
    public class DataContext : IDataContext
    {
        public DataContext(KnownDBs knownDBs = KnownDBs._2AM)
        {
            switch (knownDBs)
            {
                case KnownDBs._2AM:
                    this.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                    break;
                case KnownDBs.Capitec:
                    this.ConnectionString = ConfigurationManager.ConnectionStrings["CapitecDB"].ConnectionString;
                    break;
                default:
                    break;
            }
        }

        public QueryResults ExecuteSQLQuery(SQLStatement statement)
        {
            using (var conn = new DapperSqlConnection(this.ConnectionString))
            {
                QueryResults _results = new QueryResults();
                var dapperResults = conn.Query(statement.StatementString);
                _results.Fill(dapperResults);
                return _results;
            }
        }

        public QueryResults ExecuteSQLScalar(SQLStatement statement)
        {
            using (var conn = new DapperSqlConnection(this.ConnectionString))
            {
                QueryResults _results = new QueryResults();
                var dapperResults = conn.Query(statement.StatementString);
                _results.Fill(dapperResults);
                if (_results.HasResults)
                    _results.SQLScalarValue = _results.Rows(0).Column(0).Value;
                return _results;
            }
        }

        public bool ExecuteNonSQLQuery(SQLStatement statement)
        {
            using (var conn = new DapperSqlConnection(this.ConnectionString))
            {
                var result = conn.Execute(statement.StatementString);
                if (result > 0)
                {
                    return true;
                }
                else return false;
            }
        }

        public QueryResults ExecuteStoredProcedureWithResults(SQLStoredProcedure procedure)
        {
            using (var conn = new DapperSqlConnection(this.ConnectionString))
            {
                QueryResults _results = new QueryResults();
                var parameters = new DynamicParameters();
                foreach (var item in procedure.Parameters)
                {
                    ParameterDirection parameterDirection = item.Direction != null ? item.Direction : ParameterDirection.Input;
                    parameters.Add(item.ParameterName, item.Value, Dapper.SqlMapper.LookupDbType(item.Value.GetType(), item.ParameterName), parameterDirection);
                }
                var dapperResults = conn.Query(procedure.Name, param: parameters, commandType: CommandType.StoredProcedure);
                _results.Fill(dapperResults);
                return _results;
            }
        }

        public void ExecuteStoredProcedure(SQLStoredProcedure procedure)
        {
            using (var conn = new DapperSqlConnection(this.ConnectionString))
            {
                QueryResults _results = new QueryResults();
                var parameters = new DynamicParameters();
                foreach (var item in procedure.Parameters)
                {
                    ParameterDirection parameterDirection = item.Direction != null ? item.Direction : ParameterDirection.Input;
                    parameters.Add(item.ParameterName, item.Value, Dapper.SqlMapper.LookupDbType(item.Value.GetType(), item.ParameterName), parameterDirection);
                }
                conn.Execute(procedure.Name, param: parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<T> Query<T>(string sqlString, DynamicParameters parameters = null, CommandType? commandtype = null)
        {
            using (var conn = new DapperSqlConnection(this.ConnectionString))
            {
                return conn.Query<T>(sqlString, param: parameters, commandType: commandtype);
            }
        }

        public void Execute(string sqlString, DynamicParameters parameters = null, CommandType? commandtype = null)
        {
            using (var conn = new DapperSqlConnection(this.ConnectionString))
            {
                conn.Execute(sqlString, param: parameters, commandType: commandtype);
            }
        }

        public IEnumerable<T3> Query<T1, T2, T3>(string sqlString, Func<T1, T2, T3> map, string splitOn, CommandType commandtype, DynamicParameters parameters = null)
        {
            using (var conn = new DapperSqlConnection(this.ConnectionString))
            {
                return conn.Query<T1, T2, T3>(sqlString, map: map, commandType: commandtype, param: parameters, splitOn: splitOn);
            }
        }

        public IEnumerable<T4> Query<T1, T2, T3, T4>(string sqlString, Func<T1, T2, T3, T4> map, string splitOn, CommandType commandtype, DynamicParameters parameters = null)
        {
            using (var conn = new DapperSqlConnection(this.ConnectionString))
            {
                return conn.Query<T1, T2, T3, T4>(sqlString, map: map, commandType: commandtype, param: parameters, splitOn: splitOn);
            }
        }


        public IEnumerable<T5> Query<T1, T2, T3, T4, T5>(string sqlString, Func<T1, T2, T3, T4, T5> map, string splitOn, CommandType commandType, DynamicParameters parameters = null)
        {
            using (var conn = new DapperSqlConnection(this.ConnectionString))
            {
                return conn.Query<T1, T2, T3, T4, T5>(sqlString, map: map, commandType: commandType, param: parameters, splitOn: splitOn);
            }
        }

        public IEnumerable<T6> Query<T1, T2, T3, T4, T5, T6>(string sqlString, Func<T1, T2, T3, T4, T5, T6> map, string splitOn, CommandType commandtype, DynamicParameters parameters = null)
        {
            using (var conn = new DapperSqlConnection(this.ConnectionString))
            {
                return conn.Query<T1, T2, T3, T4, T5, T6>(sqlString, map: map, commandType: commandtype, param: parameters, splitOn: splitOn);
            }
        }
        protected string ConnectionString {get;private set;}
    }
}