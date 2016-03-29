using System;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Views.Common.Models.Affordability;
using System.Linq;
namespace SAHL.Web.Views.Common.Presenters.AffordabilityDetailsPresenters
{
    /// <summary>
    ///
    /// </summary>
    public class AffordabilityDetails : AffordabilityDetailsBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public AffordabilityDetails(IAffordabilityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.OnCancelButtonClicked += _view_OnCancelButtonClicked;
            _view.application = Application;
            _view.LegalEntity = LegalEntity;
            _view.BindControls();
        }

        static void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;
            _view.ReadOnly = true;
            _view.ShowButtons = false;
        }
    }
}