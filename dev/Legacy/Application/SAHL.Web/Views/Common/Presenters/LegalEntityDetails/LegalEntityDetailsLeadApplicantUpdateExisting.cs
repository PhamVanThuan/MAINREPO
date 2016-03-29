using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Helpers;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    /// <summary>
    /// Called when an existing legal entity is added. (usually via AJAX).
    /// </summary>
    public class LegalEntityDetailsLeadApplicantUpdateExisting : LegalEntityDetailsUpdateBase
    {
        private IApplicationRepository _applicationRepo;
        private IX2Repository _x2Repo;
        private IApplicationRole _applicationRole;
        // private IApplicationRoleAttribute _applicationRoleAttribute;
        private int _applicationRoleKey;

        public LegalEntityDetailsLeadApplicantUpdateExisting(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            // get legalentity from global cache
            base.LoadLegalEntityFromGlobalCache();

            // get applicationrolekey from global cache
            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationRoleKey))
            {
                _applicationRoleKey = Convert.ToInt32(GlobalCacheData[ViewConstants.ApplicationRoleKey]);
                _applicationRole = _applicationRepo.GetApplicationRoleByKey(_applicationRoleKey);

                //foreach (IApplicationRoleAttribute aroleAttribute in _applicationRole.ApplicationRoleAttributes)
                //{
                //    if (aroleAttribute.OfferRoleAttributeType.Key == (int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor)
                //    {
                //        _applicationRoleAttribute = aroleAttribute;
                //        break;
                //    }
                //}
            }

            _view.IncomeContributorVisible = true;
            _view.SelectedIncomeContributor = true; // set to true by default

            BindLegalEntity();

            // bind the Lead Main applicant role type only
            IDictionary<string, string> RoleTypes = new Dictionary<string, string>();
            string roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant);
            RoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.LeadMainApplicant]));
            _view.BindRoleTypes(RoleTypes, String.Empty);

            _view.OnReBindLegalEntity += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnReBindLegalEntity);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.UpdateRoleTypeVisible = true;
        }

        void _view_OnReBindLegalEntity(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            base.LegalEntity = _legalEntityRepository.GetLegalEntityByKey(Convert.ToInt32(e.Key));

            // Persist the objects in the Global cache (and call the next presenter)
            base.ClearGlobalCache();
            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, e.Key, new List<ICacheObjectLifeTime>());
            GlobalCacheData.Add(ViewConstants.ApplicationRoleKey, _applicationRole.Key, new List<ICacheObjectLifeTime>());

            Navigator.Navigate("Rebind");
        }     

        protected override void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // Get the details from the screen
            _view.PopulateLegalEntityDetailsForUpdate(base.LegalEntity);
            // Populate the marketing options ...
            PopulateMarketingOptions();

            TransactionScope ts =  new TransactionScope(TransactionMode.Inherits);

            try
            {
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
                    applicationRoleAttribute.OfferRoleAttributeType = LookupRepository.ApplicationRoleAttributesTypes.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor)];
                    applicationRole.ApplicationRoleAttributes.Add(_view.Messages, applicationRoleAttribute);
                }

                // save the application
                _applicationRepo.SaveApplication(_applicationRole.Application);

                if (!_view.IsValid)
                    throw new Exception();

                // we need to update the subject on the instance record.
                _x2Repo.UpdateInstanceSubject(_applicationRole.Application.Key, _applicationRole.Application.GetLegalName(LegalNameFormat.Full));

                ts.VoteCommit();

                // The base will do the navigate
                base.OnSubmitButtonClicked(sender, e);
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
