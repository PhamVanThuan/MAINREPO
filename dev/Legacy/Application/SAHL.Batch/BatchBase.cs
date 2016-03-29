using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using Castle.ActiveRecord.Queries;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using SAHL.Common.Logging;
using SAHL.Common.Security;

namespace SAHL.Batch
{
    public abstract class BatchBase
    {
        private UIStatementRepository _uiStatementRepo;
        private BulkBatchRepository _bulkBatchRepo;
        private BulkBatch_DAO _bulkBatch;

        /// <summary>
        /// Culture for UK (Great Britain) - this should be used for dates as we use dd/mm/yyyy rather
        /// than yyyy/mm/dd which is za-culture.
        /// </summary>
        public const string CultureGb = "en-GB";

        public BatchBase()
        {
            /*
            LogSection section = ConfigurationManager.GetSection("LogSection") as LogSection;
            LogSettingsClass lsl = new LogSettingsClass();
            lsl.AppName = "Batch Application";
            lsl.ConsoleLevel = Convert.ToInt32(section.Logging["Console"].level);
            lsl.ConsoleLevelLock = Convert.ToBoolean(section.Logging["Console"].Lock);
            lsl.FileLevel = Convert.ToInt32(section.Logging["File"].level);
            lsl.FileLevelLock = Convert.ToBoolean(section.Logging["File"].Lock);
            lsl.FilePath = section.Logging["File"].path;
            lsl.NumDaysToStore = 14;
            lsl.RollOverSizeInKB = 2048;
            LogPlugin.SeedLogSettings(lsl);
            */
            InitialiseActiveRecord();
        }

        ////loads data for specific batch key
        //public BatchBase(int BatchKey)
        //{
        //    InitialiseBatchMetrics(this.ToString());
        //    PopulateLookUps();
        //    m_Log = new BulkBatch.BulkBatchLogDataTable();
        //    if (m_BatchKey != -1)
        //    {
        //        m_BatchKey = BatchKey;
        //        InitialiseBatchData();
        //    }
        //}

        private bool GetNextBatchForProcessing(BulkBatchTypes batchType)
        {
            string hql = "from BulkBatch_DAO bb where bb.BulkBatchType.Key = ? and bb.BulkBatchStatus.Key = ? order by bb.Key";
            SimpleQuery<BulkBatch_DAO> query = new SimpleQuery<BulkBatch_DAO>(hql, (int)batchType, (int)BulkBatchStatuses.ReadyforProcessing);
            query.SetQueryRange(1);
            BulkBatch_DAO[] results = query.Execute();
            if (results.Length == 0)
                return false;
            else
            {
                _bulkBatch = results[0];
                return true;
            }
        }

        ~BatchBase()
        {
        }

        /// <summary>
        /// Worker method - must be implemented by all Batch classes.
        /// </summary>
        /// <returns></returns>
        protected abstract void RunBatch();

