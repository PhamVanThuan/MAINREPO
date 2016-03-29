using System;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service;
using SAHL.Common.X2.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.DomainMessages;


namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class LeadReassignLife : SAHLCommonBasePresenter<SAHL.Web.Views.Life.Interfaces.ILeadReassign>
    {
        private ILifeRepository _lifeRepo;
        private IApplicationRepository _applicationRepo;
        private ILookupRepository _lookupRepo;
        private IOrganisationStructureRepository _osRepo;
        private IX2Repository _x2Repo;

        private IList<IADUser> _lstConsultantsSearch;
        private IList<IADUser> _lstConsultantsReassign;

        private IApplicationSearchCriteria _searchCriteria = new ApplicationSearchCriteria();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LeadReassignLife(SAHL.Web.Views.Life.Interfaces.ILeadReassign view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }


        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
            _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            _osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            _x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

            // initialize events
            _view.OnSearchButtonClicked += new EventHandler(OnSearchButtonClicked);
            _view.OnReassignButtonClicked += new EventHandler(OnReassignButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
            _view.OnConfirmationButtonClicked += new EventHandler(OnConfirmationButtonClicked);

            // Check if the user is an Admin user
            _view.AdminUser = _lifeRepo.IsAdminUser(_view.CurrentPrincipal);

            // bind applicationstatuses
            _view.BindApplicationStatuses(_lookupRepo.ApplicationStatuses);

            // setup page headings
            _view.PageHeading = "Search for Life Applications and reassign them to the selected Consultant.";
            _view.AccountHeading = "Policy Number";
            _view.DisplayParentAccount = true;
            _view.ParentAccountHeading = "Loan Number";

            // get the life consultants including INACTIVE consultants
            // if the user is an admin user then all consultants are available for search & reassigning
            // non admin users can only search for their own leads but reassign to anyone
            _lstConsultantsSearch = _view.AdminUser ? _lifeRepo.GetLifeConsultants(true) : _lifeRepo.GetLifeConsultants(_view.CurrentPrincipal.Identity.Name);

            // get the ACTIVE life consultants
            _lstConsultantsReassign = _lifeRepo.GetLifeConsultants();

            // bind the consultants
            _view.BindConsultants(_lstConsultantsSearch, _lstConsultantsReassign);

            // restore the saved results
            if (PrivateCacheData.ContainsKey(ViewConstants.LifeSearchCriteria))
                _searchCriteria = PrivateCacheData[ViewConstants.LifeSearchCriteria] as IApplicationSearchCriteria;
            if (PrivateCacheData.ContainsKey(ViewConstants.LifeSearchResults))
            {
                _view.SearchResults = PrivateCacheData[ViewConstants.LifeSearchResults] as IEventList<SAHL.Common.BusinessModel.Interfaces.IApplication>;
                _view.BindSearchResults();
            }
        }

        void OnSearchButtonClicked(object sender, EventArgs e)
        {
            ValidateSearchCriteria();

            if (_view.IsValid)
            {
                _searchCriteria = new ApplicationSearchCriteria();

                // Set the search criteria
                if (!String.IsNullOrEmpty(_view.SearchAccountKey))
                    _searchCriteria.AccountKey = Convert.ToInt32(_view.SearchAccountKey.Trim());
                if (!String.IsNullOrEmpty(_view.SearchClientName))
                    _searchCriteria.ClientName = _view.SearchClientName.Trim();
                if (!String.IsNullOrEmpty(_view.SearchConsultant))
                    _searchCriteria.ConsultantADUserName = _view.SearchConsultant.Trim();

                _searchCriteria.ApplicationStatuses.Add(_view.SearchApplicationStatus);

                // set life specific search criteria
                _searchCriteria.ApplicationTypes.Add(OfferTypes.Life);
                _searchCriteria.WorkflowsAndProcesses.Add(SAHL.Common.Constants.WorkFlowName.LifeOrigination, SAHL.Common.Constants.WorkFlowProcessName.LifeOrigination);
                _searchCriteria.ApplicationHasAccount = true;

                // do the search
                PerformSearch();
            }
        }

        private void PerformSearch()
        {
            // perform the search
            _view.SearchResults = _applicationRepo.SearchApplications(_searchCriteria, _view.MaxSearchResults + 1, true);

            PrivateCacheData.Add(ViewConstants.LifeSearchCriteria, _searchCriteria);
            PrivateCacheData.Add(ViewConstants.LifeSearchResults, _view.SearchResults);

            // bind the search results grid
            _view.BindSearchResults();
        }

        void OnReassignButtonClicked(object sender, EventArgs e)
        {
            ValidateSelection();

            if (_view.IsValid)
            {
                // get the legalentity for the selected consultant
                ILegalEntity selectedLegalEntity = _osRepo.GetAdUserForAdUserName(_view.SelectedReassignADUserName).LegalEntity;
                if (selectedLegalEntity != null)
                {
                    foreach (int applicationKey in _view.SelectedApplicationKeys)
                    {
                        // get the application
                        IApplicationLife selectedApplicationLife = _applicationRepo.GetApplicationByKey(applicationKey) as IApplicationLife;

                        // get the instance for each of the applications
                        IInstance instance = _x2Repo.GetInstanceForGenericKey(selectedApplicationLife.Key, SAHL.Common.Constants.WorkFlowName.LifeOrigination, SAHL.Common.Constants.WorkFlowProcessName.LifeOrigination);

                        if (instance != null)
                        {
                            TransactionScope txn = new TransactionScope();

                            try
                            {
                                // exclude relevant rules
                                this.ExclusionSets.Add(RuleExclusionSets.LegalEntityOperators);

                                // insert/update record into offerrole for new consultant.
                                // this method will either create a new OfferRole record or reuse and existing on if it exists already for
                                // the selected LegalEntity. It will also set the status on all the other Consultant OfferRoles to Inactive.
                                IApplicationRole newAppRole = _osRepo.GenerateApplicationRole((int)SAHL.Common.Globals.OfferRoleTypes.Consultant,applicationKey,selectedLegalEntity.Key,true);

                                // set the consultant adusername on the offerlife record
                                selectedApplicationLife.ConsultantADUserName = _view.SelectedReassignADUserName;

                                // save the application with the new consultant
                                _applicationRepo.SaveApplication(selectedApplicationLife);

                                // write the stage transition  record
                                IStageDefinitionRepository sdRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                                ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                                string comments = "Existing lead re-assigned to " + _view.SelectedReassignADUserName;
                                sdRepo.SaveStageTransition(selectedApplicationLife.Key, (int)SAHL.Common.Globals.StageDefinitionGroups.LifeOrigination, SAHL.Common.Constants.StageDefinitionConstants.AssignApplication, comments, secRepo.GetADUserByPrincipal(_view.CurrentPrincipal));

                                // Create X2.WorkflowAssignment record 
                                _osRepo.CreateWorkflowAssignment(newAppRole, (int)instance.ID, GeneralStatuses.Active);

                                // RefreshWorkListAndSecurity - this will reassign the instance based on the dynamic role created above
                                X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
                                IX2Info XI = svc.GetX2Info(_view.CurrentPrincipal);
                                if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                                    svc.LogIn(_view.CurrentPrincipal);
                                bool succes = svc.RefreshWorkListAndSecurity(instance.ID);

                                if (succes)
                                    txn.VoteCommit();
                                else
                                    txn.VoteRollBack();
                            }
                            catch (Exception)
                            {
                                txn.VoteRollBack();
                                if (_view.IsValid)
                                    throw;
                            }
                            finally
                            {
                                txn.Dispose();
                            }
                        }
                    }

                    // refresh results
                    if (PrivateCacheData.ContainsKey(ViewConstants.LifeSearchCriteria))
                    {
                        _searchCriteria = PrivateCacheData[ViewConstants.LifeSearchCriteria] as IApplicationSearchCriteria;
                        PerformSearch();

                        _view.DisplayConfirmationPanel = true;
                    }
                }
            }
        }

        void OnCancelButtonClicked(object sender, EventArgs e)
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.LifeSearchResults))
                GlobalCacheData.Remove(ViewConstants.LifeSearchResults);
            if (GlobalCacheData.ContainsKey(ViewConstants.LifeSearchCriteria))
                GlobalCacheData.Remove(ViewConstants.LifeSearchCriteria);

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
            {
                string navigateTo = GlobalCacheData[ViewConstants.NavigateTo].ToString();
                GlobalCacheData.Remove(ViewConstants.NavigateTo);
                _view.Navigator.Navigate(navigateTo);
            }
            else
                _view.Navigator.Navigate("Cancel");
        }

        void OnConfirmationButtonClicked(object sender, EventArgs e)
        {
            _view.DisplayConfirmationPanel = false;
        }

        void ValidateSearchCriteria()
        {
            if (String.IsNullOrEmpty(_view.SearchAccountKey) && String.IsNullOrEmpty(_view.SearchConsultant) && String.IsNullOrEmpty(_view.SearchClientName))
            {
                _view.Messages.Add(new Error("Search Criteria must be entered.", "Search Criteria must be entered."));
            }
        }

        void ValidateSelection()
        {
            if (_view.SelectedApplicationKeys.Count <= 0)
                _view.Messages.Add(new Error("Must select at least one Application.", "Must select at least one Application."));
            if (String.IsNullOrEmpty(_view.SelectedReassignADUserName))
                _view.Messages.Add(new Error("Must select a Consultant.", "Must select a Consultant."));
        }

    }
}
