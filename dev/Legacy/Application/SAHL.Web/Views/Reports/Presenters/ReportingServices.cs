using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Reports.Interfaces;
using System.Collections;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.Reports.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportingServices : SAHLCommonBasePresenter<IReportingServicesReport>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ReportingServices(IReportingServicesReport view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            string RequestString = "?ReportPath="; // build report name here
            string ReportPath = "";
            if (GlobalCacheData.ContainsKey(ViewConstants.ReportParameters))
            {
                IDictionary<IReportParameter, object> parameters = GlobalCacheData[ViewConstants.ReportParameters] as IDictionary<IReportParameter, object>;
                _view.parameters = parameters;

                if (GlobalCacheData.ContainsKey(ViewConstants.ReportRequestString))
                {
                    string request = GlobalCacheData[ViewConstants.ReportRequestString] as string;
                    RequestString = request;
                }
                else if (GlobalCacheData.ContainsKey(ViewConstants.ReportStatement))
                {
                    IReportStatement rs = GlobalCacheData[ViewConstants.ReportStatement] as IReportStatement;
                    RequestString += rs.StatementName;
                    ReportPath = rs.StatementName;
                }

                if (GlobalCacheData.ContainsKey(ViewConstants.ReportPath))
                {
                    ReportPath = GlobalCacheData[ViewConstants.ReportPath] as string;
                }

                _view.ReportPath = ReportPath;
            }
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }
    }
}
