using System;
using System.Data;
using System.Data.SqlClient;
using SAHL.Common.DataSets;

namespace SAHL.Common.DataAccess
{
    /// <summary>
    /// The TransactionController provides instances of the TransactionContext class.  Depending on the method that is called
    /// the Context will be populated in different manners.
    /// </summary>
    public static class TransactionController
    {
        #region public transaction generation

        /// <summary>
        /// A TransactionContext is returned.
        /// </summary>
        /// <param name="p_bAuditRequired">If set to true the AuditConnection and AuditTransaction is populated.
        /// Otherwise the DataConnection alone is populated, with no transaction support.</param>
        /// <param name="p_MetricInfo">Set to null if p_AuditRequired is false, otherwise it has to be a Metrics dataset with at least the first row populated with valid metrics information.</param>
        /// <returns>TransactionContext</returns>
        public static TransactionContext GetContext(bool p_bAuditRequired, Metrics p_MetricInfo)
        {
            if (p_bAuditRequired)
            {
                TransactionContext retval = GetUpdateContext(null, null);
                AddAuditRowGivenMetricInfo(retval, p_MetricInfo);
                return retval;
            }
            else
                return GetSelectContext(null);
        }

        /// <summary>
        /// A TransactionContext with only a DataConnection is returned. No Audit connection, or transactions are created.
        /// This method will is used for select statements, as well as Metric inserts (they aren't Audited).
        /// </summary>
        /// <param name="p_DataConnectionSource"> The name of the connectionstring in the applicatoin settings to use for the Data Connection</param>
        /// <returns>TransactionContext</returns>
        public static TransactionContext GetContext(string p_DataConnectionSource)
        {
            return GetSelectContext(p_DataConnectionSource);
        }

        /// <summary>
        /// A TransactionContext with Data and Audit Connection as well as transactions for both is returned. This method is
        /// used for inserts where non-default datasources for the Audit and Data connections.
        /// </summary>
        /// <param name="p_DataConnectionSource"> The name of the connectionstring in the applicatoin settings to use for the Data Connection</param>
        /// <param name="p_AuditConnectionSource"> The name of the connectionstring in the applicatoin settings to use for the Audit Connection</param>
        /// <param name="p_MetricInfo">a Metrics dataset with at least the first row populated with valid metrics information.  If this is null, auditing cannot happen.</param>
        /// <returns>TransactionContext</returns>
        public static TransactionContext GetContext(string p_DataConnectionSource, string p_AuditConnectionSource, Metrics p_MetricInfo)
        {
            TransactionContext retval = GetUpdateContext(p_DataConnectionSource, p_AuditConnectionSource);
            AddAuditRowGivenMetricInfo(retval, p_MetricInfo);
            return retval;
        }

        /// <summary>
        /// A TransactionContext with Data and Audit Connection as well as transactions for both is returned. This method is
        /// used for inserts where non-default datasources for the Audit and Data connections.
        /// </summary>
        /// <param name="p_DataConnection"> The actual connectionstring to use for the Data Connection</param>
        /// <param name="p_AuditConnection"> The actual connectionstring to use for the Audit Connection</param>
        /// <param name="p_MetricInfo">a Metrics dataset with at least the first row populated with valid metrics information.  If this is null, auditing cannot happen.</param>
        /// <returns>TransactionContext</returns>
        public static TransactionContext GetContextFromConnectionString(string p_DataConnection, string p_AuditConnection, Metrics p_MetricInfo)
        {
            TransactionContext retval = new TransactionContext();
            string DataConnectionString = null, AuditConnectionString = null;

            DataConnectionString = p_DataConnection;
            AuditConnectionString = p_AuditConnection;

            retval.AuditConnection = new SqlConnection(AuditConnectionString);
            retval.AuditConnection.Open();
            retval.AuditTransaction = retval.AuditConnection.BeginTransaction();

            retval.DataConnection = new SqlConnection(DataConnectionString);
            retval.DataConnection.Open();
            retval.DataTransaction = retval.DataConnection.BeginTransaction();

            AddAuditRowGivenMetricInfo(retval, p_MetricInfo);
            return retval;
        }

        #endregion public transaction generation

        #region private transaction generation

        private static TransactionContext GetSelectContext(string p_DataConnectionSource)
        {
            TransactionContext retval;
            retval = GetDataContext(p_DataConnectionSource);
            return retval;
        }

        private static TransactionContext GetUpdateContext(string p_DataConnectionSource, string p_AuditConnectionSource)
        {
            TransactionContext retval;
            retval = GetFullContext(p_DataConnectionSource, p_AuditConnectionSource);
            return retval;
        }

