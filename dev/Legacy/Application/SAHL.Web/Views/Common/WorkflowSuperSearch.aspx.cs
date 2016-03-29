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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using System.Text;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Web.AJAX;
using SAHL.Common.Globals;
using System.Diagnostics.CodeAnalysis;
using SAHL.Web.Controls;
using DevExpress.Web.ASPxGridView;
using SAHL.Common.CacheData;
using SAHL.Common.Security;

namespace SAHL.Web.Views.Common
{
	/// <summary>
	/// 
	/// </summary>
	public partial class WorkflowSuperSearch : SAHLCommonBaseView, IWorkflowSuperSearch
	{
		// IX2Service _X2 = TypeFactory.CreateType<IX2Service>();
		private bool _orgStructureFilterVisible;
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		private IDictionary<int, IList<int>> _dictWorkflowFilters = new Dictionary<int, IList<int>>();
		private const string SearchInMyCases = "My Cases";
		private const string SearchInAllCases = "All Cases";
		protected const string WorkflowFilterOptionTextFormat = "{0} - {1}";
		protected const string WorkflowFilterOptionValueFormat = "{0}|{1}";
		private List<int> _lstOrgStructure = new List<int>();
		private List<int> _lstOrgStructureUsers = new List<int>();
		private bool _showSearchResults;

		/// <summary>
		/// Enumeration for grid positions (note this is for VISIBLE columns only)
		/// </summary>
		private enum GridColumnPositions
		{
			ApplicationKey = 0,
			ApplicationDetails = 1,
			WorkflowName = 2,
			WorkflowState = 3,
			ApplicationType = 4,
			AssignedTo = 5
		}


		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (!ShouldRunPage) return;

			ddlSearchIn.Items.Add(SearchInMyCases);
			ddlSearchIn.Items.Add(SearchInAllCases);

			if (IsPostBack)
			{
				// build the list of work flow filters for retrieval if a postback has occurred
				string postedWorkflowStates = Page.Request.Form[hidWorkflowStates.UniqueID];
				if (!String.IsNullOrEmpty(postedWorkflowStates))
				{
					string[] selValues = postedWorkflowStates.Split(',');
					foreach (string val in selValues)
					{
						string[] wfs = val.Split('|');
						int workflowId = Int32.Parse(wfs[0]);
						if (!_dictWorkflowFilters.ContainsKey(workflowId))
							_dictWorkflowFilters.Add(workflowId, new List<int>());
						_dictWorkflowFilters[workflowId].Add(Int32.Parse(wfs[1]));
					}
				}

				// build a list of org structure filters if a postback has occurred
				foreach (string orgStructValue in tvOrgStruct.CheckedValues)
				{
					int v = Int32.Parse(orgStructValue.Substring(1));
					if (orgStructValue.StartsWith("A"))
						_lstOrgStructureUsers.Add(v);
					else
						_lstOrgStructure.Add(v);
				}

			}

		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (!ShouldRunPage) return;

			this.RegisterWebService(ServiceConstants.X2);

			if (grdResults.IsCallback)
			{
				SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
				IList<IInstance> instances = (IList<IInstance>)spc.GetPresenterData()[ViewConstants.WorkFlowSearchResultsKey];
				BindSearchResult(instances);
			}

		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (!ShouldRunPage) return;

			// this needs to get set every time - the control itself tries to maintain state and will reset
			// any changes made earlier if client-side clicks occur
			tcMain.ActiveTabIndex = (ShowSearchResults ? 1 : 0);
			//set the hiddenfield to hold the active tab index 
			hidActiveTabIndex.Value = tcMain.ActiveTabIndex.ToString(); ;

			// hide rows that aren't needed
			// rowWorkflows.Visible = (ddlSearchIn.SelectedValue == SearchInAllCases);
			rowOrgStruct.Visible = _orgStructureFilterVisible;
			rowOrgStructLabel.Visible = _orgStructureFilterVisible;

