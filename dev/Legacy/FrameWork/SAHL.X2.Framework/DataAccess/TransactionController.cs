using System;
using System.Data;
using System.Data.SqlClient;
using SAHL.X2.Common.DataAccess;

namespace SAHL.X2.Framework.DataAccess
{
    /// <summary>
    /// The TransactionController provides instances of the TransactionContext class.  Depending on the method that is called
    /// the Context will be populated in different manners.
    /// </summary>
    public static class TransactionController
    {
        #region public transaction generation

        public static ITransactionContext GetContext()
        {
            return GetSelectContext(null);
        }

        /// <summary>
        /// A TransactionContext with only a DataConnection is returned. No Audit connection, or transactions are created.
        /// This method will is used for select statements, as well as Metric inserts (they aren't Audited).
        /// </summary>
        /// <param name="p_DataConnectionSource"> The name of the connectionstring in the applicatoin settings to use for the Data Connection</param>
        /// <returns>TransactionContext</returns>
        public static ITransactionContext GetContext(string p_DataConnectionSource)
        {
            return GetSelectContext(p_DataConnectionSource);
        }

        /// <summary>
        /// A TransactionContext with Data and Audit Connection as well as transactions for both is returned. This method is
        /// used for inserts where non-default datasources for the Audit and Data connections.
        /// </summary>
        /// <param name="p_DataConnectionSource"> The name of the connectionstring in the applicatoin settings to use for the Data Connection</param>
        /// <param name="p_AuditConnectionSource"> The name of the connectionstring in the applicatoin settings to use for the Audit Connection</param>
        /// <returns>TransactionContext</returns>
        public static ITransactionContext GetContext(string p_DataConnectionSource, string p_AuditConnectionSource/*, Metrics p_MetricInfo*/)
        {
            ITransactionContext retval = GetUpdateContext(p_DataConnectionSource);
            return retval;
        }

        /// <summary>
        /// A TransactionContext with Data and Audit Connection as well as transactions for both is returned. This method is
        /// used for inserts where non-default datasources for the Audit and Data connections.
        /// </summary>
        /// <param name="p_DataConnection"> The actual connectionstring to use for the Data Connection</param>
        /// <returns>TransactionContext</returns>
        public static ITransactionContext GetContextFromConnectionString(string p_DataConnection)
        {
            ITransactionContext retval = new TransactionContext();
            string DataConnectionString = null;

            DataConnectionString = p_DataConnection;
            retval.DataConnection = new SqlConnection(DataConnectionString);
            retval.DataConnection.Open();
            retval.DataTransaction = retval.DataConnection.BeginTransaction();

            return retval;
        }

        #endregion public transaction generation

        #region private transaction generation

        private static ITransactionContext GetSelectContext(string p_DataConnectionSource)
        {
            ITransactionContext retval;
            retval = GetDataContext(p_DataConnectionSource);
            return retval;
        }

        private static ITransactionContext GetUpdateContext(string p_DataConnectionSource)
        {
            ITransactionContext retval;
            retval = GetFullContext(p_DataConnectionSource);
            return retval;
        }

        private static ITransactionContext GetFullContext(string p_DataConnectionSource)
        {
            ITransactionContext retval = new TransactionContext();
            string DataConnectionString = null;

            if (p_DataConnectionSource != null)
                if (p_DataConnectionSource != "")
                    DataConnectionString = Convert.ToString(Properties.Settings.Default[p_DataConnectionSource]);

            if (DataConnectionString == null)
                DataConnectionString = Properties.Settings.Default.DBConnectionString;

            retval.DataConnection = new SqlConnection(DataConnectionString);
            retval.DataConnection.Open();
            retval.DataTransaction = retval.DataConnection.BeginTransaction();
            return retval;
        }

        private static ITransactionContext GetDataContext(string p_DataConnectionSource)
        {
            ITransactionContext retval = new TransactionContext();
            string DataConnectionString = null;
            if (p_DataConnectionSource != null)
                if (p_DataConnectionSource != "")
                    DataConnectionString = Convert.ToString(Properties.Settings.Default[p_DataConnectionSource]);

            if (DataConnectionString == null)
                DataConnectionString = Properties.Settings.Default.DBConnectionString;
            retval.DataConnection = new SqlConnection(DataConnectionString);
            return retval;
        }

        #endregion private transaction generation

        #region Transaction methods

        public static void Commit(ITransactionContext p_TransactionContext)
        {
            if (p_TransactionContext.DataTransaction != null)
                p_TransactionContext.DataTransaction.Commit();
        }

        public static void RollBack(ITransactionContext p_TransactionContext)
        {
            if ((p_TransactionContext.DataTransaction != null) &&
              (null != p_TransactionContext.DataTransaction.Connection) &&
              ((p_TransactionContext.DataTransaction.Connection.State == ConnectionState.Open)))
            {
                p_TransactionContext.DataTransaction.Rollback();
                if (null != p_TransactionContext.DataTransaction.Connection)
                    p_TransactionContext.DataTransaction.Connection.Dispose();
            }
        }

        public static void BeginTransactions(ITransactionContext p_TransactionContext)
        {
            if (p_TransactionContext.DataConnection != null)
            {
                if (p_TransactionContext.DataConnection.State != ConnectionState.Open)
                    p_TransactionContext.DataConnection.Open();
                p_TransactionContext.DataTransaction = p_TransactionContext.DataConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
            }
        }

        #endregion Transaction methods
    }
}