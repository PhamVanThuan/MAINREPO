using System;
using System.Data;
using SAHL.X2.Common.DataAccess;
using SAHL.X2.Framework.DataSets;

namespace SAHL.X2.Framework.DataAccess
{
    /// <summary>
    /// The transactionContext class contains all the required connections and information to allow us to
    /// atomically update data and submit relevant the audit trail for that data.
    /// </summary>
    /// <seealso cref="TransactionController"/>
    [Serializable]
    public class TransactionContext : ITransactionContext
    {
        public TransactionContext()
        {
        }

        internal TransactionContext(IDbConnection p_DataConnection)
        {
            m_DataConnection = p_DataConnection;
        }

        internal TransactionContext(IDbConnection p_DataConnection, IDbConnection p_AuditConnection)
        {
            m_DataConnection = p_DataConnection;
            m_AuditConnection = p_AuditConnection;
        }

        int m_UniqueID;

        public int UniqueID
        {
            get
            {
                return m_UniqueID;
            }
            set
            {
                m_UniqueID = value;
            }
        }

        public void Dispose()
        {
            if (m_DataConnection != null)
            {
                m_DataConnection.Close();
                m_DataConnection.Dispose();
                m_DataConnection = null;
            }
            if (m_DataTransaction != null)
            {
                m_DataTransaction.Dispose();
                m_DataTransaction = null;
            }
            if (m_AuditConnection != null)
            {
                m_AuditConnection.Close();
                m_AuditConnection.Dispose();
                m_AuditConnection = null;
            }
            if (m_AuditTransaction != null)
            {
                m_AuditTransaction.Dispose();
                m_AuditTransaction = null;
            }
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
    }
}