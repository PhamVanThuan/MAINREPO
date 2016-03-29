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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using System.Collections.Generic;

namespace SAHL.Web.Views.Reports.Presenters
{
    public class ReAdvanceRecommendation : SAHLCommonBasePresenter<IReAdvanceRecommendation>
    {
        public ReAdvanceRecommendation(IReAdvanceRecommendation view, SAHLCommonBaseController controller)
            : base(view, controller)
        {        
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            //if (GlobalCacheData.ContainsKey(ViewConstants.ReportStatement))
            //{
            //    IReportStatement rs = GlobalCacheData[ViewConstants.ReportStatement] as IReportStatement;
            //    _view.ReportStatement = rs;
            //}
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("RSViewer");
        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();
            if (GlobalCacheData.ContainsKey(ViewConstants.ReportParameters))
            {
                GlobalCacheData.Remove(ViewConstants.ReportParameters);
            }
            GlobalCacheData.Add(ViewConstants.ReportParameters, _view.lstParameters, LifeTimes);
            if (GlobalCacheData.ContainsKey(ViewConstants.ReportPath))
            {
                GlobalCacheData.Remove(ViewConstants.ReportPath);
            }
            GlobalCacheData.Add(ViewConstants.ReportPath, "/SAHL/Serv.LS.Readvance Recommendation", LifeTimes);


            _view.Navigator.Navigate("ReportingServices");
        }
    }
}
