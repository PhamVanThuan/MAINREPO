using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.DataAccess.Exceptions;

namespace SAHL.Common.DataAccess
{
    /// <summary>
    /// Exposes the SAHL UIStatements.
    /// </summary>
    public sealed class UIStatementRepository
    {
        private static object syncObj = new object();

        /// <summary>
        /// Clears out the UIStatement cache.
        /// </summary>
        public static void ClearCache()
        {
            CacheManager statementCache = GetCache();
            statementCache.Flush();
        }

        /// <summary>
        /// Gets a reference to the UIStatement cache.
        /// </summary>
        /// <returns></returns>
        private static CacheManager GetCache()
        {
            return CacheFactory.GetCacheManager("UISTATEMENT");
        }

        /// <summary>
        /// Gets the latest version of a statement from the uiStatement table.
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="statementName"></param>
        /// <returns></returns>
        public static string GetStatement(string applicationName, string statementName)
        {
            lock (syncObj)
            {
                string key = String.Concat(applicationName, statementName);
                CacheManager statementCache = GetCache();

                if (statementCache != null && statementCache.Contains(key))
                {
                    return statementCache[key] as string;
                }
                else
                {
                    string statement = String.Empty;
                    string query = @"select StatementKey, Statement
                        from dbo.uiStatement
                        where Version = (
                            select max(Version)
                            from dbo.uiStatement
                            where ApplicationName = @Application
                            and StatementName = @Statement)
                        and ApplicationName = @Application
                        and StatementName = @Statement";

                    using (IDbConnection conn = Helper.GetSQLDBConnection())
                    {
                        int statementKey = 0;

                        conn.Open();
                        ParameterCollection parameters = new ParameterCollection();
                        Helper.AddVarcharParameter(parameters, "@Application", applicationName);
                        Helper.AddVarcharParameter(parameters, "@Statement", statementName);

                        using (IDataReader reader = Helper.ExecuteReader(conn, query, parameters))
                        {
                            if (reader.Read())
                            {
                                statementKey = reader.GetInt32(0);
                                statement = reader.GetString(1);
                                statementCache.Add(key, statement);
                            }
                            else
                            {
                                throw new DataAccessException(String.Format("Unable to find UIStatement {0}.{1}", applicationName, statementName));
                            }
                            reader.Close();
                        }
                        // update the LastAccessedDate field
                        string sql = "UPDATE uiStatement SET LastAccessedDate = @LastAccessedDate WHERE StatementKey = @StatementKey";
                        parameters = new ParameterCollection();
                        Helper.AddDateParameter(parameters, "@LastAccessedDate", DateTime.Now);
                        Helper.AddIntParameter(parameters, "@StatementKey", statementKey);
                        Helper.ExecuteNonQuery(conn, sql, parameters);
                    }
                    return statement;
                }
            }
        }
    }
}