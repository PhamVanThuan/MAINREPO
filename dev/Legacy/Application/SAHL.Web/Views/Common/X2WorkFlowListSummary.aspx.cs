using System;
using System.Data;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common;
using SAHL.Common.Web.UI;
using SAHL.Common.Authentication;

using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.UI;
using System.Collections.Generic;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;


namespace SAHL.Web.Views.Common
{
    public partial class X2WorkFlowListSummary : SAHLCommonBaseView, IX2WorkFlowListSummary
    {
        #region Private Variables
        #endregion

        protected void btnGo_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtIID.Text))
                return;

            Int64 IID = Convert.ToInt64(txtIID.Text);
            // go search and add to CBO
            if (IID > 0)
            {
                //add the instance to the tasks node and navigate to the appropriate view
                //Instance.State.StateForms[0].Form.Name is the navigate value
                IX2Service svc = ServiceFactory.GetService<IX2Service>();
                //ICBOService cbo = ServiceFactory.GetService<ICBOService>();
                IX2Repository _repo = RepositoryFactory.GetRepository<IX2Repository>();
                IInstance instance = _repo.GetInstanceByKey(IID);
                List<CBONode> nodes = CBOManager.GetMenuNodes(CurrentPrincipal, CBONodeSetType.X2);

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
                    CBOManager.AddCBOMenuNode(CurrentPrincipal, null, taskListNode, CBONodeSetType.X2);
                }

                //the name of the relevant x2data table will be the same as the storagetable field from the workflow table
                //the storagekey field in the workflow table is the name of the column in the x2data table that has the actual key
                //forms for a stage are ordered by the formorder column in the stateform table
                IWorkFlow wf = _repo.GetWorkFlowByKey(instance.WorkFlow.ID);

                IDictionary<string, object> dict = svc.GetX2DataRow(IID);
                int businessKey = Convert.ToInt32(dict[wf.StorageKey]);
                string longDesc = String.Format("{0} ({1}: {2})", instance.Subject, wf.StorageKey, businessKey);
                InstanceNode iNode = new InstanceNode(businessKey, taskListNode, instance.Name, longDesc, instance.ID, instance.State.Forms[0].Name);
                CBOManager.AddCBOMenuNode(CurrentPrincipal, taskListNode, iNode, CBONodeSetType.X2);
                CBOManager.SetCurrentCBONode(CurrentPrincipal, iNode, CBONodeSetType.X2);
                Navigator.Navigate(iNode.URL);
            }
        }

    }
}
