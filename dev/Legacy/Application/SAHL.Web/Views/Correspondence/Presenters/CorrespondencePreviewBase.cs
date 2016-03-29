using AjaxControlToolkit;
using Microsoft.Reporting.WebForms;
using SAHL.Common.BusinessModel.Authentication;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Logging;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Correspondence.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace SAHL.Web.Views.Correspondence.Presenters.Correspondence
{
    public class CorrespondencePreviewBase : SAHLCommonBasePresenter<ICorrespondencePreview>
    {
        private List<ReportData> reportDataList = new List<ReportData>();
        private IReportRepository _reportRepo;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CorrespondencePreviewBase(ICorrespondencePreview view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        private static void SAHLReport_ReportError(object sender, ReportErrorEventArgs e)
        {
            LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().Name, string.Empty, e.Exception);
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.OnSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);

            _reportRepo = RepositoryFactory.GetRepository<IReportRepository>();

            if (GlobalCacheData.ContainsKey(ViewConstants.CorrespondenceReportDataList))
                reportDataList.AddRange(GlobalCacheData[ViewConstants.CorrespondenceReportDataList] as List<ReportData>);

            RenderReports();
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage)
                return;
        }

        private void RenderReports()
        {
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            try
            {
                // Create the tabpanel container
                TabContainer tabContainer = new TabContainer();
                tabContainer.ID = "tabContainer";
                tabContainer.Width = new Unit(99, UnitType.Percentage);

                int reportIndex = 0;

                foreach (ReportData reportData in reportDataList)
                {
                    // force an output tpe parameter of 'email'
                    foreach (ReportDataParameter parm in reportData.ReportParameters)
                    {
                        if (parm.ParameterName.ToLower() == reportData.MailingTypeParameterName.ToLower())
                        {
                            parm.ParameterValue = Convert.ToString((int)SAHL.Common.Globals.CorrespondenceMediums.Email);
                            break;
                        }
                    }

                    reportIndex++;

                    // set the report on the reportviewer
                    IReportStatement reportStatement = RepositoryFactory.GetRepository<IReportRepository>().GetReportStatementByKey(reportData.ReportStatementKey);

                    // setup the viewer controls
                    ReportViewer reportViewer = null;
                    SAHLPdfViewer pdfViewer = null;
                    switch (reportStatement.ReportType.Key)
                    {
                        case (int)SAHL.Common.Globals.ReportTypes.ReportingServicesReport:

                            #region Reporting Services Report

                            // setup the reportviewer control
                            reportViewer = new ReportViewer();
                            reportViewer.ID = "SAHLReport" + reportIndex.ToString();
                            reportViewer.Width = new Unit(100, UnitType.Percentage);
                            reportViewer.BorderStyle = BorderStyle.Solid;
                            reportViewer.BorderColor = Color.BlanchedAlmond;
                            reportViewer.BorderWidth = new Unit(1, UnitType.Pixel);
                            reportViewer.ProcessingMode = ProcessingMode.Remote;
                            reportViewer.AsyncRendering = false;

                            reportViewer.ReportError += new ReportErrorEventHandler(SAHLReport_ReportError);

                            if (!_view.IsPostBack)
                            {
                                //Get the location of the SQL 2005 Reporting Server out of the appsettings in the web config
                                reportViewer.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerURL"]);
                                reportViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials(HttpContext.Current);
                                reportViewer.ShowParameterPrompts = false;
                                reportViewer.PromptAreaCollapsed = true;
                                reportViewer.ZoomMode = ZoomMode.Percent;
                                reportViewer.ZoomPercent = 100;
                                reportViewer.ShowZoomControl = true;
                                reportViewer.ShowExportControls = false;
                                reportViewer.ShowPrintButton = false;

                                // set the report path
                                reportViewer.ServerReport.ReportPath = reportStatement.StatementName;

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

                                    foreach (ReportDataParameter reportDataParameter in reportData.ReportParameters)
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
                            }

                            #endregion Reporting Services Report

                            break;

                        case (int)SAHL.Common.Globals.ReportTypes.StaticPDF:
                        case (int)SAHL.Common.Globals.ReportTypes.PDFReport:

                            #region GeneratedPDF / PDFReport

                            // setup the pdfviewer control
                            pdfViewer = new SAHLPdfViewer();
                            pdfViewer.BorderStyle = BorderStyle.Solid;
                            pdfViewer.BorderColor = Color.BlanchedAlmond;
                            pdfViewer.BorderWidth = new Unit(1, UnitType.Pixel);
                            pdfViewer.Height = new Unit(440, UnitType.Pixel);
                            pdfViewer.Width = new Unit(100, UnitType.Percentage);

                            if (reportStatement.ReportType.Key == (int)SAHL.Common.Globals.ReportTypes.StaticPDF)
                            {
                                pdfViewer.FilePath = reportStatement.StatementName;
                            }
                            else
                            {
                                // get parameters to pass to pdf generator
                                Dictionary<string, string> reportParams = new Dictionary<string, string>();
                                foreach (ReportDataParameter parm in reportData.ReportParameters)
                                {
                                    reportParams.Add(parm.ParameterName, parm.ParameterValue.ToString());
                                }

                                //generate the pdf reports
                                string errorMessage = "";
                                pdfViewer.FilePath = _reportRepo.GeneratePDFReport(reportStatement.Key, reportParams, out errorMessage);
                            }
                            #endregion  GeneratedPDF / PDFReport

                            break;

                        default:
                            break;
                    }

                    //create the tab panel
                    TabPanel tabPanel = new TabPanel();
                    tabPanel.HeaderText = reportData.ReportName;
                    //tabPanel.Height = new Unit(100, UnitType.Percentage);
                    tabPanel.ScrollBars = ScrollBars.Auto;

                    // add the viewer control to the tab panel
                    if (reportStatement.ReportType.Key == (int)SAHL.Common.Globals.ReportTypes.PDFReport || reportStatement.ReportType.Key == (int)SAHL.Common.Globals.ReportTypes.StaticPDF)
                        tabPanel.Controls.Add(pdfViewer);
                    else
                        tabPanel.Controls.Add(reportViewer);

                    // add the tab panel to the tab container
                    tabContainer.Tabs.Add(tabPanel);
                }

                // call the method to add the panels to the panel container
                _view.AddReportsToPage(tabContainer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected virtual void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // navigate back to calling page
            string navigateTo = GlobalCacheData[ViewConstants.CorrespondenceNavigateTo].ToString();
            GlobalCacheData.Remove(ViewConstants.CorrespondenceNavigateTo);
            _view.Navigator.Navigate(navigateTo);
        }
    }
}