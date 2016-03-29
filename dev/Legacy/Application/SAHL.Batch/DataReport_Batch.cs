using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using SAHL.Common;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Logging;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Utils;

namespace SAHL.Batch
{
    public class DataReport_Batch : BatchBase
    {
        public DataReport_Batch()
            : base()
        {
        }

        protected override void RunBatch()
        {
            try
            {
                LogPlugin.Logger.LogInfoMessage("DataReport_Batch.RunBatch", "DataReportBatch Starting ");

                ValidateBulkBatch();

                IReportRepository repo = RepositoryFactory.GetRepository<IReportRepository>();
                int reportKey = this.GetIntParameterValue(BulkBatchParameterNames.ReportKey);
                string mailAddressList = this.GetStringParameterValue(BulkBatchParameterNames.MailAddress);

                // fix bad characters in the address list if there are any
                mailAddressList = mailAddressList.Replace("\n", ",").Replace("\r", "").Replace(";", "");

                IReportStatement reportStatement = repo.GetReportStatementByKey(reportKey);

                // TODO: Change domain!  For now, getting the report parameters has to be done by an HQL query
                // as for some unknown reason they haven't been exposed on the ReportStatement_DAO and I can't
                // change the framework as this needs to roll out without framework changes - if you see this
                // code after 23/01/2009 please adjust the DAOs so this becomes a property!
                IReadOnlyEventList<IReportParameter> reportParameters = repo.GetReportParametersByReportStatementKey(reportKey);

                LogPlugin.Logger.LogInfoMessage("DataReport_Batch.RunBatch", string.Format("DataReportBatch : Processing {0} - ({1}) Requested by {2} on the {3}", reportStatement.ReportName,
                    reportStatement.StatementName, CurrentBulkBatch.UserID,CurrentBulkBatch.EffectiveDate.ToString()));

                // create a dictionary with the report parameters and their associated values
                Dictionary<IReportParameter, object> dictParmValues = new Dictionary<IReportParameter, object>();
                foreach (IReportParameter rp in reportParameters)
                {
                    BulkBatchParameter_DAO bbp = base.GetParameter(rp.ParameterName);
                    if (bbp == null)
                    {
                        if (rp.Required.HasValue && rp.Required.Value)
                            throw new Exception(String.Format("Unable to find expected parameter {0}", rp.ParameterName));
                        else
                            continue;
                    }

                    object parmValue = bbp.ParameterValue;
                    if (rp.ReportParameterType.Key == (int)ReportParameterTypes.DateTime)
                        parmValue = new DateTime(Convert.ToInt64(parmValue));
                    dictParmValues.Add(rp, parmValue);
                }

                // now execute the statement to populate a datatable
                DataTable dt = ExecuteSqlReport(dictParmValues, reportStatement);
                string filePath = CurrentBulkBatch.BulkBatchType.FilePath;

                if (!Directory.Exists(filePath))
                    throw new DirectoryNotFoundException(String.Format("Folder {0} does not exist", filePath));

                filePath = StringUtils.JoinNullableStrings(filePath, "\\", CurrentBulkBatch.Description, DateTime.Now.ToString("ddMMyyyyHHmmss"), "_", CurrentBulkBatch.UserID.Replace("\\", "_"), ".xls");
                if (File.Exists(filePath))
                    throw new Exception("File [" + filePath + "] already exists - Cannot Overwrite File");

                string data = repo.ExportDataReportToExcel(dt, reportStatement);
                StreamWriter sw = new StreamWriter(File.Create(filePath));
                try
                {
                    sw.Write(data);
                }
                finally
                {
                    sw.Close();
                }

                // send an email to everyone on the list (list is comma-delimited, so we can use this directly
                // in the "to" field)
                IMessageService msgServc = ServiceFactory.GetService<IMessageService>();

                StringBuilder sbBody = new StringBuilder();
                sbBody.Append("<ul type=\"square\">");
                sbBody.AppendFormat("<li>Report Requested : {0}", reportStatement.ReportName);
                sbBody.AppendFormat("<li>Requested By User: {0}", CurrentBulkBatch.UserID);
                sbBody.AppendFormat("<li>Request Date: {0}", CurrentBulkBatch.EffectiveDate.ToLongDateString());
                sbBody.Append("<li>Parameters:<ol>");

                foreach (KeyValuePair<IReportParameter, object> kvp in dictParmValues)
                    sbBody.AppendFormat("<li> {0} : {1}", kvp.Key.DisplayName, kvp.Value.ToString());
                sbBody.Append("</ol>");

                sbBody.AppendFormat("<br><br>File Location: <a href=\"{0}\">{0}</a>", filePath);
                sbBody.Append("</ul>");

                msgServc.SendEmailInternal(Properties.Settings.Default.ReportBatchFromEMail, mailAddressList,
                    null, null, String.Format("Data Report: {0}", reportStatement.ReportName), sbBody.ToString());

                LogPlugin.Logger.LogInfoMessage("DataReport_Batch.RunBatch", "DataReportBatch Complete ");
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException("DataReport_Batch.RunBatch", "DataReportBatch",ex);
                ExceptionPolicy.HandleException(ex, "UI Exception");
            }

            #region Old Code

            //    StringBuilder body = new StringBuilder();

            //    mailMsg.Body = body.ToString();
            //    //Attachment attFile = new Attachment(zipFileName);
            //    //mailMsg.Attachments.Add(attFile);
            //    SmtpClient client = new SmtpClient(smtpClient);
            //    client.Send(mailMsg);
            //    //System.IO.Directory.SetCurrentDirectory(LastDirectory);

            //    //update the batch status to completed
            //    BulkBatchDS._BulkBatch[0].BulkBatchStatusKey = 2;
            //    BulkBatchDS._BulkBatch[0].CompletedDateTime = DateTime.Now;
            //    BB_SFE.UpdateBulkBatch(BulkBatchDS, BatchMetrics);

            //    return 0;

            //}
            //catch (Exception ex)
            //{
            //    LogException(ex.Message,null,null);
            //    UpdateStatusError();
            //    return 1;

            #endregion Old Code
        }

