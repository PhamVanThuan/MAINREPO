using System.Data;

//using SAHL.Auditing;
using SAHL.Common.DataSets;

namespace SAHL.Common.DataAccess
{
    /// <summary>
    /// The transactionContext class contains all the required connections and information to allow us to
    /// atomically update data and submit relevant the audit trail for that data.
    /// </summary>
    /// <seealso cref="TransactionController"/>
    public class TransactionContext
    {
        public TransactionContext()
        {
        }

        public TransactionContext(IDbConnection p_DataConnection)
        {
            m_DataConnection = p_DataConnection;
        }

        public TransactionContext(IDbConnection p_DataConnection, IDbConnection p_AuditConnection)
        {
            m_DataConnection = p_DataConnection;
            m_AuditConnection = p_AuditConnection;
        }

        public void DisposeContext()
        {
            if (m_DataConnection != null)
                m_DataConnection.Close();
            if (m_AuditConnection != null)
                m_AuditConnection.Close();
        }

        #region variable declaration

        private IDbConnection m_DataConnection;
        private IDbTransaction m_DataTransaction;
        private IDbConnection m_AuditConnection;
        private IDbTransaction m_AuditTransaction;
        private Audits m_AuditInfo;
        //        private IDbCommand m_CurrentCommand;

        #endregion variable declaration

        /// <summary>
        /// This connection is used for access to the data datasource.  It has to be used for starting,
        /// committing and rolling back atomic data transactions.
        /// </summary>
        public IDbConnection DataConnection
        {
            get
            {
                return m_DataConnection;
            }
            set
            {
                m_DataConnection = value;
            }
        }

        /// <summary>
        /// A transaction that is started on the DataConnection is held here, in order to pass it to commands that
        /// execute on the DataConnection.
        /// </summary>
        public IDbTransaction DataTransaction
        {
            get
            {
                return m_DataTransaction;
            }
            set
            {
                m_DataTransaction = value;
            }
        }

        /// <summary>
        /// This connection is used for access to the auditing datasource.  It has to be used for starting,
        /// committing and rolling back atomic data transactions.
        /// </summary>
        public IDbConnection AuditConnection
        {
            get
            {
                return m_AuditConnection;
            }
            set
            {
                m_AuditConnection = value;
            }
        }

        /// <summary>
        /// A transaction that is started on the AuditConnection is held here, in order to pass it to commands that
        /// execute on the AuditConnection.
        /// </summary>
        public IDbTransaction AuditTransaction
        {
            get
            {
                return m_AuditTransaction;
            }
            set
            {
                m_AuditTransaction = value;
            }
        }

        /// <summary>
        /// the AuditInfo property contains the information required to meaningfully log audit and metric data.
        /// </summary>
        public Audits AuditInfo
        {
            get
            {
                return m_AuditInfo;
            }
            set
            {
                m_AuditInfo = value;
            }
        }

        #region this should not be required ever again

        //public IDbCommand CurrentCommand
        //{
        //    get
        //    {
        //        return m_CurrentCommand;
        //    }
        //}

        //public void ResetCommand()
        //{
        //    if (m_DataConnection != null)
        //    {
        //        if (m_DataConnection.State != ConnectionState.Open)
        //            m_DataConnection.Open();
        //        m_CurrentCommand = m_DataConnection.CreateCommand();
        //    }
        //    else
        //        m_CurrentCommand = null;
        //}

        //public void ReleaseCommand()
        //{
        //    m_CurrentCommand = null;
        //}

        #endregion this should not be required ever again
    }
}