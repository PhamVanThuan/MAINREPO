using System;
using System.Data;
using System.Configuration;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class FAISConfirm : FAISBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public FAISConfirm(IFAIS view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            Activity = "FAISConfirmationDone";
            _view.ConfirmationMode = true;

            base.OnViewInitialised(sender, e);

            _view.OnNextButtonClicked += new EventHandler(OnNextButtonClicked);
        }
        /// <summary>
        /// Handles the event fired by the view when the Next button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnNextButtonClicked(object sender, EventArgs e)
        {
            base.ValidateInput();

            if (_view.IsValid)
            {
                X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;

                // Navigate to the next State
                svc.WorkFlowWizardNext(_view.CurrentPrincipal, _view.ViewName, _view.Navigator);
            }
        }
    }
}