        //private void PopulateReportParameter(List<ReportParameterDef> ReportParamterList , SAHL.Datasets.BulkBatch.BulkBatchParameterRow ParameterRow)
        //{
        //    foreach (ReportParameterDef def in ReportParamterList)
        //    {
        //        if (def.ParameterName == ParameterRow.ParameterName)
        //        {
        //            switch (def.ParameterType)
        //            {
        //                case ReportParameterTypeEnum.eSelectList:
        //                case ReportParameterTypeEnum.eBoolean:
        //                case ReportParameterTypeEnum.eMultiValueString:
        //                case ReportParameterTypeEnum.eMultiValueInt:
        //                case ReportParameterTypeEnum.eString:
        //                case ReportParameterTypeEnum.eMultiValueSelect:
        //                    def.Value = ParameterRow.ParameterValue;
        //                    break;

        //                case ReportParameterTypeEnum.eDateTime:
        //                    DateTime dt = new DateTime(long.Parse(ParameterRow.ParameterValue));
        //                    if (dt.Year > 1900)
        //                        def.Value = dt;
        //                    else
        //                        def.Value = null;

        //                    break;
        //                case ReportParameterTypeEnum.eFloat:
        //                    if (ParameterRow.ParameterValue.Length > 0)
        //                        def.Value = float.Parse(ParameterRow.ParameterValue);
        //                    else
        //                        def.Value = null;

        //                    break;
        //                case ReportParameterTypeEnum.eInteger:
        //                    if (ParameterRow.ParameterValue.Length > 0)
        //                        def.Value = int.Parse(ParameterRow.ParameterValue);
        //                    else
        //                        def.Value = null;
        //                    break;
        //            }

        //            break;
        //        }
        //    }
        //}

        //private List<ReportParameterDef>  GetDataReportParameters( Report.ReportParameterDataTable res, Metrics BatchMetrics)
        //{
        //    List<ReportParameterDef> ParameterValues = new List<ReportParameterDef>();

        //    foreach (Report.ReportParameterRow rpr in res)
        //    {
        //        ReportParameterDef def = new ReportParameterDef();

        //        def.ParameterName = rpr.ParameterName;
        //        def.DisplayName = rpr.DisplayName;

        //        if (!rpr.IsDomainFieldKeyNull())
        //            def.DomainField = rpr.DomainFieldKey;

        //        def.ParameterType = (ReportParameterTypeEnum)rpr.ParameterTypeKey;

