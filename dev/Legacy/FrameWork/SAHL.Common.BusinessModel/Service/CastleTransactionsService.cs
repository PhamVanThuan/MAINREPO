using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Logging;
using SAHL.Common.Security;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Shared.Messages.Metrics;
using System.Text;
using System.Security.Principal;
using System.Web;
using Microsoft.ApplicationBlocks.UIProcess;

namespace SAHL.Common.BusinessModel.Service
{
    public class CastleTransactionsService : ICastleTransactionsService
    {
        public DataSet ExecuteQueryOnCastleTran(string query, Common.Globals.Databases database, DataAccess.ParameterCollection parameters, System.Collections.Specialized.StringCollection tablemappings, System.Data.DataSet ds)
        {
            Type ty = this.GetTypeFromDBEnum(database);

            return ExecuteQueryOnCastleTran(query, ty, parameters, tablemappings, ds);
        }

        private DataSet ExecuteQueryOnCastleTran(string query, Type Ty, ParameterCollection parameters, StringCollection tablemappings, DataSet ds)
        {
            object[] oo = new object[3];

            using (IDbCommand cmd = new SqlCommand())
            {
                cmd.CommandText = query;
                if (null != parameters)
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        SqlParameter param = parameters[i];
                        cmd.Parameters.Add(param);
                    }
                }

                oo[0] = tablemappings;
                oo[1] = cmd;
                oo[2] = ds;

