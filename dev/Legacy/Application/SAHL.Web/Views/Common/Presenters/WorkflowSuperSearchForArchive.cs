using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.Globals;
using System.Collections.Generic;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Web.Views.Common.Presenters
{
	public class WorkflowSuperSearchForArchive : WorkflowSuperSearch
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public WorkflowSuperSearchForArchive(IWorkflowSuperSearch view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);
			if (!_view.ShouldRunPage)
				return;

			BindArchiveWorkflows();

			_view.OrganisationalStructureFilterVisible = false;
			_view.MyCreatedCasesFilterVisible = false;
		}

		protected override void OnViewPreRender(object sender, EventArgs e)
		{
			base.OnViewPreRender(sender, e);
			if (!_view.ShouldRunPage)
				return;

			_view.SelectedSearchType = 1;
			_view.SetSelectedSearchTypeEnabled = false;
			_view.IncludeHistoricUsers = false;
			_view.SetIncludeHistoricUsersEnabled = false;

			_view.ccdCategory = "Archive";
		}

		protected override void _view_SearchInSelectedIndexChanged(object sender, KeyChangedEventArgs e)
		{
			// if it has changed we must refresh the data so clear it here
			_workflowData.Clear();
			BindArchiveWorkflows();
		}

		private void BindArchiveWorkflows()
		{
			_workflowData.Clear();
			IEventList<IWorkFlow> Workflows = null;
			IX2Repository X2R = RepositoryFactory.GetRepository<IX2Repository>();

			string archiveWorkflowsSetting = System.Configuration.ConfigurationManager.AppSettings.Get("ArchiveSuperSearchWorkflows");

			if (!string.IsNullOrEmpty(archiveWorkflowsSetting))
			{
				Workflows = X2R.GetArchiveSearchWorkflows(archiveWorkflowsSetting);
			}

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

			// bind the workflows
			_workflows = new EventList<IWorkFlow>();
			foreach (KeyValuePair<IWorkFlow, IEventList<IState>> w in _workflowData)
				_workflows.Add(_view.Messages, w.Key);

			if (_workflows.Count > 0)
			{
				_workflowStates = _workflows[0].States;
				base.PrivateCacheData[workflowStates] = _workflowStates;
			}

			_view.BindWorkflows(_workflows);
		}

		protected override void _view_SearchButtonClicked(object sender, EventArgs e)
		{
			IX2Repository X2 = RepositoryFactory.GetRepository<IX2Repository>();
			WorkflowSearchCriteria WSC = new WorkflowSearchCriteria();

			WSC.ApplicationNumber = _view.ApplicationNo;
			WSC.Firstname = _view.Firstname;
			WSC.Surname = _view.Surname;
			WSC.IDNumber = _view.IDNumber;
			WSC.MaxResults = _view.MaximumRowCount + 1; // make it one more so we know it's been exceeded

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

			IList<IInstance> Results = X2.WorkflowArchiveSuperSearch(WSC);

			_view.BindSearchResult(Results);

			base.PrivateCacheData.Add("WorkFlowSearchResults", Results);
		}

		protected override void _view_SearchResultsDoubleClick(object sender, KeyChangedEventArgs e)
		{
			if (e == null)
				return;

			long instanceId = Convert.ToInt32(e.Key);

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
			IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
			IApplication application = appRepo.GetApplicationFromInstance(instance);
			if (application == null)
				throw new Exception(String.Format("There is a problem with the X2 instance {0}: there is no associated application.  Please report this error to HelpDesk.", instance.ID));

			nodeDesc = "Loan: " + application.ReservedAccount.Key + " (" + wf.Name + ")";
			longDesc = String.Format("{0} ({1}: {2})", instance.Subject, "Stage", instance.State.Name);

			InstanceNode iNode = new InstanceNode(businessKey, taskListNode, nodeDesc, longDesc, instance.ID, "Orig_ApplicationSummaryRedirect");

			CBOManager.AddCBOMenuNode(_view.CurrentPrincipal, taskListNode, iNode, CBONodeSetType.X2);
			CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, iNode, CBONodeSetType.X2);
			_view.Navigator.Navigate("Orig_ApplicationSummaryRedirect");
		}
	}
}
