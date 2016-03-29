using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.CacheData;
using NHibernate;


namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    public class LegalEntityDetailsUpdateApplicant : LegalEntityDetailsUpdateBase
    {
        private IApplicationRole _applicationRole;
        private IApplicationRoleAttribute _applicationRoleAttribute;
        private bool _origionalIncomeContributorValue;

        public LegalEntityDetailsUpdateApplicant(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            // if we have got here via a menu click then clear out the cache
            if (_view.IsMenuPostBack)
                base.ClearGlobalCache();

            if (!_view.ShouldRunPage) 
                return;
            
            _view.OnReBindLegalEntity += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnReBindLegalEntity);

            // look for legalentity in global cache - if its found then this is adding existing legalentity
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
            {
                int legalEntityKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedLegalEntityKey]);
                base.LegalEntity = base.LegalEntityRepository.GetLegalEntityByKey(legalEntityKey);

                // get applicationrolekey from global cache
                if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationRoleKey))
                {
                    int applicationRoleKey = Convert.ToInt32(GlobalCacheData[ViewConstants.ApplicationRoleKey]);
                    _applicationRole = base.ApplicationRepository.GetApplicationRoleByKey(applicationRoleKey);
                    _view.ApplicantRoleTypeKey = _applicationRole.ApplicationRoleType.Key;
                    base.Application = _applicationRole.Application;
                }

                _view.SelectedIncomeContributor = true; // set to true by default

            }
            else // otherwise we are updating current legal entity from cbo
            {
                base.LoadLegalEntityFromCBO();

                // get the application role
                if (base.CBOMenuNode.ParentNode.GenericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.Offer)
                {
                    base.Application = base.ApplicationRepository.GetApplicationByKey(base.CBOMenuNode.ParentNode.GenericKey);
                    if (base.Application != null)
                    {
                        // get the role of the legalentity
                        foreach (IApplicationRole role in base.Application.ApplicationRoles)
                        {
                            if (role.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)SAHL.Common.Globals.OfferRoleTypeGroups.Client
                            && role.LegalEntity.Key == base.LegalEntity.Key)
                            {
                                _applicationRole = role;
                                _view.ApplicantRoleTypeKey = role.ApplicationRoleType.Key;

                                // get income contributor offer role attribute
                                if (_applicationRole != null)
                                {
                                    foreach (IApplicationRoleAttribute aroleAttribute in _applicationRole.ApplicationRoleAttributes)
                                    {
                                        if (aroleAttribute.OfferRoleAttributeType.Key == (int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor)
                                        {
                                            _applicationRoleAttribute = aroleAttribute;
                                            _view.SelectedIncomeContributor = true;
                                            _origionalIncomeContributorValue = true;
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

            _view.IncomeContributorVisible = true;

            // disable the ajax functionality so that the users cannot use the idnumber ajax to "pull in" another legal entities information
            //_view.DisableAjaxFunctionality = true;

            // Bind the lookups
            base.BindLookups();
            // Bind the legalentity
            base.BindLegalEntity();
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected override void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();
			bool recalculateApplication = false;
            try
            {
                // do this check below so we can pick up if the user has changed the legalentity type
                base.ReloadLegalEntityIfTypeChanged();

                // Get the details from the screen
                _view.PopulateLegalEntityDetailsForUpdate(base.LegalEntity);
                // Populate the marketing options ...
                PopulateMarketingOptions();

                // set the role type 
                bool roleFound = false;
                foreach (IApplicationRole applicationRole in base.LegalEntity.GetApplicationRolesByRoleTypeGroups(OfferRoleTypeGroups.Client))
                {
                    if (applicationRole.Application.Key == base.Application.Key)
                    {
                        applicationRole.ApplicationRoleType = base.ApplicationRepository.GetApplicationRoleTypeByKey(base.SelectedRoleTypeKey);
                        roleFound = true;

                        // Add/Remove the 'Income Contributor' offer role attribute
                        if (_view.SelectedIncomeContributor == true && _origionalIncomeContributorValue == false)
                        {
                            // add a new offer role attribute
                            IApplicationRoleAttribute applicationRoleAttribute = base.ApplicationRepository.GetEmptyApplicationRoleAttribute();
                            applicationRoleAttribute.OfferRole = _applicationRole;
                            applicationRoleAttribute.OfferRoleAttributeType = base.ApplicationRepository.GetApplicationRoleAttributeTypeByKey((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor);
                            applicationRole.ApplicationRoleAttributes.Add(_view.Messages, applicationRoleAttribute);
							recalculateApplication = true;
                        }
                        else if (_view.SelectedIncomeContributor == false && _origionalIncomeContributorValue == true)
                        {
                            // remove offer role attribute
                            applicationRole.ApplicationRoleAttributes.Remove(_view.Messages, _applicationRoleAttribute);
							recalculateApplication = true;
                        }

                        break;
                    }
                }

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
                // if no application role found then add new role
                if (roleFound == false)
                {
                    IApplicationRole applicationRole = base.ApplicationRepository.GetEmptyApplicationRole();

                    applicationRole.Application = base.Application;
                    applicationRole.ApplicationRoleType = base.ApplicationRepository.GetApplicationRoleTypeByKey(base.SelectedRoleTypeKey);
                    applicationRole.GeneralStatus = base.LookupRepository.GeneralStatuses[GeneralStatuses.Active];
                    applicationRole.LegalEntity = base.LegalEntity;
                    applicationRole.StatusChangeDate = DateTime.Now;

                    // add the 'income contributor' application role attribute
                    if (_view.SelectedIncomeContributor)
                    {
                        IApplicationRoleAttribute applicationRoleAttribute = base.ApplicationRepository.GetEmptyApplicationRoleAttribute();
                        applicationRoleAttribute.OfferRole = applicationRole;
                        applicationRoleAttribute.OfferRoleAttributeType = base.ApplicationRepository.GetApplicationRoleAttributeTypeByKey((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor);
                        applicationRole.ApplicationRoleAttributes.Add(_view.Messages, applicationRoleAttribute);
                    }

                    base.LegalEntity.ApplicationRoles.Add(_view.Messages, applicationRole);

                    ApplicationRepository.SaveApplication(base.Application);
                }
                
                // Save the legal entity and role
				LegalEntityRepository.SaveLegalEntity(base.LegalEntity, recalculateApplication);

                // Need To Update The Refresh The Application and Update
                base.Application = base.ApplicationRepository.GetApplicationByKey(base.Application.Key);
                base.Application.SetApplicantType();
                ApplicationRepository.SaveApplication(base.Application);

                base.Application.CalculateApplicationDetail(false, false);

                if (!_view.IsValid)
                    throw new Exception();

                // we need to update the subject on the instance record.
                IX2Repository _x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
                _x2Repo.UpdateInstanceSubject(base.Application.Key, base.Application.GetLegalName(LegalNameFormat.Full));

                ts.VoteCommit();

                //add the node here
                CBOManager.RefreshInstanceNode(_view.CurrentPrincipal);

                // The base will attempt to navigate, so save first
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