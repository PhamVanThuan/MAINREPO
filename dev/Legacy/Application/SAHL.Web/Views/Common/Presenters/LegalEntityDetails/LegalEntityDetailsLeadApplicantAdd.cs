using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using System.Collections.Generic;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    public class LegalEntityDetailsLeadApplicantAdd : LegalEntityDetailsAddBase
    {
        public LegalEntityDetailsLeadApplicantAdd(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            // bind the Lead Main applicant role type only
            IDictionary<string, string> RoleTypes = new Dictionary<string, string>();
            string roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant);
            RoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.LeadMainApplicant]));
            _view.BindRoleTypes(RoleTypes, String.Empty);

            // display income contributor checkbox and set to true
            _view.IncomeContributorVisible = true;
            _view.SelectedIncomeContributor = true;
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return; 

            _view.AddRoleTypeVisible = true;

            _view.SubmitButtonText = "Create Lead";
            _view.CancelButtonVisible = false;
        }

        protected override void ReBindLegalEntity(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            base.ReBindLegalEntity(sender, e);

            Navigator.Navigate("Rebind"); 
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected override void SubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();
            long instanceID = 0;

            try
            {
                // Create a blank LE populate it and save it
                base.LegalEntity = base.LegalEntityRepository.GetEmptyLegalEntity((LegalEntityTypes)View.SelectedLegalEntityType);

                // Get the details from the screen
                _view.PopulateLegalEntityDetailsForAdd(base.LegalEntity);
                base.LegalEntity.IntroductionDate = DateTime.Today;

                // Populate the marketing options ...
                PopulateMarketingOptions();

                // only validate fields applicable to lead applicants/suretors.
                this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);

                // Save the legal entity 
                LegalEntityRepository.SaveLegalEntity(base.LegalEntity, false);
        
                // add the application role to the application
                IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
                IApplicationUnknown applicationUnknown = AR.GetEmptyUnknownApplicationType(OriginationSourceHelper.PrimaryOriginationSourceKey(_view.CurrentPrincipal));
                IApplicationRole applicationRole = applicationUnknown.AddRole(base.SelectedRoleTypeKey, base.LegalEntity);

                // add the 'income contributor' application role attribute
                if (_view.SelectedIncomeContributor)
                {
                    IApplicationRoleAttribute applicationRoleAttribute = AR.GetEmptyApplicationRoleAttribute();
                    applicationRoleAttribute.OfferRole = applicationRole;
                    applicationRoleAttribute.OfferRoleAttributeType = AR.GetApplicationRoleAttributeTypeByKey((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor);
                    applicationRole.ApplicationRoleAttributes.Add(_view.Messages, applicationRoleAttribute);
                }

                // save the application
                AR.SaveApplication(applicationUnknown);

                if (!_view.IsValid)
                    throw new Exception();

                // once we have an application create a workflow case
                Dictionary<string, string> Inputs = new Dictionary<string, string>();
                Inputs.Add("ApplicationKey", applicationUnknown.Key.ToString());
                IX2Info XI = X2Service.GetX2Info(_view.CurrentPrincipal);
                if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                    X2Service.LogIn(_view.CurrentPrincipal);

                X2Service.CreateWorkFlowInstance(_view.CurrentPrincipal,SAHL.Common.Constants.WorkFlowProcessName.Origination, (-1).ToString(), SAHL.Common.Constants.WorkFlowName.ApplicationCapture, "Create Lead", Inputs, false);
                if (!_view.IsValid)
                {
                    X2Service.CancelActivity(_view.CurrentPrincipal);
                    throw new Exception();
                }

                X2Service.CreateCompleteActivity(_view.CurrentPrincipal, Inputs, false, null);

                //get the instanceID
                instanceID = XI.InstanceID;

                ts.VoteCommit();

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
                    CBOManager.SetCurrentNodeSet(_view.CurrentPrincipal, CBONodeSetType.X2);
                    Navigator.Navigate("Submit");
                }
            }
            catch (Exception)
            {
                ts.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                //db can rollback txn when rules fail, need to not throw ex 
                //if view is valid
                try
                {
                    ts.Dispose();
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
