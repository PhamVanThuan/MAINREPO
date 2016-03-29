using SAHL.Core.Data;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Dapper;
using SAHL.Core.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace SAHL.Core.Testing
{
    public static class StatementRunner
    {
        private static Type sqlStatementTypeInterface;
        private static IDbFactory dbFactory;
        private static TestDbConfigurationProvider dbProvider;

        static StatementRunner()
        {
            sqlStatementTypeInterface = typeof(ISqlStatement<>);
            dbProvider = new TestDbConfigurationProvider("Halo");
            var uiStatementProvider = new DapperUIStatementProvider(dbProvider);
            var testLoggerAppSource = new LoggerAppSourceFromImplicitSource("TestLoggerAppSource");
            var sqlRepositoryFactory = new DapperSqlRepositoryFactory(uiStatementProvider,
                new Logger(new NullRawLogger(), new NullMetricsRawLogger(), testLoggerAppSource, new MetricTimerFactory()),
                new LoggerSource("StatementTests", LogLevel.Error, false));
            var dbConfigurationProvider = new DefaultDbConfigurationProvider();
            var dbConnectionProviderFactory = new DefaultDbConnectionProviderFactory(dbConfigurationProvider);
            var dbConnectionProviderStorage = new DefaultDbConnectionProviderStorage();

            DbContextConfiguration.Instance.RepositoryFactory = sqlRepositoryFactory;
            DbContextConfiguration.Instance.DbConfigurationProvider = dbConfigurationProvider;
            DbContextConfiguration.Instance.DbConnectionProviderFactory = dbConnectionProviderFactory;
            DbContextConfiguration.Instance.DbConnectionProviderStorage = dbConnectionProviderStorage;
            dbFactory = new DbFactory(sqlRepositoryFactory, dbConnectionProviderStorage, dbConnectionProviderFactory);
        }

        public static IEnumerable<T> RunServiceQueryStatement<T>(object queryStatementInstance, object sqlStatementInstance)
        {
            foreach (var result in RunServiceQueryStatement(queryStatementInstance, sqlStatementInstance))
            {
                yield return (T)result;
            }
        }

        public static IEnumerable<T> RunSqlStatement<T>(object queryStatementInstance)
        {
            foreach (var result in RunSqlStatement(queryStatementInstance))
            {
                yield return (T)result;
            }
        }

        public static IEnumerable RunServiceQueryStatement(object queryStatementInstance, object sqlStatementInstance)
        {
            var serviceStatementType = queryStatementInstance.GetType();
            var statementInstance = (dynamic)queryStatementInstance;
            var serviceQueryInterface = serviceStatementType
                .GetInterfaces()
                .FirstOrDefault(x => x.Name.StartsWith("IServiceQuery"));
            if (serviceQueryInterface == null)
            {
                throw new Exception(String.Format("Failed to find data model for {0}", queryStatementInstance));
            }
            var dataModelType = serviceQueryInterface.GetGenericArguments()[1];
            MethodInfo[] methods = GetMatchingSqlMethods(queryStatementInstance);
            var sqlMethod = methods.First(x => !x.GetParameters().Any(y => y.ParameterType.Name.Contains(sqlStatementTypeInterface.Name)))
                .MakeGenericMethod(dataModelType);
            using (var db = new Db().InAppContext())
            {
                try
                {
                    return (IEnumerable)sqlMethod.Invoke(db, new object[] { statementInstance.GetStatement(), sqlStatementInstance });
                }
                catch (Exception e)
                {
                    throw GetFormattedException(e, sqlStatementInstance);
                }
            }
        }

        public static IEnumerable RunSqlStatement(object sqlStatementInstance)
        {
            Type sqlStatementType = sqlStatementInstance.GetType();
            MethodInfo[] methods = GetMatchingSqlMethods(sqlStatementInstance);
            var dataModelType = sqlStatementType.GetInterfaces().First().GetGenericArguments().First();
            var sqlMethod = methods
                .FirstOrDefault(x => x.GetParameters().Count(y => y.ParameterType.Name.Contains(sqlStatementTypeInterface.Name)) == 1)
                .MakeGenericMethod(dataModelType);
            using (var db = dbFactory.NewDb().InAppContext())
            {
                try
                {
                    return (IEnumerable)sqlMethod.Invoke(db, new [] { sqlStatementInstance });
                }
                catch (Exception e)
                {
                    throw GetFormattedException(e, sqlStatementInstance);
                }
            }
        }

        private static Exception GetFormattedException(Exception e, object statementInstance)
        {
            var dataSourceName = "N/A";
            try
            {
                if (dbProvider.ConnectionStringForApplicationRole != null)
                {
                    var connectionStrBuilder = new SqlConnectionStringBuilder(dbProvider.ConnectionStringForApplicationRole);
                    dataSourceName = connectionStrBuilder.DataSource;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(String.Format("DataSource: {0}, {1}, Ctor Args: {2}", dataSourceName, e.InnerException.Message, CreateCtorArgsMessage(statementInstance)), exception);
            }
        
            return new Exception(String.Format("DataSource: {0}, {1}, Ctor Args: {2}", dataSourceName, e.InnerException.Message, CreateCtorArgsMessage(statementInstance)));
        }

        private static string CreateCtorArgsMessage(object instance)
        {
            var allProperties = new List<PropertyInfo>();
            var message = String.Empty;
            foreach (var prop in instance.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                allProperties.Add(prop);
            }
            foreach (var prop in instance.GetType().GetProperties())
            {
                allProperties.Add(prop);
            }
            foreach (var prop in allProperties)
            {
                message += String.Format("{0}: {1}, ", prop.Name, prop.GetValue(instance, null));
            }
            return message;
        }

        private static MethodInfo[] GetMatchingSqlMethods(dynamic sqlStatement)
        {
            var sql = sqlStatement.GetStatement().ToLower().Trim();
            var sqlStatementInfo = new SqlStatementInfo(sql);
            return typeof(DbContext)
                .GetMethods()
                .Where(x => x.IsGenericMethod && x.Name.Contains(sqlStatementInfo.StatementType.ToString()))
                .ToArray();
        }
    }
}