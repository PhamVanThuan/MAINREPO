using System;
using System.Data;
using System.Data.SqlClient;
using SAHL.X2.Common.DataAccess;

//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace SAHL.X2.Framework.DataAccess
{
    public enum HelperAction
    {
        select,
        update
    }

    public class DBHelperException : Exception
    {
        public DBHelperException()
            : base()
        { }

        public DBHelperException(string message)
            : base(message)
        { }

        public DBHelperException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    /// <summary>
    /// DBHelper is a wrapper for database access activities, it hides datam_Adapterter actions for the user,
    /// taking command names, as stored in the DB commmand repository and parameters and executes an internal m_Adapterter
    /// to either fill or update a dataset that's passed in.  Additional support of a passed in m_Connectionection is provided.
    /// </summary>
    public class DBHelper : IDisposable
    {
        protected SqlConnection m_Connection = null;
        protected SqlDataAdapter m_Adapter = null;
        protected DataSet m_DataSet = null;
        protected bool isManagedConnection = false;
        public DBHelper(string Command, CommandType type)
        {
            m_Connection = new SqlConnection(Properties.Settings.Default.DBConnectionString);
            m_Adapter = new SqlDataAdapter(Command, m_Connection);

            m_Adapter.SelectCommand.CommandType = type;
        }

        public DBHelper(string Command, string ConnectionString, CommandType type)
        {
            m_Connection = new SqlConnection(ConnectionString);
            m_Adapter = new SqlDataAdapter(Command, m_Connection);
            m_Adapter.SelectCommand.CommandType = type;
        }

        public DBHelper(ITransactionContext Context, string Command, CommandType type)
        {
            m_Connection = Context.DataConnection as SqlConnection;
            isManagedConnection = true;
            m_Adapter = new SqlDataAdapter(Command, m_Connection);

            m_Adapter.SelectCommand.CommandType = type;
        }

        public DBHelper(string Command)
        {
            m_Connection = new SqlConnection(Properties.Settings.Default.DBConnectionString);
            m_Adapter = new SqlDataAdapter(Command, m_Connection);

            m_Adapter.SelectCommand.CommandType = CommandType.Text;
        }

        public DBHelper(string Command, HelperAction Action, CommandType type)
        {
            m_Connection = new SqlConnection(Properties.Settings.Default.DBConnectionString);
            switch (Action)
            {
                case (HelperAction.select):
                    m_Adapter = new SqlDataAdapter();
                    break;
                case (HelperAction.update):
                    break;
            }
        }

        public SqlDataAdapter Adapter
        {
            get { return m_Adapter; }
        }

        public SqlParameterCollection Parameters
        {
            get { return m_Adapter.SelectCommand.Parameters; }
        }

        public DataTable Fill(DataTable table)
        {
            try
            {
                m_Adapter.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet Fill()
        {
            try
            {
                if (m_DataSet == null)
                    m_DataSet = new DataSet();
                m_Adapter.Fill(m_DataSet);

                return m_DataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteNonQuery()
        {
            try
            {
                m_Connection.Open();
                return m_Adapter.SelectCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_Connection.Close();
            }
        }

        public void AddDateParameter(string Name, object value)
        {
            AddParameter(Name, SqlDbType.DateTime, ParameterDirection.Input, value);
        }

        public void AddIntParameter(string Name, object value)
        {
            AddParameter(Name, SqlDbType.Int, ParameterDirection.Input, value);
        }

        public void AddVarcharParameter(string Name, object value)
        {
            AddParameter(Name, SqlDbType.VarChar, ParameterDirection.Input, value);
        }

        public void AddParameter(string Name, SqlDbType dbType, ParameterDirection Direction, object value)
        {
            try
            {
                SqlParameter parm = new SqlParameter(Name, dbType);
                parm.Direction = Direction;
                parm.Value = value;
                Parameters.Add(parm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Table(int index)
        {
            return m_DataSet.Tables[index];
        }

        public DataTable Table(string Name)
        {
            return m_DataSet.Tables[Name];
        }

        public DataTable FirstTable()
        {
            return m_DataSet.Tables[0];
        }

        public DataTable CurrentTable()
        {
            return Table(m_CurrentTableCounter);
        }

        public DataTable NextTable()
        {
            if (m_DataSet.Tables.Count > 0)
            {
                try
                {
                    if (++m_CurrentTableCounter >= TableCount)
                        m_CurrentTableCounter = 0;
                    return m_DataSet.Tables[m_CurrentTableCounter];
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public DataTable PrevTable()
        {
            if (m_DataSet.Tables.Count > 0)
            {
                try
                {
                    if (--m_CurrentTableCounter < 0)
                        m_CurrentTableCounter = TableCount - 1;
                    return m_DataSet.Tables[m_CurrentTableCounter];
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public DataRow FirstRow()
        {
            if (RowCount > 0)
            {
                try
                {
                    return CurrentTable().Rows[0];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return null;
        }

        public DataRow NextRow()
        {
            if (RowCount > 0)
            {
                try
                {
                    if (++m_CurrentRowCounter >= RowCount)
                        m_CurrentRowCounter = 0;
                    return CurrentTable().Rows[m_CurrentRowCounter];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return null;
        }

        public DataRow PrevRow()
        {
            if (RowCount > 0)
            {
                try
                {
                    if (--m_CurrentRowCounter < 0)
                        m_CurrentRowCounter = RowCount - 1;
                    return CurrentTable().Rows[m_CurrentRowCounter];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return null;
        }

        public string CommandText
        {
            get { return Command.CommandText; }
            set { Command.CommandText = value; }
        }

        public SqlCommand Command
        {
            get { return m_Adapter.SelectCommand; }
            set
            {
                m_Adapter.SelectCommand = value;
            }
        }

        public int TableCount
        {
            get
            {
                if (m_DataSet != null)
                    return m_DataSet.Tables.Count;
                else return 0;
            }
        }

        /// <summary>
        /// Returns the rowcount in the current datatable of the internal helper dataset.
        /// </summary>
        public int RowCount
        {
            get
            {
                try
                {
                    if (CurrentTable() == null)
                        throw new DBHelperException("CurrentTable not available.");
                    return CurrentTable().Rows.Count;
                }
                catch
                {
                    throw;
                }
            }
        }

        private int m_CurrentTableCounter = 0;
        private int m_CurrentRowCounter = 0;

        public void Dispose()
        {
            if (m_Adapter != null)
            {
                m_Adapter.Dispose();
                m_Adapter = null;
            }

            if (m_Connection != null && isManagedConnection == false)
            {
                m_Connection.Dispose();
                m_Connection = null;
            }


            if (m_DataSet != null)
            {
                m_DataSet.Dispose();
                m_DataSet = null;
            }

        }
    }
}