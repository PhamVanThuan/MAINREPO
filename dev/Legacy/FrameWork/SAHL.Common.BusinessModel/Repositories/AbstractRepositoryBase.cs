using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Logging;
using SAHL.Common.Security;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    /// A base class for all repositories, providing common generic functionality they can all use.
    /// </summary>
    public abstract class AbstractRepositoryBase
    {
        /// <summary>
        /// Gets an entity by its primary key. This is a useful helper function to use inside any repository to
        /// get any business object by its primary key.
        /// </summary>
        /// <typeparam name="TInterface">The Interface to map to.</typeparam>
        /// <typeparam name="TDAO">The DAO object to map from.</typeparam>
        /// <param name="Key">The primary key of the business object to find.</param>
        /// <returns>Returns the business object for the primary key value.</returns>
        protected TInterface GetByKey<TInterface, TDAO>(int Key) where TDAO : class
        {
            TDAO DAO = ActiveRecordMediator<TDAO>.FindByPrimaryKey(Key);
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return BMTM.GetMappedType<TInterface, TDAO>(DAO);
        }

        /// <summary>
        /// Creates and empty business object. This is a useful helper function to use inside any repository to
        /// create an empty business object.
        /// </summary>
        /// <typeparam name="TInterface">The Interface to map to.</typeparam>
        /// <typeparam name="TDAO">The DAO object to map from.</typeparam>
        /// <returns>A new instance of the businessobject wrapped in an interface.</returns>
        protected TInterface CreateEmpty<TInterface, TDAO>() where TDAO : class, new()
        {
            TDAO DAO = new TDAO();
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return BMTM.GetMappedType<TInterface, TDAO>(DAO);
        }

        /// <summary>
        /// Saves a business object. This is a useful helper function to use inside any repository to
        /// save any business object.
        /// </summary>
        /// <typeparam name="TInterface">The Interface to map to.</typeparam>
        /// <typeparam name="TDAO">The DAO object to map from.</typeparam>
        /// <param name="ObjectToSave">The object to save to the database.</param>
        protected void Save<TInterface, TDAO>(TInterface ObjectToSave) where TDAO : class
        {
            IDAOObject daoobj = ObjectToSave as IDAOObject;
            TDAO dao = (TDAO)daoobj.GetDAOObject();
            ActiveRecordMediator<TDAO>.SaveAndFlush(dao);

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        private static object ProcessQuery(NHibernate.ISession session, object data)
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

        private static object ProcessNonQuery(NHibernate.ISession session, object data)
        {
            IDbConnection conn = session.Connection;
            IDbCommand cmd = data as IDbCommand;
            cmd.Connection = conn;
            if (session.Transaction != null && session.Transaction.IsActive)
            {
                session.Transaction.Enlist(cmd);
            }
            return cmd.ExecuteNonQuery();
        }

        private static object FillFromQuery(NHibernate.ISession session, object data)//(DataSet dataSet, StringCollection tableMappings, string query, IDbConnection connection, ParameterCollection parameters)
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

        public static DataSet ExecuteQueryOnCastleTran(string query, Type Ty, ParameterCollection parameters, StringCollection tablemappings, DataSet ds)
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

        public static int ExecuteNonQueryOnCastleTran(string query, Type Ty, ParameterCollection parameters)
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

        public static DataSet ExecuteQueryOnCastleTran(string query, Type Ty, ParameterCollection parameters)
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

        public static object ExecuteScalarOnCastleTran(string query, Type Ty, ParameterCollection parameters)
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
                            return ds.Tables[0].Rows[0][0];
                }
                return null;
            }
        }

        public static void FillFromQueryOnCastleTran(DataSet dataSet, string tableName, string query, Type Ty, ParameterCollection parameters)
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

        public static void FillFromQueryOnCastleTran(DataTable datatable, string query, Type Ty, ParameterCollection parameters)
        {
            DataSet tmp = new DataSet();
            tmp.Tables.Add(datatable);
            FillFromQueryOnCastleTran(tmp, datatable.TableName, query, Ty, parameters);
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
        private static void ExecuteHaloAPI_UIStatement(string applicationName, string statementName, ParameterCollection parameters)
        {
            using (IDbCommand cmd = new SqlCommand())
            {
                //get the sql to execute
                cmd.CommandText = UIStatementRepository.GetStatement(applicationName, statementName);
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
                    LogPlugin.Logger.LogFormattedErrorWithException("ExecuteHaloAPI_UIStatement", "Error Executing Halo API: {0} - {1}", ex, new Dictionary<string, object>() { { "StatementName", statementName } }, applicationName, statementName);
                    errorMsg = String.Format(@"ExecuteHaloAPI_UIS_OnCastleTran Exception in {0}-{1}: {2}", applicationName, statementName, ex.Message);
                    spc.DomainMessages.Add(new Error(errorMsg, errorMsg));
                    throw;
                }

                if (prmRes != null && prmRes.Value != null)
                {
                    ret = Convert.ToInt32(prmRes.Value.ToString());
                    if (ret == 0) //we have a successful response
                        return;
                }

                if (msgPrm != null && msgPrm.Value != null)
                {
                    errorMsg = String.Format(@"ExecuteHaloAPI_UIS_OnCastleTran Exception in {0}-{1}: {2} return code was: {3}", applicationName, statementName, msgPrm.Value.ToString(), ret);
                    spc.DomainMessages.Add(new Error(errorMsg, errorMsg));
                    throw new DomainValidationException(errorMsg);
                }

                string prmv = String.Empty;
                foreach (SqlParameter prm in parameters)
                    prmv += String.Format("{0}: {1}; ", prm.ParameterName, prm.Value.ToString());

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
        public static void ExecuteHaloAPI_uiS_OnCastleTranForUpdate(string applicationName, string statementName, ParameterCollection parameters)
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
        public static void ExecuteHaloAPI_uiS_OnCastleTranForRead(string applicationName, string statementName, ParameterCollection parameters)
        {
            ExecuteHaloAPI_UIStatement(applicationName, statementName, parameters);
        }
    }
}