        private static TransactionContext GetFullContext(string p_DataConnectionSource, string p_AuditConnectionSource)
        {
            TransactionContext retval = new TransactionContext();
            string DataConnectionString = null, AuditConnectionString = null;

            if (p_DataConnectionSource != null)
                if (p_DataConnectionSource != "")
                    DataConnectionString = Convert.ToString(SAHL.Common.Properties.Settings.Default[p_DataConnectionSource]);

            if (DataConnectionString == null)
                DataConnectionString = SAHL.Common.Properties.Settings.Default.DBConnectionString;

            if (p_AuditConnectionSource != null)
                if (p_AuditConnectionSource != "")
                    AuditConnectionString = Convert.ToString(SAHL.Common.Properties.Settings.Default[p_AuditConnectionSource]);
            if (AuditConnectionString == null)
                AuditConnectionString = SAHL.Common.Properties.Settings.Default.Warehouse;

            retval.AuditConnection = new SqlConnection(AuditConnectionString);
            retval.AuditConnection.Open();
            retval.AuditTransaction = retval.AuditConnection.BeginTransaction();

            retval.DataConnection = new SqlConnection(DataConnectionString);
            retval.DataConnection.Open();
            retval.DataTransaction = retval.DataConnection.BeginTransaction();
            return retval;
        }

        private static TransactionContext GetDataContext(string p_DataConnectionSource)
        {
            TransactionContext retval = new TransactionContext();
            string DataConnectionString = null;
            if (p_DataConnectionSource != null)
                if (p_DataConnectionSource != "")
                    DataConnectionString = Convert.ToString(SAHL.Common.Properties.Settings.Default[p_DataConnectionSource]);

            if (DataConnectionString == null)
                DataConnectionString = SAHL.Common.Properties.Settings.Default.DBConnectionString;
            retval.DataConnection = new SqlConnection(DataConnectionString);
            return retval;
        }

        #endregion private transaction generation

        #region Transaction methods

        public static void Commit(TransactionContext p_TransactionContext)
        {
            if (p_TransactionContext.AuditTransaction != null)
                p_TransactionContext.AuditTransaction.Commit();
            if (p_TransactionContext.DataTransaction != null)
                p_TransactionContext.DataTransaction.Commit();
        }

        public static void RollBack(TransactionContext p_TransactionContext)
        {
            if ((p_TransactionContext.AuditTransaction != null) &&
              (null != p_TransactionContext.AuditTransaction.Connection) &&
              (p_TransactionContext.AuditTransaction.Connection.State == ConnectionState.Open))
                p_TransactionContext.AuditTransaction.Rollback();

            if ((p_TransactionContext.DataTransaction != null) &&
              (null != p_TransactionContext.DataTransaction.Connection) &&
              ((p_TransactionContext.DataTransaction.Connection.State == ConnectionState.Open)))
                p_TransactionContext.DataTransaction.Rollback();
        }

        public static void BeginTransactions(TransactionContext p_TransactionContext)
        {
            if (p_TransactionContext.DataConnection != null)
            {
                if (p_TransactionContext.DataConnection.State != ConnectionState.Open)
                    p_TransactionContext.DataConnection.Open();
                p_TransactionContext.DataTransaction = p_TransactionContext.DataConnection.BeginTransaction();
            }

            if (p_TransactionContext.AuditConnection != null)
            {
                if (p_TransactionContext.AuditConnection.State != ConnectionState.Open)
                    p_TransactionContext.AuditConnection.Open();
                p_TransactionContext.AuditTransaction = p_TransactionContext.AuditConnection.BeginTransaction();
            }
        }

        #endregion Transaction methods

        #region private methods

        /// <summary>
        /// All the common information in the metric datarow is copied to the Audit datarow.
        /// </summary>
        /// <param name="p_Metric">The Metric Datarow</param>
        /// <param name="p_Audit">The Audit Datarow</param>
        private static void TransformMetricToAuditInfo(Metrics.MetricsRow p_Metric, Audits.AuditsRow p_Audit)
        {
            p_Audit.ApplicationName = p_Metric.ApplicationName;
            p_Audit.FormName = p_Metric.FormName;
            p_Audit.HostName = p_Metric.HostName;
            p_Audit.WindowsLogon = p_Metric.WindowsLogon;
            p_Audit.WorkStationID = p_Metric.WorkStationID;
        }

        private static void AddAuditRowGivenMetricInfo(TransactionContext p_TransactionContext, Metrics p_Metrics)
        {
            if ((p_Metrics != null) && (p_Metrics._Metrics.Count > 0))
            {
                Metrics.MetricsRow R = p_Metrics._Metrics[0];
                p_TransactionContext.AuditInfo = new Audits();
                Audits.AuditsRow AuditsRow = p_TransactionContext.AuditInfo._Audits.NewAuditsRow();
                TransformMetricToAuditInfo(R, AuditsRow);
                p_TransactionContext.AuditInfo._Audits.AddAuditsRow(AuditsRow);
            }
        }

        #endregion private methods
    }
}