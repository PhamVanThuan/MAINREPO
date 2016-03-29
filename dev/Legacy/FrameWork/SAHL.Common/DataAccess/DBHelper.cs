using System;
using System.Data;
using System.Data.SqlClient;
using SAHL.Common.Globals;
using SAHL.Common.Properties;
using SAHL.Common.Logging;

namespace SAHL.Common.DataAccess
{
    /// <summary>
    /// Database helper class for accessing SAHL databases.  This is a newer improved version of the
    /// <see cref="Helper"/> which wasn't object-oriented and didn't expose some useful properties
    /// e.g. the Connection, Command, etc.
    /// </summary>
    public sealed class DBHelper : IDisposable
    {
        private IDbConnection _connection;

        #region Constructors

        /// <summary>
        /// Default constructor.  If this is used, the <see cref="Connection"/> property will return null - you will
        /// need to manually set the connection.
        /// </summary>
        public DBHelper()
        {
        }

        /// <summary>
        /// Creates a new DBHelper object with the <see cref="Connection"/> property intialised and open.
        /// </summary>
        /// <param name="db">The database to open the connection to.</param>
        public DBHelper(Databases db)
            : this(db, true)
        {
        }

        /// <summary>
        /// Creates a new DBHelper object with the <see cref="Connection"/> property intialised.  The connection will
        /// be opened in <c>openConnection</c> is set to <c>true</c>.
        /// </summary>
        /// <param name="db">The database to open the connection to.</param>
        /// <param name="openConnection">Whether to open the connection on creation.</param>
        public DBHelper(Databases db, bool openConnection)
        {
            string connString = String.Empty;
            switch (db)
            {
                case Databases.Batch:
                    connString = Settings.Default.BatchConnectionString;
                    break;
                case Databases.EWork:
                    connString = Settings.Default.EWorkConnectionString;
                    break;
                case Databases.ImageIndex:
                    connString = Settings.Default.ImageIndexConnectionString;
                    break;
                case Databases.SAHLDB:
                    connString = Settings.Default.SAHLConnectionString;
                    break;
                case Databases.TwoAM:
                    connString = Settings.Default.DBConnectionString;
                    break;
                case Databases.Warehouse:
                    connString = Settings.Default.Warehouse;
                    break;
                case Databases.X2:
                    connString = Settings.Default.X2;
                    break;
            }

            CreateConnection(connString, openConnection);
        }

        /// <summary>
        /// Creates a new DBHelper object with the <see cref="Connection"/> property intialised and open.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        public DBHelper(string connectionString)
            : this(connectionString, true)
        {
        }

        /// <summary>
        /// Creates a new DBHelper object with the <see cref="Connection"/> property intialised.  The connection will
        /// be opened in <c>openConnection</c> is set to <c>true</c>.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="openConnection">Whether to open the connection on creation.</param>
        public DBHelper(string connectionString, bool openConnection)
        {
            CreateConnection(connectionString, openConnection);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets/sets the connection used by the DBHelper.
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates an empty IDbCommand object.
        /// </summary>
        /// <returns>A new IDbCommand.</returns>
        public IDbCommand CreateCommand()
        {
            return CreateCommand(null, null);
        }

        /// <summary>
        /// Creates an IDbCommand initialized with CommandText set to <c>commandText</c>.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns>A new IDbCommand.</returns>
        public IDbCommand CreateCommand(string commandText)
        {
            return CreateCommand(commandText, null);
        }

        /// <summary>
        /// Creates an IDbCommand initialized with CommandText set to <c>commandText</c> and
        /// a list of parameters.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns>A new IDbCommand.</returns>
        public IDbCommand CreateCommand(string commandText, ParameterCollection parameters)
        {
            if (Connection == null)
                throw new DataException("Unable to create command: Connection property uninitialized");

            IDbCommand command = new SqlCommand();
            command.Connection = Connection;

            if (!String.IsNullOrEmpty(commandText))
                command.CommandText = commandText;

            if (parameters != null)
            {
                foreach (SqlParameter p in parameters)
                    command.Parameters.Add(p);
            }

            return command;
        }

        /// <summary>
        /// Creates a connection object from a connection string.
        /// </summary>
        /// <param name="connString">The connection string.</param>
        /// <param name="open">Whether to open the connection.</param>
        /// <remarks>If <see cref="Connection"/> is not null, the connection will be disposed.</remarks>
        public void CreateConnection(string connString, bool open)
        {
            if (_connection != null)
                _connection.Dispose();

            _connection = (IDbConnection)new SqlConnection(connString);
            if (open)
                _connection.Open();
        }

        #region ExecuteNonQuery Overloads

        /// <summary>
        /// Executes a statement with no return values.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        public void ExecuteNonQuery(IDbCommand command)
        {
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, ex);
                throw;
            }
        }

