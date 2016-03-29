using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI.CBOSecurityFilters;
using SAHL.Common.UI.Configuration;
using SAHL.Common.X2.BusinessModel.DAO;

namespace SAHL.Common.UI
{
    public class CBOManager
    {
        public CBOManager()
        {
            CreateNodeSets();
        }

        public void CreateNodeSets()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            if (!spc.NodeSets.ContainsKey(CBONodeSetType.CBO))
                spc.NodeSets.Add(CBONodeSetType.CBO, new CBONodeSet(CBONodeSetType.CBO));

            if (!spc.NodeSets.ContainsKey(CBONodeSetType.X2))
                spc.NodeSets.Add(CBONodeSetType.X2, new CBONodeSet(CBONodeSetType.X2));
        }

        /// <summary>
        ///
        /// </summary>
        public List<CBONode> GetMenuNodes(SAHLPrincipal principal, CBONodeSetType nodeSetName)
        {
            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = (CBONodeSet)spc.NodeSets[nodeSetName];

            switch (nodeSetName)
            {
                case CBONodeSetType.CBO:

                    if (spc.CBOMenus.Count == 0)
                    {
                        ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
                        // filter cbomenu's based on our principal's set of feature keys
                        foreach (ICBOMenu menuItem in lookups.CBOMenus)
                        {
                            if (menuItem.Feature != null && !spc.CBOMenus.ContainsKey(menuItem.Key) && spc.FeatureKeys.Contains(menuItem.Feature.Key))
                                spc.CBOMenus.Add(menuItem.Key, menuItem);
                        }
                        // filter contextmenu's
                        foreach (IContextMenu contextMenu in lookups.ContextMenus)
                        {
                            if (contextMenu.Feature != null && !spc.ContextMenus.ContainsKey(contextMenu.Key) && spc.FeatureKeys.Contains(contextMenu.Feature.Key))
                                spc.ContextMenus.Add(contextMenu.Key, contextMenu);
                        }
                    }

                    if (nodeSet.Nodes.Count == 0)
                        BuildPrincipalNodes(spc);

                    break;

                case CBONodeSetType.X2:

                    RefreshWorkflowNodes(principal);
                    break;
            }

            return nodeSet.Nodes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="nodeSetName"></param>
        /// <returns></returns>
        public List<CBONode> GetContextNodes(SAHLPrincipal principal, CBONodeSetType nodeSetName)
        {
            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = (CBONodeSet)spc.NodeSets[nodeSetName];

            if (nodeSet.ContextNodes.Count == 0 && nodeSet.SelectedNode != null)
            {
                nodeSet.SelectedNode.GetContextNodes(principal, nodeSet.ContextNodes, spc.ContextMenus);

                // for now we are only interested in filtering CBO context nodes, we can apply this to workflownodes if required.
                IList<ICBOSecurityFilter> Filters = CheckForWorkflowFilter(nodeSet.SelectedNode);

                if (Filters != null)
                {
                    foreach (ICBOSecurityFilter Filter in Filters)
                    {
                        Filter.FilterContextNodes(nodeSet.ContextNodes);
                    }
                }
            }

            return nodeSet.ContextNodes;
        }

        private void BuildPrincipalNodes(SAHLPrincipalCache spc)
        {
            CBONodeSet cboSet = (CBONodeSet)spc.NodeSets[CBONodeSetType.CBO];
            IEventList<ICBOMenu> TopLevelMenus = GetRootMenus(spc.CBOMenus);
            BuildStaticCBONodes(cboSet.Nodes, TopLevelMenus, null, spc);
        }

        private void BuildStaticCBONodes(List<CBONode> NodeList, IEventList<ICBOMenu> CBOMenuList, CBOMenuNode Parent, SAHLPrincipalCache spc)
        {
            for (int i = 0; i < CBOMenuList.Count; i++)
            {
                if (CBOMenuList[i].NodeType == 'S')
                {
                    List<CBOMenuNode> Nodes = BuildCBONode(CBOMenuList[i], Parent, -1, spc);

                    if (Nodes != null)
                    {
                        foreach (CBOMenuNode Node in Nodes)
                        {
                            NodeList.Add(Node);
                            BuildStaticCBONodes(Node.ChildNodes, CBOMenuList[i].ChildMenus, Node, spc);
                        }
                    }
                }
            }
        }

        private void BuildDynamicCBONodes(List<CBONode> NodeList, IEventList<ICBOMenu> CBOMenuList, CBOMenuNode Parent, Int64 GenericKey, SAHLPrincipalCache spc)
        {
            foreach (ICBOMenu cboMenu in CBOMenuList)
            {
                List<CBOMenuNode> Nodes = BuildCBONode(cboMenu, Parent, GenericKey, spc);
                if (Nodes != null)
                {
                    foreach (CBOMenuNode Node in Nodes)
                    {
                        NodeList.Add(Node);
                        if (cboMenu.ChildMenus.Count > 0)
                        {
                            BuildDynamicCBONodes(Node.ChildNodes, cboMenu.ChildMenus, Node, -1, spc);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Builds a CBO Node by dynamically creating the appropriate node class.
        /// </summary>
        /// <param name="Menu">The CoreBusinessObjectMenu to base the CBONode on.</param>
        /// <param name="ParentNode">The parent node.</param>
        /// <param name="GenericKey">The generickey used to store the business data key for the node.</param>
        /// <param name="spc"></param>
        /// <returns></returns>
        public List<CBOMenuNode> BuildCBONode(ICBOMenu Menu, CBOMenuNode ParentNode, Int64 GenericKey, SAHLPrincipalCache spc)
        {
            // check feature access - if no access then return null
            IDictionary<int, object> allowedMenus = spc.CBOMenus; // (IDictionary<int, ICBOMenu>)principal.GetCachedItem(SAHLPrincipalCacheItems.CBOMenus);

            if (Menu.Feature != null && !allowedMenus.ContainsKey(Menu.Key))
                return null;

            List<Dictionary<string, object>> NodeDataList = new List<Dictionary<string, object>>();

            // get the CBOMenus configuration section
            CBOSection CBOConfig = ConfigurationManager.GetSection("CBOMenus") as CBOSection;
            CBOMenuElement CBOElem = null;

            // check if we have specified a different CBONode class for this CBOMenu
            if (CBOConfig != null)
                CBOElem = FindNodeElement(CBOConfig.CBOMenus, Menu);

            // use the base CBONode class by default and change it only if we have specified a different CBONode
            Type type = typeof(CBOMenuNode);// "SAHL.Common.BusinessModel.Interfaces.UI.CBOMenuNode, SAHL.Common.BusinessModel.Interfaces";

            if (CBOElem != null)
                type = Type.GetType(CBOElem.NodeClass);

            Dictionary<string, object> NodeData = new Dictionary<string, object>();
            // add the basic node data
            NodeData.Add("CBOMENU", Menu);
            NodeData.Add("PARENTNODE", ParentNode);

            // Is it a static CBOMenu
            if (string.IsNullOrEmpty(Menu.StatementNameKey))
            {
                // if its static we get the data from the CBOMenu
                NodeData.Add("GENERICKEY", -1);
                NodeData.Add("DESCRIPTION", Menu.Description);
                NodeData.Add("LONGDESCRIPTION", Menu.Description);
                if (Menu.NodeType != 'D')
                    NodeDataList.Add(NodeData);
            }
            // or a dynamic CBOMenu
            else
            {
                // else if its dynamic we need to execute the query
                // get the statement and execute to get the data
                // fill the nodedata from the results
                using (IDbConnection conn = Helper.GetSQLDBConnection())
                {
                    ParameterCollection Params = new ParameterCollection();

                    if (GetStatementParameters(Menu, ParentNode, Params, GenericKey))
                    {
                        DataTable DT = new DataTable();
                        string Query = UIStatementRepository.GetStatement("COMMON", Menu.StatementNameKey);
                        // if the query expects origination sources we need to build a string of this principals
                        // origination sources.
                        if (Menu.HasOriginationSource)
                        {
                            string OS = spc.OriginationSourceKeysStringForQuery;
                            Query = string.Format(Query, OS);
                        }
                        Helper.FillFromQuery(DT, Query, conn, Params);
                        if (DT.Rows.Count >= 1) // Changed from '=' to '>=' by CraigF 23/10/2007
                        {
                            for (int row = 0; row < DT.Rows.Count; row++)
                            {
                                Dictionary<string, object> ND = new Dictionary<string, object>();
                                foreach (KeyValuePair<string, object> var in NodeData)
                                {
                                    ND.Add(var.Key, var.Value);
                                }
                                for (int i = 0; i < DT.Columns.Count; i++)
                                {
                                    string CName = DT.Columns[i].Caption.ToUpper();
                                    ND.Add(CName, DT.Rows[row][i]);
                                }

                                NodeDataList.Add(ND);
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            // dynamically create a class and populate
            List<CBOMenuNode> Nodes = new List<CBOMenuNode>();
            for (int i = 0; i < NodeDataList.Count; i++)
            {
                CBOMenuNode Node = Activator.CreateInstance(type, new object[] { NodeDataList[i] }) as CBOMenuNode;
                Nodes.Add(Node);
            }
            return Nodes;
        }

        #region NodeBuilder methods used by BuildCBONode

        /// <summary>
        /// Get the statement parameters from the input generic types linked to the CBOMenu.
        /// </summary>
        /// <param name="Menu"></param>
        /// <param name="ParentNode"></param>
        /// <param name="Params"></param>
        /// <param name="GenericKeyData"></param>
        /// <returns></returns>
        private bool GetStatementParameters(ICBOMenu Menu, CBOMenuNode ParentNode, ParameterCollection Params, Int64 GenericKeyData)
        {
            for (int i = 0; i < Menu.InputGenericTypes.Count; i++)
            {
                if (GenericKeyData != -1)
                {
                    if (!GetGenericType(Params, Menu.InputGenericTypes[i].GenericKeyTypeParameter.GenericKeyType.Key, GenericKeyData, Menu.InputGenericTypes[i].GenericKeyTypeParameter.ParameterName))
                    {
                        return false;
                    }
                    GetParentGenericType(Params, Menu.InputGenericTypes[i].GenericKeyTypeParameter.GenericKeyType.Key, ParentNode, Menu.InputGenericTypes[i].GenericKeyTypeParameter.ParameterName);
                }
                else
                    if (!GetParentGenericType(Params, Menu.InputGenericTypes[i].GenericKeyTypeParameter.GenericKeyType.Key, ParentNode, Menu.InputGenericTypes[i].GenericKeyTypeParameter.ParameterName))
                    {
                        return false;
                    }
            }
            return true;
        }

        /// <summary>
        /// Iterates up the CBONode hierarchy getting values for the required statement parameters along the way.
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="GenericKeyType"></param>
        /// <param name="Node"></param>
        /// <param name="ParamName"></param>
        /// <returns></returns>
        private bool GetParentGenericType(ParameterCollection Params, int GenericKeyType, CBOMenuNode Node, string ParamName)
        {
            if (Node.GenericKeyTypeKey == GenericKeyType)
            {
                SqlParameter SP = new SqlParameter(ParamName, Node.GenericKey);

                bool exists = false;
                foreach (SqlParameter p in Params)
                {
                    if (p.ParameterName == SP.ParameterName)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                    Params.Add(SP);

                return true;
            }
            else
                if (Node.ParentNode != null)
                {
                    return GetParentGenericType(Params, GenericKeyType, Node.ParentNode as CBOMenuNode, ParamName);
                }
                else
                {
                    return false;
                }
        }

        /// <summary>
        /// Gets the statement parameter for the generickey.
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="GenericKeyType"></param>
        /// <param name="KeyValue"></param>
        /// <param name="ParamName"></param>
        /// <returns></returns>
        private bool GetGenericType(ParameterCollection Params, int GenericKeyType, Int64 KeyValue, string ParamName)
        {
            SqlParameter SP = new SqlParameter(ParamName, KeyValue);

            bool exists = false;
            foreach (SqlParameter p in Params)
            {
                if (p.ParameterName == SP.ParameterName)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
                Params.Add(SP);

            return true;
        }

        /// <summary>
        /// Iterates the CBOMenus configuration section to determine whether the CBOMenu has a specified CBONode class.
        /// </summary>
        /// <param name="Elements"></param>
        /// <param name="Menu"></param>
        /// <returns></returns>
        private CBOMenuElement FindNodeElement(CBOElementCollection Elements, ICBOMenu Menu)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i].CBOKey == Menu.Key)
                    return Elements[i];
            }
            return null;
        }

        #endregion NodeBuilder methods used by BuildCBONode

        /// <summary>
        /// rebuilds the workflow part of the tree.
        /// </summary>
        /// <param name="principal"></param>
        public void RefreshWorkflowNodes(SAHLPrincipal principal)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet x2Set = (CBONodeSet)spc.NodeSets[CBONodeSetType.X2];

            if (x2Set.Nodes.Count == 0)
            {
                x2Set.Nodes.Add(new WorkFlowListNode(null));
                x2Set.Nodes.Add(new TaskListNode(null));
            }

            List<CBONode> nodes = x2Set.Nodes;
            ILookupRepository repo = RepositoryFactory.GetRepository<ILookupRepository>();

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] is WorkFlowListNode)
                {
                    BuildWorkflowNodes(spc, nodes[i] as WorkFlowListNode);
                }
                else if (nodes[i] is TaskListNode)
                {
                    foreach (InstanceNode node in nodes[i].ChildNodes)
                    {
                        node.Refresh();
                        string key = node.WorkflowMenuWildKey;

                        if (!repo.WorkflowMenus.ContainsKey(key))
                            key = node.WorkflowMenuKey;

                        // if WorkflowMenus does not contain our key, clear the nodes.
                        if (!repo.WorkflowMenus.ContainsKey(key))
                        {
                            node.ChildNodes.Clear();
                        }
                        else if (node.IsDirty) //the key has changed (usually when the state changes), or a view has set it.
                        {
                            node.ChildNodes.Clear();

                            IWorkflowMenu wfm = repo.WorkflowMenus[key];
                            //List<CBONode> childNodes = new List<CBONode>();
                            AddCBOMenuNodesFromTemplate(spc, wfm.CoreBusinessObjectMenu, node);

                            //for (int k = 0; k < childNodes.Count; k++)
                            //    node.ChildNodes.Add(childNodes[k]);

                            node.IsDirty = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="nodeSetName"></param>
        /// <param name="genericKey"></param>
        /// <returns></returns>
        public CBOMenuNode SetSelectedNode(SAHLPrincipal principal, CBONodeSetType nodeSetName, int genericKey)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = (CBONodeSet)spc.NodeSets[nodeSetName];

            if (nodeSet.Nodes.Count < 1)
                return null;

            CBOMenuNode selectedNode = FindNodeByGenericKey(nodeSet.Nodes, genericKey) as CBOMenuNode;

            if (selectedNode == null)
                selectedNode = nodeSet.Nodes[0] as CBOMenuNode;

            SetCurrentCBONode(principal, selectedNode, nodeSetName);
            return selectedNode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="nodeSetName"></param>
        /// <param name="genericKey"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public CBOMenuNode SetSelectedNodeByKeyAndDescription(SAHLPrincipal principal, CBONodeSetType nodeSetName, int genericKey, string description)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = (CBONodeSet)spc.NodeSets[nodeSetName];

            if (nodeSet.Nodes.Count < 1)
                return null;

            CBOMenuNode selectedNode = FindNodeByGenericKeyAndDescription(nodeSet.Nodes, genericKey, description) as CBOMenuNode;

            if (selectedNode == null)
                selectedNode = nodeSet.Nodes[0] as CBOMenuNode;

            SetCurrentCBONode(principal, selectedNode, nodeSetName);
            return selectedNode;
        }

        /// <summary>
        /// Selects a default node if selectednode is null, else reselects the node based on it's path
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="nodeSetName"></param>
        /// <returns></returns>
        public CBOMenuNode UpdateNodeSelection(SAHLPrincipal principal, CBONodeSetType nodeSetName)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = (CBONodeSet)spc.NodeSets[nodeSetName];

            if (nodeSet.Nodes.Count < 1)
                return null;

            CBOMenuNode selectedNode = null;
            string selectedNodePath = null;

            if (nodeSet.SelectedNode != null)
            {
                selectedNodePath = nodeSet.SelectedNode.NodePath;

                if (!string.IsNullOrEmpty(selectedNodePath))
                    selectedNode = FindNodeByPath(nodeSet.Nodes, selectedNodePath) as CBOMenuNode;

                if (selectedNode == null) //its possible that the node description changed
                {
                    selectedNode = FindNodeByGenericKey(nodeSet.Nodes, nodeSet.SelectedNode.GenericKey) as CBOMenuNode;
                }
            }

            if (selectedNode == null)
                selectedNode = nodeSet.Nodes[0] as CBOMenuNode;

            SetCurrentCBONode(principal, selectedNode, nodeSetName);
            return selectedNode;
        }

        /// <summary>
        /// Reselects the contextnode based on nodepath, if null and the selectednode is an instancenode, select the first form
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="nodeSetName"></param>
        /// <returns></returns>
        public CBOContextNode UpdateContextNodeSelection(SAHLPrincipal principal, CBONodeSetType nodeSetName)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = (CBONodeSet)spc.NodeSets[nodeSetName];

            if (nodeSet.SelectedNode == null || nodeSet.Nodes.Count < 1)
                return null;

            CBOContextNode selectedContextNode = null;
            string selectedContextPath = null;

            if (nodeSet.SelectedContextNode != null)
                selectedContextPath = nodeSet.SelectedContextNode.NodePath;

            List<CBONode> contextNodes = new List<CBONode>();
            nodeSet.SelectedNode.GetContextNodes(principal, contextNodes, spc.ContextMenus);

            if (contextNodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(selectedContextPath))
                    selectedContextNode = FindNodeByPath(contextNodes, selectedContextPath) as CBOContextNode;

                if (selectedContextNode == null)
                {
                    if (nodeSetName == CBONodeSetType.X2 && nodeSet.SelectedNode is InstanceNode)
                    {
                        for (int i = contextNodes.Count - 1; i > -1; i--)
                        {
                            if (contextNodes[i].Description == "Forms" && contextNodes[i].ChildNodes.Count > 0)
                            {
                                selectedContextNode = contextNodes[i].ChildNodes[0] as CBOContextNode;
                                break;
                            }
                        }
                    }
                    else
                    {
                        selectedContextNode = contextNodes[0] as CBOContextNode;
                    }
                }
            }

            SetCurrentContextNode(principal, selectedContextNode, nodeSetName);
            return selectedContextNode;
        }

        //private void BuildStaticWorkflowNodes(CBONodeSet x2Set)
        //{
        //    WorkFlowListNode workflowsNode = new WorkFlowListNode(null);
        //    TaskListNode tasksNode = new TaskListNode(null);
        //    x2Set.Nodes.Add(workflowsNode);
        //    x2Set.Nodes.Add(tasksNode);
        //}

        private void BuildWorkflowNodes(SAHLPrincipalCache spc, WorkFlowListNode wflNode)
        {
            //need to clear the nodes first!
            while (wflNode.ChildNodes.Count > 0)
                wflNode.ChildNodes.RemoveAt(0);

            using (IDbConnection conn = Helper.GetSQLDBConnection())
            {
                string sql = UIStatementRepository.GetStatement("COMMON", "GetWorkflowStateSummaryByUser");
                string groups = spc.GetCachedRolesAsStringForQuery(true, true);

                conn.Open();

                using (IDataReader reader = Helper.ExecuteReader(conn, String.Format(sql, groups)))
                {
                    WorkFlowNode wfnCurrent = null;

                    while (reader.Read())
                    {
                        int workflowID = reader.GetInt32(0);
                        string workflowName = reader.GetString(1);
                        int stateID = reader.GetInt32(2);
                        string stateName = reader.GetString(3);
                        int instanceCount = reader.GetInt32(4);

                        // if the workflow description, we've encountered a new set of states under a new
                        // workflow, so create a new node
                        if (wfnCurrent == null || wfnCurrent.Description != workflowName)
                        {
                            wfnCurrent = new WorkFlowNode(workflowID, wflNode, workflowName, workflowName);
                            wflNode.ChildNodes.Add(wfnCurrent);
                        }

                        // add the state to the current workflow node
                        string finalName = string.Format("{0} ({1})", stateName, instanceCount);
                        StateNode stateNode = new StateNode(stateID, wfnCurrent, finalName, "");
                        wfnCurrent.ChildNodes.Add(stateNode);
                    }
                }
            }
        }

        public static WorkFlowListNode GeWorkFlowListNode(SAHLPrincipal principal)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet x2Set = (CBONodeSet)spc.NodeSets[CBONodeSetType.X2];
            List<CBONode> nodes = x2Set.Nodes;

            WorkFlowListNode workflowNode = null;

            for (int i = 0; i < nodes.Count; i++)
            {
                workflowNode = nodes[i] as WorkFlowListNode;

                if (workflowNode != null)
                    return workflowNode;
            }

            return null;
        }

        public static TaskListNode GetTaskListNode(SAHLPrincipal principal)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet x2Set = (CBONodeSet)spc.NodeSets[CBONodeSetType.X2];
            List<CBONode> nodes = x2Set.Nodes;

            TaskListNode taskListNode = null;

            for (int i = 0; i < nodes.Count; i++)
            {
                taskListNode = nodes[i] as TaskListNode;

                if (taskListNode != null)
                    return taskListNode;
            }

            return null;
        }


        public void AddCBOMenuNode(SAHLPrincipal principal, CBONode ParentNode, CBONode NewNode, CBONodeSetType nodeSetName)
        {
            if (NewNode == null)
                return;

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = spc.NodeSets[nodeSetName] as CBONodeSet;

            List<CBONode> Nodes = nodeSet.Nodes;

            bool found = false;

            if (ParentNode == null)
            {
                for (int i = 0; i < Nodes.Count; i++)
                {
                    if (Nodes[i].NodePath == NewNode.NodePath)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    Nodes.Add(NewNode);
            }
            else
            {
                for (int i = 0; i < ParentNode.ChildNodes.Count; i++)
                {
                    if (ParentNode.ChildNodes[i].NodePath == NewNode.NodePath)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    ParentNode.ChildNodes.Add(NewNode);
            }

            spc.MenuVersion++;
        }


        public void AddCBOMenuNodeToSelection(SAHLPrincipal principal, ICBOMenu MenuTemplate, long GenericKey, CBONodeSetType nodeSetName)
        {
            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = spc.NodeSets[nodeSetName] as CBONodeSet;

            List<CBONode> Nodes = nodeSet.Nodes;

            if (nodeSet.SelectedNode != null)
                Nodes = nodeSet.SelectedNode.ChildNodes;

            // we need to check whether this generickey is not already available in the tree, if it is
            // we just set it to be the selected node
            CBONode NodeExists = CheckNodeGenericKeyExists(GenericKey, Nodes);

            if (NodeExists != null)
                nodeSet.SelectedNode = NodeExists as CBOMenuNode;
            else
            {
                // A node doesn't exist for the generickey, so build the node
                BuildDynamicCBONodes(Nodes, new EventList<ICBOMenu>(new ICBOMenu[] { MenuTemplate }), nodeSet.SelectedNode, GenericKey, spc);
                //if (nodeSet.SelectedNode.ChildNodes.Count > 0)
                //    SetCurrentCBONode(principal, nodeSet.SelectedNode.ChildNodes[0] as CBOMenuNode, nodeSetName);
            }
            spc.MenuVersion++;
        }

        public void AddCBOMenuToNode(SAHLPrincipal principal, CBOMenuNode ParentNode, ICBOMenu MenuTemplate, Int64 GenericKey, SAHL.Common.Globals.GenericKeyTypes GenericKeyType, CBONodeSetType nodeSetName)
        {
            AddCBOMenuToNode(principal, ParentNode, MenuTemplate, GenericKey, GenericKeyType, nodeSetName, false);
        }

        /// <summary>
        ///
        /// </summary>
        public void AddCBOMenuToNode(SAHLPrincipal principal, CBOMenuNode ParentNode, ICBOMenu MenuTemplate, Int64 GenericKey, SAHL.Common.Globals.GenericKeyTypes GenericKeyType, CBONodeSetType nodeSetName, bool setCurrentCBONode)
        {
            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);

            if (ParentNode == null)
                return;

            List<CBONode> Nodes = ParentNode.ChildNodes;

            // we need to check whether this generickey is not already available in the tree, if it is
            // we just set it to be the selected node
            CBONode NodeExists = CheckNodeGenericKeyExists(GenericKey, (int)GenericKeyType, Nodes);

            if (NodeExists != null)
                ParentNode = NodeExists as CBOMenuNode;
            else
            {
                // A node doesn't exist for the generickey, so build the node
                BuildDynamicCBONodes(Nodes, new EventList<ICBOMenu>(new ICBOMenu[] { MenuTemplate }), ParentNode as CBOMenuNode, GenericKey, spc);

                if (setCurrentCBONode)
                {
                    if (ParentNode.ChildNodes.Count > 0)
                    {
                        //find the newly added node and set it to be the current node
                        foreach (CBOMenuNode childNode in ParentNode.ChildNodes)
                        {
                            if (childNode.GenericKey == GenericKey && childNode.GenericKeyTypeKey == (int)GenericKeyType)
                            {
                                SetCurrentCBONode(principal, childNode, nodeSetName);
                                break;
                            }
                        }

                        //this.SetCurrentCBONode(principal, ParentNode.ChildNodes[0] as CBOMenuNode, nodeSetName);
                    }
                }
            }
            spc.MenuVersion++;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spc"></param>
        /// <param name="MenuTemplate"></param>
        /// <param name="parent"></param>
        public void AddCBOMenuNodesFromTemplate(SAHLPrincipalCache spc, ICBOMenu MenuTemplate, CBONode parent)
        {
            List<CBONode> nodes = new List<CBONode>();
            BuildDynamicCBONodes(nodes, new EventList<ICBOMenu>(new ICBOMenu[] { MenuTemplate }), parent as CBOMenuNode, parent.GenericKey, spc);
            parent.ChildNodes.Clear();
            parent.ChildNodes.AddRange(nodes);
        }


        public void RemoveCBOMenuNodeByUrl(SAHLPrincipal principal, string url, CBONodeSetType nodeSetName)
        {
            CBOMenuNode node = GetCBOMenuNodeByUrl(principal, url, nodeSetName);

            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = spc.NodeSets[nodeSetName] as CBONodeSet;
            CBONode nodeToRemove = FindNodeByPath(nodeSet.Nodes, node.NodePath);

            if (nodeToRemove.NodePath == nodeSet.SelectedNode.NodePath)
                SetCurrentCBONode(principal, null, nodeSetName);

            if (nodeSetName == CBONodeSetType.X2)
            {
                IX2Service svc = ServiceFactory.GetService<IX2Service>();
                svc.TryCancelActivity(principal);
            }

            CBONode parentNode = nodeToRemove.ParentNode;

            if (parentNode == null)
            {
                nodeSet.Nodes.Remove(nodeToRemove);
            }
            else
            {
                parentNode.ChildNodes.Remove(nodeToRemove);
            }

            nodeSet.ContextNodes.Clear();
        }

        public void RemoveCBOMenuNode(SAHLPrincipal principal, string nodePath, CBONodeSetType nodeSetName)
        {
            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = spc.NodeSets[nodeSetName] as CBONodeSet;
            CBONode nodeToRemove = FindNodeByPath(nodeSet.Nodes, nodePath);

            if (nodeToRemove == null || nodeToRemove.IsRemovable == false)
                return;

            if (nodeToRemove.NodePath == nodeSet.SelectedNode.NodePath)
                SetCurrentCBONode(principal, null, nodeSetName);

            if (nodeSetName == CBONodeSetType.X2)
            {
                IX2Service svc = ServiceFactory.GetService<IX2Service>();
                svc.TryCancelActivity(principal);
            }

            CBONode parentNode = nodeToRemove.ParentNode;

            if (parentNode == null)
            {
                nodeSet.Nodes.Remove(nodeToRemove);
            }
            else
            {
                parentNode.ChildNodes.Remove(nodeToRemove);
            }

            nodeSet.ContextNodes.Clear();
        }

        public void ClearContextNodes(SAHLPrincipal principal, CBONodeSetType nodeSetName)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = spc.NodeSets[nodeSetName] as CBONodeSet;
            nodeSet.ContextNodes.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private IList<ICBOSecurityFilter> CheckForWorkflowFilter(CBOMenuNode CurrentNode)
        {
            // iterate up the tree and check for a workflownode, if so check the config for a specified filter.
            // create the filter and return it
            InstanceNode WF = CurrentNode as InstanceNode;
            CBONode CMN = CurrentNode;
            if (WF == null)
            {
                while (CMN.ParentNode != null)
                {
                    if (CMN.ParentNode is InstanceNode)
                    {
                        WF = CMN.ParentNode as InstanceNode;
                        // check for a config section that matches this workflow
                        CBOSecurityFilterSection Filters = ConfigurationManager.GetSection("CBOFilters") as CBOSecurityFilterSection;

                        // check if we have specified a different CBONode class for this CBOMenu
                        if (Filters != null)
                        {
                            List<ICBOSecurityFilter> FilterList = new List<ICBOSecurityFilter>();
                            Instance_DAO I = Instance_DAO.Find(WF.InstanceID);
                            string wfname = I.WorkFlow.Name;
                            string prname = I.WorkFlow.Process.Name;
                            for (int i = 0; i < Filters.CBOSecurityFilters.Count; i++)
                            {
                                if (Filters.CBOSecurityFilters[i].ProcessName == prname && Filters.CBOSecurityFilters[i].WorkflowName == wfname)
                                {
                                    string FilterType = Filters.CBOSecurityFilters[i].ClassType;
                                    ICBOSecurityFilter SecFilter = Activator.CreateInstance(Type.GetType(FilterType)) as ICBOSecurityFilter;
                                    if (SecFilter != null)
                                        FilterList.Add(SecFilter);
                                }
                            }
                            return FilterList;
                        }
                        break;
                    }
                    else
                    {
                        CMN = CMN.ParentNode;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves the currently selected <see cref="CBONode">CBONode</see> from the current list of CBONodes for the current <see cref="SAHLPrincipal">SAHL security principal</see>.
        /// </summary>
        /// <param name="principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <returns>The currently selected <see cref="CBONode">CBONode</see>.</returns>
        public CBONode GetCurrentCBONode(SAHLPrincipal principal)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = spc.NodeSets[spc.CurrentNodeSetType] as CBONodeSet;
            return nodeSet.SelectedNode;
        }

        /// <summary>
        /// Retrieves the currently selected <see cref="CBONode">CBONode</see> from the current list of CBONodes for the current <see cref="SAHLPrincipal">SAHL security principal</see>.
        /// </summary>
        /// <param name="principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <param name="nodeSetName">The current nodeSetName.</param>
        /// <returns>The currently selected <see cref="CBONode">CBONode</see>.</returns>
        public CBONode GetCurrentCBONode(SAHLPrincipal principal, CBONodeSetType nodeSetName)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = spc.NodeSets[nodeSetName] as CBONodeSet;
            return nodeSet.SelectedNode;
        }


        public CBOMenuNode GetTopParentCBOMenuNode(CBOMenuNode cboMenuNode)
        {
            CBOMenuNode node = null;
            if (cboMenuNode.ParentNode == null)
                node = cboMenuNode;
            else
                node = GetTopParentCBOMenuNode(cboMenuNode.ParentNode as CBOMenuNode);

            return node;
        }


        public void SetCurrentCBONode(SAHLPrincipal principal, CBOMenuNode Node, CBONodeSetType nodeSetName)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = spc.NodeSets[nodeSetName] as CBONodeSet;
            CBOMenuNode currentNode = nodeSet.SelectedNode;
            nodeSet.SelectedNode = Node;
            nodeSet.SelectedNodeKey = Node == null ? null : Node.NodePath;

            //clear context nodes if NOT the same node
            if (Node == null || (currentNode != null && currentNode.NodePath != Node.NodePath))
            {
                nodeSet.SelectedContextNode = null;
                nodeSet.ContextNodes.Clear();
            }
        }


        public CBOContextNode GetCurrentContextNode(SAHLPrincipal principal, CBONodeSetType nodeSetName)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            //            MenuNodeSet nodeSetName = spc.NodeSets.CurrentNodeSet;
            CBONodeSet nodeSet = spc.NodeSets[nodeSetName] as CBONodeSet;
            return nodeSet.SelectedContextNode;
        }


        public void SetCurrentContextNode(SAHLPrincipal principal, CBOContextNode Node, CBONodeSetType nodeSetName)
        {
            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            //            MenuNodeSet nodeSetName = spc.NodeSets.CurrentNodeSet;
            // store the selected context menu
            CBONodeSet nodeSet = spc.NodeSets[nodeSetName] as CBONodeSet;
            nodeSet.SelectedContextNode = Node;
        }


        public bool SetCurrentNodeSet(SAHLPrincipal principal, CBONodeSetType nodeSetName)
        {
            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            if (spc.NodeSets.ContainsKey(nodeSetName))
            {
                spc.CurrentNodeSetType = nodeSetName;
                return true;
            }
            else
                return false;
        }


        public CBONodeSetType GetCurrentNodeSetName(SAHLPrincipal principal)
        {
            return SAHLPrincipalCache.GetPrincipalCache(principal).CurrentNodeSetType;
        }


        public CBOMenuNode GetCBOMenuNodeByKey(SAHLPrincipal principal, string nodePath, CBONodeSetType nodeSetName)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = spc.NodeSets[nodeSetName] as CBONodeSet;

            return FindNodeByPath(nodeSet.Nodes, nodePath) as CBOMenuNode;
        }


        public CBOMenuNode GetCBOMenuNodeByUrl(SAHLPrincipal principal, string url, CBONodeSetType nodeSetName)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = spc.NodeSets[nodeSetName] as CBONodeSet;

            return FindNodeByUrl(url, nodeSet.Nodes) as CBOMenuNode;
        }

        public CBOContextNode GetCBOContextNodeByKey(SAHLPrincipal principal, string nodePath, CBONodeSetType nodeSetName)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = spc.NodeSets[nodeSetName] as CBONodeSet;

            return FindNodeByPath(nodeSet.ContextNodes, nodePath) as CBOContextNode;
        }


        private InstanceNode GetInstanceNodeParentFromCBONode(CBONode node)
        {
            CBONode parent = node.ParentNode;

            while (parent != null)
            {
                InstanceNode instance = parent as InstanceNode;

                if (instance != null)
                    return instance;

                parent = parent.ParentNode;
            }

            return null;
        }

        public void RefreshInstanceNode(SAHLPrincipal principal)
        {
            CBONode node = GetCurrentCBONode(principal, CBONodeSetType.X2);
            InstanceNode iNode = GetInstanceNodeParentFromCBONode(node);

            if (iNode != null)
            {
                //iNode.Refresh();
                iNode.IsDirty = true;
            }
        }

        public InstanceNode GetInstanceNode(SAHLPrincipal principal)
        {
            CBONode node = GetCurrentCBONode(principal, CBONodeSetType.X2);
            InstanceNode iNode = GetInstanceNodeParentFromCBONode(node);
            return iNode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="currentNode"></param>
        /// <param name="parentNodeType">eg : typeof(InstanceNode)</param>
        /// <returns></returns>
        public bool CheckForParentNodeByType(SAHLPrincipal principal, CBONode currentNode, Type parentNodeType)
        {
            bool nodeFound = false;

            if (currentNode.GetType() == parentNodeType)
                nodeFound = true;
            else if (currentNode.ParentNode != null && nodeFound == false)
                nodeFound = CheckForParentNodeByType(principal, currentNode.ParentNode, parentNodeType);

            return nodeFound;
        }


        public int GetMenuVersion(SAHLPrincipal principal)
        {
            return SAHLPrincipalCache.GetPrincipalCache(principal).MenuVersion;
        }


        private bool IsRootNode(ICBOMenu Node)
        {
            if (Node.ParentMenu == null)
                return true;
            else
                return false;
        }

        private IEventList<ICBOMenu> GetRootMenus(IDictionary<int, object> AllowedCBOMenus)
        {
            IEventList<ICBOMenu> TopLevels = new EventList<ICBOMenu>();

            foreach (ICBOMenu menu in AllowedCBOMenus.Values)
            {
                if (IsRootNode(menu))
                    TopLevels.Add(null, menu);
            }
            return TopLevels;
        }

        private CBONode CheckNodeGenericKeyExists(long GenericKey, List<CBONode> Nodes)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].GenericKey == GenericKey)
                    return Nodes[i];
            }
            return null;
        }

