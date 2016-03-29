using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.Logging;
using SAHL.X2.Common.DataAccess;
using SAHL.X2.Framework.DataAccess.Exceptions;

namespace SAHL.X2.Framework.DataAccess
{
    public partial class Repository
    {
        static object syncObj = new object();

        public static string Get(string p_sApplicationName, string p_sStatementName, ITransactionContext ctx)
        {
            lock (syncObj)
            {
                string StatementName = p_sApplicationName + p_sStatementName;

                CacheManager statementCache = CacheFactory.GetCacheManager("UISTATEMENT");

                if (statementCache != null && statementCache.Contains(StatementName))
                {
                    return statementCache[StatementName] as string;
                }
                else
                {
                    string Q = Properties.Settings.Default.RepositoryFind;
                    //DBHelper db = new DBHelper(ctx, Q, System.Data.CommandType.Text);
                    DBHelper db = new DBHelper(Q, System.Data.CommandType.Text);

                    try
                    {
                        DataTable Dt = new DataTable();
                        db.AddVarcharParameter("@Application", p_sApplicationName);
                        db.AddVarcharParameter("@Statement", p_sStatementName);
                        db.Fill();
                        DataRow R = db.FirstRow();
                        if (R == null)
                            throw new DataAccessException();

                        string Statement = R["Statement"].ToString();
                        statementCache.Add(StatementName, Statement);
                        try
                        {
                            int StatementKey = Convert.ToInt32(R["StatementKey"]);
                            string sql = string.Format("UPDATE [2am]..uiStatement SET LastAccessedDate = '{0}' WHERE StatementKey = {1}", DateTime.Now.ToString("yyyy-MM-dd"), StatementKey);
                            WorkerHelper.ExecuteNonQuery(ctx, sql, new ParameterCollection());
                        }
                        catch (Exception tex)
                        {
                            Dictionary<string, object> methodParameters = new Dictionary<string, object>();
                            methodParameters.Add("p_sApplicationName", p_sApplicationName);
                            methodParameters.Add("p_sStatementName", p_sStatementName);
                            LogPlugin.Logger.LogErrorMessageWithException("Repository.Get", "Unable to update UIStatement LastUsed", tex, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParameters } });
                        }
                        return Statement;
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (db != null)
                        {
                            db.Dispose();
                            db = null;
                        }
                    }
                }
            }
        }

        public static string GetWithContext(ITransactionContext Context, string p_sApplicationName, string p_sStatementName)
        {
            lock (syncObj)
            {
                string StatementName = p_sApplicationName + p_sStatementName;

                CacheManager statementCache = CacheFactory.GetCacheManager("UISTATEMENT");

                if (statementCache != null && statementCache.Contains(StatementName))
                {
                    return statementCache[StatementName] as string;
                }
                else
                {
                    string Q = Properties.Settings.Default.RepositoryFind;
                    DBHelper db = new DBHelper(Context, Q, System.Data.CommandType.Text);
                    try
                    {
                        DataTable Dt = new DataTable();
                        db.AddVarcharParameter("@Application", p_sApplicationName);
                        db.AddVarcharParameter("@Statement", p_sStatementName);
                        if (Context.DataTransaction != null)
                            db.Adapter.SelectCommand.Transaction = (SqlTransaction)Context.DataTransaction;
                        db.Fill();
                        DataRow R = db.FirstRow();
                        if (R == null)
                            throw new DataAccessException();

                        string Statement = R["Statement"].ToString();
                        statementCache.Add(StatementName, Statement);
                        return Statement;
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (db != null)
                        {
                            db.Dispose();
                            db = null;
                        }
                    }
                }
            }
        }

        public static void ClearCache()
        {
            lock (syncObj)
            {
                CacheManager cache = CacheFactory.GetCacheManager("UISTATEMENT");
                if (null != cache)
                    cache.Flush();
            }
        }
    }
}