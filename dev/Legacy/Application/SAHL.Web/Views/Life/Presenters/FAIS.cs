using System;
using System.Data;
using System.Configuration;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class FAIS : FAISBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public FAIS(IFAIS view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            Activity = "FAISDone";
            _view.ConfirmationMode = false;

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
                IX2Service svc = ServiceFactory.GetService<IX2Service>() as IX2Service;

                // Update the ConfirmationRequired and ContactNumber on the X2DATA.LifeOrigination table
                Dictionary<string, string> x2Data = new Dictionary<string, string>();
                x2Data.Add("ConfirmationRequired", _view.ConfirmationRequired.ToString());
                x2Data.Add("ContactNumber", _view.ContactNumber.ToString());

                // Navigate to the next State - base upon whether confirmation is required or not.
                // 1. If confirmatioj is required then perform "Awaiting Spouse Confirmation" activity
                // 2. If no confirmation required then perform "Accept FAIS" activity

                // use the X2InstanceRedirect view here so it gives the x2map system decision time to finish before the navigate happens
                // otherwise it navigates correctly but the actions are not refreshed correctly.
                // add the instanceID to the global cache for the X2InstanceRedirect to use
                GlobalCacheData.Remove(ViewConstants.InstanceID);
                GlobalCacheData.Add(ViewConstants.InstanceID, base.Node.InstanceID, new List<ICacheObjectLifeTime>());

                svc.WorkFlowWizardNext(_view.CurrentPrincipal, _view.ViewName, _view.Navigator, x2Data,"X2InstanceRedirectSystemDecision");
            }
        }
    }
}
