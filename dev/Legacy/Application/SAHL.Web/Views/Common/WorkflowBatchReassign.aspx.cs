using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.Collections;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common
{
    public partial class WorkflowBatchReassign : SAHLCommonBaseView, IWorkflowBatchReassign
    {
        private string _selectedSearchADUserName = "";
        private string _selectedReassignADUserName;
        private int _maxSearchResults;
        private bool _allowLeadReassign;
        private IList<int> _selectedInstanceIDs;
        private IList<IInstance> _searchResults;
        private AssignmentRoleType _selectedAssignmentRoleType;
        private Dictionary<long, IApplication> _applicationInstanceDict;

        private IEventList<IADUser> _userList;

        private IWorkflowSearchCriteria _searchCriteria;

        private enum GridColumnPositions
        {
            Select = 0,
            InstanceID = 1,
            GenericKey = 2,
            CaseName = 3,
            WorkflowName = 4,
            WorkflowState = 5,
            CreationDate = 6,
            ApplicationType = 7,
            ApplicationStatus = 8,
            Consultant = 9
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.ShouldRunPage)
                return;


            btnReassignLeads.Visible = _allowLeadReassign;
            lblReassignUser.Visible = _allowLeadReassign;
            ddlReassignUser.Visible = _allowLeadReassign;

            if (!String.IsNullOrEmpty(_selectedSearchADUserName) && _selectedSearchADUserName != SAHL.Common.Constants.DefaultDropDownItem)
                ddlSearchUser.SelectedValue = _selectedSearchADUserName;

            if (!String.IsNullOrEmpty(_selectedReassignADUserName) && _selectedReassignADUserName != SAHL.Common.Constants.DefaultDropDownItem)
                ddlReassignUser.SelectedValue = _selectedReassignADUserName;
        }

        #region ILeadReassign Members

        /// <summary>
        /// 
        /// </summary>
        public IWorkflowSearchCriteria SearchCriteria
        {
            get
            {
                return _searchCriteria;
            }
            set
            {
                _searchCriteria = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedSearchADUserName
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlSearchUser.SelectedValue) && ddlSearchUser.SelectedValue != SAHL.Common.Constants.DefaultDropDownItem)
                    return ddlSearchUser.SelectedValue;
                else if (!string.IsNullOrEmpty(Request.Form[ddlSearchUser.UniqueID].ToString()) && Request.Form[ddlSearchUser.UniqueID].ToString() != SAHL.Common.Constants.DefaultDropDownItem)
                    return Request.Form[ddlSearchUser.UniqueID].ToString();
                else return _selectedSearchADUserName;
            }
            set { _selectedSearchADUserName = value; }
        }


        public AssignmentRoleType SelectedAssignmentRoleType
        {
            get
            {
                return _selectedAssignmentRoleType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedReassignADUserName
        {
            get { return _selectedReassignADUserName; }
            set { _selectedReassignADUserName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<int> SelectedInstanceIDs
        {
            get { return _selectedInstanceIDs; }
            set { _selectedInstanceIDs = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MaxSearchResults
        {
            get { return _maxSearchResults; }
            set { _maxSearchResults = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AllowLeadReassign
        {
            get { return _allowLeadReassign; }
            set { _allowLeadReassign = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<IInstance> SearchResults
        {
            get { return _searchResults; }
            set { _searchResults = value; }
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appInstDict"></param>
        public void ApplicationInstanceDict(Dictionary<long, IApplication> appInstDict)
        {
            _applicationInstanceDict = appInstDict;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IADUser> UserList
        {
            get { return _userList; }
            set { _userList = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSearchButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnReassignButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnRoleTypeSelectedIndexChanged;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstAssignmentRoleTypes"></param>
        public void BindRolesTypes(IList<AssignmentRoleType> lstAssignmentRoleTypes)
        {
            foreach (AssignmentRoleType asstype in lstAssignmentRoleTypes)
            {
                ddlRoleType.Items.Add(new ListItem(asstype.Description.TrimEnd('D').Trim(), (int)asstype.Type + "|" + asstype.Key.ToString()));
            }

            ddlRoleType.DataBind();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="principalADUSerName"></param>
        /// <param name="lstUsers"></param>
        /// <param name="searchADUser"></param>
        /// <param name="dicInstanceCount"></param>
        public void BindUsers(string principalADUSerName, IEventList<IADUser> lstUsers, IADUser searchADUser, IDictionary<int, int> dicInstanceCount,Hashtable ADUserOrgStruct)
        {
            ddlSearchUser.Items.Clear();
            ddlReassignUser.Items.Clear();

            if (lstUsers != null)
            {
                _userList = lstUsers;

                // Bind the users to search for
                // - all users
                foreach (IADUser aduser in lstUsers)
                {
                    ddlSearchUser.Items.Add(new ListItem(aduser.LegalEntity.FirstNames + " " + aduser.LegalEntity.Surname, aduser.ADUserName));
                }
                ddlSearchUser.DataBind();

                // Bind the users to reassign to
                // - only active users
                // - exclude current logged-in user
                // - exclude user whose cases we searched for
                if (dicInstanceCount == null || dicInstanceCount.Count <= 0)
                {
                    // no instance count so bind normally
                    foreach (IADUser aduser in lstUsers)
                    {
                        if (aduser.ADUserName != principalADUSerName && aduser.Key != (searchADUser == null ? 0 : searchADUser.Key) 
                            && aduser.GeneralStatusKey.Key == (int)GeneralStatuses.Active && Convert.ToInt32(ADUserOrgStruct[aduser.Key]) < 0)
                            ddlReassignUser.Items.Add(new ListItem(aduser.LegalEntity.FirstNames + " " + aduser.LegalEntity.Surname, aduser.ADUserName));
                    }
                }
                else
                {
                    // add users and their instance count to a dictionary
                    IDictionary<IADUser, int> dicUserCount = new Dictionary<IADUser, int>();
                    foreach (IADUser activeADUser in lstUsers)
                    {
                        if (activeADUser.ADUserName != principalADUSerName && activeADUser.Key != (searchADUser == null ? 0 : searchADUser.Key)
                            && activeADUser.GeneralStatusKey.Key == (int)GeneralStatuses.Active && Convert.ToInt32(ADUserOrgStruct[activeADUser.Key]) < 0)
                        {
                            // find instance count in dictionary
                            int instanceCount = 0;
                            if (dicInstanceCount.ContainsKey(activeADUser.Key))
                                dicInstanceCount.TryGetValue(activeADUser.Key, out instanceCount);

                            dicUserCount.Add(activeADUser, instanceCount);
                        }
                    }

                    // sort the dictionary items
                    List<KeyValuePair<IADUser, int>> myList = new List<KeyValuePair<IADUser, int>>(dicUserCount);
                    myList.Sort((firstPair, nextPair) => {return firstPair.Value.CompareTo(nextPair.Value);});

                    // bind the sorted list to the dropdown list
                    foreach (var user in myList)
                    {
                        ddlReassignUser.Items.Add(new ListItem(user.Key.LegalEntity.FirstNames + " " + user.Key.LegalEntity.Surname + " (" + user.Value + " )", user.Key.ADUserName));
                    }
                }

                ddlReassignUser.DataBind();
            }
            else
            {
                ddlSearchUser.Items.Add(new ListItem("", ""));
                ddlSearchUser.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void BindSearchResults()
        {
            if (_searchResults.Count > _maxSearchResults)
            {
                lblSearchResults.Text = "Your search results have been limited to " + _maxSearchResults + " records";
                lblSearchResults.Visible = true;
            }
            else
                lblSearchResults.Visible = false;


            SearchGrid.Columns.Clear();

            bool appColsVisible = true;
            if (_applicationInstanceDict == null)
                appColsVisible = false;

            SearchGrid.AddCheckBoxColumn("chkSelect", "", true, Unit.Percentage(1), HorizontalAlign.Center, true);
            SearchGrid.AddGridBoundColumn("", "Instance ID", Unit.Percentage(0), HorizontalAlign.Left, false);
            SearchGrid.AddGridBoundColumn("", "App No", Unit.Percentage(10), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("", "App Details", Unit.Percentage(25), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("", "Workflow", Unit.Percentage(10), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("", "Stage", Unit.Percentage(15), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("", "Creation Date", Unit.Percentage(5), HorizontalAlign.Center, true);
            SearchGrid.AddGridBoundColumn("", "App Type", Unit.Percentage(10), HorizontalAlign.Left, appColsVisible);
            SearchGrid.AddGridBoundColumn("", "App Status", Unit.Percentage(10), HorizontalAlign.Left, appColsVisible);
            SearchGrid.AddGridBoundColumn("", "Assigned To", Unit.Percentage(10), HorizontalAlign.Left, true);


            SearchGrid.DataSource = _searchResults;
            SearchGrid.DataBind();

            if (_searchResults.Count > 0)
                _allowLeadReassign = true;
            else
                _allowLeadReassign = false;

        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Get the selected consultant
            _selectedSearchADUserName = Request.Form[ddlSearchUser.UniqueID].ToString();

            // validate that the required search criteria has been selected
            if (ValidateSearchCriteria() == false)
                return;

            if (!string.IsNullOrEmpty(ddlRoleType.SelectedValue) && ddlRoleType.SelectedValue.Trim().Length > 0)
                _selectedAssignmentRoleType = GetAssignmentRoleType(ddlRoleType.SelectedValue);

            // set the search criteria          
            _searchCriteria = new WorkflowSearchCriteria();

            // number of recs to return 
            _searchCriteria.MaxResults = _maxSearchResults;

            // user to search for
            if (!String.IsNullOrEmpty(_selectedSearchADUserName))
                _searchCriteria.UserFilter.Add(_selectedSearchADUserName);

            OnSearchButtonClicked(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReassignLeads_Click(object sender, EventArgs e)
        {
            _allowLeadReassign = true;

            // Get the selected consultant
            _selectedSearchADUserName = Request.Form[ddlSearchUser.UniqueID].ToString();

            // Get the selected reassign user
            _selectedReassignADUserName = Request.Form[ddlReassignUser.UniqueID].ToString();

            if (!string.IsNullOrEmpty(ddlRoleType.SelectedValue) && ddlRoleType.SelectedValue.Trim().Length > 0)
                _selectedAssignmentRoleType = GetAssignmentRoleType(ddlRoleType.SelectedValue);

            // validate the reassign user 
            if (String.IsNullOrEmpty(_selectedReassignADUserName) || _selectedReassignADUserName == SAHL.Common.Constants.DefaultDropDownItem)
            {
                SetErrorMessage("Must select a user to reassign.");
                return;
            }

            // Get the selected rows via the checkboxes from the GridView control
            bool bSelected = false;
            _selectedInstanceIDs = new List<int>();
            for (int i = 0; i < SearchGrid.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)SearchGrid.Rows[i].FindControl("chkSelect");
                object o = Page.Request.Form[cb.UniqueID];
                bool isChecked = (o == null) ? false : true;
                if (isChecked)
                {
                    bSelected = true;
                    string instanceIdText = SearchGrid.Rows[i].Cells[(int)GridColumnPositions.InstanceID].Text;
                    if (!String.IsNullOrEmpty(instanceIdText))
                    {
                        int instanceID = Convert.ToInt32(instanceIdText);

                        // check that the instance id doesnt already exists in the selectedlist
                        bool found = false;
                        foreach (int instID in _selectedInstanceIDs)
                        {
                            if (instID == instanceID)
                            {
                                found = true;
                                break;
                            }

                        }
                        // only add the instance id if it doenat already exist
                        if (found == false)
                            _selectedInstanceIDs.Add(instanceID);
                    }
                }
            }

            if (bSelected == false)
            {
                SetErrorMessage("Must select at least one application.");
                return;
            }

            OnReassignButtonClicked(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool bEnableSelect = true;

                // Get the Instance Row
                IInstance instance = e.Row.DataItem as IInstance;

                // if we are dealing with a debtcounselling instance then implement group tickbox logic
                //  - when we tick an instance, we need to tick all other instances in the same group 
                //  - when we untick an instance, we need to untick all other instances in the same group 
                if (instance.WorkFlow.GenericKeyTypeKey == (int)SAHL.Common.Globals. GenericKeyTypes.DebtCounselling2AM)
                {
                    // get the X2Data
                    X2Data x2Data = RepositoryFactory.GetRepository<IX2Repository>().GetX2DataForInstance(instance);

                    if (x2Data != null && x2Data.GenericKey > 0)
                    {
                        // get debtcounselling object
                        IDebtCounselling debtCounselling = RepositoryFactory.GetRepository<IDebtCounsellingRepository>().GetDebtCounsellingByKey(x2Data.GenericKey);
                        if (debtCounselling != null)
                        {
                            // find the checkbox
                            CheckBox cb = (CheckBox)e.Row.FindControl("chkSelect");
                            cb.Attributes.Add("onclick", "SelectRelated(this,\'gk" + debtCounselling.DebtCounsellingGroup.Key + "\')");
                            cb.CssClass += " gk" + debtCounselling.DebtCounsellingGroup.Key;
                        }
                    }
                }

                // Instance ID
                e.Row.Cells[(int)GridColumnPositions.InstanceID].Text = instance.ID.ToString();

                // Workflow Name
                e.Row.Cells[(int)GridColumnPositions.WorkflowName].Text = instance.WorkFlow.Name;

                // Workflow State
                e.Row.Cells[(int)GridColumnPositions.WorkflowState].Text = instance.State.Name;

                // Creation Date
                e.Row.Cells[(int)GridColumnPositions.CreationDate].Text = instance.CreationDate.ToString(SAHL.Common.Constants.DateFormat);

                // Consultant
                if (instance.WorkLists.Count > 1)
                {
                    e.Row.Cells[(int)GridColumnPositions.Consultant].Text = "Multiple";
                    string consultants = "";
                    int cnt = 0;
                    foreach (IWorkList worklist in instance.WorkLists)
                    {
                        if (cnt == 0)
                            consultants += worklist.ADUserName;
                        else
                            consultants += "," + worklist.ADUserName;
                        cnt++;
                    }
                    e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "Application exists on multiple worklists : " + consultants;
                    bEnableSelect = false;
                }
                else if (instance.WorkLists.Count == 0)
                {
                    e.Row.Cells[(int)GridColumnPositions.Consultant].Text = "None";
                    bEnableSelect = false;
                }
                else
                    e.Row.Cells[(int)GridColumnPositions.Consultant].Text = instance.WorkLists[0].ADUserName == null ? "Unknown" : instance.WorkLists[0].ADUserName;

                if (_applicationInstanceDict != null)
                {
                    #region Get the Application Data
                    IApplication application = _applicationInstanceDict[instance.ID];

                    if (application != null)
                    {
                        // Application Key
                        e.Row.Cells[(int)GridColumnPositions.GenericKey].Text = application.Key.ToString();

                        // Application Details
                        e.Row.Cells[(int)GridColumnPositions.CaseName].Text = application.GetLegalName(LegalNameFormat.InitialsOnly);

                        // Application Type
                        e.Row.Cells[(int)GridColumnPositions.ApplicationType].Text = application.ApplicationType.Description;

                        // Application Status
                        e.Row.Cells[(int)GridColumnPositions.ApplicationStatus].Text = application.ApplicationStatus.Description;

                        switch (application.ApplicationStatus.Key)
                        {
                            case (int)SAHL.Common.Globals.OfferStatuses.Open:
                                bEnableSelect = true;
                                break;
                            default:
                                bEnableSelect = false;
                                e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "Application Status is : " + application.ApplicationStatus.Description;
                                break;
                        }

                    }
                    #endregion
                }
                else
                {
                    #region Get the Instance Data
                    if (instance != null)
                    {
                        // Generic Key
                        e.Row.Cells[(int)GridColumnPositions.GenericKey].Text = instance.Name;

                        // Case Name
                        e.Row.Cells[(int)GridColumnPositions.CaseName].Text = instance.Subject;


                        switch (instance.State.StateType.ID)
                        {
                            case (int)SAHL.Common.Globals.X2StateTypes.User:
                                bEnableSelect = true;
                                break;
                            default:
                                bEnableSelect = false;
                                e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "Case is at a state of type : " + instance.State.StateType.Name;
                                break;
                        }
                        

                    }
                    #endregion
                }

                // Select
                e.Row.Cells[(int)GridColumnPositions.Select].Enabled = bEnableSelect;
            }
        }

        protected bool ValidateSearchCriteria()
        {
            bool valid = true;

            if (ddlRoleType.SelectedIndex <= 0)
            {
                SetErrorMessage("Please select a Role Type");
                valid = false;
            }

            if (String.IsNullOrEmpty(_selectedSearchADUserName) || _selectedSearchADUserName == SAHL.Common.Constants.DefaultDropDownItem)
            {
                SetErrorMessage("Must select a user to search.");
                valid = false;
            }

            return valid;
        }



        protected void ddlRoleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnRoleTypeSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlRoleType.SelectedItem.Value));

        }

        public AssignmentRoleType GetAssignmentRoleType(string selectedValue)
        {
            string[] keys = selectedValue.Split('|');
            int assignmentTypeKey = Convert.ToInt32(keys[0]);
            int assignmentRoleTypeKey = Convert.ToInt32(keys[1]);
            AssignmentType assignmentType = (AssignmentType)assignmentTypeKey;

            AssignmentRoleType art = new AssignmentRoleType();

            switch (assignmentType)
            {
                case AssignmentType.OfferRoleType:
                    IApplicationRepository applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IApplicationRoleType selectedApplicationRoleType = applicationRepo.GetApplicationRoleTypeByKey(assignmentRoleTypeKey);
                    art.Type = AssignmentType.OfferRoleType;
                    art.Key = selectedApplicationRoleType.Key;
                    art.Description = selectedApplicationRoleType.Description;
                    art.GroupKey = selectedApplicationRoleType.ApplicationRoleTypeGroup.Key;

                    break;
                case AssignmentType.WorkflowRoleType:
                    IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
                    IWorkflowRoleType selectedWorkflowRoleType = x2Repo.GetWorkflowRoleTypeByKey(assignmentRoleTypeKey);
                    art.Type = AssignmentType.WorkflowRoleType;
                    art.Key = selectedWorkflowRoleType.Key;
                    art.Description = selectedWorkflowRoleType.Description;
                    art.GroupKey = selectedWorkflowRoleType.WorkflowRoleTypeGroup.Key;
                    break;
                default:
                    break;
            }
            return art;
        }

        /// <summary>
        /// Set Error Message
        /// </summary>
        /// <param name="errorMessage"></param>
        private void SetErrorMessage(string errorMessage)
        {
            this.Messages.Add(new Error(errorMessage, errorMessage));
        }
    }
}