        //        if ((def.ParameterType == ReportParameterTypeEnum.eSelectList) || (def.ParameterType == ReportParameterTypeEnum.eMultiValueSelect))
        //            if (!rpr.IsStatementNameNull())
        //                def.ValidValues = GetParameterValuesFromStatement(rpr.StatementName, BatchMetrics);

        //        def.Value = null;

        //        if (!rpr.IsParameterLengthNull())
        //            def.MaxLength = rpr.ParameterLength;
        //        else
        //            def.MaxLength = 0;

        //        if (!rpr.IsRequiredNull())
        //            def.Required = rpr.Required;
        //        else
        //            def.Required = false;

        //        ParameterValues.Add(def);
        //    }

        //    return  ParameterValues;
        //}

        //public List<NameValue> GetParameterValuesFromStatement(string StatementName, Metrics p_MI)
        //{
        //    List<NameValue> retval = new List<NameValue>();

        //    Report_SFE Report_SFE = new Report_SFE();
        //    DataTable DT = new DataTable();

        //    Report_SFE.GetParameterValuesFromStatement(DT, StatementName, p_MI);

        //    if (DT.Columns.Count >= 1)
        //    {
        //        foreach (DataRow r in DT.Rows)
        //        {
        //            if (DT.Columns.Count == 1)
        //                retval.Add(new NameValue((string)r[0], r[0]));
        //            else
        //                retval.Add(new NameValue((string)r[1], r[0]));
        //        }
        //    }

        //    return retval;
        //}

        //private void GetFileBuffer(MemoryStream ms, sbyte[] buffer)
        //{
        //    byte[] bytebuffer = new byte[ms.Length];
        //    int len = ms.Read(bytebuffer, 0, (int)ms.Length);

        //    //buffer = new sbyte[ms.Length + 1];
        //    for (int i = 0; i < ms.Length; i++)
        //    {
        //        buffer[i] = (sbyte)bytebuffer[i];
        //    }
        //}

        /// <summary>
        /// Executes a SQL report.  This is a copy of the method in the ReportRepository, but we had to do this
        /// because there is no way to get a handle on the Command in the data helper, and therefore no way of
        /// adjusting the command timeout
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="reportStatement"></param>
        /// <returns></returns>
        public DataTable ExecuteSqlReport(Dictionary<IReportParameter, object> dict, IReportStatement reportStatement)
        {
            ParameterCollection pc = new ParameterCollection();
            foreach (Object o in dict.Keys)
            {
                IReportParameter param = o as IReportParameter;

                SqlParameter p = new SqlParameter();

                object val = null;
                dict.TryGetValue(param, out val);
                SqlDbType type = new SqlDbType();
                switch (param.ReportParameterType.Key)
                {
                    case 1:
                        type = SqlDbType.Bit;
                        break;
                    case 2:
                        type = SqlDbType.DateTime;
                        break;
                    case 3:
                        type = SqlDbType.Float;
                        break;
                    case 4:
                        type = SqlDbType.Int;
                        break;
                    case 5:
                        type = SqlDbType.VarChar;
                        break;
                    case 6:
                        type = SqlDbType.VarChar;
                        break;
                    case 7:
                        type = SqlDbType.VarChar;
                        break;
                    case 9:
                        type = SqlDbType.Int;
                        break;
                    case 8:
                        type = SqlDbType.Int;
                        break;
                }

                p.Value = val;
                p.Direction = ParameterDirection.Input;
                p.ParameterName = param.ParameterName;
                p.SqlDbType = type;
                pc.Add(p);
            }

            string query = UIStatementRepository.GetStatement("COMMON", reportStatement.StatementName);

            //return Statement.ExecuteUIStatement(pc);
            IDbConnection conn = Helper.GetSQLDBConnection();
            DataTable dataTable = new DataTable();
            SqlDataAdapter adapter = null;

            try
            {
                conn.Open();

                adapter = new SqlDataAdapter();
                adapter.SelectCommand = (SqlCommand)conn.CreateCommand();
                adapter.SelectCommand.CommandText = query;
                adapter.SelectCommand.CommandTimeout = 0;

                pc.TransferParameters(adapter.SelectCommand.Parameters);

                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "UI Exception");
            }
            finally
            {
                if (adapter != null)
                    adapter.Dispose();
                if (conn != null)
                    conn.Dispose();
            }

            return dataTable;
        }
    }
}