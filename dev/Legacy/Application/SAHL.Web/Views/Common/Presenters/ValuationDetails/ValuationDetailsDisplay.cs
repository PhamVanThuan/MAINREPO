using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{
    /// <summary>
    /// Valuation Details Display
    /// </summary>
    public class ValuationDetailsDisplay : ValuationDetailsBase
    {
        /// <summary>
        /// ValuationDetail Display constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ValuationDetailsDisplay(IValuationManualDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// On View Initialised
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _view.Properties = Properties;
            _view.ShowLabels = true;
            _view.ShowNavigationButtons = true;

            _view.OnValuationDetailsClicked+= (_view_OnValuationDetailsClicked);
        }

        /// <summary>
        /// OnView PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            _view.ShowButtons = false;
                   
        }

        protected void _view_OnValuationDetailsClicked(object sender, KeyChangedEventArgs e)
        {
            int valSelected = _view.GetSelectedValuationKey;

            // Save Selected Valuation to Global Cache

            GlobalCacheData.Remove(ViewConstants.SelectedValuationKey);
            GlobalCacheData.Add(ViewConstants.SelectedValuationKey, valSelected, new List<ICacheObjectLifeTime>());

            GlobalCacheData.Remove(ViewConstants.ValuationPresenter);
            GlobalCacheData.Add(ViewConstants.ValuationPresenter, _view.ViewName, new List<ICacheObjectLifeTime>());

            Navigator.Navigate("Details");
        }

    }
}
