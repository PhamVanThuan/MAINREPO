using Dapper;
using Omu.ValueInjecter;
using SAHL.Core.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace SAHL.Core.Data.Dapper
{
    public class DapperRepository : SqlRepositoryBase, IReadWriteSqlRepository
    {
        private IUIStatementProvider uiStatementProvider;
        private ILogger logger;
        private ILoggerSource loggerSource;
        private string userName = "";

        public DapperRepository(IUIStatementProvider uiStatementProvider, ILogger logger, ILoggerSource loggerSource)
        {
            this.uiStatementProvider = uiStatementProvider;
            this.logger = logger;
            this.loggerSource = loggerSource;

            if (userName == "")
            {
                if (Thread.CurrentPrincipal != null)
                {
                    try
                    {
                        userName = Thread.CurrentPrincipal.Identity.Name;
                    }
                    catch (ObjectDisposedException)
                    {
                        userName = "Unknown";
                    }
                }
                else
                {
                    userName = "Unknown";
                }
            }
        }

        private void CatchAndLog(string methodName, Action actionToCatchAndLog)
        {
            try
            {
                this.logger.LogMethodMetric(this.loggerSource, userName, methodName, () =>
                {
                    actionToCatchAndLog();
                });
            }
            catch (Exception ex)
            {
                this.logger.LogErrorWithException(this.loggerSource, userName, methodName, string.Format("Error executing Sql repository {0}", methodName), ex);
                throw;
            }
        }

        private T CatchAndLog<T>(string methodName, Func<T> actionToCatchAndLog)
        {
            T result;
            try
            {
                result = default(T);

                this.logger.LogMethodMetric(this.loggerSource, userName, methodName, () =>
                {
                    result = actionToCatchAndLog();
                });
            }
            catch (Exception ex)
            {
                this.logger.LogErrorWithException(this.loggerSource, userName, methodName, string.Format("Error executing Sql repository {0}", methodName), ex);
                throw;
            }

            return result;
        }

        public override T GetByKey<T, K>(K key)
        {
            return CatchAndLog<T>("GetByKey", () =>
            {
                string statement = this.GetStatement(typeof(T), "SelectByKey");
                return this.Query<T>(statement, new { PrimaryKey = key }).FirstOrDefault();
            });
        }

        public override void Insert<T>(T modelToInsert)
        {
            CatchAndLog("Insert", () =>
                {
                    string statement = this.GetStatement(typeof(T), "Insert");
                    this.Insert(statement, modelToInsert);
                });
        }

        public override void InsertNoLog<T>(IEnumerable<T> modelsToInsert)
        {
            string statement = this.GetStatement(typeof(T), "Insert");
            foreach (var model in modelsToInsert)
            {
                this.Insert(statement, model);
            }
        }

        public override void Insert<T>(IEnumerable<T> modelsToInsert)
        {
            CatchAndLog("Insert", () =>
                {
                    string statement = this.GetStatement(typeof(T), "Insert");
                    foreach (var model in modelsToInsert)
                    {
                        this.Execute(statement, model);
                    }
                });
        }

        public override void Insert<T>(ISqlStatement<T> insertStatement)
        {
            CatchAndLog("Insert", () =>
                {
                    string statement = insertStatement.GetStatement();
                    this.Execute(statement, param: insertStatement);
                });
        }

        public override void Update<T>(T modelToUpdate)
        {
            CatchAndLog("Update", () =>
                {
                    string statement = this.GetStatement(typeof(T), "Update");
                    this.Execute(statement, modelToUpdate);
                });
        }

        public override void Update<T>(IEnumerable<T> modelsToUpdate)
        {
            CatchAndLog("Update", () =>
                {
                    string statement = this.GetStatement(typeof(T), "Update");
                    this.Execute(statement, modelsToUpdate);
                });
        }

        public override void Update<T>(ISqlStatement<T> updateStatement)
        {
            CatchAndLog("Update", () =>
                {
                    string statement = updateStatement.GetStatement();
                    this.Execute(statement, param: updateStatement);
                });
        }

        public override void DeleteByKey<T, K>(K key)
        {
            CatchAndLog("DeleteByKey", () =>
                {
                    string statement = this.GetStatement(typeof(T), "Delete");
                    this.Execute(statement, new { PrimaryKey = key });
                });
        }

        public override void DeleteWhere<T>(string whereClause, object param = null)
        {
            CatchAndLog("DeleteWhere", () =>
                {
                    string statement = this.GetStatement(typeof(T), "DeleteWhere");
                    statement = string.Format("{0} {1}", statement.Trim(), whereClause.Trim());
                    this.Execute(statement, param);
                });
        }

        public override void Delete<T>(ISqlStatement<T> deleteStatement)
        {
            CatchAndLog("Delete", () =>
                {
                    string statement = deleteStatement.GetStatement();
                    this.Execute(statement, param: deleteStatement);
                });
        }

        public override IEnumerable<T> Select<T>(string sqlQuery, object param = null)
        {
            return CatchAndLog<IEnumerable<T>>("Select", () =>
                {
                    return Query<T>(sqlQuery, param);
                });
        }

        public override IEnumerable<T> Select<T>(ISqlStatement<T> sqlStatementObject)
        {
            return CatchAndLog<IEnumerable<T>>("Select", () =>
                {
                    string sqlQuery = sqlStatementObject.GetStatement();
                    return Query<T>(sqlQuery, sqlStatementObject);
                });
        }

        public override T SelectOne<T>(string sqlQuery, object param = null)
        {
            return CatchAndLog<T>("SelectOne", () =>
                {
                    return Query<T>(sqlQuery, param).FirstOrDefault();
                });
        }

        public override T SelectOne<T>(ISqlStatement<T> sqlStatementObject)
        {
            return CatchAndLog<T>("SelectOne", () =>
                {
                    string sqlQuery = sqlStatementObject.GetStatement();
                    return Query<T>(sqlQuery, sqlStatementObject).FirstOrDefault();
                });
        }

        public override void SelectOneInto<T>(string sqlQuery, T instanceToPopulate, object param = null)
        {
            CatchAndLog("SelectOneInto", () =>
                {
                    var result = SelectOne<T>(sqlQuery, param);
                    instanceToPopulate.InjectFrom(result);
                });
        }

        public override void SelectOneInto<T>(ISqlStatement<T> sqlStatementObject, T instanceToPopulate)
        {
            CatchAndLog("SelectOneInto", () =>
                {
                    var result = SelectOne<T>(sqlStatementObject);
                    instanceToPopulate.InjectFrom(result);
                });
        }

        public override T SelectOneWhere<T>(string whereClause, object param = null)
        {
            return CatchAndLog<T>("SelectOneWhere", () =>
                {
                    string statement = this.GetStatement(typeof(T), "SelectWhere");
                    statement = string.Format("{0} {1}", statement.Trim(), whereClause.Trim());
                    return Query<T>(statement, param).FirstOrDefault();
                });
        }

        public override IEnumerable<T> SelectWhere<T>(string whereClause, object param = null)
        {
            return CatchAndLog<IEnumerable<T>>("SelectWhere", () =>
                {
                    string statement = this.GetStatement(typeof(T), "SelectWhere");
                    statement = string.Format("{0} {1}", statement.Trim(), whereClause.Trim());
                    return Query<T>(statement, param);
                });
        }

        public void ExecuteSqlStatement(string statementToExecute, dynamic param = null)
        {
            CatchAndLog("Execute", () =>
                {
                    this.Execute(statementToExecute, param);
                });
        }

        public override void ExecuteNonQuery<T>(ISqlStatement<T> statementToExecute)
        {
            CatchAndLog("ExecuteNonQuery", () =>
                {
                    string statement = statementToExecute.GetStatement();
                    this.Execute(statement, param: statementToExecute);
                });
        }

        private int Execute(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            int numRowsAffected = SqlMapper.Execute(this.DbConnection, sql, param, transaction, commandTimeout, commandType);
            return numRowsAffected;
        }

        private string GetStatementNameFromType(Type modelType, string operation)
        {
            return string.Format("{0}_{1}", modelType.FullName, operation);
        }

        private string GetStatement(Type type, string operation)
        {
            string uniqueStatementName = GetStatementNameFromType(type, operation);
            string statement = this.uiStatementProvider.Get(type.Namespace, uniqueStatementName);
            return statement;
        }

        private void Insert(string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (param != null && param is IDataModelWithIdentitySeed)
            {
                IEnumerable<int> keys = SqlMapper.Query<int>(this.DbConnection, sql, param, transaction, buffered, commandTimeout, commandType);
                int key = keys.Single<int>();
                Type insertType = param.GetType();
                MethodInfo setKeyMethod = insertType.GetMethod("SetKey");
                setKeyMethod.Invoke(param, new object[] { key });
            }
            else
            {
                SqlMapper.Execute(this.DbConnection, sql, param, transaction, commandTimeout, commandType);
            }
        }

        private IEnumerable<dynamic> Query(string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.Query(this.DbConnection, sql, param, transaction, buffered, commandTimeout, commandType);
        }

        private IEnumerable<T> Query<T>(string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.Query<T>(this.DbConnection, sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public override void InsertNoLog<T>(T modelToInsert)
        {
            string statement = this.GetStatement(typeof(T), "Insert");
            this.Insert(statement, modelToInsert);
        }
    }
}