        public int Run(BulkBatchTypes type)
        {
            int errorCode = 0;

            using (new SessionScope())
            {
                try
                {
                    while (GetNextBatchForProcessing(type))
                    {
                        LogPlugin.Logger.LogFormattedInfo("OnStart - {0}", type.ToString());
                        UpdateStatus(BulkBatchStatuses.ProcessingStarted);

                        // use try catch on the actual running that won't make the job fall over - if just
                        // this job falls over but we can still update the status then it's ok to try and
                        // process the other jobs
                        try
                        {
                            RunBatch();
                            UpdateStatus(BulkBatchStatuses.Successful);
                            LogPlugin.Logger.LogFormattedInfo("OnComplete - {0}", type.ToString());
                        }
                        catch (Exception ex)
                        {
                            ExceptionPolicy.HandleException(ex, "UI Exception");
                            errorCode = 1;
                            LogException(ex, "Internal error: " + ex.Message, "Internal Error", "Internal Error");
                            UpdateStatus(BulkBatchStatuses.Failed);
                            continue;
                        }
                        finally
                        {
                            _bulkBatch = null;
                        }
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    // try and write the bulk batch error so the user knows
                    LogException(ex, "Internal error: " + ex.Message, "Internal Error", "Internal Error");
                    UpdateStatus(BulkBatchStatuses.Failed);

                    // exit with error code
                    errorCode = 1;
                }
                return errorCode;
            }
        }

        private void Log(MessageTypes messageType, string message, string messageReferenceKey, string messageReference)
        {
            if (CurrentBulkBatch != null)
            {
                BulkBatchLog_DAO bulkBatchLog = new BulkBatchLog_DAO();
                bulkBatchLog.BulkBatch = CurrentBulkBatch;
                bulkBatchLog.MessageReferenceKey = messageReferenceKey;
                bulkBatchLog.MessageReference = messageReference;
                bulkBatchLog.Description = message;
                bulkBatchLog.MessageType = MessageType_DAO.Find((int)messageType);
                bulkBatchLog.SaveAndFlush();
            }
        }

        public void LogException(Exception ex, string ErrorMessage , string MessageReferenceKey , string MessageReference )
        {
            // try and write to the event log
            if (ex != null)
            {
                try
                {
                    string line = "-----------------------------------------";
                    string crlf = Environment.NewLine + Environment.NewLine;

                    StringBuilder sb = new StringBuilder();
                    sb.Append(ex.Message);
                    sb.Append(crlf);
                    while (ex != null)
                    {
                        sb.Append(line);
                        sb.Append(crlf);
                        sb.Append(ex.StackTrace);
                        sb.Append(crlf);
                        ex = ex.InnerException;
                    }

                    EventLog.WriteEntry(GetType().Name, sb.ToString(), EventLogEntryType.Error);
                }
                catch (Exception)
                {
                }
            }

            Log(MessageTypes.Exception, ErrorMessage, MessageReferenceKey, MessageReference);
        }

        public void LogInfo(string ErrorMessage, string MessageReferenceKey, string MessageReference)
        {
            Log(MessageTypes.Information, ErrorMessage, MessageReferenceKey, MessageReference);
        }

        public void LogWarning(string ErrorMessage, string MessageReferenceKey, string MessageReference)
        {
            Log(MessageTypes.Warning, ErrorMessage, MessageReferenceKey, MessageReference);
        }

        public void UpdateStatus(BulkBatchStatuses status)
        {
            if (CurrentBulkBatch != null)
            {
                switch (status)
                {
                    case BulkBatchStatuses.ProcessingStarted:
                        CurrentBulkBatch.StartDateTime = DateTime.Now;
                        break;
                    case BulkBatchStatuses.Failed:
                    case BulkBatchStatuses.Successful:
                        CurrentBulkBatch.CompletedDateTime = DateTime.Now;
                        break;
                }
                CurrentBulkBatch.BulkBatchStatus = BulkBatchStatus_DAO.Find((int)status);
                CurrentBulkBatch.SaveAndFlush();
            }
        }

        /// <summary>
        /// Writes headers from a datatable to an output stream.
        /// </summary>
        /// <param name="extractFile"></param>
        /// <param name="data">The data table.</param>
        /// <param name="ignoredColumns">Columns that should be ignored when creating the headers line.</param>
        private void WriteHeaders(StreamWriter extractFile, DataTable data, List<string> ignoredColumns)
        {
            StringBuilder header = new StringBuilder();
            foreach (DataColumn column in data.Columns)
            {
                if (!ignoredColumns.Contains(column.ColumnName))
                {
                    header.Append(column.ColumnName);
                    header.Append(",");
                }
            }
            extractFile.WriteLine(header);
        }

        /// <summary>
        /// Writes the contents of a data table to a CSV file.
        /// </summary>
        /// <param name="filePath">The output file path.</param>
        /// <param name="data">The populated data table.</param>
        /// <param name="ignoredColumns">Columns that should be ignored when creating the report.</param>
        protected void WriteReport(string filePath, DataTable data, params string[] ignoredColumns)
        {
            StreamWriter extractFile = null;
            try
            {
                extractFile = new StreamWriter(filePath);
                List<string> lstIgnoredColumns = new List<string>(ignoredColumns);
                // write out headers from the data table
                WriteHeaders(extractFile, data, lstIgnoredColumns);

                // now write out the data
                foreach (DataRow row in data.Rows)
                {
                    StringBuilder sb = new StringBuilder();
                    object[] cells = row.ItemArray;
                    for (int i=0; i<cells.Length; i++)
                    {
                        if (!lstIgnoredColumns.Contains(data.Columns[i].ColumnName))
                        {
                            sb.Append(cells[i].ToString());
                            sb.Append(",");
                        }
                    }
                    extractFile.WriteLine(sb.ToString());
                }
            }
            finally
            {
                if (extractFile != null)
                    extractFile.Close();
            }
        }

        /// <summary>
        /// Gets the current bulk batch object.
        /// </summary>
        protected BulkBatch_DAO CurrentBulkBatch
        {
            get
            {
                return _bulkBatch;
            }
        }

        protected BulkBatchRepository BulkBatchRepo
        {
            get
            {
                if (_bulkBatchRepo == null)
                    _bulkBatchRepo = new BulkBatchRepository();
                return _bulkBatchRepo;
            }
        }

        protected UIStatementRepository UIStatementRepository
        {
            get
            {
                if (_uiStatementRepo == null)
                    _uiStatementRepo = new UIStatementRepository();
                return _uiStatementRepo;
            }
        }

        /// <summary>
        /// Returns the string value of a parameter with the specified name out of the parameters collection of the
        /// <see cref="CurrentBulkBatch"/>.
        /// This will throw an exception if it cannot find the parameter.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        protected int GetIntParameterValue(BulkBatchParameterNames parameterName)
        {
            string sVal = GetStringParameterValue(parameterName);
            int val = 0;
            if (Int32.TryParse(sVal, out val))
                return val;
            else
                throw new Exception(parameterName.ToString() + " parameter not correctly defined for batch");
        }

        /// <summary>
        /// Gets a parameter from the current bulk batch by name.  If it does not find the parameter it will
        /// return null.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        protected BulkBatchParameter_DAO GetParameter(string parameterName)
        {
            foreach (BulkBatchParameter_DAO parameter in CurrentBulkBatch.BulkBatchParameters)
            {
                if (parameter.ParameterName == parameterName)
                    return parameter;
            }
            return null;
        }

        /// <summary>
        /// Gets a parameter from the current bulk batch by name.  If it does not find the parameter it will
        /// return null.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        protected BulkBatchParameter_DAO GetParameter(BulkBatchParameterNames parameterName)
        {
            return GetParameter(parameterName.ToString());
        }

        /// <summary>
        /// Returns the string value of a parameter with the specified name out of the parameters collection of the
        /// This will throw an exception if it cannot find the parameter.
        /// <see cref="CurrentBulkBatch"/>.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        protected string GetStringParameterValue(BulkBatchParameterNames parameterName)
        {
            BulkBatchParameter_DAO parameter = GetParameter(parameterName);
            if (parameter == null || String.IsNullOrEmpty(parameter.ParameterValue))
                throw new Exception(parameterName.ToString() + " parameter not defined for batch");
            else
                return parameter.ParameterValue;
        }

        protected static SAHLPrincipal SetThreadPrincipal(string ADUser)
        {
            SAHLPrincipal pcp = new SAHLPrincipal(new GenericIdentity(ADUser));
            Thread.CurrentPrincipal = pcp;
            string key = pcp.Identity.Name;

            CacheManager principalStore = CacheFactory.GetCacheManager("SAHLPrincipalStore");
            SAHLPrincipalCache principalCache = SAHLPrincipalCache.GetPrincipalCache(pcp);
            principalStore.Add(key, principalCache);

            //PrincipalHelper.InitialisePrincipal(pcp, principalCache.AllowedCBOMenus, principalCache.AllowedContextMenus, principalCache.UserOriginationSources);
            Thread.CurrentPrincipal = pcp;

            return pcp;
        }

        protected static void ClearThreadPrincipal()
        {
            Thread.CurrentPrincipal = null;
        }

        /// <summary>
        /// Initialises ActiveRecord.
        /// </summary>
        private void InitialiseActiveRecord()
        {
            InPlaceConfigurationSource configSource = new InPlaceConfigurationSource();
            configSource.IsRunningInWebApp = false;
            // configSource.ThreadScopeInfoImplementation = typeof(WebThreadScopeInfo);

            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            properties.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
            properties.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            properties.Add("connection.connection_string", Properties.Settings.Default.DBConnectionString);
            properties.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
            configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_2AM<>), properties);