                NHibernateDelegate d = new NHibernateDelegate(FillFromQuery);
                return (DataSet)ActiveRecordMediator.Execute(Ty, d, oo);
            }
        }

        private object FillFromQuery(NHibernate.ISession session, object data)//(DataSet dataSet, StringCollection tableMappings, string query, IDbConnection connection, ParameterCollection parameters)
        {

            object[] oo = data as object[];
            if (null == oo)
                throw new Exception("Null object data in FillFromQuery");

            StringCollection tableMappings = (StringCollection)oo[0];

            //ParameterCollection parameters = (ParameterCollection)oo[1];
            DataSet ds = (DataSet)oo[2];

            IDbConnection conn = session.Connection;
            SqlConnection sc = conn as SqlConnection;
            using (SqlCommand cmd = oo[1] as SqlCommand)
            {
                cmd.Connection = sc;
                if (session.Transaction != null && session.Transaction.IsActive)
                {
                    session.Transaction.Enlist(cmd);
                }
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.SelectCommand = (SqlCommand)conn.CreateCommand();//new SqlCommand(p_sQuery, (SqlConnection)p_TransactionContext.DataConnection);
                    session.Transaction.Enlist(da.SelectCommand);
                    da.SelectCommand.CommandText = cmd.CommandText;

                    if (cmd.Parameters != null)
                    {
                        SqlParameter[] prms = new SqlParameter[cmd.Parameters.Count];
                        for (int i = 0; i < cmd.Parameters.Count; i++)
                        {
                            cmd.Parameters.CopyTo(prms, i);
                        }
                        cmd.Parameters.Clear();
                        da.SelectCommand.Parameters.AddRange(prms);
                    }

                    // Add the table mappings.
                    if ((tableMappings != null) && (tableMappings.Count > 0))
                    {
                        da.TableMappings.Add("Table", tableMappings[0]);
                        for (int i = 1; i < tableMappings.Count; i++)
                        {
                            da.TableMappings.Add("Table" + i, tableMappings[i]);
                        }
                    }

                    da.Fill(ds);
                    da.SelectCommand.Parameters.Clear();
                }
            }
            return ds;
        }

        public int ExecuteNonQueryOnCastleTran(string query, Common.Globals.Databases database, DataAccess.ParameterCollection parameters)
        {
            DateTime initTime = DateTime.Now;
            MetricsPlugin.Metrics.PublishThroughputMetric("ExecuteScalarOnCastleTran", new List<TimeUnit>() { TimeUnit.Seconds });

            try
            {
                Type ty = this.GetTypeFromDBEnum(database);

                return ExecuteNonQueryOnCastleTran(query, ty, parameters);
            }
            finally
            {
                MetricsPlugin.Metrics.PublishLatencyMetric(initTime, DateTime.Now - initTime, "ExecuteScalarOnCastleTran");
            }
        }

        public DataSet ExecuteQueryOnCastleTran(string query, Databases database, ParameterCollection parameters)
        {
            DateTime initTime = DateTime.Now;
            MetricsPlugin.Metrics.PublishThroughputMetric("ExecuteScalarOnCastleTran", new List<TimeUnit>() { TimeUnit.Seconds });

            try
            {
                Type ty = this.GetTypeFromDBEnum(database);

                return this.ExecuteQueryOnCastleTran(query, ty, parameters);
            }
            finally
            {
                MetricsPlugin.Metrics.PublishLatencyMetric(initTime, DateTime.Now - initTime, "ExecuteScalarOnCastleTran");
            }
        }

        public DataSet ExecuteQueryOnCastleTran(string query, Type Ty, ParameterCollection parameters)
        {
            DateTime initTime = DateTime.Now;
            MetricsPlugin.Metrics.PublishThroughputMetric("ExecuteScalarOnCastleTran", new List<TimeUnit>() { TimeUnit.Seconds });

            try
            {
                using (IDbCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = query;
                    if (null != parameters)
                    {
                        for (int i = 0; i < parameters.Count; i++)
                        {
                            SqlParameter param = parameters[i];
                            cmd.Parameters.Add(param);
                        }
                    }
                    NHibernateDelegate d = new NHibernateDelegate(ProcessQuery);
                    return (DataSet)ActiveRecordMediator.Execute(Ty, d, cmd);
                }
            }
            finally
            {
                MetricsPlugin.Metrics.PublishLatencyMetric(initTime, DateTime.Now - initTime, "ExecuteScalarOnCastleTran");
            }
        }

        public object ExecuteScalarOnCastleTran(string query, Common.Globals.Databases database, DataAccess.ParameterCollection parameters)
        {
            DateTime initTime = DateTime.Now;
            MetricsPlugin.Metrics.PublishThroughputMetric("ExecuteScalarOnCastleTran", new List<TimeUnit>() { TimeUnit.Seconds });

            try
            {
                Type ty = this.GetTypeFromDBEnum(database);

                return ExecuteScalarOnCastleTran(query, ty, parameters);
            }
            finally
            {
                MetricsPlugin.Metrics.PublishLatencyMetric(initTime, DateTime.Now - initTime, "ExecuteScalarOnCastleTran");
            }
        }

        public object ExecuteScalarOnCastleTran(string query, Type Ty, ParameterCollection parameters)
        {
            DateTime initTime = DateTime.Now;
            MetricsPlugin.Metrics.PublishThroughputMetric("ExecuteScalarOnCastleTran", new List<TimeUnit>() { TimeUnit.Seconds });

            try
            {
                using (IDbCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = query;
                    if (null != parameters)
                    {
                        for (int i = 0; i < parameters.Count; i++)
                        {
                            SqlParameter param = parameters[i];
                            cmd.Parameters.Add(param);
                        }
                    }
                    NHibernateDelegate d = new NHibernateDelegate(ProcessQuery);
                    DataSet ds = (DataSet)ActiveRecordMediator.Execute(Ty, d, cmd);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                return ds.Tables[0].Rows[0][0];
                            }
                    }
                    return null;
                }
            }
            finally
            {
                MetricsPlugin.Metrics.PublishLatencyMetric(initTime, DateTime.Now - initTime, "ExecuteScalarOnCastleTran");
            }
        }

        private object ProcessQuery(NHibernate.ISession session, object data)
        {
            IDbConnection conn = session.Connection;
            SqlConnection sc = conn as SqlConnection;
            using (SqlCommand cmd = data as SqlCommand)
            {
                cmd.Connection = sc;
                if (session.Transaction != null && session.Transaction.IsActive)
                {
                    session.Transaction.Enlist(cmd);
                }
                DataSet ds = new DataSet();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(ds, "DATA");
                }
                return ds;
            }
        }

        private object ProcessNonQuery(NHibernate.ISession session, object data)
        {
            IDbConnection conn = session.Connection;
            using (IDbCommand cmd = data as IDbCommand)
            {
                cmd.Connection = conn;
                if (session.Transaction != null && session.Transaction.IsActive)
                {
                    session.Transaction.Enlist(cmd);
                }
                return cmd.ExecuteNonQuery();
            }
        }

        public int ExecuteNonQueryOnCastleTran(string query, Type Ty, ParameterCollection parameters)
        {
            using (IDbCommand cmd = new SqlCommand())
            {
                cmd.CommandText = query;
                if (null != parameters)
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        SqlParameter param = parameters[i];
                        cmd.Parameters.Add(param);
                    }
                }
                NHibernateDelegate d = new NHibernateDelegate(ProcessNonQuery);
                return Convert.ToInt32(ActiveRecordMediator.Execute(Ty, d, cmd));
            }
        }

        public void FillDataSetFromQueryOnCastleTran(DataSet dataSet, string tableName, string query, Common.Globals.Databases database, ParameterCollection parameters)
        {
            Type ty = this.GetTypeFromDBEnum(database);
            FillFromQueryOnCastleTran(dataSet, tableName, query, ty, parameters);
        }

        private void FillFromQueryOnCastleTran(DataSet dataSet, string tableName, string query, Type Ty, ParameterCollection parameters)
        {
            object[] oo = new object[3];

            using (IDbCommand cmd = new SqlCommand())
            {
                cmd.CommandText = query;
                if (null != parameters)
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        SqlParameter param = parameters[i];
                        cmd.Parameters.Add(param);
                    }
                }

                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = "Table0";
                }

                oo[0] = new StringCollection() { tableName };
                oo[1] = cmd;
                oo[2] = dataSet;

                NHibernateDelegate d = new NHibernateDelegate(FillFromQuery);
                ActiveRecordMediator.Execute(Ty, d, oo);
            }
        }

        public void FillDataTableFromQueryOnCastleTran(DataTable dataTable, string query, Common.Globals.Databases database, ParameterCollection parameters)
        {
            Type ty = this.GetTypeFromDBEnum(database);
            FillFromQueryOnCastleTran(dataTable, query, ty, parameters);
        }

        private void FillFromQueryOnCastleTran(DataTable datatable, string query, Type Ty, ParameterCollection parameters)
        {
            DataSet tmp = new DataSet();
            tmp.Tables.Add(datatable);
            FillFromQueryOnCastleTran(tmp, datatable.TableName, query, Ty, parameters);
        }

        private Type GetTypeFromDBEnum(Common.Globals.Databases database)
        {
            Type ty;
            switch (database)
            {
                case Globals.Databases.TwoAM:
                    ty = typeof(Account_DAO);
                    break;
                case Globals.Databases.X2:
                    ty = typeof(Instance_DAO);
                    break;
                default:
                    ty = typeof(Account_DAO);
                    break;
            }

            return ty;
        }

        public IList<T> Many<T>(Globals.QueryLanguages language, string query, Globals.Databases database, params object[] parameters)
        {
            return Many<T>(language, query, null, database, parameters);
        }

        public IList<T> Many<T>(Globals.QueryLanguages language, string query, string sqlReturnDefinition, Globals.Databases database, params object[] parameters)
        {
            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            Type daoType = typeMapper.GetDAOFromInterface<T>();

            Type simpleQueryType = typeof(SimpleQuery<>).MakeGenericType(daoType);

            object simpleQueryInstance = CreateSimpleQuery<T>(language, query, parameters);

            if (!(string.IsNullOrEmpty(sqlReturnDefinition)))
            {
                MethodInfo addSqlReturnDefinitionMethod = simpleQueryType.GetMethod("AddSqlReturnDefinition", BindingFlags.Public | BindingFlags.Instance);

                addSqlReturnDefinitionMethod.Invoke(simpleQueryInstance, new object[] { daoType, sqlReturnDefinition });
            }

            MethodInfo methodInfo = simpleQueryType.GetMethod("Execute", BindingFlags.Public | BindingFlags.Instance);

            var daoResults = (object[])methodInfo.Invoke(simpleQueryInstance, null);

            return typeMapper.GetMappedTypeList<T>(daoResults);
        }

        public T Single<T>(Globals.QueryLanguages language, string query, Globals.Databases database, params object[] parameters)
        {
            return Single<T>(language, query, null, database, parameters);
        }

        public T Single<T>(Globals.QueryLanguages language, string query, string sqlReturnDefinition, Globals.Databases database, params object[] parameters)
        {
            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            Type daoType = typeMapper.GetDAOFromInterface<T>();

            Type simpleQueryType = typeof(SimpleQuery<>).MakeGenericType(daoType);

            object simpleQueryInstance = CreateSimpleQuery<T>(language, query, parameters);

            if (!(string.IsNullOrEmpty(sqlReturnDefinition)))
            {
                MethodInfo addSqlReturnDefinitionMethod = simpleQueryType.GetMethod("AddSqlReturnDefinition", BindingFlags.Public | BindingFlags.Instance);

                addSqlReturnDefinitionMethod.Invoke(simpleQueryInstance, new object[] { daoType, sqlReturnDefinition });
            }

            MethodInfo setQueryRangeMethod = simpleQueryType.GetMethod("SetQueryRange", new Type[] { typeof(int) });  //BindingFlags.Public | BindingFlags.Instance, );
            setQueryRangeMethod.Invoke(simpleQueryInstance, new object[] { 1 });

            MethodInfo executeMethod = simpleQueryType.GetMethod("Execute", BindingFlags.Public | BindingFlags.Instance);

            var daoResults = (object[])executeMethod.Invoke(simpleQueryInstance, null);

            if (daoResults.Length > 0)
            {
                return typeMapper.GetMappedType<T>(daoResults[0]);
            }

            return default(T);
        }

        private object CreateSimpleQuery<T>(Globals.QueryLanguages language, string query, params object[] parameters)
        {
            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            Type daoType = typeMapper.GetDAOFromInterface<T>();

            Type simpleQueryType = typeof(SimpleQuery<>).MakeGenericType(daoType);

            object[] constructorArgs = null;

            if (language == Globals.QueryLanguages.Hql)
            {
                constructorArgs = new object[] { QueryLanguage.Hql, query, parameters };
            }
            else
            {
                constructorArgs = new object[] { QueryLanguage.Sql, query, parameters };
            }

            return Activator.CreateInstance(simpleQueryType, constructorArgs);
        }

        /// <summary>
        /// Method to consistently execute any HALO API stored procedure in the Process DB
        /// All api's must be exectued through a uiStatement
        /// The uiStatement must contain a return parameter (@RC) and an output message (@Msg)
        ///     e.g.: EXECUTE @RC = [Process].[halo].[pProcessTran] @AccountKey, @UserID, @Msg OUTPUT
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="statementName"></param>
        /// <param name="parameters"></param>
        private void ExecuteHaloAPI_UIStatement(string applicationName, string statementName, ParameterCollection parameters)
        {
            DateTime initTime = DateTime.Now;

            Dictionary<string, object> methodParams = new Dictionary<string, object>();
            methodParams.Add(Metrics.UISTATEMENTNAME, statementName);

            ConvertDBParamsToDictionary(parameters, methodParams);

            using (IDbCommand cmd = new SqlCommand())
            {
                // get the sql to execute
                var query = UIStatementRepository.GetStatement(applicationName, statementName);

                #region determine the form name to use in audit
                // determine the form name - if we have an HTTP context then pull out the current running type,
                // and even better, if we can cast to an IView then get the view and presenter
                string formName = String.Empty;
                if (HttpContext.Current != null)
                {
                    IView view = HttpContext.Current.Handler as IView;
                    if (view != null)
                    {
                        formName = "View: {0}, Presenter: {1}";

                        // add the presenter name
                        ObjectTypeSettings presenterSettings = UIPConfiguration.Config.GetPresenterSettings(view.ViewName);
                        formName = String.Format(formName, view.ViewName, presenterSettings.Name);
                    }
                    else
                    {
                        // unable to determine UIP stuff - just use the current page type
                        formName = "Page: " + HttpContext.Current.Handler.GetType().FullName;
                    }
                }
                #endregion

                string auditData = formName + ", uiStatement: " + applicationName + "-" + statementName;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select top 0 * into #auditapi from process.template.auditapi");
                sb.AppendLine("insert into #auditapi (HostName,WindowsLogon,AuditData) values ('" + Environment.MachineName + "','" + WindowsIdentity.GetCurrent().Name + "','" + auditData + "')");
                sb.AppendLine(" ");
                sb.Append(query);

                // set the command text
                cmd.CommandText = sb.ToString();

                foreach (SqlParameter prm in parameters)
                {
                    cmd.Parameters.Add(prm);
                }

                SqlParameter msgPrm = new SqlParameter("@Msg", SqlDbType.VarChar, 1024);
                msgPrm.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgPrm);

                SqlParameter prmRes = new SqlParameter("@RC", SqlDbType.Int);
                prmRes.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(prmRes);

                int ret = -1;
                string errorMsg = String.Empty;
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

                try
                {
                    NHibernateDelegate d = new NHibernateDelegate(ProcessNonQuery);
                    ActiveRecordMediator.Execute(typeof(SAHL.Common.BusinessModel.DAO.GeneralStatus_DAO), d, cmd);
                }
                catch (Exception ex)
                {
                    Dictionary<string, object> methodParameters = new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParams } };
                    LogPlugin.Logger.LogFormattedErrorWithException("CastleTransactionsService.ExecuteHaloAPI_UIStatement", "Error Executing Halo API: {0} - {1}", ex, methodParameters, applicationName, statementName);
                    errorMsg = String.Format(@"ExecuteHaloAPI_UIS_OnCastleTran Exception in {0}-{1}: {2}", applicationName, statementName, ex.Message);
                    spc.DomainMessages.Add(new Error(errorMsg, errorMsg));
                    throw;
                }

                if (prmRes != null && prmRes.Value != null)
                {
                    ret = Convert.ToInt32(prmRes.Value.ToString());
                    if (ret == 0) //we have a successful response
                    {
                        Dictionary<string, object> metricParameters = new Dictionary<string, object>() { { Metrics.UISTATEMENTNAME, statementName } };
                        MetricsPlugin.Metrics.PublishLatencyMetric(initTime, DateTime.Now - initTime, "ExecuteHaloAPI_UIS_OnCastleTran", metricParameters);
                        MetricsPlugin.Metrics.PublishThroughputMetric("ExecuteHaloAPI_UIS_OnCastleTran", new List<TimeUnit>() { TimeUnit.Seconds }, metricParameters);
                        return;
                    }
                }

                if (msgPrm != null && msgPrm.Value != null)
                {
                    errorMsg = String.Format(@"ExecuteHaloAPI_UIS_OnCastleTran Exception in {0}-{1}: {2} return code was: {3}", applicationName, statementName, msgPrm.Value.ToString(), ret);
                    spc.DomainMessages.Add(new Error(errorMsg, errorMsg));
                    throw new DomainValidationException(errorMsg);
                }

                string prmv = String.Empty;
                foreach (SqlParameter prm in parameters)
                {
                    prmv += String.Format("{0}: {1}; ", prm.ParameterName, prm.Value.ToString());
                }


                errorMsg = String.Format(@"Call to ExecuteHaloAPI_UIS_OnCastleTran failed with no message; UIStatement: {0}-{1}, Parameters: {2}", applicationName, statementName, prmv);
                spc.DomainMessages.Add(new Error(errorMsg, errorMsg));
                throw new DomainValidationException(errorMsg);
            }
        }

        /// <summary>
        /// Requires a TransactionScope for Update
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="statementName"></param>
        /// <param name="parameters"></param>
        public void ExecuteHaloAPI_uiS_OnCastleTranForUpdate(string applicationName, string statementName, ParameterCollection parameters)
        {
            using (new TransactionScope(TransactionMode.Inherits))
            {
                ExecuteHaloAPI_UIStatement(applicationName, statementName, parameters);
            }
        }

        /// <summary>
        /// Does not require a TransactionScope for Read
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="statementName"></param>
        /// <param name="parameters"></param>
        public void ExecuteHaloAPI_uiS_OnCastleTranForRead(string applicationName, string statementName, ParameterCollection parameters)
        {
            ExecuteHaloAPI_UIStatement(applicationName, statementName, parameters);
        }

        private void ConvertDBParamsToDictionary(ParameterCollection sqlParameters, Dictionary<string, object> parametersDictionary)
        {
            foreach (SqlParameter param in sqlParameters)
            {
                try
                {
                    string name = param.ParameterName;
                    if (parametersDictionary.ContainsKey(name))
                    {
                        name = name + Guid.NewGuid();
                    }

                    parametersDictionary.Add(name, param.Value);
                }
                catch
                {
#if DEBUG
                    throw;
#endif
                }
            }
        }
    }

    public static class CastleTransactionsServiceHelper
    {
        static CastleTransactionsServiceHelper()
        {
            castleTransactionsService = new CastleTransactionsService();
        }

        private static ICastleTransactionsService castleTransactionsService;

        public static int ExecuteNonQueryOnCastleTran(string query, Type type, ParameterCollection parameters)
        {
            return castleTransactionsService.ExecuteNonQueryOnCastleTran(query, type, parameters);
        }

        public static DataSet ExecuteQueryOnCastleTran(string query, Type type, ParameterCollection parameters)
        {
            return castleTransactionsService.ExecuteQueryOnCastleTran(query, type, parameters);
        }

        public static void ExecuteHaloAPI_uiS_OnCastleTranForUpdate(string applicationName, string statementName, ParameterCollection parameters)
        {
            castleTransactionsService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate(applicationName, statementName, parameters);
        }

        public static void ExecuteHaloAPI_uiS_OnCastleTranForRead(string applicationName, string statementName, ParameterCollection parameters)
        {
            castleTransactionsService.ExecuteHaloAPI_uiS_OnCastleTranForRead(applicationName, statementName, parameters);
        }

        public static object ExecuteScalarOnCastleTran(string query, Type type, ParameterCollection parameters)
        {
            return castleTransactionsService.ExecuteScalarOnCastleTran(query, type, parameters);
        }

        public static IList<T> Many<T>(Globals.QueryLanguages language, string query, string sqlReturnDefinition, Globals.Databases database, params object[] parameters)
        {
            return castleTransactionsService.Many<T>(language, query, sqlReturnDefinition, database, parameters);
        }
    }
}