using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using SAHL.Common.Logging;
using System.Collections.Generic;
using System.Reflection;

namespace SAHL.Common.DataAccess
{
    /// <summary>
    /// The helper class is provided for retrieval of data that can not be retrieved effectively through the domain model.
    /// No data manipulation should ever be done through this class.
    /// </summary>
    public class Helper
    {
        #region Connection Management

        /// <summary>
        /// Returns a IDBConnection for the given connectionsource
        /// </summary>
        /// <param name="connectionSource">the name of the connectionsource as defined in the configuration file.</param>
        /// <returns></returns>
        public static IDbConnection GetSQLDBConnection(string connectionSource)
        {
            //TransactionContext retval = new TransactionContext();
            string DataConnectionString = null;
            if (connectionSource != null)
                if (connectionSource != "")
                    DataConnectionString = Convert.ToString(SAHL.Common.Properties.Settings.Default[connectionSource]);

            if (DataConnectionString == null)
                DataConnectionString = SAHL.Common.Properties.Settings.Default.DBConnectionString;
            IDbConnection retval = (IDbConnection)new SqlConnection(DataConnectionString);
            return retval;
        }

        /// <summary>
        /// Returns a IDBConnection, using the given connection string.
        /// </summary>
        /// <param name="connectionString">the connection string to use.</param>
        /// <returns></returns>
        public static IDbConnection GetSQLDBConnectionFromConnectionString(string connectionString)
        {
            IDbConnection retval = (IDbConnection)new SqlConnection(connectionString);
            return retval;
        }

        /// <summary>
        /// Returns a IDBConnection to the default connection.
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetSQLDBConnection()
        {
            return GetSQLDBConnection(null);
        }

        public static string GetSAHLConnectionString()
        {
            return SAHL.Common.Properties.Settings.Default.SAHLConnectionString;
        }

        #endregion Connection Management

        static object syncObj = new object();

        #region Fill

        /// <summary>
        /// Fills a dataset from a procedurename and maps the results to the tablemappings sequentially.
        /// </summary>
        /// <param name="dataSet">The dataset to fill.</param>
        /// <param name="tableMappings">A list of tables to map sequentially.</param>
        /// <param name="applicationName">The application name as in uiStatement table.</param>
        /// <param name="procedureName">The name of the prodedure to execute.</param>
        /// <param name="connection">The connection to use, if closed, it</param>
        /// <param name="parameters">A list of parameters to be used to query.</param>
        public static void Fill(DataSet dataSet, StringCollection tableMappings, string applicationName, string procedureName, IDbConnection connection, ParameterCollection parameters)
        {
            bool WasOpened = false;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    WasOpened = true;
                }
                SqlDataAdapter Adaptor = new SqlDataAdapter();
                Adaptor.SelectCommand = (SqlCommand)connection.CreateCommand();
                Adaptor.SelectCommand.CommandText = UIStatementRepository.GetStatement(applicationName, procedureName);

                if (parameters != null)
                    parameters.TransferParameters(Adaptor.SelectCommand.Parameters);

                // Add the table mappings.
                if ((tableMappings != null) && (tableMappings.Count > 0))
                {
                    Adaptor.TableMappings.Add("Table", tableMappings[0]);
                    for (int i = 1; i < tableMappings.Count; i++)
                    {
                        Adaptor.TableMappings.Add("Table" + i, tableMappings[i]);
                    }
                }

