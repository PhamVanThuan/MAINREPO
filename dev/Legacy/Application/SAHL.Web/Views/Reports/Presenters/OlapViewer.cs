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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel;
using System.Collections.Generic;

namespace SAHL.Web.Views.Reports.Presenters
{
    public class OlapViewer : SAHLCommonBasePresenter<IOlapViewer>
    {
        const string ReportStatementData = "REPORTSTATEMENTDATA";
        const string ReportParameterData = "REPORTPARAMETERDATA";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public OlapViewer(IOlapViewer view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            if (!_view.ShouldRunPage) return;

            IReportStatement rs = null;
            if (GlobalCacheData.ContainsKey(ViewConstants.ReportStatement))
            {
                rs = GlobalCacheData[ViewConstants.ReportStatement] as IReportStatement;

                //IReportRepository rr = RepositoryFactory.GetRepository<IReportRepository>();

              //  string reportText = rr.GetUIStatementText(rs);              

                string reportText = rs.Key.ToString();              

              //  reportText = string.Format(reportText, ConfigurationManager.AppSettings["CubeReportDataSource"].ToString());

                _view.ReportText = reportText;

            }
        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("RSViewer");

        }
    }
}