        private CBONode CheckNodeGenericKeyExists(long GenericKey, int GenericKeyTypeKey, List<CBONode> Nodes)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].GenericKey == GenericKey && Nodes[i].GenericKeyTypeKey == GenericKeyTypeKey)
                    return Nodes[i];
            }
            return null;
        }


        private CBONode FindNodeByPath(List<CBONode> Nodes, string nodePath)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].NodePath == nodePath)
                    return Nodes[i];

                if (Nodes[i].ChildNodes.Count > 0)
                {
                    CBONode node = FindNodeByPath(Nodes[i].ChildNodes, nodePath);

                    if (node != null)
                        return node;
                }
            }
            return null;
        }

        private CBONode FindNodeByGenericKey(List<CBONode> nodeList, int GenericKey)
        {
            if (nodeList == null)
                return null;

            for (int i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].GenericKey == GenericKey)
                {
                    return nodeList[i];
                }
                else if (nodeList[i].ChildNodes != null && nodeList[i].ChildNodes.Count > 0)
                {
                    CBONode childNode = FindNodeByGenericKey(nodeList[i].ChildNodes, GenericKey);

                    if (childNode != null)
                        return childNode;
                }
            }

            return null;
        }

        private CBONode FindNodeByGenericKeyAndDescription(List<CBONode> nodeList, int GenericKey, string description)
        {
            if (nodeList == null)
                return null;

            for (int i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].GenericKey == GenericKey &&
                    nodeList[i].Description == description)
                {
                    return nodeList[i];
                }
                else if (nodeList[i].ChildNodes != null && nodeList[i].ChildNodes.Count > 0)
                {
                    CBONode childNode = FindNodeByGenericKeyAndDescription(nodeList[i].ChildNodes, GenericKey, description);

                    if (childNode != null)
                        return childNode;
                }
            }

            return null;
        }

        private CBONode FindNodeByUrl(string url, List<CBONode> Nodes)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].URL == url)
                    return Nodes[i];
                if (Nodes[i].ChildNodes.Count > 0)
                {
                    CBONode node = FindNodeByUrl(url, Nodes[i].ChildNodes);

                    if (node != null)
                        return node;
                }
            }
            return null;
        }

        private CBONode FindChildNodeByURL(CBONode node, string URL)
        {
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].URL == URL)
                    return node.ChildNodes[i];

                if (node.ChildNodes[i].ChildNodes.Count > 0)
                {
                    CBONode newNode = FindChildNodeByURL(node.ChildNodes[i], URL);

                    if (newNode != null)
                        return newNode;
                }
            }
            return null;
        }


        public CBOMenuNode RefreshCBOMenuNodeByURL(SAHLPrincipal principal, string url)
        {
            CBOMenuNode nodeToRefresh = GetCBOMenuNodeByUrl(principal, url, CBONodeSetType.CBO);
            CBOMenuNode parentNode = (CBOMenuNode)nodeToRefresh.ParentNode;
            SetCurrentCBONode(principal, parentNode, CBONodeSetType.CBO);

            if (nodeToRefresh == null || parentNode == null)
                return null;

            int genericKey = nodeToRefresh.GenericKey;

            RemoveCBOMenuNode(principal, nodeToRefresh.NodePath, CBONodeSetType.CBO);

            bool alreadyAdded = false;

            // do a check to ensure that the legal entity hasn't already been added
            foreach (CBOMenuNode childNode in parentNode.ChildNodes)
            {
                if (childNode.GenericKey == genericKey)
                {
                    SetCurrentCBONode(principal, childNode, CBONodeSetType.CBO);
                    alreadyAdded = true;
                    return childNode;
                }
            }

            if (!alreadyAdded)
            {
                ICBOMenu ClientNameTemplate = GetLegalEntityTemplate(parentNode);
                AddCBOMenuNodeToSelection(principal, ClientNameTemplate, genericKey, CBONodeSetType.CBO);

                // try and select the new node
                CBOMenuNode newNode = (CBOMenuNode)parentNode.ChildNodes[parentNode.ChildNodes.Count - 1];
                SetCurrentCBONode(principal, newNode, CBONodeSetType.CBO);
                return newNode;
            }
            return parentNode;
        }

        private static ICBOMenu GetLegalEntityTemplate(CBOMenuNode LegalEntitiesNode)
        {
            for (int i = 0; i < LegalEntitiesNode.CBOMenu.ChildMenus.Count; i++)
            {
                if (LegalEntitiesNode.CBOMenu.ChildMenus[i].Description == "ClientName")
                    return LegalEntitiesNode.CBOMenu.ChildMenus[i];
            }
            return null;
        }

    }
}