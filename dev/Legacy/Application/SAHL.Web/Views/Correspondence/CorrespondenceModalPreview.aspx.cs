using Microsoft.Reporting.WebForms;
using SAHL.Common.BusinessModel.Authentication;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Logging;
using SAHL.Common.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAHL.Web.Views.Correspondence
{
    public partial class CorrespondenceModalPreview : System.Web.UI.Page
    {
        private int _reportStatementKey;
        private Dictionary<string, object> _queryParams;

        private IReportRepository _reportRepo;

        public IReportRepository ReportRepo
        {
            get
            {
                if (_reportRepo == null)
                {
                    _reportRepo = new ReportRepository();
                }
                return _reportRepo;
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            // this must be set here for the report control to work
            EnableViewState = true;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Request.QueryString["ReportStatementKey"].ToString().Length > 0)
            {
                _reportStatementKey = Convert.ToInt32(Request.QueryString["ReportStatementKey"].ToString());
            }

            _queryParams = new Dictionary<string, object>();
            foreach (String key in Request.QueryString.AllKeys)
            {
                if (key != "ReportStatementKey")
                {
                    _queryParams.Add(key, Request.QueryString[key]);
                }
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RenderReports();
        }

        private static void SAHLReport_ReportError(object sender, ReportErrorEventArgs e)
        {
            LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().Name, string.Empty, e.Exception);
        }

        private void RenderReports()
        {
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            try
            {
                // Create the tabpanel container
                //tabContainer = new TabContainer();
                //tabContainer.ID = "tabContainer";
                //tabContainer.Width = new Unit(99, UnitType.Percentage);

                int reportIndex = 0;
                //var ReportName = "Debt Counselling Certificate of Balance";
                int ReportStatementKey = _reportStatementKey;
                var ReportParameters = new List<ReportDataParameter>();

                foreach (var param in _queryParams)
                {
                    ReportParameters.Add(new ReportDataParameter(-1, param.Key, param.Value));
                }

                //foreach (ReportData reportData in reportDataList)
                //{
                // force an output tpe parameter of 'email'
                //foreach (ReportDataParameter parm in reportData.ReportParameters)
                //{
                //    if (parm.ParameterName.ToLower() == reportData.MailingTypeParameterName.ToLower())
                //    {
                //        parm.ParameterValue = Convert.ToString((int)SAHL.Common.Globals.CorrespondenceMediums.Email);
                //        break;
                //    }
                //}

                reportIndex++;

                // set the report on the reportviewer
                //IReportStatement reportStatement = RepositoryFactory.GetRepository<IReportRepository>().GetReportStatementByKey(reportData.ReportStatementKey);
                IReportStatement reportStatement = RepositoryFactory.GetRepository<IReportRepository>().GetReportStatementByKey(ReportStatementKey);
                //reportName.InnerText = reportStatement.ReportName;

                // setup the viewer controls
                //ReportViewer reportViewer = null;
                SAHLPdfViewer pdfViewer = null;
                switch (reportStatement.ReportType.Key)
                {
                    case (int)SAHL.Common.Globals.ReportTypes.ReportingServicesReport:

                        #region Reporting Services Report

                        // setup the reportviewer control
                        var reportViewer = new ReportViewer();
                        reportViewer.ID = "reportViewer";
                        reportViewer.Height = new Unit(100, UnitType.Percentage);
                        reportViewer.Width = new Unit(100, UnitType.Percentage);
                        reportViewer.BorderStyle = BorderStyle.Solid;
                        reportViewer.BorderColor = Color.BlanchedAlmond;
                        reportViewer.BorderWidth = new Unit(1, UnitType.Pixel);
                        reportViewer.ProcessingMode = ProcessingMode.Remote;

                        reportViewer.ReportError += new ReportErrorEventHandler(SAHLReport_ReportError);
                        //Get the location of the SQL 2005 Reporting Server out of the appsettings in the web config
                        reportViewer.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerURL"]);
                        reportViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials(HttpContext.Current);
                        reportViewer.ShowParameterPrompts = false;
                        reportViewer.PromptAreaCollapsed = true;
                        //reportViewer.ZoomMode = ZoomMode.Percent;
                        //reportViewer.ZoomPercent = 95;
                        reportViewer.ShowZoomControl = true;
                        reportViewer.ShowExportControls = false;
                        reportViewer.ShowPrintButton = false;

                        // set the report path
                        reportViewer.ServerReport.ReportPath = reportStatement.StatementName;
                        //reportViewer.AsyncRendering = false;

                        #region get the paramemters from the sql report

                        // get the paramemters from the sql report
                        ReportParameterInfoCollection reportParameterInfoCollection = reportViewer.ServerReport.GetParameters();

                        // setup our report parameters collection that we will populate and then pass to the report at runtime
                        ReportParameter[] reportParameters = new ReportParameter[reportParameterInfoCollection.Count];

                        for (int i = 0; i < reportParameterInfoCollection.Count; ++i)
                        {
                            reportParameters[i] = new ReportParameter();

                            if (reportParameterInfoCollection[i].PromptUser == false)
                            {
                                reportParameters[i].Name = reportParameterInfoCollection[i].Name;
                                continue;
                            }

                            //foreach (ReportDataParameter reportDataParameter in reportData.ReportParameters)
                            foreach (ReportDataParameter reportDataParameter in ReportParameters)
                            {
                                if (reportDataParameter.ParameterName.ToLower() == reportParameterInfoCollection[i].Name.ToLower())
                                {
                                    reportParameters[i].Name = reportParameterInfoCollection[i].Name;
                                    if (reportDataParameter.ParameterValue != null)
                                    {
                                        string RequestValue = reportDataParameter.ParameterValue.ToString();

                                        if ((RequestValue != null) && (RequestValue != "#null#"))
                                        {
                                            switch (reportParameterInfoCollection[i].DataType)
                                            {
                                                case ParameterDataType.DateTime:
                                                    DateTime dt;
                                                    if (DateTime.TryParse(RequestValue, DateTimeFormatInfo.CurrentInfo, System.Globalization.DateTimeStyles.None, out dt))
                                                    {
                                                        // Reporting Services has a bug where al ldates must be passed as MM-dd-yyyy
                                                        reportParameters[i].Values.Add(dt.ToString("yyyy-MM-dd"));
                                                    }
                                                    break;

                                                case ParameterDataType.String:
                                                    if (reportParameterInfoCollection[i].MultiValue)
                                                    {
                                                        string[] szValues = RequestValue.Split('\r');
                                                        foreach (string szValue in szValues)
                                                        {
                                                            reportParameters[i].Values.Add(szValue);
                                                        }
                                                    }
                                                    else
                                                        reportParameters[i].Values.Add(RequestValue);
                                                    break;

                                                case ParameterDataType.Float:
                                                case ParameterDataType.Integer:
                                                case ParameterDataType.Boolean:
                                                    reportParameters[i].Values.Add(RequestValue);
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                        }

                        #endregion get the paramemters from the sql report

                        // set the report parameters
                        reportViewer.ServerReport.SetParameters(reportParameters);
                        Panel1.Controls.Add(reportViewer);

                        #endregion Reporting Services Report

                        break;

                    case (int)SAHL.Common.Globals.ReportTypes.StaticPDF:
                    case (int)SAHL.Common.Globals.ReportTypes.PDFReport:

                        #region  GeneratedPDF / PDFReport

                        // setup the pdfviewer control
                        pdfViewer = new SAHLPdfViewer();
                        pdfViewer.BorderStyle = BorderStyle.Solid;
                        pdfViewer.BorderColor = Color.BlanchedAlmond;
                        pdfViewer.BorderWidth = new Unit(1, UnitType.Pixel);
                        pdfViewer.Height = new Unit(700, UnitType.Pixel);
                        pdfViewer.Width = new Unit(100, UnitType.Percentage);

                        if (reportStatement.ReportType.Key == (int)SAHL.Common.Globals.ReportTypes.StaticPDF)
                        {
                            pdfViewer.FilePath = reportStatement.StatementName;
                        }
                        else
                        {
                            // get parameters to pass to pdf generator
                            Dictionary<string, string> reportParams = new Dictionary<string, string>();
                            foreach (ReportDataParameter parm in ReportParameters)
                            {
                                reportParams.Add(parm.ParameterName, parm.ParameterValue.ToString());
                            }

                            //generate the pdf reports
                            string errorMessage = "";
                            pdfViewer.FilePath = ReportRepo.GeneratePDFReport(reportStatement.Key, reportParams, out errorMessage);
                        }

                        #endregion  GeneratedPDF / PDFReport

                        break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void Panel1_Init(object sender, EventArgs e)
        {
            if (ScriptManager.GetCurrent(Page) == null)
            {
                ScriptManager sMgr = new ScriptManager();
                sMgr.EnablePartialRendering = true;
                sMgr_place.Controls.Add(sMgr);
            }
        }
    }
}