			// if there were values sent through for the workflow filters, then rebind them to the list control
			if (_dictWorkflowFilters.Count > 0)
			{
				IX2Repository repoX2 = RepositoryFactory.GetRepository<IX2Repository>();
				lstWorkflowStates.Items.Clear();

				// TODO: This is pretty inefficient grabbing each item one at a time - might want to 
				// look at writing a method to get all of these at once if this page runs slowly
				foreach (KeyValuePair<int, IList<int>> kvp in _dictWorkflowFilters)
				{
					IWorkFlow wf = repoX2.GetWorkFlowByKey(kvp.Key);
					foreach (int stateKey in kvp.Value)
					{
						IState state = repoX2.GetStateByKey(stateKey);
						lstWorkflowStates.Items.Add(new ListItem(
							String.Format(WorkflowFilterOptionTextFormat, wf.Name, state.Name),
							String.Format(WorkflowFilterOptionValueFormat, wf.ID, state.ID)
						));
					}
				}
			}

		}


		#region IWorkflowSuperSearch Members

		public event SAHL.Common.Web.UI.Events.KeyChangedEventHandler SearchInSelectedIndexChanged;

		public event SAHL.Common.Web.UI.Events.KeyChangedEventHandler SavedSearchSelectedIndexChanged;

		public event EventHandler SaveSearchButtonClicked;

		public event EventHandler ManageSearchButtonClicked;

		public event EventHandler AddWorkflowFilterButtonClicked;

		public event EventHandler RemoveWorkflowFilterButtonClicked;

		public event EventHandler SearchButtonClicked;

		public event EventHandler ResetButtonClicked;

		public event EventHandler AddUserClicked;

		public event KeyChangedEventHandler UserSelected;

		public event SAHL.Common.Web.UI.Events.KeyChangedEventHandler SearchResultsDoubleClick;

		public bool MyCreatedCasesFilterVisible
		{
			set { trchkIncludeMyApps.Visible = value; }
		}

		public bool OrganisationalStructureFilterVisible
		{
			get
			{
				return _orgStructureFilterVisible;
			}
			set
			{
				_orgStructureFilterVisible = value;
			}
		}

		public bool SavedSearchPanelVisible
		{
			set { pnlSavedSearches.Visible = value; }
		}

		/// <summary>
		/// Gets/sets whether the search results tab is displayed.  This is automatically set to true when 
		/// the Search button is clicked.
		/// </summary>
		public bool ShowSearchResults
		{
			get
			{
				return _showSearchResults;
			}
			set
			{
				_showSearchResults = value;
			}
		}

		/// <summary>
		/// Gets the maximum number of rows to return.
		/// </summary>
		public int MaximumRowCount
		{
			get
			{
				return Int32.Parse(selMaxResults.SelectedValue);
			}
		}

		public int SelectedSearchType
		{
			get
			{
				if (Page.Request.Form[ddlSearchIn.UniqueID] != null)
					return Page.Request.Form[ddlSearchIn.UniqueID].ToString() == "My Cases" ? 0 : 1;
				return ddlSearchIn.SelectedIndex;
			}
			set
			{
				ddlSearchIn.SelectedIndex = value;
			}
		}

		public bool SetSelectedSearchTypeEnabled
		{
			set
			{
				ddlSearchIn.Enabled = value;
			}
		}

		public int SelectedSavedSearchIndex
		{
			get
			{
				return ddlSavedSearches.SelectedIndex;
			}
		}

		//public int SelectedWorkflowIndex
		//{
		//    get
		//    {
		//        if (Page.Request.Form[hidSelectedWorkflow.UniqueID] != null && Page.Request.Form[hidSelectedWorkflow.UniqueID].Length > 0)
		//            return Convert.ToInt32(Page.Request.Form[hidSelectedWorkflow.UniqueID]);
		//        if (hidSelectedWorkflow.Value.Length > 0)
		//            return Convert.ToInt32(hidSelectedWorkflow.Value);// lstWorkflows.SelectedIndex;
		//        else
		//            return -1;
		//    }
		//    set
		//    {
		//        hidSelectedWorkflow.Value = value.ToString();
		//        ddlWorkflows.SelectedIndex = value;
		//    }
		//}

