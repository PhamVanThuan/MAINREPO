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

using SAHL.Common;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    public class LegalEntityDetailsLeadApplicantUpdate : LegalEntityDetailsUpdateBase
    {
        private InstanceNode _instanceNode;
        private long _instanceID;
        private IInstance _instance;
        private IApplicationRole _applicationRole;
        private IApplicationRoleAttribute _applicationRoleAttribute;
        private IX2Repository _x2Repo;
        private bool _origionalIncomeContributorValue;

        public LegalEntityDetailsLeadApplicantUpdate(ILegalEntityDetails view, SAHLCommonBaseController controller)
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

            _x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

            // get the application
            _instanceNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            _instanceID = _instanceNode.InstanceID;
            _instance = _x2Repo.GetInstanceByKey(_instanceID);
            base.Application = base.ApplicationRepository.GetApplicationFromInstance(_instance);

            // get the application role & legalentity
            foreach (IApplicationRole arole in base.Application.ApplicationRoles)
            {
                if (arole.ApplicationRoleType.Key == (int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant)
                {
                    _applicationRole = arole;
                    base.LegalEntity = arole.LegalEntity;

                    // check if the guy is an income contributer
                    foreach (IApplicationRoleAttribute aroleAttribute in arole.ApplicationRoleAttributes)
                    {
                        if (aroleAttribute.OfferRoleAttributeType.Key == (int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor)
                        {
                            _applicationRoleAttribute = aroleAttribute;
                            _view.SelectedIncomeContributor = true;
                            _origionalIncomeContributorValue = true;
                            break;
                        }
                        
                    }
                    break;
                }
            }

            // look for legalentity in global cache - if its found then we are using an existig Le from the AJAX
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
            {
                int legalEntityKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedLegalEntityKey]);
                base.LegalEntity = base.LegalEntityRepository.GetLegalEntityByKey(legalEntityKey);
            }

            _view.IncomeContributorVisible = true;

            // Bind the lookups - dont bind roles
            base.BindLookups(false);

            // Bind the legalentity
            base.BindLegalEntity();

            // bind the Lead Main applicant role type only
            IDictionary<string, string> RoleTypes = new Dictionary<string, string>();
            string roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant);
            RoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.LeadMainApplicant]));
            _view.BindRoleTypes(RoleTypes, String.Empty);
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
            TransactionScope ts =  new TransactionScope(TransactionMode.Inherits);
            
            try
            {
                //// do this check below so we can pick up if the user has changed the legalentity type
                //base.ReloadLegalEntityIfTypeChanged();

                // Get the details from the screen
                _view.PopulateLegalEntityDetailsForUpdate(base.LegalEntity);
                // Populate the marketing options ...
                PopulateMarketingOptions();

                // only validate fields applicable to lead applicants/suretors.
                this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);

                // Save the legal entity 
                base.LegalEntityRepository.SaveLegalEntity(base.LegalEntity, false);

                // look for legalentity in global cache - if its found then we are using an existig Le from the AJAX
                // and we are going to create a new offer role record withthe new guy
                if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                {
                    IApplicationRole applicationRole = base.Application.AddRole(base.SelectedRoleTypeKey, base.LegalEntity);

                    // add the 'income contributor' application role attribute if required
                    if (_view.SelectedIncomeContributor)
                    {
                        IApplicationRoleAttribute applicationRoleAttribute = base.ApplicationRepository.GetEmptyApplicationRoleAttribute();
                        applicationRoleAttribute.OfferRole = applicationRole;
                        applicationRoleAttribute.OfferRoleAttributeType = base.ApplicationRepository.GetApplicationRoleAttributeTypeByKey((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor);
                        applicationRole.ApplicationRoleAttributes.Add(_view.Messages, applicationRoleAttribute);
                    }

                    // save the application with the new role
                    base.ApplicationRepository.SaveApplication(base.Application);
                }
                else 
                {
                    // Add/Remove the 'Income Contributor' offer role attribute
                    if (_view.SelectedIncomeContributor == true && _origionalIncomeContributorValue == false)
                    {
                        // add a new offer role attribute
                        IApplicationRoleAttribute applicationRoleAttribute = base.ApplicationRepository.GetEmptyApplicationRoleAttribute();
                        applicationRoleAttribute.OfferRole = _applicationRole;
                        applicationRoleAttribute.OfferRoleAttributeType = base.ApplicationRepository.GetApplicationRoleAttributeTypeByKey((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor);
                        _applicationRole.ApplicationRoleAttributes.Add(_view.Messages, applicationRoleAttribute);

                        // save the application
                        base.ApplicationRepository.SaveApplication(_applicationRole.Application);
                    }
                    else if (_view.SelectedIncomeContributor == false && _origionalIncomeContributorValue == true)
                    {
                        // remove offer role attribute
                        _applicationRole.ApplicationRoleAttributes.Remove(_view.Messages, _applicationRoleAttribute);

                        // save the application
                        base.ApplicationRepository.SaveApplication(_applicationRole.Application);
                    }
                }

                if (!_view.IsValid)
                    throw new Exception();

                // we need to update the subject on the instance record.
                _x2Repo.UpdateInstanceSubject(base.Application.Key, base.Application.GetLegalName(LegalNameFormat.Full));

                ts.VoteCommit();

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