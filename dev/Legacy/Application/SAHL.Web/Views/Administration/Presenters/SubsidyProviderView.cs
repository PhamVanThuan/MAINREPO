using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration.Presenters
{
    /// <summary>
    /// Subsidy Provider View
    /// </summary>
    public class SubsidyProviderView : SubsidyProviderBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public SubsidyProviderView(ISubsidyProvider view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage) return;

            _view.SetControlsForDisplay = false;
            _view.SetButtonVisibility = false;
            _view.OnReBindSubsidyDetails+=new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnReBindSubsidyDetails);
          
        }
        /// <summary>
        /// OnViewPreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            if (subsidyProvider != null)
                _view.BindSubsidyProviderDetail(subsidyProvider);
        }
        /// <summary>
        /// Rebind subsidy details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _view_OnReBindSubsidyDetails(object sender, KeyChangedEventArgs e)
        {
            subsidyProvider = empRepo.GetSubsidyProviderByKey(Convert.ToInt32(e.Key));
        }
    }
}
