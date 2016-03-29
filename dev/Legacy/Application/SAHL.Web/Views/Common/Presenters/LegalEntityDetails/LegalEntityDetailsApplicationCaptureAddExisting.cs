using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using System.Collections.Generic;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.Service;
using SAHL.Common.UI;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    public class LegalEntityDetailsApplicationCaptureAddExisting : LegalEntityDetailsUpdateBase
    {
        /// <summary>
        /// Presenter constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityDetailsApplicationCaptureAddExisting(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            LoadLegalEntityFromGlobalCache();

            // bind the Lead Main applicant role type only
            IDictionary<string, string> RoleTypes = new Dictionary<string, string>();
            string roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant);
            RoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.LeadMainApplicant]));
            _view.BindRoleTypes(RoleTypes, String.Empty);

            // display income contributor checkbox and set to true
            _view.IncomeContributorVisible = true;
            _view.SelectedIncomeContributor = true; 

            BindLegalEntity();

            _view.OnReBindLegalEntity += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnReBindLegalEntity);

        }

        void _view_OnReBindLegalEntity(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            LegalEntity = LegalEntityRepository.GetLegalEntityByKey(Convert.ToInt32(e.Key));

            // Persist the LegalEntityKey in the Global cache (and call the next presenter)
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);

            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey,e.Key, new List<ICacheObjectLifeTime>());

            Navigator.Navigate("Rebind");
        }    

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return; 

            _view.UpdateRoleTypeVisible = true;

            _view.SubmitButtonText = "Next";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected override void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            long instanceID = 0;

            try
            {
                // Get the details from the screen
                _view.PopulateLegalEntityDetailsForUpdate(base.LegalEntity);
                // Populate the marketing options ...
                PopulateMarketingOptions();

                // if we are dealing with lead applicants  - only validate fields applicable to lead applicants/suretors.
                switch (base.SelectedRoleTypeKey)
                {
                    case (int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant:
                    case (int)SAHL.Common.Globals.OfferRoleTypes.LeadSuretor:
                        this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);
                        break;
                    default:
                        break;
                }

                // Save the legal entity 
                base.LegalEntityRepository.SaveLegalEntity(base.LegalEntity, false);

                // this is a complete hack - we need to run the rule BEFORE adding the role because of the cached 
                // object - if the rule fails during the Add we already have an assigned key and it all falls to 
                // pieces - this needs to be fixed!
                IRuleService ruleService = ServiceFactory.GetService<IRuleService>();
                ruleService.ExecuteRule(_view.Messages, "LegalEntityOriginationSource", base.LegalEntity);
                if (!_view.IsValid)
                    throw new Exception();

                IApplication application = null;

                if (GlobalCacheData.ContainsKey(ViewConstants.CreateApplication))
                    application = GlobalCacheData[ViewConstants.CreateApplication] as IApplication;

                if (application != null)
                {
                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    appRepo.SaveApplication(application);

                    IApplicationRole applicationRole = application.AddRole(base.SelectedRoleTypeKey, base.LegalEntity);

                    // add the 'income contributor' application role attribute
                    if (_view.SelectedIncomeContributor)
                    {
                        IApplicationRoleAttribute applicationRoleAttribute = appRepo.GetEmptyApplicationRoleAttribute();
                        applicationRoleAttribute.OfferRole = applicationRole;
                        applicationRoleAttribute.OfferRoleAttributeType = appRepo.GetApplicationRoleAttributeTypeByKey((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor);
                        applicationRole.ApplicationRoleAttributes.Add(_view.Messages, applicationRoleAttribute);
                    }

                    // call save on the application again as the role has been added - must be done here so we 
                    // have an application key
                    appRepo.SaveApplication(application);
                }

                // if there are any errors/warnings then throw exception
                if (!_view.IsValid)
                    throw new Exception();

                // once we have an application create a workflow case
                Dictionary<string, string> Inputs = new Dictionary<string, string>();
                Inputs.Add("ApplicationKey", application.Key.ToString());
                if (GlobalCacheData.ContainsKey(ViewConstants.EstateAgentApplication))
                    Inputs.Add("isEstateAgentApplication", Convert.ToString(GlobalCacheData[ViewConstants.EstateAgentApplication]));

                IX2Info XI = X2Service.GetX2Info(_view.CurrentPrincipal);
                if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                    X2Service.LogIn(_view.CurrentPrincipal);

                X2Service.CreateWorkFlowInstance(_view.CurrentPrincipal, SAHL.Common.Constants.WorkFlowProcessName.Origination, (-1).ToString(), SAHL.Common.Constants.WorkFlowName.ApplicationCapture, SAHL.Common.Constants.WorkFlowActivityName.ApplicationCreate, Inputs, false);
                if (!_view.IsValid)
                {
                    X2Service.CancelActivity(_view.CurrentPrincipal);
                    throw new Exception();
                }
                X2Service.CreateCompleteActivity(_view.CurrentPrincipal, Inputs, false, null);
                //get the instanceID
                instanceID = XI.InstanceID;

                // cleare cache
                if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                    GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
                if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationRoleKey))
                    GlobalCacheData.Remove(ViewConstants.ApplicationRoleKey);

                txn.VoteCommit();

                if (instanceID > 0)
                {
                    // add the instanceID to the global cache for our redirect view to use
                    GlobalCacheData.Remove(ViewConstants.InstanceID);
                    GlobalCacheData.Add(ViewConstants.InstanceID, instanceID, new List<ICacheObjectLifeTime>());

                    // navigate to the workflow redirect view
                    Navigator.Navigate("X2InstanceRedirect");
                }
                else
                {
                    Navigator.Navigate("Submit");
                }
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                //db can rollback txn when rules fail, need to not throw ex 
                //if view is valid
                try
                {
                    txn.Dispose();
                }
                catch (Exception)
                {
                    if (_view.IsValid)
                        throw;
                }           
            }
        }

    }
}
