using Castle.ActiveRecord.Queries;
using PDFDocumentWriter;
using PDFUtils.PDFWriting;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Authentication;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Logging;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.WebServices.ReportExecution2005;
using SAHL.Common.WebServices.ReportingServices2010;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Xsl;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    ///
    /// </summary>
    [FactoryType(typeof(IReportRepository))]
    public class ReportRepository : AbstractRepositoryBase, IReportRepository
    {
        public ReportRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public ReportRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        #region IReportRepository Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IReportStatement GetReportStatementByKey(int Key)
        {
            return base.GetByKey<IReportStatement, ReportStatement_DAO>(Key);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IReportParameter GetReportParameterByKey(int Key)
        {
            return base.GetByKey<IReportParameter, ReportParameter_DAO>(Key);
        }

        public IReadOnlyEventList<IReportParameter> GetReportParametersByReportStatementKey(int ReportStatementKey)
        {
            ReportStatement_DAO dao = ReportStatement_DAO.Find(ReportStatementKey);

            if (dao == null)
                return null;

            ReportStatement rp = new ReportStatement(dao);
            IReadOnlyEventList<IReportParameter> list = new ReadOnlyEventList<IReportParameter>(rp.ReportParameters);
            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ReportGroupKey"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IReportStatement> GetReportStatementByReportGroupKey(int ReportGroupKey)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            return ReportStatement.GetReportStatementByReportGroupKey(spc.DomainMessages, ReportGroupKey);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ReportName"></param>
        /// <returns></returns>
        public IEventList<IReportStatement> GetReportStatementByName(string ReportName)
        {
            IEventList<IReportStatement> list = new EventList<IReportStatement>();

            ReportStatement_DAO[] res = ReportStatement_DAO.FindAllByProperty("ReportName", ReportName);

            if (res != null && res.Length > 0)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                foreach (ReportStatement_DAO rs in res)
                {
                    list.Add(null, BMTM.GetMappedType<IReportStatement, ReportStatement_DAO>(rs));
                }
            }
            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ReportName"></param>
        /// <param name="OriginationSourceProductKey"></param>
        /// <returns></returns>
        public IReportStatement GetReportStatementByNameAndOSP(string ReportName, int OriginationSourceProductKey)
        {
            string query = "";

            // find the OriginationSourceKey based on the OriginationSourceProductKey parameter
            OriginationSourceProduct_DAO os_dao = OriginationSourceProduct_DAO.Find(OriginationSourceProductKey);
            int originationSourceKey = os_dao == null ? -1 : os_dao.OriginationSource.Key;

            // 1. look for the first report statement matching the reportname & osp key exactly
            query = string.Format(UIStatementRepository.GetStatement("ReportStatement", "GetReportStatementByNameAndOSP"), ReportName, OriginationSourceProductKey);

            SimpleQuery<ReportStatement_DAO> SQ = new SimpleQuery<ReportStatement_DAO>(query);
            SQ.SetQueryRange(1);
            object o = ReportStatement_DAO.ExecuteQuery(SQ);
            ReportStatement_DAO[] rs = o as ReportStatement_DAO[];
            if (rs != null && rs.Length >= 1)
                return new ReportStatement(rs[0]);

            // 2. if we havent found the reportstatement - then return the first record matching the reportname & originationsource
            query = string.Format(UIStatementRepository.GetStatement("ReportStatement", "GetReportStatementByNameAndOS"), ReportName, originationSourceKey);
            SQ = new SimpleQuery<ReportStatement_DAO>(query);
            SQ.SetQueryRange(1);
            o = ReportStatement_DAO.ExecuteQuery(SQ);
            rs = o as ReportStatement_DAO[];
            if (rs != null && rs.Length >= 1)
                return new ReportStatement(rs[0]);

            // 3. if we still havent found the reportstatement - then return the first record matching just the reportname
            query = string.Format(UIStatementRepository.GetStatement("ReportStatement", "GetReportStatementByName"), ReportName);
            SQ = new SimpleQuery<ReportStatement_DAO>(query);
            SQ.SetQueryRange(1);
            o = ReportStatement_DAO.ExecuteQuery(SQ);
            rs = o as ReportStatement_DAO[];
            if (rs != null && rs.Length >= 1)
                return new ReportStatement(rs[0]);

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="reportStatement"></param>
        /// <returns></returns>
        public DataTable ExecuteSqlReport(Dictionary<SAHL.Common.BusinessModel.Interfaces.IReportParameter, object> dict, IReportStatement reportStatement)
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
                        {
                            type = SqlDbType.Bit;
                            break;
                        }
                    case 2:
                        {
                            type = SqlDbType.DateTime;
                            break;
                        }
                    case 3:
                        {
                            type = SqlDbType.Float;
                            break;
                        }
                    case 4:
                        {
                            type = SqlDbType.Int;
                            break;
                        }
                    case 5:
                        {
                            type = SqlDbType.VarChar;
                            break;
                        }
                    case 6:
                        {
                            type = SqlDbType.VarChar;
                            break;
                        }
                    case 7:
                        {
                            type = SqlDbType.VarChar;
                            break;
                        }
                    case 9:
                        {
                            type = SqlDbType.Int;
                            break;
                        }
                    case 8:
                        {
                            type = SqlDbType.Int;
                            break;
                        }
                }

                p.Value = val;
                p.Direction = ParameterDirection.Input;
                p.ParameterName = param.ParameterName;
                p.SqlDbType = type;
                pc.Add(p);
            }

            string query = UIStatementRepository.GetStatement("COMMON", reportStatement.StatementName);

            //return Statement.ExecuteUIStatement(pc);
            //using (IDbConnection con = Helper.GetSQLDBConnection())
            //{
            //    DataTable DT = new DataTable();
            //    Helper.Fill(DT, "COMMON", reportStatement.StatementName, con, pc);

            //    return DT;
            //}

            DataTable dtReport = new DataTable();
            DataSet dsReport = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), pc);
            if (dsReport != null)
            {
                if (dsReport.Tables.Count > 0)
                {
                    dtReport = dsReport.Tables[0];
                }
            }
            return dtReport;
        }

        public IReportParameter CreateReportParameter()
        {
            return base.CreateEmpty<IReportParameter, ReportParameter_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reportStatement"></param>
        /// <returns></returns>
        public string GetUIStatementText(IReportStatement reportStatement)
        {
            return UIStatementRepository.GetStatement("COMMON", reportStatement.StatementName);
        }

        public string ExportDataReportToExcel(DataTable tblReportData, IReportStatement reportStatement)
        {
            Export myExport = new Export("Web");

            //Setup the columns for the converter
            int[] ColList = new int[tblReportData.Columns.Count];
            for (int i = 0; i < tblReportData.Columns.Count; i++)
            {
                ColList[i] = i;
            }

            string data;

            myExport.ExportDetails(tblReportData, ColList, Export.ExportFormat.Excel, out data);

            return data;
        }

        #endregion IReportRepository Members

        public byte[] RenderSQLReport(string reportPath, IDictionary<string, string> parameterNamesAndValues, out string renderErrorMessage)
        {
            return RenderSQLReport(reportPath, parameterNamesAndValues, "PDF", out renderErrorMessage);
        }

        public byte[] RenderSQLReport(string reportPath, IDictionary<string, string> parameterNamesAndValues, string reportFormat, out string renderErrorMessage)
        {
            renderErrorMessage = "";

            var reportServerCredentials = new ReportServerCredentials(ConfigurationManager.AppSettings["ReportServiceUsername"], ConfigurationManager.AppSettings["ReportServicePassword"], ConfigurationManager.AppSettings["Domain"]);
            string sqlReportServerUrl = ConfigurationManager.AppSettings["ReportServerURL"];
            string sqlReportWebserviceUrl = sqlReportServerUrl + "/reportservice2010.asmx";
            var reportingService = new ReportingService2010();
            reportingService.Url = sqlReportWebserviceUrl;
            reportingService.Credentials = reportServerCredentials.NetworkCredentials;
            SAHL.Common.WebServices.ReportingServices2010.ItemParameter[] reportItemParameters = reportingService.GetItemParameters(reportPath, null, true, null, null);

            // SSRS 2012 has a bug where all dates must be passed as yyyy-MM-dd
            foreach (SAHL.Common.WebServices.ReportingServices2010.ItemParameter reportParam in reportItemParameters)
            {
                switch (reportParam.ParameterTypeName)
                {
                    case "DateTime":
                        {
                            foreach (var kv in parameterNamesAndValues)
                            {
                                if ((kv.Key.Equals(reportParam.Name, StringComparison.CurrentCultureIgnoreCase)) && !String.IsNullOrEmpty(kv.Value))
                                {
                                    DateTime dt;
                                    if (DateTime.TryParse(kv.Value, DateTimeFormatInfo.CurrentInfo, System.Globalization.DateTimeStyles.None, out dt))
                                    {
                                        parameterNamesAndValues[kv.Key] = dt.ToString("yyyy-MM-dd");
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            // Create a new proxy to the web service
            ReportExecutionService rs = new ReportExecutionService();
            rs.Credentials = reportServerCredentials.NetworkCredentials;
            rs.Url = sqlReportServerUrl + @"/ReportExecution2005.asmx";

            // Render arguments
            byte[] renderedReport = null;
            string historyID = null;
            string devInfo = @"<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";

            // Prepare report parameter.
            SAHL.Common.WebServices.ReportExecution2005.ParameterValue[] parameters = new SAHL.Common.WebServices.ReportExecution2005.ParameterValue[parameterNamesAndValues.Count];
            int i = 0;
            foreach (KeyValuePair<string, string> parm in parameterNamesAndValues)
            {
                parameters[i] = new SAHL.Common.WebServices.ReportExecution2005.ParameterValue();
                parameters[i].Name = parm.Key;
                parameters[i].Value = parm.Value;
                i++;
            }

            string encoding;
            string mimeType;
            string extension;
            SAHL.Common.WebServices.ReportExecution2005.Warning[] warnings = null;
            string[] streamIDs = null;

            ExecutionInfo execInfo = new ExecutionInfo();
            ExecutionHeader execHeader = new ExecutionHeader();

            try
            {
                rs.ExecutionHeaderValue = execHeader;

                var extensions = rs.ListRenderingExtensions();

                execInfo = rs.LoadReport(reportPath, historyID);

                rs.SetExecutionParameters(parameters, "en-us");

                renderedReport = rs.Render(reportFormat, devInfo, out extension, out encoding, out mimeType, out warnings, out streamIDs);

                execInfo = rs.GetExecutionInfo();
            }
            catch (SoapException e)
            {
                Dictionary<string, object> methodParameters = new Dictionary<string, object>();
                methodParameters.Add("reportPath", reportPath);
                methodParameters.Add("parameterNamesAndValues", parameterNamesAndValues);
                LogPlugin.Logger.LogErrorMessageWithException("ReportReposiotry.RenderSQLReport", e.Message, e, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParameters } });
                renderErrorMessage = e.ToString();
            }
            catch (Exception e)
            {
                Dictionary<string, object> methodParameters = new Dictionary<string, object>();
                methodParameters.Add("reportPath", reportPath);
                methodParameters.Add("parameterNamesAndValues", parameterNamesAndValues);
                LogPlugin.Logger.LogErrorMessageWithException("ReportReposiotry.RenderSQLReport", e.Message, e, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParameters } });
                renderErrorMessage = e.ToString();
            }

            return renderedReport;
        }

        public string GeneratePDFReport(int reportStatementKey, IDictionary<string, string> parameterNamesAndValues, out string renderErrorMessage)

        {
            renderErrorMessage = "";
            string renderedReport = "";

            // get parameters to pass to pdf generator
            Dictionary<string, object> reportParams = new Dictionary<string, object>();
            foreach (KeyValuePair<string, string> parm in parameterNamesAndValues)
            {
                reportParams.Add(parm.Key, parm.Value);
            }

            // note that we need to impersonate a user with local permissions and rights
            // to the remote folder otherwise we get authentication issues thanks to Kerberos vs. the SAHL network setup
            ISecurityService securityService = ServiceFactory.GetService<ISecurityService>();
            WindowsImpersonationContext wic = null;
            wic = securityService.BeginImpersonation();

            try
            {
                // setup the document generator
                Controller c = new Controller(); // this sets up the pdfgenerator logging
                PDFGenerator generator = new PDFGenerator();
                PDFGenerationObject obj = null;
                obj = new PDFGenerationObject(reportParams, reportStatementKey);

                // generate the pdf document
                renderedReport = generator.GenerateDocument(obj);
            }
            catch (Exception e)
            {
                Dictionary<string, object> methodParameters = new Dictionary<string, object>();
                methodParameters.Add("reportStatementKey", reportStatementKey);
                methodParameters.Add("parameterNamesAndValues", parameterNamesAndValues);
                LogPlugin.Logger.LogErrorMessageWithException("ReportReposiotry.GeneratePDFReport", e.Message, e, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParameters } });
                renderErrorMessage = e.ToString();
            }
            finally
            {
                // end impersonation
                securityService.EndImpersonation(wic);
            }

            return renderedReport;
        }
    }

    #region ExcelExportCodeClass

    public class Export
    {
        public enum ExportFormat : int { CSV = 1, Excel = 2 }; // Export format enumeration

        //HttpResponse response;
        //SessionState.HttpSessionState session;

        private string appType;

        public Export()
        {
            appType = "Web";
        }

        public Export(string ApplicationType)
        {
            appType = ApplicationType;
            if (appType != "Web" && appType != "Win") throw new Exception("Provide valid application format (Web/Win)");
            if (appType == "Web")
            {
                //response = System.Web.HttpContext.Current.Response;
                //session = System.Web.HttpContext.Current.Session;
            }
        }

        #region ExportDetails OverLoad : Type#1

        // Function  : ExportDetails
        // Arguments : DetailsTable, FormatType, FileName
        // Purpose	 : To get all the column headers in the datatable and
        //			   exorts in CSV / Excel format with all columns

        public void ExportDetails(DataTable DetailsTable)
        {
            if (DetailsTable.Rows.Count == 0)
                throw new Exception("There are no details to export.");

            // Create Dataset
            DataSet dsExport = new DataSet("Export");
            DataTable dtExport = DetailsTable.Copy();
            dtExport.TableName = "Values";
            dsExport.Tables.Add(dtExport);

            // Getting Field Names
            string[] sHeaders = new string[dtExport.Columns.Count];
            string[] sFileds = new string[dtExport.Columns.Count];

            for (int i = 0; i < dtExport.Columns.Count; i++)
            {
                sHeaders[i] = dtExport.Columns[i].ColumnName;
                sFileds[i] = dtExport.Columns[i].ColumnName;
            }

            //if (appType == "Web")
            //    Export_with_XSLT_Web(dsExport, sHeaders, sFileds, FormatType, FileName);
        }

        #endregion ExportDetails OverLoad : Type#1

        #region ExportDetails OverLoad : Type#2

        // Function  : ExportDetails
        // Arguments : DetailsTable, ColumnList, FormatType, FileName
        // Purpose	 : To get the specified column headers in the datatable and
        //			   exorts in CSV / Excel format with specified columns

        public void ExportDetails(DataTable DetailsTable, int[] ColumnList, ExportFormat FormatType, out string exportContent)
        {
            string Content = "";

            if (DetailsTable.Rows.Count == 0)
                throw new Exception("There are no details to export");

            // Create Dataset
            DataSet dsExport = new DataSet("Export");
            DataTable dtExport = DetailsTable.Copy();
            dtExport.TableName = "Values";
            dsExport.Tables.Add(dtExport);

            if (ColumnList.Length > dtExport.Columns.Count)
                throw new Exception("ExportColumn List should not exceed Total Columns");

            // Getting Field Names
            string[] sHeaders = new string[ColumnList.Length];
            string[] sFileds = new string[ColumnList.Length];

            for (int i = 0; i < ColumnList.Length; i++)
            {
                if ((ColumnList[i] < 0) || (ColumnList[i] >= dtExport.Columns.Count))
                    throw new Exception("ExportColumn Number should not exceed Total Columns Range");

                sHeaders[i] = dtExport.Columns[ColumnList[i]].ColumnName;
                sFileds[i] = dtExport.Columns[ColumnList[i]].ColumnName;
            }

            if (appType == "Web")
            {
                try
                {
                    // XSLT to use for transforming this dataset.
                    MemoryStream stream = new MemoryStream();
                    XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);

                    CreateStylesheet(writer, sHeaders, sFileds, FormatType);
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);

                    XmlDataDocument xmlDoc = new XmlDataDocument(dsExport);
                    XslTransform xslTran = new XslTransform();
                    xslTran.Load(new XmlTextReader(stream), null, null);

                    System.IO.StringWriter sw = new System.IO.StringWriter();
                    xslTran.Transform(xmlDoc, null, sw, null);

                    //Writeout the Content

                    Content = sw.ToString();
                    Content = Content.Replace("_x0020_", " ");

                    //string ContentType = "";
                    //if (FormatType == ExportFormat.CSV)
                    //    ContentType = "text/csv";
                    //else
                    //    ContentType = "application/vnd.ms-excel";

                    sw.Close();
                    writer.Close();
                    stream.Close();
                }
                catch (ThreadAbortException Ex)
                {
                    string ErrMsg = Ex.Message;
                }
                catch (Exception Ex)
                {
                    exportContent = null;
                    throw Ex;
                }
            }

            exportContent = Content;
        }

        #endregion ExportDetails OverLoad : Type#2

        #region ExportDetails OverLoad : Type#3

        // Function  : ExportDetails
        // Arguments : DetailsTable, ColumnList, Headers, FormatType, FileName
        // Purpose	 : To get the specified column headers in the datatable and
        //			   exorts in CSV / Excel format with specified columns and
        //			   with specified headers

        public void ExportDetails(DataTable DetailsTable, int[] ColumnList, string[] Headers)
        {
            if (DetailsTable.Rows.Count == 0)
                throw new Exception("There are no details to export");

            // Create Dataset
            DataSet dsExport = new DataSet("Export");
            DataTable dtExport = DetailsTable.Copy();
            dtExport.TableName = "Values";
            dsExport.Tables.Add(dtExport);

            if (ColumnList.Length != Headers.Length)
                throw new Exception("ExportColumn List and Headers List should be of same length");
            else if (ColumnList.Length > dtExport.Columns.Count || Headers.Length > dtExport.Columns.Count)
                throw new Exception("ExportColumn List should not exceed Total Columns");

            // Getting Field Names
            string[] sFileds = new string[ColumnList.Length];

            for (int i = 0; i < ColumnList.Length; i++)
            {
                if ((ColumnList[i] < 0) || (ColumnList[i] >= dtExport.Columns.Count))
                    throw new Exception("ExportColumn Number should not exceed Total Columns Range");

                sFileds[i] = dtExport.Columns[ColumnList[i]].ColumnName;
            }

            //if (appType == "Web")
            //    Export_with_XSLT_Web(dsExport, Headers, sFileds, FormatType, FileName);
        }

        #endregion ExportDetails OverLoad : Type#3

        #region Export_with_XSLT_Web

        // Function  : Export_with_XSLT_Web
        // Arguments : dsExport, sHeaders, sFileds, FormatType, FileName
        // Purpose   : Exports dataset into CSV / Excel format

        //private void Export_with_XSLT_Web(DataSet dsExport, string[] sHeaders, string[] sFileds, ExportFormat FormatType, string FileName)
        //{
        //}

        #endregion Export_with_XSLT_Web

        #region CreateStylesheet

        // Function  : WriteStylesheet
        // Arguments : writer, sHeaders, sFileds, FormatType
        // Purpose   : Creates XSLT file to apply on dataset's XML file

        private void CreateStylesheet(XmlTextWriter writer, string[] sHeaders, string[] sFileds, ExportFormat FormatType)
        {
            // xsl:stylesheet
            string ns = "http://www.w3.org/1999/XSL/Transform";
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("xsl", "stylesheet", ns);
            writer.WriteAttributeString("version", "1.0");
            writer.WriteStartElement("xsl:output");
            writer.WriteAttributeString("method", "text");
            writer.WriteAttributeString("version", "4.0");
            writer.WriteEndElement();

            // xsl-template
            writer.WriteStartElement("xsl:template");
            writer.WriteAttributeString("match", "/");

            // xsl:value-of for headers
            for (int i = 0; i < sHeaders.Length; i++)
            {
                writer.WriteString("\"");
                writer.WriteStartElement("xsl:value-of");
                writer.WriteAttributeString("select", "'" + sHeaders[i].Replace(" ", "_x0020_") + "'");
                writer.WriteEndElement(); // xsl:value-of
                writer.WriteString("\"");
                if (i != sFileds.Length - 1) writer.WriteString((FormatType == ExportFormat.CSV) ? "," : "	");
            }

            // xsl:for-each
            writer.WriteStartElement("xsl:for-each");
            writer.WriteAttributeString("select", "Export/Values");
            writer.WriteString("\r\n");

            // xsl:value-of for data fields
            for (int i = 0; i < sFileds.Length; i++)
            {
                writer.WriteString("\"");
                writer.WriteStartElement("xsl:value-of");
                writer.WriteAttributeString("select", sFileds[i].Replace(" ", "_x0020_"));
                writer.WriteEndElement(); // xsl:value-of
                writer.WriteString("\"");
                if (i != sFileds.Length - 1) writer.WriteString((FormatType == ExportFormat.CSV) ? "," : "	");
            }

            writer.WriteEndElement(); // xsl:for-each
            writer.WriteEndElement(); // xsl-template
            writer.WriteEndElement(); // xsl:stylesheet
            writer.WriteEndDocument();
        }
    }

        #endregion CreateStylesheet

    #endregion ExcelExportCodeClass
}