		//public int SelectedWorkflowStateIndex
		//{
		//    get
		//    {
		//        if (Page.Request.Form[hidSelectedWorkflowState.UniqueID] != null && hidSelectedWorkflowState.Value.Length == 0)
		//            return Convert.ToInt32(Page.Request.Form[hidSelectedWorkflowState.UniqueID]);
		//        if (hidSelectedWorkflowState.Value.Length > 0)
		//            return Convert.ToInt32(hidSelectedWorkflowState.Value);// lstWorkflowStates.SelectedIndex;
		//        else
		//            return -1;
		//    }
		//    set
		//    {
		//        hidSelectedWorkflowState.Value = value.ToString();
		//        lstWorkflowStates.SelectedIndex = value;
		//    }
		//}

		//public int SelectedWorkflowStateFilterIndex
		//{
		//    get
		//    {
		//        return lstWorkflowStateFilters.SelectedIndex;
		//    }
		//    set
		//    {
		//        lstWorkflowStateFilters.SelectedIndex = value;
		//    }
		//}

		public int? SelectedApplicationTypeKey
		{
			get
			{
				int selValue = Int32.Parse(ddlAppTypes.SelectedValue);
				if (selValue > -1)
					return new int?(selValue);
				else
					return new int?();
			}
		}

		/// <summary>
		/// Gets a list of work flow filters selected on the view.  This is a dictionary containing 
		/// a list of items where the key is the IWorkflow ID, and the value is the IState key.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public IDictionary<int, IList<int>> WorkflowFilters
		{
			get
			{
				return _dictWorkflowFilters;
			}
		}

		public IList<int> OrganisationStructureFilters
		{
			get
			{
				return this._lstOrgStructure;
			}
		}

		public IList<int> OrganisationStructureUserFilters
		{
			get
			{
				return this._lstOrgStructureUsers;
			}
		}

		public string ApplicationNo
		{
			get
			{
				return txtApplicationNo.Text;
			}
			set
			{
				txtApplicationNo.Text = value;
			}
		}

		public string IDNumber
		{
			get { return txtIDNumber.Text; }
			set { txtIDNumber.Text = value; }
		}

		public string Firstname
		{
			get { return txtFirstName.Text; }
			set { txtFirstName.Text = value; }
		}

		public string Surname
		{
			get { return txtSurname.Text; }
			set { txtSurname.Text = value; }
		}

		public string ccdCategory
		{
			get { return ccdStates.Category; }
			set { ccdStates.Category = value; }
		}

		public bool IncludeHistoricUsers
		{
			get { return chkHistoricUsers.Checked; }
			set { chkHistoricUsers.Checked = value; }
		}

		public bool SetIncludeHistoricUsersEnabled
		{
			set { chkHistoricUsers.Enabled = value; }
		}

		public bool IncludeMyCreatedCases
		{
			get
			{
				return chkIncludeMyCreatedCases.Checked;
			}
			set
			{
				chkIncludeMyCreatedCases.Checked = value;
			}
		}

		public void BindApplicationTypes(IEventList<IApplicationType> ApplicationTypes)
		{
			ddlAppTypes.Items.Clear();

			List<IApplicationType> sortedList = new List<IApplicationType>();
			foreach (IApplicationType app in ApplicationTypes)
				sortedList.Add(app);
			sortedList.Sort(
			  delegate(IApplicationType c1, IApplicationType c2)
			  {
				  return c1.Description.CompareTo(c2.Description);
			  });


			ddlAppTypes.DataValueField = "Key";
			ddlAppTypes.DataTextField = "Description";
			ddlAppTypes.DataSource = sortedList;
			ddlAppTypes.DataBind();

			ddlAppTypes.Items.Insert(0, new ListItem("Any", "-1"));

            // Add non-offer types to the list
            ddlAppTypes.Items.Insert(1, new ListItem("Debt Counselling", "99"));
            ddlAppTypes.Items.Insert(1, new ListItem("Disability Claim", "100"));
		}

