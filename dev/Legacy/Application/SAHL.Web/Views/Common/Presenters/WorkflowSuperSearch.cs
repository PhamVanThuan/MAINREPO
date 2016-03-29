using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.SearchCriteria;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using System.Text;
using SAHL.Common.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowSuperSearch : SAHLCommonBasePresenter<IWorkflowSuperSearch>
    {
        private const string workflowData = "WFDATA";
		protected const string workflowStates = "WFSTATES";
        private const string workflowFilters = "WFFILTERS";
        private const string orgstructure = "ORGANISATIONSTRUCTURE";
        private const string selectedusernode = "SELECTEDUSERKEY";
        private const string userFilters = "USERFILTERS";
        private IOrganisationStructureRepository _repoOrgStructure = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();


        List<string[]> _workflowFilters;
		protected Dictionary<IWorkFlow, IEventList<IState>> _workflowData;
		protected IEventList<IWorkFlow> _workflows;
		protected IEventList<IState> _workflowStates;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public WorkflowSuperSearch(IWorkflowSuperSearch view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // restore our workflowdata if we already have it.
            if (base.PrivateCacheData.ContainsKey(workflowData))
                _workflowData = base.PrivateCacheData[workflowData] as Dictionary<IWorkFlow, IEventList<IState>>;
            else
            {
                _workflowData = new Dictionary<IWorkFlow, IEventList<IState>>(new WorkflowEqualityComparer());
                base.PrivateCacheData.Add(workflowData, _workflowData);
            }
            if (base.PrivateCacheData.ContainsKey(workflowStates))
                _workflowStates = base.PrivateCacheData[workflowStates] as IEventList<IState>;
            else
            {
                _workflowStates = new EventList<IState>();
                base.PrivateCacheData.Add(workflowStates, _workflowStates);
            }
            if (base.PrivateCacheData.ContainsKey(workflowFilters))
                _workflowFilters = base.PrivateCacheData[workflowFilters] as List<string[]>;
            else
            {
                _workflowFilters = new List<string[]>();
                base.PrivateCacheData.Add(workflowFilters, _workflowFilters);
            }

            // add this view to the navigation cache item
            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo) == true)
                GlobalCacheData.Remove(ViewConstants.NavigateTo);

            // EXAMPLE GLOBAL CACHE LIFETIME
            List<string> Pages = new List<string>();
            Pages.Add(base.View.ViewName);
            GlobalCacheData.Add(ViewConstants.NavigateTo, _view.ViewName, new SimplePageCacheObjectLifeTime(Pages));

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // hook the events
            _view.ManageSearchButtonClicked += new EventHandler(_view_ManageSearchButtonClicked);
            _view.SavedSearchSelectedIndexChanged += new KeyChangedEventHandler(_view_SavedSearchSelectedIndexChanged);
            _view.SaveSearchButtonClicked += new EventHandler(_view_SaveSearchButtonClicked);
            _view.SearchButtonClicked += new EventHandler(_view_SearchButtonClicked);
            _view.SearchInSelectedIndexChanged += new KeyChangedEventHandler(_view_SearchInSelectedIndexChanged);
            _view.SearchResultsDoubleClick += new KeyChangedEventHandler(_view_SearchResultsDoubleClick);
            _view.UserSelected += new KeyChangedEventHandler(_view_UserSelected);
            _view.AddUserClicked += new EventHandler(_view_AddUserClicked);

            BindWorkflows();
            BindOrgStructure();
            BindUserFilters();

            // bind the applicationtypes
            ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
            IEventList<IApplicationType> AppTypes = LR.ApplicationTypes;
            _view.BindApplicationTypes(AppTypes);
        }

        void _view_UserSelected(object sender, KeyChangedEventArgs e)
        {
            int _selectedKey = (int) e.Key ;
            if (_selectedKey != -1)
            {
                if (PrivateCacheData.ContainsKey(selectedusernode))
                {
                    PrivateCacheData[selectedusernode] = _selectedKey;
                }
                else
                {
                    PrivateCacheData.Add(selectedusernode, _selectedKey);
                }
            }
        }

        void _view_AddUserClicked(object sender, EventArgs e)
        {
            int UserKey = -1;
            if (!PrivateCacheData.ContainsKey(selectedusernode))            
                return;            
            else
                UserKey = (int)PrivateCacheData[selectedusernode];

            List<IADUser> _userFilters = new List<IADUser>();
            if (PrivateCacheData.ContainsKey(userFilters))
                _userFilters = (List<IADUser>)PrivateCacheData[userFilters];
            else
                PrivateCacheData.Add(userFilters, _userFilters);

            foreach(IADUser user in _userFilters)
            {
            if (user.Key == UserKey)
                return;
            }
            ICommonRepository Rep = RepositoryFactory.GetRepository<ICommonRepository>();
            IADUser _aDUser = Rep.GetByKey<IADUser>(UserKey);

            _userFilters.Add(_aDUser);
            _view.BindUserFilter(_userFilters);
            PrivateCacheData[userFilters] = _userFilters;

        }

        protected virtual void _view_SearchResultsDoubleClick(object sender, KeyChangedEventArgs e)
        {
            if (e == null)
                return;

            int instanceId = Convert.ToInt32(e.Key);

            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            IInstance instance = x2Repo.GetInstanceByKey(instanceId);

            //add the instance to the tasks node and navigate to the appropriate view
            //Instance.State.StateForms[0].Form.Name is the navigate value
            List<CBONode> nodes = CBOManager.GetMenuNodes(_view.CurrentPrincipal, CBONodeSetType.X2);

            TaskListNode taskListNode = null;

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] is TaskListNode)
                {
                    taskListNode = nodes[i] as TaskListNode;
                    break;
                }
            }

            if (taskListNode == null)
            {
                taskListNode = new TaskListNode(null);
                CBOManager.AddCBOMenuNode(_view.CurrentPrincipal, null, taskListNode, CBONodeSetType.X2);
            }

            //the name of the relevant x2data table will be the same as the storagetable field from the workflow table
            //the storagekey field in the workflow table is the name of the column in the x2data table that has the actual key
            //forms for a stage are ordered by the formorder column in the stateform table
            IWorkFlow wf = instance.WorkFlow;
            IDictionary<string, object> dict = X2Service.GetX2DataRow(instance.ID);
            int businessKey = Convert.ToInt32(dict[wf.StorageKey]);

            // setup the instance node description
            string nodeDesc = "", longDesc = "";
            x2Repo.SetInstanceNodeDescription(wf, instance, businessKey, out nodeDesc, out longDesc);

            InstanceNode iNode = new InstanceNode(businessKey, taskListNode, nodeDesc, longDesc, instance.ID, instance.State.Forms[0].Name);

            CBOManager.AddCBOMenuNode(_view.CurrentPrincipal, taskListNode, iNode, CBONodeSetType.X2);
            CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, iNode, CBONodeSetType.X2);
            _view.Navigator.Navigate(iNode.URL);
        }

        // DO NOT REMOVE this suppression 
        [SuppressMessage("Microsoft.Performance", "CA1804", Justification = "We are faking a lazy load by reading the count property, do not delete the unused local.")]
        private void BindWorkflows()
        {
            bool WasLoaded = false;
            // construct our workflow data if we haven't done so before.
            if (_workflowData.Count == 0)
            {
                WasLoaded = true;
                IX2Repository X2R = RepositoryFactory.GetRepository<IX2Repository>();

                // depending on which search type we are performing the workflow data will come from the todo list
                // or from the list of workflows linked to organisation structure
                if (_view.SelectedSearchType == 0)
                {
                    IEventList<IInstance> Instances = X2R.GetInstanceByPrincipal(_view.CurrentPrincipal);
                    for (int i = 0; i < Instances.Count; i++)
                    {
                        if (!_workflowData.ContainsKey(Instances[i].WorkFlow))
                        {
                            _workflowData.Add(Instances[i].WorkFlow, new EventList<IState>());
                            // DO NOT REMOVE THIS LINE IT IS REQUIRED!!!!!!
                            int c = Instances[i].WorkFlow.States.Count;
                        }
                        else
                        {
                            if (!_workflowData[Instances[i].WorkFlow].Contains(Instances[i].State))
                                _workflowData[Instances[i].WorkFlow].Add(_view.Messages, Instances[i].State);
                        }
                    }
                }
                else
                {
                    IEventList<IWorkFlow> Workflows = X2R.GetSearchableWorkflowsForUser(_view.CurrentPrincipal);
                    for (int i = 0; i < Workflows.Count; i++)
                    {
                        if (!_workflowData.ContainsKey(Workflows[i]))
                        {
                            EventList<IState> WFStates = new EventList<IState>();
                            for (int h = 0; h < Workflows[i].States.Count; h++)
                            {
                                if (Workflows[i].States[h].StateType.ID == (int)X2StateTypes.User || Workflows[i].States[h].StateType.ID == (int)X2StateTypes.Archive)
                                    WFStates.Add(_view.Messages, Workflows[i].States[h]);
                            }
                            _workflowData.Add(Workflows[i], WFStates);
                        }
                    }
                }
            }

            // bind the workflows
            _workflows = new EventList<IWorkFlow>();
            foreach (KeyValuePair<IWorkFlow, IEventList<IState>> w in _workflowData)
                _workflows.Add(_view.Messages, w.Key);

            if (WasLoaded && _workflows.Count > 0)
            {
                _workflowStates = _workflows[0].States;
                base.PrivateCacheData[workflowStates] = _workflowStates;
            }

            _view.BindWorkflows(_workflows);
        }

        private void BindOrgStructure()
        {
            List<IBindableTreeItem> Bind = null;
            //int TopLevelKey = 1; // SAHL ... get this from ddl in future.
            ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
            IADUser user = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);

            ICommonRepository commonRep = RepositoryFactory.GetRepository<ICommonRepository>();

            commonRep.UpdateInCurrentNHibernateSession(user);

            List<int> BoundNodes = new List<int>();

            Bind = new List<IBindableTreeItem>();
            if (!PrivateCacheData.ContainsKey(orgstructure))
            {

                for (int i = 0; i < user.UserOrganisationStructure.Count; i++)
                {


                    int TopLevelKey = 0;
                    if (user.UserOrganisationStructure[i].OrganisationStructure.Parent != null)
                        TopLevelKey = user.UserOrganisationStructure[i].OrganisationStructure.Parent.Key;
                    else
                        TopLevelKey = user.UserOrganisationStructure[i].OrganisationStructure.Key;

                    if (!BoundNodes.Contains(TopLevelKey))
                    {
                        IOrganisationStructure os = _repoOrgStructure.GetOrganisationStructureForKey(TopLevelKey);
                        BindOrganisationStructure bf = new BindOrganisationStructure(os);
                        Bind.Add(bf);
                        BoundNodes.Add(TopLevelKey);
                    }
                }

                PrivateCacheData.Add(orgstructure, Bind);
            }
            else
            {
                Bind = PrivateCacheData[orgstructure] as List<IBindableTreeItem>;
            }
            _view.BindOrganisationStructure(Bind);//TopLevelKey);
        }

        private void BindUserFilters()
        {
            if (_view.SelectedSearchType == 0)
            {
                ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                List<IADUser> _aDUsers = new List<IADUser>();
                _aDUsers.Add(secRepo.GetADUserByPrincipal(_view.CurrentPrincipal));                
                _view.BindUserFilter(_aDUsers);
            }    
            else if (PrivateCacheData.ContainsKey(userFilters))
            {
                List<IADUser> _aDUsers = (List<IADUser>)PrivateCacheData[userFilters];
                _view.BindUserFilter(_aDUsers);
            }
        }

        /// <summary>
        /// Method to check if <c>parentOrgStructureKey</c> is the key of a parent of 
        /// <c>childStructureKey</c>
        /// </summary>
        /// <param name="childStructureKey"></param>
        /// <param name="parentOrgStructureKey"></param>
        /// <returns></returns>
        private bool IsOrgStructureParent(int childStructureKey, int parentOrgStructureKey)
        {
            IOrganisationStructure os = _repoOrgStructure.GetOrganisationStructureForKey(childStructureKey);
            while (os != null)
            {
                if (os.Key == parentOrgStructureKey)
                    return true;

                os = os.Parent;
            }
            return false;

        }

        protected virtual void _view_SearchButtonClicked(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            IX2Repository X2 = RepositoryFactory.GetRepository<IX2Repository>();
            WorkflowSearchCriteria WSC = new WorkflowSearchCriteria();
            
            WSC.ApplicationNumber = _view.ApplicationNo;
            WSC.Firstname = _view.Firstname;
            WSC.Surname = _view.Surname;
            WSC.IDNumber = _view.IDNumber;
            WSC.IncludeHistoricUsers = _view.IncludeHistoricUsers;
            WSC.MaxResults = _view.MaximumRowCount + 1; // make it one more so we know it's been exceeded

            if (_view.IncludeMyCreatedCases)
                WSC.CreatorUser = _view.CurrentPrincipal.Identity.Name;
            else
                WSC.CreatorUser = "";

            // set the application type
            if (_view.SelectedApplicationTypeKey.HasValue)
                WSC.ApplicationTypes.Add((OfferTypes)_view.SelectedApplicationTypeKey.Value);

            // add any user filters
            IDictionary<int, IList<int>> workflowFilters = _view.WorkflowFilters;
            foreach (KeyValuePair<int, IList<int>> kvp in workflowFilters)
            {
                int[] states = new int[kvp.Value.Count];
                for (int i = 0; i < states.Length; i++)
                    states[i] = kvp.Value[i];

                WSC.WorkflowFilter.Add(new WorkflowSearchCriteriaWorkflowFilter(kvp.Key, states));
            }

            // if its our own case view add ourselves to the filter
            if (_view.SelectedSearchType == 0)
                WSC.UserFilter.Add(_view.CurrentPrincipal.Identity.Name);
            else
            {
                // add all users from each selected node
                int lastOrgStructureKey = -1;
                foreach (int orgStructureKey in _view.OrganisationStructureFilters)
                {
                    if (!IsOrgStructureParent(orgStructureKey, lastOrgStructureKey))
                    {
                        IEventList<IADUser> users = _repoOrgStructure.GetUsersForOrganisationStructureKey(orgStructureKey, true);
                        foreach (IADUser adUser in users)
                        {
                            if (!WSC.UserFilter.Contains(adUser.ADUserName))
                                WSC.UserFilter.Add(adUser.ADUserName);
                        }
                    }
                    lastOrgStructureKey = orgStructureKey;
                }

                // add each individual user selected
                foreach (int adUserKey in _view.OrganisationStructureUserFilters)
                {
                    IADUser adUser = _repoOrgStructure.GetADUserByKey(adUserKey);
                    if (!WSC.UserFilter.Contains(adUser.ADUserName))
                        WSC.UserFilter.Add(adUser.ADUserName);
                }
            }

            // if the is a CAP2 search we need to a different search
            WSC.Cap2Search = false; 
            if (WSC.ApplicationTypes != null && WSC.ApplicationTypes.Count == 1 && String.Compare(WSC.ApplicationTypes[0].ToString(), "cap2", true) == 0)
                WSC.Cap2Search = true;

            IList<IInstance> Results = X2.SuperSearchWorkflow(WSC);
            _view.BindSearchResult(Results);

            base.PrivateCacheData.Add(ViewConstants.WorkFlowSearchResultsKey, Results);

        }

        void _view_SaveSearchButtonClicked(object sender, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected virtual void _view_SearchInSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            // if it has changed we must refresh the data so clear it here
            _workflowData.Clear();
            BindWorkflows();
        }

        void _view_SavedSearchSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void _view_ManageSearchButtonClicked(object sender, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {

            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage)
                return;

            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
            {
                _view.ApplicationNo = GlobalCacheData[ViewConstants.ApplicationKey].ToString();
                GlobalCacheData.Remove(ViewConstants.ApplicationKey);
                PerformSearch();
                _view.ShowSearchResults = true;
            }
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // show the correct panels baed on our search type
            if (_view.SelectedSearchType == 0)
            {
                _view.OrganisationalStructureFilterVisible = false;
                _view.MyCreatedCasesFilterVisible = true;
            }
            else
            {
                _view.OrganisationalStructureFilterVisible = true;
                _view.MyCreatedCasesFilterVisible = false;
            }

            _view.ccdCategory = "User";
        }

    }

    public class WorkflowEqualityComparer : IEqualityComparer<IWorkFlow>
    {
        #region IEqualityComparer<IWorkFlow> Members

        public bool Equals(IWorkFlow x, IWorkFlow y)
        {
            if (x.ID == y.ID)
                return true;
            else
                return false;
        }

        public int GetHashCode(IWorkFlow obj)
        {
            return obj.ID;
        }

        #endregion
    }
}
