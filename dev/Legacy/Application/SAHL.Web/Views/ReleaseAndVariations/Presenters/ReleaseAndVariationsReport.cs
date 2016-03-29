using System;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.ReleaseAndVariations.Interfaces;

using SAHL.Common.BusinessModel.Authentication;
using SAHL.Common.Logging;
using System.Configuration;

namespace SAHL.Web.Views.ReleaseAndVariations.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class ReleaseAndVariationsReport : SAHLCommonBasePresenter<IReleaseAndVariationsReport>
    {
        private IReleaseAndVariationsRepository releaseAndVariationsRepository;
        public CBOMenuNode node;

        static void SAHLReport_ReportError(object sender, ReportErrorEventArgs e)
        {
            LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().Name, string.Empty, e.Exception);
        }

        private IReleaseAndVariationsRepository ReleaseAndVariationsRepository
        {
            get
            {
                if (releaseAndVariationsRepository == null)
                    releaseAndVariationsRepository = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
                return releaseAndVariationsRepository;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ReleaseAndVariationsReport(IReleaseAndVariationsReport view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            View.btnCancelClicked += View_btnCancelClicked;
            View.ShowbtnCancel = true;
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            //View.ShowbtnUpdate = true;
            if (!PrivateCacheData.ContainsKey("ReportDone"))
                SetupReport();
            else
                PrivateCacheData.Remove("ReportDone");
        }

        void View_btnCancelClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }

        void  SetupReport()
        {
            int AccountKey = Convert.ToInt32(GlobalCacheData["AccountKey"]);
            int MemoKey = ReleaseAndVariationsRepository.GetMemoKey(AccountKey);

            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
           // List<ServerReport> sqlReportList = new List<ServerReport>();
            ReportViewer ReportViewer = new ReportViewer();

            ReportViewer.ID = "Release_and_Variations_Summary";
            ReportViewer.Width = new Unit(100, UnitType.Percentage);
            //_reportViewer.Height = new Unit(98, UnitType.Percentage);
            ReportViewer.BorderStyle = BorderStyle.Solid;
            ReportViewer.BorderColor = Color.BlanchedAlmond;
            ReportViewer.BorderWidth = new Unit(1, UnitType.Pixel);
            ReportViewer.ProcessingMode = ProcessingMode.Remote;

            ReportViewer.ReportError += SAHLReport_ReportError;
            ReportViewer.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerURL"]);
            ReportViewer.ServerReport.ReportPath = "/Loan Servicing/Release and Variations Summary";
            HttpContext Context = HttpContext.Current;
            ReportViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials(Context);
            ReportViewer.ShowParameterPrompts = false;
            ReportViewer.PromptAreaCollapsed = true;
            ReportViewer.ZoomMode = ZoomMode.Percent;
            ReportViewer.ZoomPercent = 100;
            ReportViewer.ShowZoomControl = true;
            ReportViewer.ShowExportControls = false;
            ReportViewer.ShowPrintButton = false;

            ReportViewer.ShowPrintButton = true;
            ReportViewer.ShowExportControls = true;

            // setup the report parameters
            ReportParameterInfoCollection reportParameterInfoCollection = ReportViewer.ServerReport.GetParameters();
            ReportParameter[] reportParameters = new ReportParameter[reportParameterInfoCollection.Count];
            reportParameters[0] = new ReportParameter();
            reportParameters[0].Name = "MemoKey";
            reportParameters[0].Values.Add(MemoKey.ToString());
            reportParameters[1] = new ReportParameter();
            reportParameters[1].Name = "AccountKey";
            reportParameters[1].Values.Add(AccountKey.ToString());
            
            ReportViewer.ServerReport.SetParameters(reportParameters);
            ReportViewer.EnableViewState = true;

            // add the report to our collection for use later
            //sqlReportList.Add(ReportViewer.ServerReport);
            PrivateCacheData.Remove("ReportDone");
            PrivateCacheData.Add("ReportDone", true);
            View.AddReport(ReportViewer);
        }



    }

}
