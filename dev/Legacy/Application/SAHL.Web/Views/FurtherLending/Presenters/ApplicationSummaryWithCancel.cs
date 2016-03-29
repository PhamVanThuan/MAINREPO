using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.FurtherLending.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.FurtherLending.Presenters
{
    public class ApplicationSummaryWithCancel : ApplicationSummaryBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ApplicationSummaryWithCancel(IApplicationSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            // set the cancel button to visible
            _view.ShowCancelButton = true;
            _view.ShowHistoryButton = true;

            // only show the selected application
            _view.SelectedApplicationOnly = true;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);

            // call the base presenter
            base.OnViewInitialised(sender, e);
        }


        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.ShouldRunPage = false;
            Navigator.Navigate("Cancel");
        }
    }
}
