using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationWizardCalculator : ApplicationWizardCalculatorBase
    {

        bool _navigateNext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ApplicationWizardCalculator(IApplicationWizardCalculator view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            _view.OnBackButtonClicked += new EventHandler(_view_OnBackButtonClicked);
            if (!_view.ShouldRunPage)
                return;

            int key = -1;

            if (GlobalCacheData.ContainsKey(ViewConstants.EstateAgentApplication))
                _view.IsEstateAgentDeal = Convert.ToBoolean(GlobalCacheData[ViewConstants.EstateAgentApplication]);
            else
                _view.IsEstateAgentDeal = false;

            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
            {
                key = int.Parse(GlobalCacheData[ViewConstants.ApplicationKey].ToString());
            }

            _application = AppRepo.GetApplicationByKey(key);
			if (_application.ApplicationSource != null)
				_view.ApplicationSource = _application.OriginationSource.Description;

            if (_application != null && _application.ApplicationType.Key != (int)SAHL.Common.Globals.OfferTypes.Unknown && _application.ApplicationInformations.Count > 0)
            {
                _view.BindControls(_application);
            }

            if (_application.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.Unknown && GlobalCacheData.ContainsKey("MUSTNAVIGATE"))
            {
                if ((bool)GlobalCacheData["MUSTNAVIGATE"] == true)
                {
                    _navigateNext = true;
                }
               
            }
            else
            {
                if (!GlobalCacheData.ContainsKey("MUSTNAVIGATE"))
                {
                    _navigateNext = true;
                }
                else
                {
                    if ((bool)GlobalCacheData["MUSTNAVIGATE"] == false)
                        _view_OnCalculateButtonClicked(sender, e);
                }

                _view.CallStartupScript = true;
            }

            _view.MortgageLoanApplication = _application;
            foreach (IApplicationRole r in _application.ApplicationRoles)
            {
                if (r.ApplicationRoleType.Key == (int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant)
                {
                    _view.legalEntity = r.LegalEntity;
                    break;
                }
            }

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnCreateApplicationButtonClicked += new EventHandler(_view_OnCreateApplicationButtonClicked);
        }

        void _view_OnBackButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Back");
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

            if (!_view.IsValid)
                _view.CreateApplicationReadyOnly = false;

            _view.ShowBackButton = true;
            _view.CreateApplicationButtonText = "Next";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnCreateApplicationButtonClicked(object sender, EventArgs e)
        {


            if (_application.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.Unknown)
                {
                    UpdateApplication(); //persist
                }
                else
                {
                    //This must be called to update any changes to the calculator
                    UpdateApplication(); //persist
                    //This probably does not need to be called at all, does not do the required work
                    UpdateExistingApplication();
                }

                _application = _appRepo.GetApplicationByKey(_application.Key);
                _view.MortgageLoanApplication = _application;

                IList<ICacheObjectLifeTime> lifeTimes = new List<ICacheObjectLifeTime>();

                if (_view.Messages.WarningMessages.Count > 0)
                {                    
                    return;
                }

                TransactionScope ts = new TransactionScope();
                
                try
                {
                    

                    if (GlobalCacheData.ContainsKey(ViewConstants.Application))
                        GlobalCacheData.Remove(ViewConstants.Application);

                    GlobalCacheData.Add(ViewConstants.Application, (IApplication)_application, lifeTimes);


                    if (!GlobalCacheData.ContainsKey("CaseCreated"))
                    {
                        int eaID = X2Repo.GetLatestExternalActivityIDFromWorkFlow("Application Capture", "EXTCreateApplication");


                        IActiveExternalActivity a = X2Repo.GetEmptyActiveExternalActivity();

                        a.ActivatingInstanceID = long.Parse(GlobalCacheData[ViewConstants.InstanceID].ToString());
                        a.ExternalActivity = X2Repo.GetExternalActivityByKey(eaID);
                        a.WorkFlowID = X2Repo.GetWorkFlowByName("Application Capture", "Origination").ID;
                        a.ActivationTime = DateTime.Now;
                        X2Repo.SaveActiveExternalActivity(a);
                        GlobalCacheData.Add("CaseCreated", true, lifeTimes);
                    }

                    ts.VoteCommit();

                    if (_navigateNext)
                    {
                        //GlobalCacheData.Remove("MUSTNAVIGATE");
                        Navigator.Navigate("Next");
                    }
                    else
                    {
                        Navigator.Navigate("Cancel");
                    }

                }
                catch
                {
                    ts.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    ts.Dispose();
                }
        }
    }
}
