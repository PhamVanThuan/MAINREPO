using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Reports.Interfaces;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using System.Security.Principal;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Web.Views.Reports.Presenters
{
    public class PDFReportViewer : SAHLCommonBasePresenter<IPDFReportViewer>
    {
        private IReportStatement _reportStatement;
        Dictionary<SAHL.Common.BusinessModel.Interfaces.IReportParameter, object> _params;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PDFReportViewer(IPDFReportViewer view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", Justification = "Controller c = new Controller() - This needs to be either removed or fixed!")]
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.onCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);

            if (!_view.Cancelled)
            {
                // get stuff from cache
                if (GlobalCacheData.ContainsKey(ViewConstants.ReportParameters))
                {
                    if (GlobalCacheData.ContainsKey(ViewConstants.ReportStatement))
                        _reportStatement = GlobalCacheData[ViewConstants.ReportStatement] as IReportStatement;

                    _params = GlobalCacheData[ViewConstants.ReportParameters] as Dictionary<IReportParameter, object>;

                    // get parameters to pass to pdf generator
                    Dictionary<string, string> reportParams = new Dictionary<string, string>();
                    foreach (KeyValuePair<IReportParameter, object> parm in _params)
                    {
                        reportParams.Add(parm.Key.ParameterName, parm.Value.ToString());
                    }

                    //generate the pdf reports
                    string errorMessage = "";

                    _view.PDFReportPath = RepositoryFactory.GetRepository<IReportRepository>().GeneratePDFReport(_reportStatement.Key, reportParams, out errorMessage);
                }
            }
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("RSViewer");
        }
    }
}