                Adaptor.Fill(dataSet);
                Adaptor.SelectCommand.Parameters.Clear();
                Adaptor.Dispose();
                Adaptor = null;
            }
            finally
            {
                if ((WasOpened) && (connection != null))
                    connection.Close();
            }
        }

        /// <summary>
        /// Fills a table in a dataset from a procedurename.
        /// </summary>
        /// <param name="dataSet">The dataset to fill</param>
        /// <param name="tableName">The name of the table in the dataset to fill.</param>
        /// <param name="applicationName">The application name as in the uiStatement table.</param>
        /// <param name="procedureName">The name of the query to execute.</param>
        /// <param name="connection">The connection to use for the query, if it is closed, it will be opened, used and re-closed.</param>
        /// <param name="parameters">A list of parameters to be used to query.</param>
        public static void Fill(DataSet dataSet, string tableName, string applicationName, string procedureName, IDbConnection connection, ParameterCollection parameters)
        {
            bool WasOpened = false;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    WasOpened = true;
                }
                SqlDataAdapter Adaptor = new SqlDataAdapter();
                Adaptor.SelectCommand = (SqlCommand)connection.CreateCommand();
                Adaptor.SelectCommand.CommandText = UIStatementRepository.GetStatement(applicationName, procedureName);

                if (parameters != null)
                    parameters.TransferParameters(Adaptor.SelectCommand.Parameters);

                Adaptor.Fill(dataSet, tableName);

                Adaptor.SelectCommand.Parameters.Clear();
                Adaptor.Dispose();
                Adaptor = null;
            }
            finally
            {
                if ((WasOpened) && (connection != null))
                    connection.Close();
            }
        }

        /// <summary>
        /// Fills a  table by executing the query specified with by procedurename.
        /// </summary>
        /// <param name="dataTable">The data table to populate</param>
        /// <param name="applicationName">The application name as in the uiStatement table.</param>
        /// <param name="procedureName">The name of the query to execute.</param>
        /// <param name="connection">The connection to use for the query, if it is closed, it will be opened, used and re-closed.</param>
        /// <param name="parameters">A list of parameters to be used to query.</param>
        public static void Fill(DataTable dataTable, string applicationName, string procedureName, IDbConnection connection, ParameterCollection parameters)
        {
            bool WasOpened = false;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    WasOpened = true;
                }

                SqlDataAdapter Adaptor = new SqlDataAdapter();
                Adaptor.SelectCommand = (SqlCommand)connection.CreateCommand();
                Adaptor.SelectCommand.CommandText = UIStatementRepository.GetStatement(applicationName, procedureName);

                if (parameters != null)
                    parameters.TransferParameters(Adaptor.SelectCommand.Parameters);

                Adaptor.Fill(dataTable);

                Adaptor.SelectCommand.Parameters.Clear();
                Adaptor.Dispose();
                Adaptor = null;
            }
            finally
            {
                if ((WasOpened) && (connection != null))
                    connection.Close();
            }
        }

        /// <summary>
        /// Fills a dataset from a query and maps the results to the tablemappings sequentially.
        /// </summary>
        /// <param name="dataSet">The dataset to fill.</param>
        /// <param name="tableMappings">A list of tables to map sequentially.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="connection">The connection to use for the query, if it is closed, it will be opened, used and re-closed.</param>
        /// <param name="parameters">A list of parameters to be used to query.</param>
        public static void FillFromQuery(DataSet dataSet, StringCollection tableMappings, string query, IDbConnection connection, ParameterCollection parameters)
        {
            bool WasOpened = false;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    WasOpened = true;
                }
                SqlDataAdapter Adaptor = new SqlDataAdapter();
                Adaptor.SelectCommand = (SqlCommand)connection.CreateCommand();//new SqlCommand(p_sQuery, (SqlConnection)p_TransactionContext.DataConnection);
                Adaptor.SelectCommand.CommandText = query;

                if (parameters != null)
                    parameters.TransferParameters(Adaptor.SelectCommand.Parameters);

                // Add the table mappings.
                if ((tableMappings != null) && (tableMappings.Count > 0))
                {
                    Adaptor.TableMappings.Add("Table", tableMappings[0]);
                    for (int i = 1; i < tableMappings.Count; i++)
                    {
                        Adaptor.TableMappings.Add("Table" + i, tableMappings[i]);
                    }
                }

                Adaptor.Fill(dataSet);
                Adaptor.SelectCommand.Parameters.Clear();
                Adaptor.Dispose();
                Adaptor = null;
            }
            finally
            {
                if ((WasOpened) && (connection != null))
                    connection.Close();
            }
        }

        /// <summary>
        /// Fills a  table by executing the query.
        /// </summary>
        /// <param name="dataTable">The data table to populate.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="connection">The connection to use for the query, if it is closed, it will be opened, used and re-closed.</param>
        /// <param name="parameters">A list of parameters to be used to query.</param>
        /// <param name="reportCommandTimeout">The time to set before timing out.</param>
        public static void FillFromQuery(DataTable dataTable, string query, IDbConnection connection, ParameterCollection parameters, int reportCommandTimeout)
        {
            bool WasOpened = false;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    WasOpened = true;
                }

                SqlDataAdapter Adaptor = new SqlDataAdapter();
                Adaptor.SelectCommand = (SqlCommand)connection.CreateCommand();
                Adaptor.SelectCommand.CommandTimeout = reportCommandTimeout;
                Adaptor.SelectCommand.CommandText = query;

                if (parameters != null)
                    parameters.TransferParameters(Adaptor.SelectCommand.Parameters);

                Adaptor.Fill(dataTable);
                Adaptor.SelectCommand.Parameters.Clear();
                Adaptor.Dispose();
                Adaptor = null;
            }
            finally
            {
                if ((WasOpened) && (connection != null))
                    connection.Close();
            }
        }

        /// <summary>
        /// Fills a  table by executing the query.
        /// </summary>
        /// <param name="dataTable">The data table to populate.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="connection">The connection to use for the query, if it is closed, it will be opened, used and re-closed.</param>
        /// <param name="parameters">A list of parameters to be used to query.</param>
        public static void FillFromQuery(DataTable dataTable, string query, IDbConnection connection, ParameterCollection parameters)
        {
            bool WasOpened = false;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    WasOpened = true;
                }

                SqlDataAdapter Adaptor = new SqlDataAdapter();
                Adaptor.SelectCommand = (SqlCommand)connection.CreateCommand();
                Adaptor.SelectCommand.CommandText = query;

                if (parameters != null)
                    parameters.TransferParameters(Adaptor.SelectCommand.Parameters);

                Adaptor.Fill(dataTable);
                Adaptor.SelectCommand.Parameters.Clear();
                Adaptor.Dispose();
                Adaptor = null;
            }
            finally
            {
                if ((WasOpened) && (connection != null))
                    connection.Close();
            }
        }

        /// <summary>
        /// Fills a table in a dataset by executing the query.
        /// </summary>
        /// <param name="dataSet">The dataset to fill.</param>
        /// <param name="tableName">The name of the table in the dataset to fill.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="connection">The connection to use for the query, if it is closed, it will be opened, used and re-closed.</param>
        /// <param name="parameters">A list of parameters to be used to query.</param>
        public static void FillFromQuery(DataSet dataSet, string tableName, string query, IDbConnection connection, ParameterCollection parameters)
        {
            bool WasOpened = false;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    WasOpened = true;
                }
                SqlDataAdapter Adaptor = new SqlDataAdapter();
                Adaptor.SelectCommand = new SqlCommand(query, (SqlConnection)connection);
                Adaptor.SelectCommand.CommandText = query;

                if (parameters != null)
                    parameters.TransferParameters(Adaptor.SelectCommand.Parameters);

                Adaptor.Fill(dataSet, tableName);
                Adaptor.SelectCommand.Parameters.Clear();
                Adaptor.Dispose();
                Adaptor = null;
            }
            finally
            {
                if ((WasOpened) && (connection != null))
                    connection.Close();
            }
        }

        #endregion Fill

        #region Update

        public static void Update(DataTable DT, IDbConnection connection, string UpdateType, string commandText, ParameterCollection Parameters)
        {
            // create our Adaptor
            SqlDataAdapter Adaptor = new SqlDataAdapter();

            try
            {
                // initialize the Commands
                switch (UpdateType)
                {
                    case "update":
                        Adaptor.UpdateCommand = (SqlCommand)connection.CreateCommand();
                        //Adaptor.UpdateCommand.Transaction = new SqlTransaction();
                        DoCommandSetup(Adaptor.UpdateCommand, commandText, Parameters);
                        break;
                    default:
                        throw new NotSupportedException("This method currently only supports updates and should only be modified if necessary.");
                }
                //if (p_UpdateInfo.DeleteName != null)
                //{
                //    Adaptor.DeleteCommand = (SqlCommand)p_TransactionContext.DataConnection.CreateCommand();
                //    DoCommandSetup(Adaptor.DeleteCommand, p_UpdateInfo.ApplicationName, p_UpdateInfo.DeleteName, p_TransactionContext.DataTransaction, p_UpdateInfo.DeleteParameters, Transacting);
                //}
                //if (p_UpdateInfo.InsertName != null)
                //{
                //    Adaptor.InsertCommand = (SqlCommand)p_TransactionContext.DataConnection.CreateCommand();
                //    DoCommandSetup(Adaptor.InsertCommand, p_UpdateInfo.ApplicationName, p_UpdateInfo.InsertName, p_TransactionContext.DataTransaction, p_UpdateInfo.InsertParameters, Transacting);
                //}

                if (Adaptor.UpdateCommand.Connection.State != ConnectionState.Open)
                    Adaptor.UpdateCommand.Connection.Open();

                Adaptor.UpdateCommand.Transaction = Adaptor.UpdateCommand.Connection.BeginTransaction();

                Adaptor.Update(DT);

                Adaptor.UpdateCommand.Transaction.Commit();

                if (Adaptor.UpdateCommand.Connection.State == ConnectionState.Open)
                    Adaptor.UpdateCommand.Connection.Close();
            }
            catch (Exception ex)
            {
                Adaptor.UpdateCommand.Transaction.Rollback();
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, ex);
                throw;
            }
        }

        private static void DoCommandSetup(SqlCommand command, string commandText, ParameterCollection parameters)
        {
            command.CommandText = commandText;

            //check the parameters
            if (parameters != null)
            {
                command.UpdatedRowSource = UpdateRowSource.OutputParameters;
                parameters.TransferParameters(command.Parameters);
            }
        }

        #endregion Update

        #region ExecuteScalar

        /// <summary>
        /// Executes a query that returns a single value.
        /// </summary>
        /// <param name="connection">The connection to use for the query, if it is closed, it will be opened, used and re-closed.</param>
        /// <param name="commandText">The query to execute.</param>
        /// <param name="parameters">A list of parameters to be used to query.</param>
        /// <returns>The single value returned by the query as an object.</returns>
        public static object ExecuteScalar(IDbConnection connection, string commandText, ParameterCollection parameters)
        {
            object obj = null;
            bool WasOpened = false;
            IDbCommand cmd = null;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    WasOpened = true;
                }

                cmd = connection.CreateCommand();
                cmd.CommandText = commandText;

                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        SqlParameter param = parameters[i];
                        cmd.Parameters.Add(param);
                    }
                }

                obj = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, ex);
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Parameters.Clear();

                if (WasOpened)
                    if (connection != null)
                    {
                        connection.Close();
                    }
            }

            return obj;
        }

        /// <summary>
        /// Executes a query that returns a single value.
        /// </summary>
        /// <param name="connection">The connection to use for the query, if it is closed, it will be opened, used and re-closed.</param>
        /// <param name="commandText">The query to execute.</param>
        /// <returns>The single value returned by the query as an object.</returns>
        public static object ExecuteScalar(IDbConnection connection, string commandText)
        {
            return ExecuteScalar(connection, commandText, null);
        }

        #endregion ExecuteScalar

        #region ExecuteReader

        /// <summary>
        /// Executes a query that returns an IDataReader.  When using this method, you are responsible for
        /// the creation and cleanup of the connection.
        /// </summary>
        /// <param name="connection">The connection to use for the query.</param>
        /// <param name="commandText">The query to execute.</param>
        /// <param name="parameters">A list of parameters to be used to query.</param>
        /// <returns>The single value returned by the query as an object.</returns>
        public static IDataReader ExecuteReader(IDbConnection connection, string commandText, ParameterCollection parameters)
        {
            IDataReader dataReader = null;
            IDbCommand cmd = null;
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = commandText;

                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        SqlParameter param = parameters[i];
                        cmd.Parameters.Add(param);
                    }
                }

                dataReader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, ex);
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Parameters.Clear();
            }

            return dataReader;
        }

        /// <summary>
        /// Executes a query that returns a single value.
        /// </summary>
        /// <param name="connection">The connection to use for the query, if it is closed, it will be opened, used and re-closed.</param>
        /// <param name="commandText">The query to execute.</param>
        /// <returns>The single value returned by the query as an object.</returns>
        public static IDataReader ExecuteReader(IDbConnection connection, string commandText)
        {
            return ExecuteReader(connection, commandText, null);
        }

        #endregion ExecuteReader

        #region ExecuteNonQuery

        /// <summary>
        /// Executes a query that returns no rows.  THIS SHOULD NOT BE USED FOR DATA MANIPULATION.
        /// </summary>
        /// <param name="connection">The connection to use for the query, if it is closed, it will be opened, used and re-closed.</param>
        /// <param name="commandText">The query to execute.</param>
        /// <param name="parameters">A list of parameters to be used to query.</param>
        /// <returns>A value indicating the number of rows affected.</returns>
        public static int ExecuteNonQuery(IDbConnection connection, string commandText, ParameterCollection parameters)
        {
            bool WasOpened = false;
            IDbCommand cmd = null;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    WasOpened = true;
                }

                cmd = connection.CreateCommand();
                cmd.CommandText = commandText;

                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        SqlParameter param = parameters[i];
                        cmd.Parameters.Add(param);
                    }
                }

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, ex);
                throw; 
            }
            finally
            {
                if (cmd != null)
                    cmd.Parameters.Clear();

                if (WasOpened)
                    if (connection != null)
                    {
                        connection.Close();
                    }
            }
        }

        public static int ExecuteNonQuery(IDbConnection connection, string commandText, ParameterCollection parameters, int reportCommandTimeout)
        {
            bool WasOpened = false;
            IDbCommand cmd = null;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    WasOpened = true;
                }

                cmd = connection.CreateCommand();
                cmd.CommandText = commandText;
                cmd.CommandTimeout = reportCommandTimeout;

                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        SqlParameter param = parameters[i];
                        cmd.Parameters.Add(param);
                    }
                }

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, ex);
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Parameters.Clear();

                if (WasOpened)
                    if (connection != null)
                    {
                        connection.Close();
                    }
            }
        }

        /// <summary>
        /// Executes a query that returns no rows.  THIS SHOULD NOT BE USED FOR DATA MANIPULATION.
        /// </summary>
        /// <param name="connection">The connection to use for the query, if it is closed, it will be opened, used and re-closed.</param>
        /// <param name="applicationName">The name of the application in the uiStatementTable.</param>
        /// <param name="statementName">The statement from the uiStatement table to execute.</param>
        /// <param name="parameters">A list of parameters to be used to query.</param>
        /// <returns>A value indicating the number of rows affected.</returns>
        public static int ExecuteNonQuery(IDbConnection connection, string applicationName, string statementName, ParameterCollection parameters)
        {
            string commandText = UIStatementRepository.GetStatement(applicationName, statementName);

            bool WasOpened = false;
            IDbCommand cmd = null;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    WasOpened = true;
                }

                cmd = connection.CreateCommand();
                cmd.CommandText = commandText;

                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        SqlParameter param = parameters[i];
                        cmd.Parameters.Add(param);
                    }
                }

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, ex);
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Parameters.Clear();

                if (WasOpened)
                    if (connection != null)
                    {
                        connection.Close();
                    }
            }
        }

        /// <summary>
        /// Executes a query that returns no rows.  THIS SHOULD NOT BE USED FOR DATA MANIPULATION.
        /// </summary>
        /// <param name="connection">The connection to use for the query, if it is closed, it will be opened, used and re-closed.</param>
        /// <param name="commandText">The query to execute.</param>
        /// <returns>A value indicating the number of rows affected.</returns>
        public static int ExecuteNonQuery(IDbConnection connection, string commandText)
        {
            return ExecuteNonQuery(connection, commandText, null);
        }

        #endregion ExecuteNonQuery

        #region Parameter Management Methods

        public static void AddXmlParameter(ParameterCollection parameters, string name, System.Xml.XmlDocument xmlDoc)
        {
            AddParameter(parameters, name, SqlDbType.Xml, ParameterDirection.Input, xmlDoc);
        }

        public static void AddDateParameter(ParameterCollection parameters, string name, DateTime value)
        {
            AddParameter(parameters, name, SqlDbType.DateTime, ParameterDirection.Input, value);
        }

        public static void AddDateParameter(ParameterCollection parameters, string name, DateTime? value)
        {
            if (value.HasValue)
                AddParameter(parameters, name, SqlDbType.DateTime, ParameterDirection.Input, value);
            else
                AddParameter(parameters, name, SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
        }

        public static void AddBigIntParameter(ParameterCollection parameters, string name, Int64 value)
        {
            AddParameter(parameters, name, SqlDbType.BigInt, ParameterDirection.Input, value);
        }

        public static void AddIntParameter(ParameterCollection parameters, string name, int value)
        {
            AddParameter(parameters, name, SqlDbType.Int, ParameterDirection.Input, value);
        }

        public static void AddFloatParameter(ParameterCollection parameters, string name, float value)
        {
            AddParameter(parameters, name, SqlDbType.Float, ParameterDirection.Input, value);
        }

        public static void AddFloatParameter(ParameterCollection parameters, string name, double value)
        {
            AddParameter(parameters, name, SqlDbType.Float, ParameterDirection.Input, value);
        }

        public static void AddDecimalParameter(ParameterCollection parameters, string name, decimal value)
        {
            AddParameter(parameters, name, SqlDbType.Decimal, ParameterDirection.Input, value);
        }

        public static void AddVarcharParameter(ParameterCollection parameters, string name, string value)
        {
            if (value == null)
                AddNullParameter(parameters, name, ParameterDirection.Input);
            else
                AddParameter(parameters, name, SqlDbType.VarChar, ParameterDirection.Input, value);
        }

        public static void AddBitParameter(ParameterCollection parameters, string name, bool value)
        {
            AddParameter(parameters, name, SqlDbType.Bit, ParameterDirection.Input, value);
        }

        public static SqlParameter AddParameter(ParameterCollection parameters, string name, SqlDbType dbType, ParameterDirection direction, object value)
        {
            try
            {
                SqlParameter param = new SqlParameter(name, dbType);
                param.Direction = direction;
                param.Value = value;
                parameters.Add(param);
                return param;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, ex);
                throw;
            }
            return null;
        }

        public static SqlParameter AddNullParameter(ParameterCollection parameters, string name, ParameterDirection direction)
        {
            try
            {
                SqlParameter param = new SqlParameter(name, System.DBNull.Value);
                param.Direction = direction;
                parameters.Add(param);
                return param;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, ex);
                throw;
            }
            return null;
        }

        public static DbType ParseDbType(string sqlType)
        {
            switch (sqlType.ToLower())
            {
                case "bigint":
                    return DbType.Int64;

                case "binary":
                case "varbinary":
                case "image":
                    return DbType.Binary;

                case "bit":
                    return DbType.Boolean;

                case "char":
                case "text":
                    return DbType.AnsiStringFixedLength;

                case "datetime":
                case "smalldatetime":
                case "timestamp":
                    return DbType.DateTime;

                case "decimal":
                    return DbType.Decimal;

                case "float":
                    return DbType.Single;

                case "int":
                    return DbType.Int32;

                case "money":
                case "smallmoney":
                    return DbType.Currency;

                case "nchar":
                case "ntext":
                    return DbType.StringFixedLength;

                case "nvarchar":
                case "varchar":
                    return DbType.String;

                case "real":
                    return DbType.Double;

                case "smallint":
                    return DbType.Int16;

                case "tinyint":
                    return DbType.Byte;

                case "uniqueidentifier":
                    return DbType.Guid;

                case "udt":
                case "variant":
                    return DbType.Object;

                case "xml":
                    return DbType.Xml;

                default:
                    return DbType.Object;
            }
        }

        public static SqlDbType ParseSqlDbType(string sqlType)
        {
            switch (sqlType.ToLower())
            {
                case "int":
                    return SqlDbType.Int;
                case "varchar":
                    return SqlDbType.VarChar;
                case "datetime":
                    return SqlDbType.DateTime;
                case "float":
                    return SqlDbType.Float;
                case "bit":
                    return SqlDbType.Bit;
                case "real":
                    return SqlDbType.Real;
                case "smallint":
                    return SqlDbType.SmallInt;
                case "tinyint":
                    return SqlDbType.TinyInt;
                case "bigint":
                    return SqlDbType.BigInt;
                case "char":
                    return SqlDbType.Char;
                case "text":
                    return SqlDbType.Text;
                case "smalldatetime":
                    return SqlDbType.SmallDateTime;
                case "timestamp":
                    return SqlDbType.Timestamp;
                case "decimal":
                    return SqlDbType.Decimal;
                case "money":
                    return SqlDbType.Money;
                case "smallmoney":
                    return SqlDbType.SmallMoney;
                case "nchar":
                    return SqlDbType.NChar;
                case "ntext":
                    return SqlDbType.NText;
                case "nvarchar":
                    return SqlDbType.NVarChar;
                case "uniqueidentifier":
                    return SqlDbType.UniqueIdentifier;
                case "udt":
                    return SqlDbType.Udt;
                case "variant":
                    return SqlDbType.Variant;
                case "xml":
                    return SqlDbType.Xml;
                case "binary":
                    return SqlDbType.Binary;
                case "varbinary":
                    return SqlDbType.VarBinary;
                case "image":
                    return SqlDbType.Image;
                default:
                    return SqlDbType.VarChar;
            }
        }

        public static string ParseQuotes(string parametervalue)
        {
            return (parametervalue.Replace("'", "''"));
        }

        #endregion Parameter Management Methods

        #region Linked Parameter Management Methods

        public static void AddLinkedDateParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.DateTime, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a interger typed linked parameter.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedIntParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.Int, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a tiny integer (single byte) typed linked parameter.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedTinyIntParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.TinyInt, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a small integer (two bytes) typed linked parameter.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedSmallIntParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.SmallInt, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a big interger typed linked parameter.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedBigIntParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.BigInt, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a xml typed linked parameter.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedXmlParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.Xml, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a linked parameter of type varchar (string).
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedVarcharParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.VarChar, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a linked parameter of type bit.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedBitParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.Bit, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a linked parameter of type float.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedFloatParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.Float, ParameterDirection.Input, p_sSourceName);
        }

        public static void AddLinkedDecimalParameter(ParameterCollection p_Parameters, string p_sName, string p_SourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.Decimal, ParameterDirection.Input, p_SourceName);
        }

        /// <summary>
        /// Add a linked parameter of type money.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedMoneyParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.Money, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a linked parameter to the collection. All properties can be passed in.
        /// </summary>
        /// <param name="p_Parameters"> The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_dbType">The type of the parameter to add.</param>
        /// <param name="p_Direction">The direction of the parameter.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedParameter(ParameterCollection p_Parameters, string p_sName, SqlDbType p_dbType, ParameterDirection p_Direction, string p_sSourceName)
        {
            try
            {
                SqlParameter param = new SqlParameter(p_sName, p_dbType);
                param.Direction = p_Direction;
                param.SourceColumn = p_sSourceName;
                p_Parameters.Add(param);
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, ex);
                throw;
            }
        }

        /// <summary>
        /// Add a linked parameter to the collection. All properties can be passed in.
        /// </summary>
        /// <param name="p_Parameters"> The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        /// <param name="p_iSize">The size (length) of the varchar column in the dataset.</param>
        public static void AddLinkedVarcharOutputParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName, int p_iSize)
        {
            try
            {
                SqlParameter param = new SqlParameter(p_sName, SqlDbType.VarChar);
                param.Direction = ParameterDirection.Output;
                param.Size = p_iSize;
                param.SourceColumn = p_sSourceName;
                p_Parameters.Add(param);
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, ex);
                throw;
            }
        }

        #endregion Linked Parameter Management Methods

        #region Map Result Set To Object
        public static List<T> Many<T>(string query, ParameterCollection parameters) where T : class, new()
        {
            IDbConnection connection = Helper.GetSQLDBConnection();
            using (connection)
            {
                connection.Open();
                IDataReader reader = Helper.ExecuteReader(connection, query, parameters);
                List<T> list = new List<T>();
                using (reader)
                {
                    var schema = reader.GetSchemaTable();

                    while (reader.Read())
                    {
                        T t = new T();
                        //Determine whether the properties on the type exists for the record
                        foreach (PropertyInfo propertyInfo in t.GetType().GetProperties())
                        {
                            if (reader[propertyInfo.Name] != null)
                            {
                                //Determine whether the type is the same,
                                //Easily assign the value
                                if (propertyInfo.PropertyType == reader[propertyInfo.Name].GetType())
                                {
                                    propertyInfo.SetValue(t, reader[propertyInfo.Name], null);
                                }
                                else //We probably have to do some conversion
                                {
                                    if (Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null)
                                    {
                                        object value = reader[propertyInfo.Name];
                                        if (value == DBNull.Value)
                                        {
                                            value = null;
                                        }
                                        propertyInfo.SetValue(t, value, null);
                                    }
                                }
                            }
                        }
                        list.Add(t);
                    }
                }
                return list;
            }
        }
        #endregion

    }
}