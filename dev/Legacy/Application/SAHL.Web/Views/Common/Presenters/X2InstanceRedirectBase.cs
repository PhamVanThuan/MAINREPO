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
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using System.Text;

using SAHL.Common;

namespace SAHL.Web.Views.Common.Presenters
{
	public class X2InstanceRedirectBase : SAHLCommonBasePresenter<IX2InstanceRedirect>
	{
		private int _instanceID = -1;
		//private int? _genericKey;
        private int? _stateID;
		private IX2Repository _x2Repo;

		public X2InstanceRedirectBase(IX2InstanceRedirect view, SAHLCommonBaseController controller)
			: base(view, controller)
		{

		}

		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);

			// get the instanceID from the global cache
			if (GlobalCacheData.ContainsKey(ViewConstants.InstanceID))
				_instanceID = Convert.ToInt32(GlobalCacheData[ViewConstants.InstanceID]);

			// get the instanceID from the global cache
			if (GlobalCacheData.ContainsKey(ViewConstants.StateID))
				_stateID = Convert.ToInt32(GlobalCacheData[ViewConstants.StateID]);

			// get the generic key from the global cache
			// this is required to find a node by the generic key and set it as selected
            //if (GlobalCacheData.ContainsKey(ViewConstants.GenericKey))
            //    _genericKey = Convert.ToInt32(GlobalCacheData[ViewConstants.GenericKey]);

			if (_x2Repo == null)
				_x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

			if (_instanceID > -1)
			{
				// get the instance 
				IInstance instance = _x2Repo.GetInstanceByKey(_instanceID);

				int attempts = 1;

				if (instance.State.Forms.Count > 0 && !String.IsNullOrEmpty(instance.State.Forms[0].Name))
				{
					NavigateToWFTaskListNode(instance);
				}
				else
				{
					// the worklow activity hasnt yet completed so try again
					if (PrivateCacheData.ContainsKey(ViewConstants.InstanceReloadAttempts))
						attempts = Convert.ToInt32(PrivateCacheData[ViewConstants.InstanceReloadAttempts]);
					else
						PrivateCacheData.Add(ViewConstants.InstanceReloadAttempts, attempts);

					// if we have tried 3 times and it hasnt completed then thwo exception
					if (attempts > 3)
					{
						throw new Exception("X2InstanceRedirect has failed after " + attempts.ToString() + " attempts.");
					}
					else // otherwise increment the attempts count and try again
					{
						attempts++;
						PrivateCacheData.Remove(ViewConstants.InstanceReloadAttempts);
						PrivateCacheData.Add(ViewConstants.InstanceReloadAttempts, attempts);
					}
				}
			}
			else
			{
				throw new Exception("X2InstanceRedirect has no instance set.");
			}
		}

		protected void NavigateToWFTaskListNode(IInstance instance)
		{
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

			// This will be loading a specific node in this case a WorkList
			if (_stateID.HasValue)
			{
				CBOManager.SetCurrentNodeSet(_view.CurrentPrincipal, CBONodeSetType.X2);
				CBOMenuNode selectedNode = CBOManager.SetSelectedNode(_view.CurrentPrincipal, CBONodeSetType.X2, _stateID.Value);
				if (selectedNode != null)
				{
					CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, selectedNode, CBONodeSetType.X2);
					_view.Navigator.Navigate(selectedNode.URL);
				}
			}
			// This will be loading a specific instance from a WorkList
			else if (_instanceID > 0)
			{
				//the name of the relevant x2data table will be the same as the storagetable field from the workflow table
				//the storagekey field in the workflow table is the name of the column in the x2data table that has the actual key
				//forms for a stage are ordered by the formorder column in the stateform table
				SAHL.Common.X2.BusinessModel.Interfaces.IWorkFlow wf = _x2Repo.GetWorkFlowByKey(instance.WorkFlow.ID);
				IDictionary<string, object> dict = X2Service.GetX2DataRow(instance.ID);
				int businessKey = Convert.ToInt32(dict[wf.StorageKey]);

                // setup the instance node description
                string nodeDesc = "", longDesc = "";
                _x2Repo.SetInstanceNodeDescription(wf, instance, businessKey, out nodeDesc, out longDesc);

				InstanceNode iNode = new InstanceNode(businessKey, taskListNode, nodeDesc, longDesc, instance.ID, instance.State.Forms[0].Name);

                CBOManager.AddCBOMenuNode(_view.CurrentPrincipal, taskListNode, iNode, CBONodeSetType.X2);
				CBOManager.SetCurrentNodeSet(_view.CurrentPrincipal, CBONodeSetType.X2);
				CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, iNode, CBONodeSetType.X2);
				_view.Navigator.Navigate(iNode.URL);
			}
		}
	}
}

