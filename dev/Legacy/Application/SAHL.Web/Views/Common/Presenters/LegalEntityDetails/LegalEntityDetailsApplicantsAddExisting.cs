using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using Castle.ActiveRecord;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    /// <summary>
    /// Called when an existing legal entity is added. (usually via AJAX).
    /// </summary>
    public class LegalEntityDetailsApplicantsAddExisting : LegalEntityDetailsUpdateBase
    {
        //private int _applicationKey;
        private IApplication _application; 

        public LegalEntityDetailsApplicantsAddExisting(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            LoadLegalEntityFromGlobalCache();

            _application = null;
            if (GlobalCacheData.ContainsKey(ViewConstants.CreateApplication))
            {
                IApplication cachedApplication = GlobalCacheData[ViewConstants.CreateApplication] as IApplication;
                _application = base.ApplicationRepository.GetApplicationByKey(cachedApplication.Key);
            }

            _view.IncomeContributorVisible = true;
            _view.SelectedIncomeContributor = true; // set to true by default

            BindLegalEntity();

            _view.OnReBindLegalEntity += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnReBindLegalEntity);
        }

        void _view_OnReBindLegalEntity(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            // Persist the LegalEntityKey in the Global cache (and call the next presenter)
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);

            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, e.Key, new List<ICacheObjectLifeTime>());

            Navigator.Navigate("Rebind");
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            _view.UpdateRoleTypeVisible = true;

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected override void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();

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

                if (_application != null)
                {
                    IApplicationRole applicationRole = _application.AddRole(base.SelectedRoleTypeKey, base.LegalEntity);

                    // add the 'income contributor' application role attribute
                    if (_view.SelectedIncomeContributor)
                    {
                        IApplicationRoleAttribute applicationRoleAttribute = base.ApplicationRepository.GetEmptyApplicationRoleAttribute();
                        applicationRoleAttribute.OfferRole = applicationRole;
                        applicationRoleAttribute.OfferRoleAttributeType = base.ApplicationRepository.GetApplicationRoleAttributeTypeByKey((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor);
                        applicationRole.ApplicationRoleAttributes.Add(_view.Messages, applicationRoleAttribute);
                    }

                    base.ApplicationRepository.SaveApplication(_application);
                    _application.CalculateApplicationDetail(false, false);

                    if (!_view.IsValid)
                        throw new Exception();

                    // we need to update the subject on the instance record.
                    IX2Repository _x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
                    _x2Repo.UpdateInstanceSubject(_application.Key, _application.GetLegalName(LegalNameFormat.Full));

                }

                txn.VoteCommit();

                //add the node here
                CBOManager.RefreshInstanceNode(_view.CurrentPrincipal);

                // The base will attempt to navigate, so save first
                base.OnSubmitButtonClicked(sender, e);
                
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
