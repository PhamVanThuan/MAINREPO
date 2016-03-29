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
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.DomainMessages;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationWizardDeclarations : ApplicationLegalEntityDeclarationBase
    {
        public ApplicationWizardDeclarations(IApplicationLegalEntityDeclaration view, SAHLCommonBaseController controller)
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
            // get the legalentitykey and the applicationkey from the global cache
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                base.LegalEntityKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedLegalEntityKey].ToString());
            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
                base.ApplicationKey  = Convert.ToInt32(GlobalCacheData[ViewConstants.ApplicationKey].ToString());

            // set update mode
            _view.UpdateMode = true;
            _view.UpdateButtonText = "Next";

            // call the base initialise
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.onBackButtonClicked += new EventHandler(_view_onBackButtonClicked);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_onBackButtonClicked(object sender, EventArgs e)
        {
            if (GlobalCacheData.ContainsKey("SKIPCALCULATOR"))
            {
                _view.Navigator.Navigate("BackToApplicant");
            }
            else
            {
                IList<ICacheObjectLifeTime> lifeTimes = new List<ICacheObjectLifeTime>();
                GlobalCacheData.Add("MUSTNAVIGATE", true, lifeTimes);
                _view.Navigator.Navigate("BackToCalculator");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            _view.ShowUpdateButton = true;
            _view.ShowBackButton = true;
            _view.ShowCancelButton = false;
        }
    }
}
