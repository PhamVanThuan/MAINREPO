using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Presenters;
using SAHL.Web.Views.Reports.Interfaces;

namespace SAHL.Web.Views.Reports.Presenters
{
    /// <summary>
    /// Presenter Class for the View Report Option of RS Viewer
    /// </summary>
    public class RSViewerView : RSViewerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="View">Interface</param>
        /// <param name="Controller">SAHLCommonController</param>
        public RSViewerView(IRSViewer View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            _view.onCancelButtonClicked += new EventHandler(_view_onCancelButtonClicked);
            _view.onViewButtonClicked += new EventHandler<ReportParametersEventArgs>(_view_onViewButtonClicked);

        }

        protected void _view_onCancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("ClientSearch");
        }

        protected void _view_onViewButtonClicked(object sender, ReportParametersEventArgs e)
        {
           
        }

     
        protected void _view_onExportButtonClicked(object sender, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