		/// <summary>
		/// Binds the list of workflows to the dropdown list.  This method will do the sorting 
		/// so there is no need to sort the incoming values.
		/// </summary>
		/// <param name="Workflows"></param>
		public void BindWorkflows(SAHL.Common.Collections.Interfaces.IEventList<IWorkFlow> Workflows)
		{
			ddlWorkflows.Items.Clear();

			// sort the workflows by name
			List<IWorkFlow> sortedList = new List<IWorkFlow>();
			foreach (IWorkFlow wf in Workflows)
				sortedList.Add(wf);
			sortedList.Sort(
			  delegate(IWorkFlow c1, IWorkFlow c2)
			  {
				  return c1.Name.CompareTo(c2.Name);
			  });

			for (int i = 0; i < sortedList.Count; i++)
			{
				ddlWorkflows.Items.Add(new ListItem(sortedList[i].Name, sortedList[i].ID.ToString()));
			}
			ddlWorkflows.VerifyPleaseSelect();
		}

		/// <summary>
		/// Binds a list of instances to the search results grid.
		/// </summary>
		/// <param name="instances"></param>
		public void BindSearchResult(IList<IInstance> instances)
		{
			bool limitExceeded = (instances.Count > MaximumRowCount);
			divMaxResultsError.Visible = limitExceeded;
			lblMaxCount.Text = MaximumRowCount.ToString();
			spanResultCount.InnerText = (limitExceeded ? (instances.Count - 1).ToString() : instances.Count.ToString());

			// loop through the instances and create a list of bindable objects - this is necessary
			// otherwise the grid doesn't know what to do when the user groups columns or tries to 
			// sort (many of the data values are as a result of properties of child objects)
			List<WorkflowBindableObject> bindObjects = new List<WorkflowBindableObject>();
			for (int i = 0; i < instances.Count; i++)
			{
				IInstance instance = instances[i];

				// if we've exceeded the maximum allowed amount, exit the loop here - the results will return the 
				// maximum amount + 1 so we know it was exceeded but we don't want to display more than the count 
				// to the user
				if (i == MaximumRowCount)
					break;

				string genericKey = String.Format("<span title=\"Data Error: No associated application for instance {0}\">Error</span>", instance.ID);
				string appTypeDesc = genericKey;
                bool isCapitec = false;

				//TODO: We need to make this more generic as the WorkflowRole is joining straight on to DebtCounselling which is incorrect
				// if this is a CAP Offer then handle differently
				switch (instance.WorkFlow.Name)
				{
					case SAHL.Common.Constants.WorkFlowName.Cap2Offers:
						ICapRepository capRepo = RepositoryFactory.GetRepository<ICapRepository>();
						ICapApplication capApplication = capRepo.GetCapApplicationFromInstance(instance);
						if (capApplication != null)
						{
							genericKey = capApplication.Account.Key.ToString();
							appTypeDesc = "CAP2";
						}
						break;
					case SAHL.Common.Constants.WorkFlowName.DebtCounselling:
						genericKey = instance.Name;
						appTypeDesc = "Debt Counselling";
						break;
                    case SAHL.Common.Constants.WorkFlowName.DisabilityClaim:
                        ILifeRepository lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
                        genericKey = lifeRepo.GetLoanAccountKeyByDisabilityClaimInstanceID(instance.ID).ToString();
                        appTypeDesc = "Disability Claim";
                        break;
					default:
						IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
						IApplication application = appRepo.GetApplicationFromInstance(instance);
						if (application != null)
						{
							genericKey = application.Key.ToString();
							appTypeDesc = application.ApplicationType.Description;
                            isCapitec = application.IsCapitec();
						}
						break;
				}
				// assigned to
				StringBuilder sbAssigned = new StringBuilder();
				foreach (IWorkList wl in instance.WorkLists)
				{
					if (sbAssigned.Length > 0)
						sbAssigned.Append(", ");
					sbAssigned.Append(wl.ADUserName);
				}

				WorkflowBindableObject bo = new WorkflowBindableObject(instance.ID, genericKey, instance.Subject,
					instance.WorkFlow.Name, instance.State.Name, appTypeDesc, sbAssigned.ToString(), isCapitec);

				bindObjects.Add(bo);

			}
			grdResults.DataSource = bindObjects;
			grdResults.DataBind();
			// reset the selections because re-bindig dthe grid doesnt reset the selections
			// this can cause multiple selections to exist
			grdResults.Selection.UnselectAll();
		}

