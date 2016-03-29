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
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.UI;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;


namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    public class LegalEntityDetailsApplicantsAdd : LegalEntityDetailsAddBase
    {
        private IApplication _application;
        private IApplicationRepository _applicationRepository;
        private int _applicationKey;
        private CBOMenuNode cboMenuNode;

        public LegalEntityDetailsApplicantsAdd(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            cboMenuNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _applicationKey = cboMenuNode.GenericKey;

            // Get the Application from the repository
            _application = _applicationRepository.GetApplicationByKey(_applicationKey);

            _view.IncomeContributorVisible = true;
            _view.SelectedIncomeContributor = true;

            IDictionary<string, string> RoleTypes = _applicationRepository.GetApplicantRoleTypesForApplication(_application);

            _view.BindRoleTypes(RoleTypes, String.Empty);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            _view.AddRoleTypeVisible = true;

            _view.CancelButtonVisible = true;
            _view.SubmitButtonText = "Save";
        }

        protected override void SubmitButtonClicked(object sender, EventArgs e)
        {
            SaveLegalEntity();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected override void SaveLegalEntity()
        {
            // Create a blank LE populate it and save it
            base.LegalEntity = LegalEntityRepository.GetEmptyLegalEntity((LegalEntityTypes)View.SelectedLegalEntityType);
            base.LegalEntity.IntroductionDate = DateTime.Now;

            // Get the details from the screen
            _view.PopulateLegalEntityDetailsForAdd(base.LegalEntity);
            // Populate the marketing options ...
            PopulateMarketingOptions();

            TransactionScope txn = new TransactionScope();

            try
            {
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
                        IApplicationRoleAttribute applicationRoleAttribute = _applicationRepository.GetEmptyApplicationRoleAttribute();
                        applicationRoleAttribute.OfferRole = applicationRole;
                        applicationRoleAttribute.OfferRoleAttributeType = _applicationRepository.GetApplicationRoleAttributeTypeByKey((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor);
                        applicationRole.ApplicationRoleAttributes.Add(_view.Messages, applicationRoleAttribute);
                    }

                    // Need To Update The Refresh The Application and Update
                    _application.SetApplicantType();

                    _applicationRepository.SaveApplication(_application);

                    // if there are any errors/warnings then throw exception
                    if (!_view.IsValid)
                        throw new Exception();

                    // we need to update the subject on the instance record.
                    IX2Repository _x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
                    _x2Repo.UpdateInstanceSubject(_application.Key, _application.GetLegalName(LegalNameFormat.Full));
                }

                txn.VoteCommit();

                //add the node here
                CBOManager.RefreshInstanceNode(_view.CurrentPrincipal);

                Navigator.Navigate("Submit");
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                //If the DB rolls the transaction back for any validation the 
                //dispose method will fail because there is no open connection
                //eat the error in the application if the view is valid
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

        protected override void ReBindLegalEntity(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            base.ReBindLegalEntity(sender, e);

            if (GlobalCacheData.ContainsKey(ViewConstants.CreateApplication))
                GlobalCacheData.Remove(ViewConstants.CreateApplication);

            GlobalCacheData.Add(ViewConstants.CreateApplication, _application, new List<ICacheObjectLifeTime>());

            Navigator.Navigate("Rebind");
        }
    }
}
