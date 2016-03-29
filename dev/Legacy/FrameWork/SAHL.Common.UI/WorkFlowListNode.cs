using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using System.Security.Principal;
using SAHL.Common.X2.BusinessModel;

using SAHL.Common.CacheData;
using System.Data;
using SAHL.Common.DataAccess;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Common.UI
{
    public class WorkFlowListNode : CBOWorkflowNode
    {
        public WorkFlowListNode(CBONode Parent)
            : base(0, Parent, "WorkFlows", "")
        {
            _url = "X2WorkFlowListSummary";
            _menuIcon = "Workflow.gif";
            _isRemovable = false;
            //base._cboUniqueKey = "WORKFLOWS";
        }

        public override void GetContextNodes(SAHLPrincipal principal, List<CBONode> contextNodes, IDictionary<int, object> allowedContextMenus)
        {
            // Add the workflow batch reassign contextmenu node
            AddStaticContextMenuNodes(contextNodes);
        }

        //public override List<CBONode> ChildNodes
        //{
        //    get
        //    {
        //        if (this._childNodes == null)
        //        {
        //            this._childNodes = new List<CBONode>();

        //            //SAHLPrincipal principal = SAHLPrincipal.GetCurrent();
        //            //SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
        //            //string groups = spc.GetCachedRolesAsStringForQuery( true, true);
        //            //IDbConnection conn = null;
        //            //IDataReader reader = null;

        //            //// using a query to get everything at once - this gets hit with EVERY page load so we want this 
        //            //// to be as quick as possible!!
        //            //try
        //            //{
        //            //    string sql = UIStatementRepository.GetStatement("COMMON", "GetWorkflowStateSummaryByUser");

        //            //    conn = Helper.GetSQLDBConnection();
        //            //    conn.Open();
        //            //    reader = Helper.ExecuteReader(conn, String.Format(sql, groups));
        //            //    WorkFlowNode wfnCurrent = null;

        //            //    while (reader.Read())
        //            //    {
        //            //        int workflowID = reader.GetInt32(0);
        //            //        string workflowName = reader.GetString(1);
        //            //        int stateID = reader.GetInt32(2);
        //            //        string stateName = reader.GetString(3);
        //            //        int instanceCount = reader.GetInt32(4);

        //            //        // if the workflow description, we've encountered a new set of states under a new 
        //            //        // workflow, so create a new node
        //            //        if (wfnCurrent == null || wfnCurrent.Description != workflowName)
        //            //        {
        //            //            wfnCurrent = new WorkFlowNode(workflowID, this, workflowName, workflowName);
        //            //            _childNodes.Add(wfnCurrent);
        //            //        }

        //            //        // add the state to the current workflow node
        //            //        string finalName = string.Format("{0} ({1})", stateName, instanceCount);
        //            //        StateNode stateNode = new StateNode(stateID, wfnCurrent, finalName, "");
        //            //        wfnCurrent.ChildNodes.Add(stateNode);
        //            //    }
        //            //}
        //            //finally
        //            //{
        //            //    if (reader != null)
        //            //        reader.Dispose();
        //            //    if (conn != null)
        //            //        conn.Dispose();
        //            //}


        //        }
        //        /*

        //        ICBOService CBO = ServiceFactory.GetService<ICBOService>();
        //        DomainMessageCollection Messages = new DomainMessageCollection();
        //        while(this._childNodes.Count > 0)
        //            this._childNodes.RemoveAt(Messages, 0);
        //        SAHLPrincipal principal = new SAHLPrincipal(WindowsIdentity.GetCurrent());

        //        string selectedKey = null;
        //        SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
        //        if (spc.NodeSets[MenuNodeSet.X2].SelectedNode != null)
        //        {
        //            selectedKey = spc.NodeSets[MenuNodeSet.X2].SelectedNode.CBOUniqueKey;
        //        }

        //        IEventList<IInstance> instances = Instance.FindByPrincipal(principal);

        //        Dictionary<string, int> workflows = new Dictionary<string, int>();

        //        //first get the workflows with max ID's:
        //        for (int i = instances.Count - 1; i > -1; i--)
        //        {
        //            IWorkFlow wf = instances[i].WorkFlow as IWorkFlow;
        //            if (workflows.ContainsKey(wf.Name))
        //            {
        //                int id = workflows[wf.Name];
        //                if (wf.ID < id)
        //                    instances.RemoveAt(Messages, i);
        //                else
        //                    workflows[wf.Name] = wf.ID;
        //            }
        //            else
        //                workflows.Add(wf.Name, wf.ID);
        //        }

        //        string[] keyArr = new string[workflows.Keys.Count];
        //        workflows.Keys.CopyTo(keyArr, 0);
        //        List<string> keys = new List<string>(keyArr);

        //        //for each workflow, find the instances and states that belong to it.
        //        for (int i = 0; i < keys.Count; i++)
        //        {
        //            int id = workflows[keys[i]];
        //            WorkFlowNode wfNode = new WorkFlowNode(id, this, keys[i], keys[i]);

        //            if (wfNode.CBOUniqueKey == selectedKey)
        //                CBO.SetCurrentCBONode(principal, wfNode, MenuNodeSet.X2);

        //            List<string> stateNames = new List<string>();
        //            List<int> stateIDs = new List<int>();
        //            List<int> stateCount = new List<int>();

        //            for (int k = instances.Count - 1; k > -1; k--)
        //            {
        //                IWorkFlow wf = instances[k].WorkFlow;

        //                if (wf.Name == keys[i])
        //                {
        //                    if (wf.ID == id) //bingo!
        //                    {
        //                        //ok there could be many states, we need to know how many of each
        //                        IState st = instances[k].State;
        //                        int idx = stateNames.IndexOf(st.Name);

        //                        int insertPoint = stateIDs.Count;

        //                        if (idx > -1)
        //                        {
        //                            stateCount[idx] = stateCount[idx] + 1;
        //                        }
        //                        else
        //                        {
        //                            for (int j = 0; j < stateIDs.Count; j++)
        //                            {
        //                                if (stateIDs[j] > st.ID)
        //                                {
        //                                    insertPoint = j;
        //                                    break;
        //                                }
        //                            }

        //                            stateIDs.Insert(insertPoint, st.ID);
        //                            stateNames.Insert(insertPoint, st.Name);
        //                            stateCount.Insert(insertPoint, 1);
        //                        }
        //                    }

        //                    instances.RemoveAt(Messages, k);
        //                }
        //            }

        //            //now add all state nodes to wfNode
        //            for (int k = 0; k < stateNames.Count; k++)
        //            {
        //                string finalName = string.Format("{0}({1})", stateNames[k], stateCount[k]);
        //                StateNode stateNode = new StateNode(stateIDs[k], wfNode, finalName, "");
        //                wfNode.ChildNodes.Add(Messages, stateNode);

        //                if (stateNode.CBOUniqueKey == selectedKey)
        //                    CBO.SetCurrentCBONode(principal, stateNode, MenuNodeSet.X2);
        //            }

        //            _childNodes.Add(Messages, wfNode);
        //        }
        //        */

        //        return _childNodes;
        //    }
        //}
    }
}