            //Hashtable properties2 = new Hashtable();
            //properties2.Add("hibernate.connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            //properties2.Add("hibernate.dialect", "NHibernate.Dialect.MsSql2000Dialect");
            //properties2.Add("hibernate.connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            //properties2.Add("hibernate.connection.connection_string", SAHL.Batch.Properties.Settings.Default.X2ConnectionString);
            //configSource.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.Database.DB_X2<>), properties2);

            Dictionary<string, string> properties3 = new Dictionary<string, string>();
            properties3.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            properties3.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
            properties3.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            properties3.Add("connection.connection_string", Properties.Settings.Default.SAHLConnectionString);
            properties3.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
            configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_SAHL<>), properties3);

            Dictionary<string, string> properties4 = new Dictionary<string, string>();
            properties4.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            properties4.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
            properties4.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            properties4.Add("connection.connection_string", Properties.Settings.Default.WarehouseConnectionString);
            properties4.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
            configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_Warehouse<>), properties4);

            Dictionary<string, string> properties5 = new Dictionary<string, string>();
            properties5.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            properties5.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
            properties5.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            properties5.Add("connection.connection_string", Properties.Settings.Default.ImageIndexConnectionString);
            properties5.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
            configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_ImageIndex<>), properties5);

            // load the business model assembly(ies) and initialise
            Assembly[] asm = new Assembly[1];
            asm[0] = Assembly.Load("SAHL.Common.BusinessModel.DAO");
            // asm[1] = Assembly.Load("SAHL.Common.X2.BusinessModel.DAO");

            ActiveRecordStarter.Initialize(asm, configSource);
        }

        protected void ValidateBulkBatch()
        {
            if (CurrentBulkBatch.BulkBatchParameters.Count == 0)
            {
                throw new Exception("No Batch Config found for batch ID [" + CurrentBulkBatch.Key + "]");
            }

            switch ((BulkBatchTypes)CurrentBulkBatch.BulkBatchType.Key)
            {
                case BulkBatchTypes.CapImportClientList:
                case BulkBatchTypes.CapMailingHouseExtract:
                    if (String.IsNullOrEmpty(CurrentBulkBatch.BulkBatchType.FilePath))
                        throw new Exception("No FilePath captured for batch output");
                    break;
            }
        }
    }
}