		#endregion

		#region IViewOrganisationStructure Members

		public void BindOrganisationStructure(System.Collections.Generic.List<IBindableTreeItem> OrganisationStructure)
		{
			foreach (IBindableTreeItem f in OrganisationStructure)
			{
				SAHLTreeNode ParentNode = AddTopLevelNode(f);

				// now we have added the parent we can add their kids by recursing back in here.
				if (f.Children.Count > 0)
				{
					BindChildOrgStructure(f.Children, ParentNode);
				}
			}
		}

		public void BindUserFilter(List<IADUser> ADUsers)
		{
			chklstUsers.Items.Clear();
			foreach (IADUser user in ADUsers)
			{
				ListItem AddListItem = new ListItem(user.ADUserName, user.Key.ToString());
				AddListItem.Selected = true;
				chklstUsers.Items.Add(AddListItem);
			}
		}

		private void BindChildOrgStructure(System.Collections.Generic.List<IBindableTreeItem> OrganisationStructure, SAHLTreeNode ParentNode)
		{
			foreach (IBindableTreeItem node in OrganisationStructure)
			{
				string keyPrefix = (node is BindADUser ? "A" : "O");
				SAHLTreeNode Childnode = new SAHLTreeNode(node.Desc, keyPrefix + node.Key.ToString());
				Childnode.AutoPostBack = false;

				ParentNode.Nodes.Add(Childnode);
				if (tvOrgStruct.CheckedValuePaths.Contains(Childnode.ValuePath))
					ExpandNode(Childnode);

				if (node.Children.Count > 0)
				{
					BindChildOrgStructure(node.Children, Childnode);
				}
			}
		}

		private SAHLTreeNode AddTopLevelNode(IBindableTreeItem o)
		{
			SAHLTreeNode tn = new SAHLTreeNode(o.Desc, "O" + o.Key.ToString());
			if (o is BindOrganisationStructure)
				tn.AutoPostBack = false;
			tvOrgStruct.Nodes.Add(tn);
			return tn;
		}

		/// <summary>
		/// Recursively expands a node.
		/// </summary>
		/// <param name="Childnode"></param>
		private static void ExpandNode(SAHLTreeNode Childnode)
		{
			SAHLTreeNode node = Childnode;
			while (node != null)
			{
				node.Expanded = true;
				node = node.ParentNode;
			}
		}

		#endregion

