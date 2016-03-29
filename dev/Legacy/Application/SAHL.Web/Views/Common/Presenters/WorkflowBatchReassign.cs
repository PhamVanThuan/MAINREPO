using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class WorkflowBatchReassign : SAHLCommonBasePresenter<IWorkflowBatchReassign>
    {
        private IApplicationRepository _applicationRepo;
        private IOrganisationStructureRepository _osRepo;
        private IX2Repository _x2Repo;
        private AssignmentRoleType _selectedAssignmentRoleType;
        private IEventList<IADUser> _lstAllUsers;
        //private IEventList<IADUser> _lstActiveUsers;
        private IList<IApplicationRoleType> _applicationRoleTypes;
        private IList<IWorkflowRoleType> _workflowRoleTypes;
        private IList<AssignmentRoleType> _assignmentRoleTypes;
        private IList<IOrganisationStructure> _orgStructList;
        private Hashtable _adUserOrgStructHT;
        private IADUser _currentADUser;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public WorkflowBatchReassign(IWorkflowBatchReassign view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            _x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            _view.MaxSearchResults = 1000;
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // initialize events
            _view.OnSearchButtonClicked += new EventHandler(OnSearchButtonClicked);
            _view.OnReassignButtonClicked += new EventHandler(OnReassignButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
            _view.OnRoleTypeSelectedIndexChanged += new KeyChangedEventHandler(OnRoleTypeSelectedIndexChanged);

            _currentADUser = _osRepo.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);

            // get org structures for the logged-in user
            _orgStructList = _osRepo.GetOrgStructsPerADUser(_currentADUser);

            _assignmentRoleTypes = new List<AssignmentRoleType>();

            // get offer role types and add to assignment role types list
            _applicationRoleTypes = _osRepo.GetAppRoleTypesForOrgStructList(_orgStructList);
            foreach (IApplicationRoleType art in _applicationRoleTypes)
            {
                AssignmentRoleType asrt = new AssignmentRoleType();
                asrt.Type = AssignmentType.OfferRoleType;
                asrt.Key = art.Key;
                asrt.Description = art.Description;
                asrt.GroupKey = art.ApplicationRoleTypeGroup.Key;
                _assignmentRoleTypes.Add(asrt);
            }

            // get workflow role types and add to assignment role types list
            _workflowRoleTypes = _osRepo.GetWorkflowRoleTypesForOrgStructList(_orgStructList);
            foreach (IWorkflowRoleType wrt in _workflowRoleTypes)
            {
                AssignmentRoleType asrt = new AssignmentRoleType();
                asrt.Type = AssignmentType.WorkflowRoleType;
                asrt.Key = wrt.Key;
                asrt.Description = wrt.Description;
                asrt.GroupKey = wrt.WorkflowRoleTypeGroup.Key;
                _assignmentRoleTypes.Add(asrt);
            }

            // bind assignment role types
            _view.BindRolesTypes(_assignmentRoleTypes);

            if (PrivateCacheData.ContainsKey(ViewConstants.BatchReassignSelectedRoleType))
            {
                _selectedAssignmentRoleType = (AssignmentRoleType)this.PrivateCacheData[ViewConstants.BatchReassignSelectedRoleType];
                RetrieveADUsers();
            }

            if (PrivateCacheData.ContainsKey(ViewConstants.BatchReassignSelectedSearchUser))
                _view.SelectedSearchADUserName = Convert.ToString(PrivateCacheData[ViewConstants.BatchReassignSelectedSearchUser]);

            if (PrivateCacheData.ContainsKey(ViewConstants.BatchReassignSelectedReassignUser))
                _view.SelectedReassignADUserName = Convert.ToString(PrivateCacheData[ViewConstants.BatchReassignSelectedReassignUser]);

            // bind the users - blank the first time
            _view.BindUsers(_currentADUser.ADUserName, null, null, null, null);

            // restore and saved results
            if (PrivateCacheData.ContainsKey(ViewConstants.ApplicationSearchCriteria))
                _view.SearchCriteria = PrivateCacheData[ViewConstants.ApplicationSearchCriteria] as IWorkflowSearchCriteria;

            if (PrivateCacheData.ContainsKey(ViewConstants.ApplicationSearchResults))
            {
                _view.SearchResults = PrivateCacheData[ViewConstants.ApplicationSearchResults] as IList<IInstance>;
                _view.ApplicationInstanceDict(PrivateCacheData[ViewConstants.ApplicationInstanceDictionary] as Dictionary<long, IApplication>);
                _view.BindSearchResults();
            }
        }

        #region Event Handlers

        private void OnSearchButtonClicked(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void OnReassignButtonClicked(object sender, EventArgs e)
        {
            // Add exclusion set
            this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);

            // get the legalentity for the selected consultant
            ILegalEntity selectedLegalEntity = _osRepo.GetAdUserForAdUserName(_view.SelectedReassignADUserName).LegalEntity;
            // get the legalentity for the ADUser that are cases are being search for
            ILegalEntity SearchLegalEntity = _osRepo.GetAdUserForAdUserName(_view.SelectedSearchADUserName).LegalEntity;

            if (selectedLegalEntity != null)
            {
                PrivateCacheData.Add(ViewConstants.BatchReassignSelectedReassignUser, _view.SelectedReassignADUserName);
                foreach (int instanceID in _view.SelectedInstanceIDs)
                {
                    // get the instance
                    IInstance selectedInstance = _x2Repo.GetInstanceByKey(instanceID);

                    if (selectedInstance != null)
                    {
                        switch (_view.SelectedAssignmentRoleType.Type)
                        {
                            case AssignmentType.OfferRoleType:

                                #region OfferRoleType assignment

                                // get the application for each of the instances
                                IApplication selectedApplication = _applicationRepo.GetApplicationFromInstance(selectedInstance);

                                //  First search with all the specified criteria
                                IApplicationRole applicationRole = null;
                                applicationRole = _osRepo.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyAndLegalEntityKey(selectedApplication.Key, _view.SelectedAssignmentRoleType.Key, SearchLegalEntity.Key, (int)GeneralStatuses.Active);

                                // If the user being searched does not play an Active OfferRole for the given OfferRoleType in the application
                                // then we dont wanna do anything further
                                if (applicationRole != null)
                                {
                                    // Transaction should be dealt on an instance basis and not batch basis,
                                    // as it could cause issues when Rolling Back changes on the X2 side.
                                    TransactionScope txn = new TransactionScope();
                                    try
                                    {
                                        // Set the "applicationRole" found to Inactive
                                        //_osRepo.DeactivateApplicationRole(applicationRole.Key);

                                        IApplicationRole newAppRole = null;

                                        // Assign the "applicationRole" found to the Selected User for the given OfferRoleType and Offer
                                        if (selectedApplication.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.Life)
                                            newAppRole = _osRepo.GenerateApplicationRole((int)SAHL.Common.Globals.OfferRoleTypes.Consultant, selectedApplication.Key, selectedLegalEntity.Key, true);
                                        else
                                            newAppRole = _osRepo.GenerateApplicationRole(applicationRole.ApplicationRoleType.Key, selectedApplication.Key, selectedLegalEntity.Key, true);

                                        // Do X2 Stuff
                                        _osRepo.CreateWorkflowAssignment(newAppRole, (int)instanceID, GeneralStatuses.Active);

                                        // RefreshWorkListAndSecurity - this will reassign the instance based on the dynamic role created above
                                        X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
                                        IX2Info XI = svc.GetX2Info(_view.CurrentPrincipal);
                                        if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                                            svc.LogIn(_view.CurrentPrincipal);

                                        bool succes = svc.RefreshWorkListAndSecurity(instanceID);

                                        if (succes)
                                            txn.VoteCommit();
                                        else
                                        {
                                            txn.VoteRollBack();
                                            string errorMessage = string.Format("WorkflowBatchReassign - RefreshWorkListAndSecurity failed for Application : {0}.", selectedApplication.Key);
                                            _view.Messages.Add(new Error(errorMessage, errorMessage));
                                            RefreshResults();
                                            break;
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
                                        txn.Dispose();
                                    }
                                }

                                #endregion OfferRoleType assignment

                                break;
                            case AssignmentType.WorkflowRoleType:

                                #region WorkflowRoleType assignment

                                // Transaction should be dealt on an instance basis and not batch basis,
                                // as it could cause issues when Rolling Back changes on the X2 side.
                                TransactionScope txn2 = new TransactionScope();
                                try
                                {
                                    X2Data x2Data = _x2Repo.GetX2DataForInstance(selectedInstance);
                                    _x2Repo.AssignWorkflowRoleForADUser(instanceID, _view.SelectedReassignADUserName, _view.SelectedAssignmentRoleType.Key, x2Data.GenericKey, selectedInstance.State.Name);

                                    // RefreshWorkListAndSecurity - this will reassign the instance based on the dynamic role created above
                                    X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
                                    IX2Info XI = svc.GetX2Info(_view.CurrentPrincipal);
                                    if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                                        svc.LogIn(_view.CurrentPrincipal);

                                    bool succes = svc.RefreshWorkListAndSecurity(instanceID);

                                    if (succes)
                                        txn2.VoteCommit();
                                    else
                                    {
                                        txn2.VoteRollBack();
                                        string errorMessage = string.Format("WorkflowBatchReassign - RefreshWorkListAndSecurity failed for Instance : {0}.", selectedInstance.ID);
                                        _view.Messages.Add(new Error(errorMessage, errorMessage));
                                        RefreshResults();
                                        break;
                                    }
                                }
                                catch (Exception)
                                {
                                    txn2.VoteRollBack();
                                    if (_view.IsValid)
                                        throw;
                                }
                                finally
                                {
                                    txn2.Dispose();
                                }

                                #endregion WorkflowRoleType assignment

                                break;
                            default:
                                break;
                        }
                    }
                }

                // refresh results
                RefreshResults();
            }
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationSearchResults))
                GlobalCacheData.Remove(ViewConstants.ApplicationSearchResults);
            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationSearchCriteria))
                GlobalCacheData.Remove(ViewConstants.ApplicationSearchCriteria);

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
            {
                string navigateTo = GlobalCacheData[ViewConstants.NavigateTo].ToString();
                GlobalCacheData.Remove(ViewConstants.NavigateTo);
                _view.Navigator.Navigate(navigateTo);
            }
            else
                _view.Navigator.Navigate("Cancel");
        }

        private void OnRoleTypeSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (e.Key.ToString() != SAHL.Common.Constants.DefaultDropDownItem)
            {
                _selectedAssignmentRoleType = _view.GetAssignmentRoleType(e.Key.ToString());

                if (this.PrivateCacheData.ContainsKey(ViewConstants.BatchReassignSelectedRoleType))
                {
                    AssignmentRoleType cachedAssignmentRoleType = (AssignmentRoleType)this.PrivateCacheData[ViewConstants.BatchReassignSelectedRoleType];

                    if (cachedAssignmentRoleType.Key != _selectedAssignmentRoleType.Key)
                    {
                        _view.SearchResults = new List<IInstance>();
                        _view.BindSearchResults();
                    }
                    this.PrivateCacheData[ViewConstants.BatchReassignSelectedRoleType] = _selectedAssignmentRoleType;
                }
                else
                {
                    this.PrivateCacheData.Add(ViewConstants.BatchReassignSelectedRoleType, _selectedAssignmentRoleType);
                    _view.SearchResults = new List<IInstance>();
                    _view.BindSearchResults();
                }

                RetrieveADUsers();
            }
        }

        #endregion Event Handlers

        #region Helper Methods

        private void PerformSearch()
        {
            if (_view.SearchCriteria != null)
            {
                PrivateCacheData.Remove(ViewConstants.ApplicationSearchCriteria);
                PrivateCacheData.Remove(ViewConstants.ApplicationSearchResults);
                PrivateCacheData.Remove(ViewConstants.BatchReassignUsers);
                PrivateCacheData.Remove(ViewConstants.BatchReassignSelectedSearchUser);
                PrivateCacheData.Remove(ViewConstants.ApplicationInstanceDictionary);

                IList<IInstance> instanceList = new List<IInstance>();

                switch (_view.SelectedAssignmentRoleType.Type)
                {
                    case AssignmentType.OfferRoleType:

                        #region OfferRoleType Search

                        instanceList = _x2Repo.GetInstancesOnWorkListForOfferRoleTypeAndUser(_view.SelectedAssignmentRoleType.Key, _view.SelectedSearchADUserName);

                        #endregion OfferRoleType Search

                        break;
                    case AssignmentType.WorkflowRoleType:

                        #region WorkflowRoleType Search

                        instanceList = _x2Repo.GetInstancesForWorkflowRoleTypeAndUser(_view.SelectedAssignmentRoleType.Key, _view.SelectedSearchADUserName);

                        #endregion WorkflowRoleType Search

                        break;
                    default:
                        break;
                }

                PrivateCacheData.Add(ViewConstants.ApplicationSearchCriteria, _view.SearchCriteria);
                PrivateCacheData.Add(ViewConstants.ApplicationSearchResults, instanceList);
                PrivateCacheData.Add(ViewConstants.BatchReassignUsers, _view.UserList);
                PrivateCacheData.Add(ViewConstants.BatchReassignSelectedSearchUser, _view.SelectedSearchADUserName);

                _view.SearchResults = instanceList;

                // bind the search results grid
                _view.BindSearchResults();
            }
        }

        private void RefreshResults()
        {
            if (_view.Messages.Count == 0)
            {
                if (PrivateCacheData.ContainsKey(ViewConstants.ApplicationSearchCriteria))
                {
                    _view.SearchCriteria = PrivateCacheData[ViewConstants.ApplicationSearchCriteria] as IWorkflowSearchCriteria;
                    PerformSearch();
                }
            }
        }

        private void RetrieveADUsers()
        {
            if (_selectedAssignmentRoleType != null)
            {
                IADUser searchADUser = null;
                IDictionary<IADUser, int> adUserDict = null;
                IDictionary<int, int> dicInstanceCount = null;

                switch (_selectedAssignmentRoleType.Type)
                {
                    case AssignmentType.OfferRoleType:
                        IApplicationRoleType art = _applicationRepo.GetApplicationRoleTypeByKey(_selectedAssignmentRoleType.Key);
                        adUserDict = _osRepo.GetADUsersPerRoleTypeAndOrgStructDictionary(art, _orgStructList);
                        break;
                    case AssignmentType.WorkflowRoleType:
                        IWorkflowRoleType wrt = _x2Repo.GetWorkflowRoleTypeByKey(_selectedAssignmentRoleType.Key);
                        adUserDict = _osRepo.GetADUsersPerWorkflowRoleTypeAndOrgStructDictionary(wrt, _orgStructList);
                        dicInstanceCount = _x2Repo.GetInstanceCountForWorkflowRoleTypeAndUser(_selectedAssignmentRoleType.Key);
                        break;
                    default:
                        break;
                }

                _lstAllUsers = new EventList<IADUser>();
                //_lstActiveUsers = new EventList<IADUser>();
                _adUserOrgStructHT = new Hashtable();

                if (_view.SelectedSearchADUserName != null && !string.IsNullOrEmpty(_view.SelectedSearchADUserName))
                    searchADUser = _osRepo.GetAdUserForAdUserName(_view.SelectedSearchADUserName);

                foreach (KeyValuePair<IADUser, int> kv in adUserDict)
                {
                    // From List
                    _lstAllUsers.Add(_view.Messages, kv.Key);

                    //// To List
                    //if (searchADUser != null)
                    //{
                    //    if (kv.Value < 0 && kv.Key.GeneralStatusKey.Key == (int)GeneralStatuses.Active && kv.Key.Key != searchADUser.Key)
                    //        _lstActiveUsers.Add(_view.Messages, kv.Key);
                    //}
                    //else
                    //{
                    //    if (kv.Value < 0 && kv.Key.GeneralStatusKey.Key == (int)GeneralStatuses.Active)
                    //        _lstActiveUsers.Add(_view.Messages, kv.Key);
                    //}

                    /* This table stores the ADUserKey and related Organisation Structure Key.
                     * It is used in the search.
                     * Another use is in the bind as a lookup:
                     *  -> IF the OrgStructKey > 0 THEN it is not be bound to the too list
                     *  -> IF the OrgStructKey < 0 THEN it is be bound to the too list
                     */
                    // Hashtable entries
                    _adUserOrgStructHT.Add(kv.Key.Key, kv.Value);
                }
                // Sort From List
                _lstAllUsers.Sort(
                delegate(IADUser ad1, IADUser ad2)
                {
                    return ad1.ADUserName.CompareTo(ad2.ADUserName);
                });

                // Sort To List
                //_lstActiveUsers.Sort(
                //delegate(IADUser ad1, IADUser ad2)
                //{
                //    return ad1.ADUserName.CompareTo(ad2.ADUserName);
                //});
                //Bind To View
                _view.BindUsers(_currentADUser.ADUserName, _lstAllUsers, searchADUser, dicInstanceCount, _adUserOrgStructHT);
            }
        }

        #endregion Helper Methods
    }
}