        /// <summary>
        /// Executes a statement with no return values.
        /// </summary>
        /// <param name="commandText">The query to execute.</param>
        public void ExecuteNonQuery(string commandText)
        {
            IDbCommand cmd = null;
            try
            {
                cmd = CreateCommand(commandText);
                ExecuteNonQuery(cmd);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        /// <summary>
        /// Executes a statement with no return values.
        /// </summary>
        /// <param name="commandText">The query to execute.</param>
        /// <param name="parameters">A list of parameters to be used to query.</param>
        public void ExecuteNonQuery(string commandText, ParameterCollection parameters)
        {
            IDbCommand cmd = null;
            try
            {
                cmd = CreateCommand(commandText, parameters);
                ExecuteNonQuery(cmd);
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }

        #endregion ExecuteNonQuery Overloads

        #region ExecuteReader Overloads

        /// <summary>
        /// Executes a statement returning an IDataReader
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>The IDataReader containing data for the executed command.</returns>
        public IDataReader ExecuteReader(IDbCommand command)
        {
            IDataReader reader = null;
            try
            {
                reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, ex);
                throw;
            }
            return reader;
        }

        /// <summary>
        /// Executes a statement returning an IDataReader
        /// </summary>
        /// <param name="commandText">The query to execute.</param>
        /// <returns>The IDataReader containing data for the executed command.</returns>
        public IDataReader ExecuteReader(string commandText)
        {
            IDbCommand cmd = null;
            try
            {
                cmd = CreateCommand(commandText);
                return ExecuteReader(cmd);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        /// <summary>
        /// Executes a statement returning an IDataReader
        /// </summary>
        /// <param name="commandText">The query to execute.</param>
        /// <param name="parameters">A list of parameters to be used to query.</param>
        /// <returns>The IDataReader containing data for the executed command.</returns>
        public IDataReader ExecuteReader(string commandText, ParameterCollection parameters)
        {
            IDbCommand cmd = null;
            try
            {
                cmd = CreateCommand(commandText, parameters);
                return ExecuteReader(cmd);
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }

        #endregion ExecuteReader Overloads

        #region ExecuteScalar Overloads

        /// <summary>
        /// Executes a query that returns a single value.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>The single value returned by the query as an object.</returns>
        public object ExecuteScalar(IDbCommand command)
        {
            object scalar = null;
            try
            {
                scalar = command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, ex);
                throw;
            }
            return scalar;
        }

        /// <summary>
        /// Executes a query that returns a single value.
        /// </summary>
        /// <param name="commandText">The query to execute.</param>
        /// <returns>The single value returned by the query as an object.</returns>
        public object ExecuteScalar(string commandText)
        {
            IDbCommand cmd = null;
            try
            {
                cmd = CreateCommand(commandText);
                return ExecuteScalar(cmd);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        /// <summary>
        /// Executes a query that returns a single value.
        /// </summary>
        /// <param name="commandText">The query to execute.</param>
        /// <param name="parameters">A list of parameters to be used to query.</param>
        /// <returns>The single value returned by the query as an object.</returns>
        public object ExecuteScalar(string commandText, ParameterCollection parameters)
        {
            IDbCommand cmd = null;
            try
            {
                cmd = CreateCommand(commandText, parameters);
                return ExecuteScalar(cmd);
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }

        #endregion ExecuteScalar Overloads

        #region Fill Overloads

        /// <summary>
        /// Fills a DataTable by executing the <c>query</c> specified.
        /// </summary>
        /// <param name="dataTable">The data table to populate</param>
        /// <param name="query">The query to execute.</param>
        public void Fill(DataTable dataTable, string query)
        {
            Fill(dataTable, CreateCommand(query), true);
        }

        /// <summary>
        /// Fills a DataTable by executing the <c>query</c> specified.
        /// </summary>
        /// <param name="dataTable">The data table to populate</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="parameters">Parameters to add to the query.</param>
        public void Fill(DataTable dataTable, string query, ParameterCollection parameters)
        {
            Fill(dataTable, CreateCommand(query, parameters), true);
        }

        /// <summary>
        /// Fills a DataTable by executing the <c>command</c> query specified.  The <c>command</c>
        /// object will NOT be disposed and will need to be cleaned up by the calling
        /// application.
        /// </summary>
        /// <param name="dataTable">The data table to populate</param>
        /// <param name="command">The command to execute.</param>
        public void Fill(DataTable dataTable, IDbCommand command)
        {
            Fill(dataTable, command, false);
        }

        /// <summary>
        /// Fills a DataTable by executing the <c>command</c> query specified.
        /// </summary>
        /// <param name="dataTable">The data table to populate</param>
        /// <param name="command">The command to execute.</param>
        /// <param name="disposeCommand">If set to <c>true</c>, the <c>command</c> object will be disposed after the query is executed.</param>
        public void Fill(DataTable dataTable, IDbCommand command, bool disposeCommand)
        {
            using (SqlDataAdapter sqlAdapter = new SqlDataAdapter())
            {
                SqlCommand sqlCommand = command as SqlCommand;

                if (sqlCommand == null)
                    throw new NotSupportedException("DBHelper.Fill currently only supports SqlCommand objects");

                sqlAdapter.SelectCommand = sqlCommand;
                sqlAdapter.Fill(dataTable);

                if (disposeCommand)
                    command.Dispose();
            }
        }

        #endregion Fill Overloads

        #endregion Methods

        #region IDisposable Members

        /// <summary>
        /// Closes the underlying connection.
        /// </summary>
        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
        }

        #endregion IDisposable Members
    }
}