		protected void ddlSearchIn_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Page.Request.Form["__EVENTTARGET"] == ddlSearchIn.UniqueID)
			{
				if (SearchInSelectedIndexChanged != null)
					SearchInSelectedIndexChanged(this, new SAHL.Common.Web.UI.Events.KeyChangedEventArgs(Page.Request.Form[ddlSearchIn.UniqueID].ToString() == "MyCases" ? 0 : 1));
			}
		}

		protected void tv_NodeSelected(object source, SAHL.Common.Web.UI.Events.SAHLTreeNodeEventArgs e)
		{
			if (null != UserSelected)
			{
				int SelectedKey = -1;
				if (e.TreeNode.HasChildren == false)
					SelectedKey = Convert.ToInt32(e.TreeNode.Value);

				UserSelected(source, new KeyChangedEventArgs(SelectedKey));
			}
		}

		protected void btnAddWorkflow_Click(object sender, EventArgs e)
		{
			if (AddWorkflowFilterButtonClicked != null)
				AddWorkflowFilterButtonClicked(sender, e);
		}

		protected void btnRemoveWorkflow_Click(object sender, EventArgs e)
		{
			if (RemoveWorkflowFilterButtonClicked != null)
				RemoveWorkflowFilterButtonClicked(sender, e);
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			if (SaveSearchButtonClicked != null)
				SaveSearchButtonClicked(sender, e);
		}

		protected void btnManage_Click(object sender, EventArgs e)
		{
			if (ManageSearchButtonClicked != null)
				ManageSearchButtonClicked(sender, e);
		}

		protected void btnSearch_Click(object sender, EventArgs e)
		{
			if (SearchButtonClicked != null)
				SearchButtonClicked(sender, e);

			ShowSearchResults = true;
		}

		protected void btnReset_Click(object sender, EventArgs e)
		{
			if (ResetButtonClicked != null)
				ResetButtonClicked(sender, e);
		}

		protected void btnAddUser_Click(object sender, EventArgs e)
		{
			// TODO: Add the selected node to the listbox.
			if (tvOrgStruct.SelectedNode != null)
			{
				chklstUsers.Items.Add(new ListItem(tvOrgStruct.SelectedNode.Text));
			}
			if (AddUserClicked != null)
				AddUserClicked(sender, e);

		}

		protected void grdResults_SelectionChanged(object sender, EventArgs e)
		{
			// check the hidden field to see if the "Results" tab is selected
			int activeTabIndex = Convert.ToInt32(hidActiveTabIndex.Value);

			// only perform the select if we are on the results tab
			if (grdResults.SelectedKeyValue != null && SearchResultsDoubleClick != null && activeTabIndex != 0)
			{
				SearchResultsDoubleClick(sender, new KeyChangedEventArgs(grdResults.SelectedKeyValue));
				ShouldRunPage = false;
			}
		}

        protected void grdResults_Load(object sender, EventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            GridViewDataColumn column = (GridViewDataColumn)gridView.Columns["IsCapitec"];
            column.DataItemTemplate = new SAHL.Web.Controls.CapitecImageTemplate(); 
        }

		#region Search Binding Class

		/// <summary>
		/// Simple class for binding to the grid. 
		/// </summary>
		protected class WorkflowBindableObject
		{
			private long _instanceId;
			private string _appKey;
			private string _appDetails;
			private string _workflow;
			private string _stage;
			private string _appType;
			private string _assignedTo;
            private bool _isCapitec;

			public WorkflowBindableObject(long instanceId, string appKey, string appDetails, string workflow,
				string stage, string appType, string assignedTo, bool isCapitec)
			{
				_instanceId = instanceId;
				_appKey = appKey;
				_appDetails = appDetails;
				_workflow = workflow;
				_stage = stage;
				_appType = appType;
				_assignedTo = assignedTo;
                _isCapitec = isCapitec;
			}

			public long InstanceID
			{
				get { return _instanceId; }
				set { _instanceId = value; }
			}

			public string AppKey
			{
				get { return _appKey; }
				set { _appKey = value; }
			}

			public string AppDetails
			{
				get { return _appDetails; }
				set { _appDetails = value; }
			}

			public string Workflow
			{
				get { return _workflow; }
				set { _workflow = value; }
			}

			public string Stage
			{
				get { return _stage; }
				set { _stage = value; }
			}

			public string AppType
			{
				get { return _appType; }
				set { _appType = value; }
			}


			public string AssignedTo
			{
				get { return _assignedTo; }
				set { _assignedTo = value; }
			}

            public bool IsCapitec
            {
                get { return _isCapitec; }
                set { _isCapitec = value; }
            }
		}

		#endregion

	}
}
