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
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanCreateLeadFromLegalEntityExisting : SAHL.Web.Views.Common.Presenters.LegalEntityDetails.LegalEntityDetailsUpdateBase
    {
        public PersonalLoanCreateLeadFromLegalEntityExisting(ILegalEntityDetails view, SAHLCommonBaseController controller)
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

            BindLegalEntity();

            _view.OnReBindLegalEntity += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnReBindLegalEntity);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return; 

            _view.UpdateRoleTypeVisible = true;

            _view.SubmitButtonText = "Create Lead";
        }

        void _view_OnReBindLegalEntity(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            // Persist the LegalEntityKey in the Global cache (and call the next presenter)
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);

            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, e.Key, new List<ICacheObjectLifeTime>());

            Navigator.Navigate("Rebind");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected override void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            IApplicationUnsecuredLending applicationPersonalLoan = null;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    // Get the details from the screen
                    _view.PopulateLegalEntityDetailsForUpdate(base.LegalEntity);
                    
                    // Populate the marketing options ...
                    PopulateMarketingOptions();

                    this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);

                    LegalEntityRepository.SaveLegalEntity(base.LegalEntity, false);

                    // run rules
                    var ruleService = ServiceFactory.GetService<IRuleService>();
                    ruleService.ExecuteRule(_view.Messages, "CheckUniquePersonalLoanApplication", base.LegalEntity.Key);
                    ruleService.ExecuteRule(_view.Messages, "LegalEntityUnderDebtCounselling", base.LegalEntity);
                    ruleService.ExecuteRule(_view.Messages, "CheckIfCapitecClient", base.LegalEntity);

                    if (!_view.IsValid)
                        return;

                    // create the personal loan offer
                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                    applicationPersonalLoan = appRepo.CreateUnsecuredLendingLead();

                    appRepo.SaveApplication(applicationPersonalLoan);

                    base.LegalEntityRepository.InsertExternalRole(ExternalRoleTypes.Client, applicationPersonalLoan.Key, GenericKeyTypes.Offer, base.LegalEntity.Key, false);

                    if (_view.IsValid)
                    {
                        long instanceID = CreateWorkflowCase(applicationPersonalLoan.Key);

                        transactionScope.VoteCommit();

                        if (instanceID > 0)
                        {
                            // add the instanceID to the global cache for our redirect view to use
                            GlobalCacheData.Remove(ViewConstants.InstanceID);
                            GlobalCacheData.Add(ViewConstants.InstanceID, instanceID, new List<ICacheObjectLifeTime>());

                            // navigate to the workflow redirect view
                            Navigator.Navigate("Worklist");
                        }
                        else
                        {
                            Navigator.Navigate("Submit");
                        }
                    }
                }
                catch (Exception)
                {
                    transactionScope.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
            }

        }

        /// <summary>
        /// Create a workflow case on lead create
        /// </summary>
        ///<param name="applicationKey"></param>
        private long CreateWorkflowCase(int applicationKey)
        {
            bool created = false;

            long instanceID = 0;

            try
            {
                // once we have an application create a workflow case
                Dictionary<string, string> Inputs = new Dictionary<string, string>();
                Inputs.Add("ApplicationKey", applicationKey.ToString());

                IX2Info XI = X2Service.GetX2Info(_view.CurrentPrincipal);
                if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                    X2Service.LogIn(_view.CurrentPrincipal);

                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                X2Service.CreateWorkFlowInstance(_view.CurrentPrincipal,
                                                    SAHL.Common.Constants.WorkFlowProcessName.PersonalLoan,
                                                    (-1).ToString(),
                                                    SAHL.Common.Constants.WorkFlowName.PersonalLoans,
                                                    SAHL.Common.Constants.WorkFlowActivityName.CreatePersonalLoanLead,
                                                    Inputs,
                                                    spc.IgnoreWarnings);

                created = true;
                X2Service.CreateCompleteActivity(_view.CurrentPrincipal, Inputs, spc.IgnoreWarnings, null);

                //get the instanceID
                instanceID = XI.InstanceID;

            }
            catch (Exception)
            {
                if (created)
                    X2Service.CancelActivity(_view.CurrentPrincipal);

                if (_view.IsValid) // if not domain validation exc
                    throw;
            }

            return instanceID;
        }
    }
}
