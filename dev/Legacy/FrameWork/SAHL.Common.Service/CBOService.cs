using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

using SAHL.Common;
using SAHL.Common.UI;
using SAHL.Common.Security;
using SAHL.Common.Authentication;
using SAHL.Common.Service.Configuration;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.BusinessModel;
using SAHL.Common.Attributes;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.X2.BusinessModel;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
//using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.BusinessModel.DAO;
using System.Security.Principal;
using SAHL.Common.BusinessModel.Interfaces.UI;
using SAHL.Common.Caching;

namespace SAHL.Common.Service
{
    [FactoryType(typeof(ICBOService))] 
    public class CBOService : ICBOService
    {
        #region ICBOService Members

        //public void RefreshCBO(SAHLPrincipal Principal, string CurrentViewName, ISimpleNavigator Navigator)
        //{
        //    SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
        //    MenuNodeSet nodeSetName = GetCurrentNodeSetName(Principal);
        //    CBONode node = GetCurrentCBONode(Principal, nodeSetName);
        //    CBOContextNode contextNode = SPC.NodeSets[nodeSetName].SelectedContextNode;

        //    switch (nodeSetName)
        //    {
        //        case MenuNodeSet.CBO:
        //            //navigate to the currently selected page
        //            Navigator.Navigate(CurrentViewName);
        //            break;

        //        case MenuNodeSet.X2:
        //            //if you are on an instance node, navigate to the current x2 state

        //            if (SPC.X2Info != null && SPC.X2Info.InstanceID > 0)
        //            {
        //                Instance_DAO i_dao = Instance_DAO.Find(SPC.X2Info.InstanceID);
        //                i_dao.Refresh();
        //                Instance instance = new Instance(i_dao);

        //            }


        //            if (node != null)
        //            {
        //                //look at node type
        //                if (node is WorkFlowListNode || node is TaskListNode || node is WorkFlowNode)
        //                    Navigator.Navigate(CurrentViewName);
        //                else if (node is StateNode)
        //                    Navigator.Navigate(CurrentViewName);
        //                else if (node is InstanceNode)
        //                    Navigator.Navigate(CurrentViewName);
        //            }
        //            else if (contextNode != null)
        //            {
        //                //navigate to current x2 state, but which workflow?
        //            }
        //            else
        //            {
        //                //navigate to Workflows node
        //            }

        //            break;
        //    }


        //}


        /// <summary>
        /// Implements <see cref="ICBOService.GetMenuNodes"></see>.
        /// </summary>
        public IEventList<CBONode> GetMenuNodes(SAHLPrincipal Principal, MenuNodeSet nodeSetName)
        {
            // get the cached data for the principal
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);

            if (SPC.CBOMenus.Count == 0)
                RefreshPrincipalNodes(Principal);

            switch (nodeSetName)
            {
                case MenuNodeSet.CBO:
                    if (SPC.NodeSets[nodeSetName].Nodes.Count == 0)
                        BuildPrincipalNodes(Principal);
                    break;

                case MenuNodeSet.X2:
                    RefreshWorkFlowNodes(Principal);
                     break;
            }

            return SPC.NodeSets[nodeSetName].Nodes;
        }

        /// <summary>
        /// rebuilds the workflow part of the tree.
        /// </summary>
        /// <param name="Principal"></param>
        public void RefreshWorkFlowNodes(SAHLPrincipal Principal)
        {
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
            IEventList<CBONode> nodes = SPC.NodeSets[MenuNodeSet.X2].Nodes;


            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] is WorkFlowListNode)
                {
                    nodes.RemoveAt(null, i);
                    //workflowsNode = nodes[i] as WorkFlowListNode;
                    ////clear all existing nodes
                    //for (int j = 0; j < workflowsNode.ChildNodes.Count; j++)
                    //{
                    //    workflowsNode.ChildNodes.RemoveAt(Messages, j);
                    //}
                    break;
                }
            }

            WorkFlowListNode workflowsNode = new WorkFlowListNode(null);
            
            SPC.NodeSets[MenuNodeSet.X2].Nodes.Insert(null, 0, workflowsNode);

            //WorkflowSuperSearch needs to be added...
            //WorkFlowSearchNode searchNode = new WorkFlowSearchNode(0, null, "Application Search", "WorkFlow SuperSearch");
             //SPC.NodeSets[MenuNodeSet.X2].Nodes.Insert(Messages, 0, searchNode);

            //if (key > -1)
            //    SPC.NodeSets[MenuNodeSet.X2].SelectedNode = FindNode(key, SPC.NodeSets[MenuNodeSet.X2].Nodes) as CBOMenuNode;
        }

        /// <summary>
        /// Implements <see cref="ICBOService.GetContextNodes"/>
        /// </summary>
        /// <param name="Principal"></param>
        /// <param name="nodeSetName"></param>
        /// <returns></returns>
        public IEventList<CBONode> GetContextNodes(SAHLPrincipal Principal, MenuNodeSet nodeSetName)
        {
            // get the cached data for the principal
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);

            //if (SPC.NodeSets[nodeSetName].SelectedNode == null)
            //    return new EventList<CBONode>();

            //if (nodeSetName == MenuNodeSet.X2)
            //{
            //    CBOMenuNode node = SPC.NodeSets[MenuNodeSet.X2].SelectedNode;

            //    if (node != null && node is InstanceNode)
            //    {
            //        InstanceNode iNode = node as InstanceNode;
            //        ClearContextNodes(Principal, MenuNodeSet.X2);
            //        iNode.GetContextNodes(Messages, Principal, SPC.NodeSets[MenuNodeSet.X2].ContextNodes);
            //        return SPC.NodeSets[MenuNodeSet.X2].ContextNodes;
            //    }
            //}
            
			if (SPC.NodeSets[nodeSetName].ContextNodes.Count == 0 && SPC.NodeSets[nodeSetName].SelectedNode != null)
            {
                SPC.NodeSets[nodeSetName].SelectedNode.GetContextNodes(Principal, SPC.NodeSets[nodeSetName].ContextNodes, SPC.ContextMenus);

                // for now we are only interested in filtering CBO context nodes, we can apply this to workflownodes if required.
                IList<ICBOSecurityFilter> Filters = CheckForWorkflowFilter(SPC.NodeSets[nodeSetName].SelectedNode);
                if (Filters != null)
                {
                    foreach (ICBOSecurityFilter Filter in Filters)
                    {
                        Filter.FilterContextNodes(SPC.NodeSets[nodeSetName].ContextNodes); 
                    }
                }
            }

            return SPC.NodeSets[nodeSetName].ContextNodes;
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
                                if(Filters.CBOSecurityFilters[i].ProcessName == prname && Filters.CBOSecurityFilters[i].WorkflowName == wfname)
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
        /// Implements <see cref="ICBOService.AddCBOMenuNode"></see>.
        /// </summary>
        public void AddCBOMenuNode(SAHLPrincipal Principal, CBONode ParentNode, CBONode NewNode, MenuNodeSet nodeSetName)
        {
            if (NewNode == null)
                return;

            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
            IEventList<CBONode> Nodes = SPC.NodeSets[nodeSetName].Nodes;

            bool found = false;

            if (ParentNode == null)
            {
                for (int i = 0; i < Nodes.Count; i++)
                {
                    if (Nodes[i].CBOUniqueKey == NewNode.CBOUniqueKey)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    Nodes.Add(null, NewNode);
            }
            else
            {
                for (int i = 0; i < ParentNode.ChildNodes.Count; i++)
                {
                    if (ParentNode.ChildNodes[i].CBOUniqueKey == NewNode.CBOUniqueKey)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    ParentNode.ChildNodes.Add(null, NewNode);
            }

            SPC.MenuVersion++;
        }

        /// <summary>
        /// Implements <see cref="ICBOService.AddCBOMenuNodeToSelection"></see>.
        /// </summary>
        public void AddCBOMenuNodeToSelection(SAHLPrincipal Principal, ICBOMenu MenuTemplate, long GenericKey, MenuNodeSet nodeSetName)
        {
            // get the cached data for the principal
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);

            IEventList<CBONode> Nodes = SPC.NodeSets[nodeSetName].Nodes;
            
            if(SPC.NodeSets[nodeSetName].SelectedNode != null)
                Nodes = SPC.NodeSets[nodeSetName].SelectedNode.ChildNodes;

            // we need to check whether this generickey is not already available in the tree, if it is
            // we just set it to be the selected node
            CBONode NodeExists = CheckNodeGenericKeyExists(GenericKey, Nodes);

            if (NodeExists != null)
                SPC.NodeSets[nodeSetName].SelectedNode = NodeExists as CBOMenuNode;
            else
            {
                // A node doesn't exist for the generickey, so build the node
                BuildDynamicCBONodes(Nodes, new EventList<ICBOMenu>(new ICBOMenu[] { MenuTemplate }), SPC.NodeSets[nodeSetName].SelectedNode, GenericKey, Principal);
                if (SPC.NodeSets[nodeSetName].SelectedNode.ChildNodes.Count > 0)
                    this.SetCurrentCBONode(Principal, SPC.NodeSets[nodeSetName].SelectedNode.ChildNodes[0] as CBOMenuNode, nodeSetName);
            }
            SPC.MenuVersion++;
        }

        /// <summary>
        /// Implements <see cref="ICBOService.AddCBOMenuToNode"></see>.
        /// </summary>
        public void AddCBOMenuToNode(SAHLPrincipal Principal, CBOMenuNode ParentNode, ICBOMenu MenuTemplate, Int64 GenericKey, SAHL.Common.Globals.GenericKeyTypes GenericKeyType, MenuNodeSet nodeSetName)
        {
            // get the cached data for the principal
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);

            if (ParentNode == null)
                return;

            IEventList<CBONode> Nodes = ParentNode.ChildNodes;

            // we need to check whether this generickey is not already available in the tree, if it is
            // we just set it to be the selected node
            CBONode NodeExists = CheckNodeGenericKeyExists(GenericKey, (int)GenericKeyType, Nodes);

            if (NodeExists != null)
                ParentNode = NodeExists as CBOMenuNode;
            else
            {
                // A node doesn't exist for the generickey, so build the node
                BuildDynamicCBONodes(Nodes, new EventList<ICBOMenu>(new ICBOMenu[] { MenuTemplate }), ParentNode as CBOMenuNode, GenericKey, Principal);

                if (ParentNode.ChildNodes.Count > 0)
                {
                    //find the newly added node and set it to be the current node
                    foreach (CBOMenuNode childNode in ParentNode.ChildNodes)
                    {
                        if (childNode.GenericKey == GenericKey && childNode.GenericKeyTypeKey == (int)GenericKeyType)
                        {
                            this.SetCurrentCBONode(Principal, childNode, nodeSetName);
                            break;
                        }
                    }

                    //this.SetCurrentCBONode(Principal, ParentNode.ChildNodes[0] as CBOMenuNode, nodeSetName);
                }
            }
            SPC.MenuVersion++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Principal"></param>
        /// <param name="Nodes"></param>
        /// <param name="MenuTemplate"></param>
        /// <param name="GenericKey"></param>
        /// <param name="nodeSetName"></param>
        public void AddCBOMenuNodeFromTemplate(SAHLPrincipal Principal, IEventList<CBONode> Nodes, ICBOMenu MenuTemplate, Int64 GenericKey, MenuNodeSet nodeSetName)
        {
            // get the cached data for the principal
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);

            if (SPC.NodeSets[nodeSetName].SelectedNode != null)
            {
                // so build the node
                BuildDynamicCBONodes(Nodes, new EventList<ICBOMenu>(new ICBOMenu[] { MenuTemplate }), SPC.NodeSets[nodeSetName].SelectedNode, GenericKey, Principal);
                SPC.MenuVersion++;
            }
        }

        /// <summary>
        /// Retrieves the currently selected <see cref="CBONode">CBONode</see> from the current list of CBONodes for the current <see cref="SAHLPrincipal">SAHL security principal</see>.
        /// </summary>
        /// <param name="Principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <returns>The currently selected <see cref="CBONode">CBONode</see>.</returns>
        public CBONode GetCurrentCBONode(SAHLPrincipal Principal)
        {
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
            MenuNodeSet nodeSetName = GetCurrentNodeSetName(Principal);

            return SPC.NodeSets[nodeSetName].SelectedNode;

            //if (SPC.NodeSets[nodeSetName].SelectedNodeKey == null)
            //    return null;

            //return FindNodeByKey(SPC.NodeSets[nodeSetName].SelectedNodeKey, SPC.NodeSets[nodeSetName].Nodes);
        }

        /// <summary>
        /// Retrieves the currently selected <see cref="CBONode">CBONode</see> from the current list of CBONodes for the current <see cref="SAHLPrincipal">SAHL security principal</see>.
        /// </summary>
        /// <param name="Principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <param name="nodeSetName">The current nodeSetName.</param>
        /// <returns>The currently selected <see cref="CBONode">CBONode</see>.</returns>
        public CBONode GetCurrentCBONode(SAHLPrincipal Principal, MenuNodeSet nodeSetName)
        {
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
//            MenuNodeSet nodeSetName = SPC.NodeSets.CurrentNodeSet;

            return SPC.NodeSets[nodeSetName].SelectedNode;

            //if (SPC.NodeSets[nodeSetName].SelectedNodeKey == null)
            //    return null;

            //return FindNodeByKey(SPC.NodeSets[nodeSetName].SelectedNodeKey, SPC.NodeSets[nodeSetName].Nodes);

            //return SPC.NodeSets[nodeSetName].SelectedNode;
        }

        /// <summary>
        /// Implements <see cref="ICBOService.GetTopParentCBOMenuNode"></see>.
        /// </summary>
        public CBOMenuNode GetTopParentCBOMenuNode(CBOMenuNode cboMenuNode)
        {
            CBOMenuNode node = null;
            if (cboMenuNode.ParentNode == null)
                node = cboMenuNode;
            else
                node = GetTopParentCBOMenuNode(cboMenuNode.ParentNode as CBOMenuNode);

            return node;
        }
        /// <summary>
        /// Implements <see cref="ICBOService.SetCurrentCBONode"></see>.
        /// </summary>
        public void SetCurrentCBONode(SAHLPrincipal Principal, CBOMenuNode Node, MenuNodeSet nodeSetName)
        {
            // get the cached data for the principal
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
//            MenuNodeSet nodeSetName = SPC.NodeSets.CurrentNodeSet;
            // store the selected node
            SPC.NodeSets[nodeSetName].SelectedNode = Node;
            SPC.NodeSets[nodeSetName].SelectedNodeKey = Node == null ? null : Node.CBOUniqueKey;
            
            // set the selected context menu to null
            SPC.NodeSets[nodeSetName].SelectedContextNode = null;

            DomainMessageCollection messages = new DomainMessageCollection();

            for (int i= SPC.NodeSets[nodeSetName].ContextNodes.Count-1; i>-1; i--)
            {
                SPC.NodeSets[nodeSetName].ContextNodes.RemoveAt(messages, i);
            }
        }

        /// <summary>
        /// Implements <see cref="ICBOService.GetCurrentContextNode"></see>.
        /// </summary>
        public CBOContextNode GetCurrentContextNode(SAHLPrincipal Principal, MenuNodeSet nodeSetName)
        {
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
//            MenuNodeSet nodeSetName = SPC.NodeSets.CurrentNodeSet;
            return SPC.NodeSets[nodeSetName].SelectedContextNode;
        }

        /// <summary>
        /// Implements <see cref="ICBOService.SetCurrentContextNode"></see>.
        /// </summary>
        public void SetCurrentContextNode(SAHLPrincipal Principal, CBOContextNode Node, MenuNodeSet nodeSetName)
        {
            // get the cached data for the principal
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
//            MenuNodeSet nodeSetName = SPC.NodeSets.CurrentNodeSet;
            // store the selected context menu
            SPC.NodeSets[nodeSetName].SelectedContextNode = Node;
        }

        public void ClearContextNodes(SAHLPrincipal Principal, MenuNodeSet nodeSetName)
        {
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
            DomainMessageCollection messages = new DomainMessageCollection();

            for (int i = SPC.NodeSets[nodeSetName].ContextNodes.Count - 1; i > -1; i--)
            {
                SPC.NodeSets[nodeSetName].ContextNodes.RemoveAt(messages, i);
            }
        }

        /// <summary>
        /// Implements <see cref="ICBOService.SetCurrentNodeSet"></see>.
        /// </summary>
        public bool SetCurrentNodeSet(SAHLPrincipal Principal, MenuNodeSet nodeSetName)
        {
            // get the cached data for the principal
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
            if (SPC.NodeSets.ContainsKey(nodeSetName))
            {
                SPC.NodeSets.CurrentNodeSet = nodeSetName;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Implements <see cref="ICBOService.GetCurrentNodeSetName"></see>.
        /// </summary>
        public MenuNodeSet GetCurrentNodeSetName(SAHLPrincipal Principal)
        {
            return SAHLPrincipalCache.GetPrincipalCache(Principal).NodeSets.CurrentNodeSet;
        }

        /// <summary>
        /// Implements <see cref="ICBOService.GetCBOMenuNodeByKey"/>
        /// </summary>
        public CBOMenuNode GetCBOMenuNodeByKey(SAHLPrincipal Principal, string CBOUniqueKey, MenuNodeSet nodeSetName)
        {
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
            return FindNodeByKey(CBOUniqueKey, SPC.NodeSets[nodeSetName].Nodes) as CBOMenuNode;
        }

        /// <summary>
        /// Implements <see cref="ICBOService.GetCBOMenuNodeByUrl"/>
        /// </summary>
        public CBOMenuNode GetCBOMenuNodeByUrl(SAHLPrincipal principal, string url, MenuNodeSet nodeSetName)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            return FindNodeByUrl(url, spc.NodeSets[nodeSetName].Nodes) as CBOMenuNode;
        }

        public CBOContextNode GetCBOContextNodeByKey(SAHLPrincipal Principal, string CBOUniqueKey, MenuNodeSet nodeSetName)
        {
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
            return FindNodeByKey(CBOUniqueKey, SPC.NodeSets[nodeSetName].ContextNodes) as CBOContextNode;
        }

        /// <summary>
        /// Implements <see cref="ICBOService.RemoveCBOMenuNode"/>
        /// </summary>
        public void RemoveCBOMenuNode(SAHLPrincipal Principal, string CBOUniqueKey, MenuNodeSet nodeSetName)
        {
            // get the cached data for the principal
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
//            MenuNodeSet nodeSetName = SPC.NodeSets.CurrentNodeSet;
            CBONode Node = FindNodeByKey(CBOUniqueKey, SPC.NodeSets[nodeSetName].Nodes);
            if (Node == null)
                return;

            CBOMenuNode newSelectedNode = null;

            if (Node.IsRemovable)
            {
                IEventList<CBONode> Nodes = SPC.NodeSets[nodeSetName].Nodes;
                CBONode selectedNode = SPC.NodeSets[nodeSetName].SelectedContextNode as CBONode;

                //if (Node == selectedNode && Node.ParentNode != null)
                //{
                //    SetCurrentCBONode(Principal, Node.ParentNode as CBOMenuNode, nodeSetName);
                //}
                CBONode parentNode = Node.ParentNode;
                if (parentNode == null)
                {
                    Nodes.Remove(null, Node);
                    if (Nodes.Count > 0)
                        newSelectedNode = Nodes[0] as CBOMenuNode;
                }
                else
                {
                    parentNode.ChildNodes.Remove(null, Node);
                    newSelectedNode = parentNode as CBOMenuNode;
                }

            }

            if (newSelectedNode != null)
                SetCurrentCBONode(Principal, newSelectedNode, nodeSetName);

            SPC.MenuVersion++;
        }

        /// <summary>
        /// This method forces a refresh of a principal's CBO nodes.  This shouldn't be exposed to 
        /// external methods as other issues need to be taken into account - features, nodeset caching, 
        /// cbo caching, etc.  
        /// </summary>
        /// <param name="principal"></param>
        internal static void RefreshPrincipalNodes(SAHLPrincipal principal)
        {
            // get a filtered list of featuregroups for the principal
            // IList<IFeatureGroup> allowedFeatureGroups = new List<IFeatureGroup>();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            // get the list of features available for the spc
            IList<int> allowedFeatures = spc.FeatureKeys;

            // loop though the full cbo and filter based on our principal's filtered set of feature keys
            foreach (ICBOMenu menuItem in lookups.CBOMenus)
            {
                if (spc.CBOMenus.ContainsKey(menuItem.Key))
                    break;

                if (menuItem.Feature != null && allowedFeatures.Contains(menuItem.Feature.Key))
                    spc.CBOMenus.Add(menuItem.Key, menuItem);
            }

            // loop through the full list of contextmenus and filter based on our
            // principals filtered set of feature keys
            foreach (IContextMenu contextMenu in lookups.ContextMenus)
            {
                if (spc.ContextMenus.ContainsKey(contextMenu.Key))
                    break;

                if (contextMenu.Feature != null && allowedFeatures.Contains(contextMenu.Feature.Key))
                {
                    spc.ContextMenus.Add(contextMenu.Key, contextMenu);
                    // set the allowed contextmenu node
                    //IContextMenu parentContextMenu = lookups.AllContextMenus[i];
                    // check the children of the contextmenu node to see if they are allowed and remove if not.
                    //foreach (IContextMenu childContextMenu in parentContextMenu.ChildMenus)
                    //{
                    //    bool allowed = false;
                    //    if (childContextMenu.Feature != null && AllowedFs.ContainsKey(childContextMenu.Feature.Key))
                    //        allowed = true;
                    //    if (allowed == false)
                    //    {
                    //        parentContextMenu.ChildMenus.Remove(MC, childContextMenu);
                    //    }
                    //}
                    //UserContextMenus.Add(MC, parentContextMenu);
                    //UserContextMenus.Add(MC, Lookups.AllContextMenus[i]);
                }
            }
        }


        public void RefreshInstanceNode(SAHLPrincipal Principal)
        {
            CBONode node = GetCurrentCBONode(Principal, MenuNodeSet.X2);
            RefreshInstanceNodeInternal(Principal, node);
        }

        private void RefreshInstanceNodeInternal(SAHLPrincipal Principal, CBONode CurrentNode)
        {
            string desc = null;

            if (CurrentNode == null)
            {
                throw new Exception("No InstanceNode found");
            }
            else if (CurrentNode is InstanceNode)
            {
                CBONode selectedNode = GetCurrentCBONode(Principal, MenuNodeSet.X2);

                if (selectedNode != null)
                {
                    desc = selectedNode.GenericKey.ToString();

                    //while (selectedNode.ParentNode != CurrentNode)
                    //{
                    //    selectedNode = selectedNode.ParentNode;
                    //    desc = String.Format("{0}.{1}", selectedNode.GenericKey, desc); 
                    //}
                }

                InstanceNode inst = CurrentNode as InstanceNode;
                inst.RefreshChildNodes(Principal, selectedNode);
                //inst.IsDirty = true;

                //SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
                //SPC.MenuVersion++;
            }
            else
            {
                RefreshInstanceNodeInternal(Principal, CurrentNode.ParentNode);
            }

        }

        /// <summary>
        /// Implements <see cref="ICBOService.CheckForParentNodeByType"/>
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
        /// <summary>
        /// Implements <see cref="ICBOService.GetMenuVersion"/>
        /// </summary>
        public int GetMenuVersion(SAHLPrincipal Principal)
        {
            return SAHLPrincipalCache.GetPrincipalCache(Principal).MenuVersion;
        }

        //public void AddPropertyNode(SAHLPrincipal Principal, CBONode Node, IProperty property)
        //{
        //    SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
        //    IAddressRepository repo = RepositoryFactory.GetRepository<IAddressRepository>();

        //    string desc = "";
        //    string URL = "PropertyDetails";
        //    string icon = "PhysicalAddress3.png";

        //    if (property.Address == null)
        //        desc = String.Format("Erf: {0},{1},{2}", property.PropertyDescription1, property.PropertyDescription2, property.PropertyDescription3);
        //    else
        //        desc = repo.fGetFormattedAddressDelimited(property.Address, false);

        //    CBOMenu_DAO cbo = CBOMenu_DAO.Find(111);

        //    if (cbo != null)
        //    {
        //        URL = cbo.URL;
        //        icon = cbo.MenuIcon;
        //    }

        //    CBOMenuNode newNode = new CBOMenuNode(null, property.Key, Node as CBOMenuNode, desc, desc, URL, icon, false);
        //    Node.ChildNodes.Add(null, newNode as CBONode);
        //    SPC.MenuVersion++;
        //}

        //public void AddApplicantNode(SAHLPrincipal Principal, CBONode Node, IApplicationRole Role)
        //{
        //    SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(Principal);
        //    string desc = Role.LegalEntity.GetLegalName(LegalNameFormat.Full);
        //    string ldesc = String.Format("{0} ({1})", Role.LegalEntity.GetLegalName(LegalNameFormat.InitialsOnly), Role.ApplicationRoleType.Description);
        //    string URL = "LegalEntityDetailsDisplayApplicant";
        //    string icon = "Client.gif";

        //    CBOMenu_DAO cbo = CBOMenu_DAO.Find(106);

        //    if (cbo != null)
        //    {
        //        URL = cbo.URL;
        //        icon = cbo.MenuIcon;
        //    }

        //    CBOMenuNode newNode = new CBOMenuNode(null, Role.LegalEntity.Key, Node as CBOMenuNode, desc, ldesc, URL, icon, false);
        //    Node.ChildNodes.Add(null, newNode as CBONode);
        //    SPC.MenuVersion++;
        //}

        #endregion

        #region private Members

        private bool IsRootNode(ICBOMenu Node)
        {
            if (Node.ParentMenu == null)
                return true;
            else
                return false;
        }

        private IEventList<ICBOMenu> GetRootMenus(IDictionary<int, ICBOMenu> AllowedCBOMenus)
        {
            IEventList<ICBOMenu> TopLevels = new EventList<ICBOMenu>();

            foreach (ICBOMenu menu in AllowedCBOMenus.Values)
            {
                if (IsRootNode(menu))
                    TopLevels.Add(null, menu);

            }
            return TopLevels;
        }

        private void BuildPrincipalNodes(SAHLPrincipal principal)
        {
            // start from the top level nodes
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            IEventList<ICBOMenu> TopLevelMenus = GetRootMenus(spc.CBOMenus); //(IDictionary<int, ICBOMenu>)principal.GetCachedItem(SAHLPrincipalCacheItems.CBOMenus));

            // build down the node tree
            BuildStaticCBONodes(spc.NodeSets[MenuNodeSet.CBO].Nodes, TopLevelMenus, null, principal);
        }

        private void BuildStaticCBONodes(IEventList<CBONode> NodeList, IEventList<ICBOMenu> CBOMenuList, CBOMenuNode Parent, SAHLPrincipal principal)
        {
            for (int i = 0; i < CBOMenuList.Count; i++)
            {
                if (CBOMenuList[i].NodeType == 'S')
                {
                    List<CBOMenuNode> Nodes = CBONodeBuilder.BuildCBONode(CBOMenuList[i], Parent, -1, principal);

                    if (Nodes != null)
                    {
                        foreach (CBOMenuNode Node in Nodes)
                        {
                            NodeList.Add(null, Node);
                            BuildStaticCBONodes(Node.ChildNodes, CBOMenuList[i].ChildMenus, Node, principal);                          
                        }
                    }
                }
            }
        }

        private void BuildDynamicCBONodes(IEventList<CBONode> NodeList, IEventList<ICBOMenu> CBOMenuList, CBOMenuNode Parent, Int64 GenericKey, SAHLPrincipal principal)
        {
            foreach (ICBOMenu cboMenu in CBOMenuList)
            {
                List<CBOMenuNode> Nodes = CBONodeBuilder.BuildCBONode(cboMenu, Parent, GenericKey, principal);
                if (Nodes != null)
                {
                    foreach (CBOMenuNode Node in Nodes)
                    {
                        NodeList.Add(null, Node);
                        if (cboMenu.ChildMenus.Count > 0)
                        {
                            BuildDynamicCBONodes(Node.ChildNodes, cboMenu.ChildMenus, Node, -1, principal);
                        }                        
                    }
                }
            }
        }

        private CBONode CheckNodeGenericKeyExists(long GenericKey, IEventList<CBONode> Nodes)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].GenericKey == GenericKey)
                    return Nodes[i];
            }
            return null;
        }

        private CBONode CheckNodeGenericKeyExists(long GenericKey, int GenericKeyTypeKey, IEventList<CBONode> Nodes)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].GenericKey == GenericKey && Nodes[i].GenericKeyTypeKey == GenericKeyTypeKey)
                    return Nodes[i];
            }
            return null;
        }

        //private CBONode FindNode(int CBONodeKey, IEventList<CBONode> Nodes)
        //{
        //    for (int i = 0; i < Nodes.Count; i++)
        //    {
        //        if (Nodes[i].CBONodeKey == CBONodeKey)
        //            return Nodes[i];
        //        if (Nodes[i].ChildNodes.Count > 0)
        //        {
        //            CBONode node = FindNode(CBONodeKey, Nodes[i].ChildNodes);
 
        //            if (node != null)
        //                return node;
        //        }
        //    }
        //    return null;
        //}

        private CBONode FindNodeByKey(string CBOUniqueKey, IEventList<CBONode> Nodes)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].CBOUniqueKey == CBOUniqueKey)
                    return Nodes[i];
                if (Nodes[i].ChildNodes.Count > 0)
                {
                    CBONode node = FindNodeByKey(CBOUniqueKey, Nodes[i].ChildNodes);

                    if (node != null)
                        return node;
                }
            }
            return null;
        }

        private CBONode FindNodeByUrl(string url, IEventList<CBONode> Nodes)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].URL == url)
                    return Nodes[i];
                if (Nodes[i].ChildNodes.Count > 0)
                {
                    CBONode node = FindNodeByKey(url, Nodes[i].ChildNodes);

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

        #endregion
    }

    internal class CBONodeBuilder
    {
        /// <summary>
        /// Builds a CBO Node by dynamically creating the appropriate node class.
        /// </summary>
        /// <param name="Menu">The CoreBusinessObjectMenu to base the CBONode on.</param>
        /// <param name="ParentNode">The parent node.</param>
        /// <param name="GenericKey">The generickey used to store the business data key for the node.</param>
        /// <param name="principal">The principal identity.</param>
        /// <returns></returns>
        public static List<CBOMenuNode> BuildCBONode(ICBOMenu Menu, CBOMenuNode ParentNode, Int64 GenericKey, SAHLPrincipal principal)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            if (spc.CBOMenus.Count == 0)
                CBOService.RefreshPrincipalNodes(principal);

            // check feature access - if no access then return null
            IDictionary<int, ICBOMenu> allowedMenus = spc.CBOMenus; // (IDictionary<int, ICBOMenu>)principal.GetCachedItem(SAHLPrincipalCacheItems.CBOMenus);

            if (Menu.Feature != null && !allowedMenus.ContainsKey(Menu.Key))
                return null;

            List<Dictionary<string, object>> NodeDataList = new List<Dictionary<string, object>>();

            /*
            // check feature access
            bool allowed = false;
            if (Menu.Feature != null)
            {
                IEventList<ICBOMenu> principalCboMenus = (IEventList<ICBOMenu>)principal.GetCachedItem(SAHLPrincipalCacheItems.CBOMenus);
                foreach (ICBOMenu cm in principalCboMenus)
                {
                    if (Menu.Key == cm.Key)
                    {
                        allowed = true;
                        break;
                    }

                }
            }

            // if no access then return null
            if (allowed == false)
                return null;
            */

            // get the CBOMenus configuration section
            CBOSection CBOConfig = ConfigurationManager.GetSection("CBOMenus") as CBOSection;
            CBOMenuElement CBOElem = null;

            // check if we have specified a different CBONode class for this CBOMenu
            if(CBOConfig != null)
                CBOElem = FindNodeElement(CBOConfig.CBOMenus, Menu);

            // use the base CBONode class by default and change it only if we have specified a different CBONode
            Type type = typeof(CBOMenuNode);// "SAHL.Common.BusinessModel.Interfaces.UI.CBOMenuNode, SAHL.Common.BusinessModel.Interfaces";


            if (CBOElem != null)
                type = Type.GetType(CBOElem.NodeClass);
                
                //TypeName = CBOElem.NodeClass;

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
                if(Menu.NodeType != 'D')
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
                            string OS = "";
                            for (int i = 0; i < spc.UserOriginationSources.Count; i++)
                            {
                                string oOS = spc.UserOriginationSources[i].OriginationSource.Key.ToString();
                                if (i < spc.UserOriginationSources.Count - 1)
                                    OS += (oOS + ", ");
                                else
                                    OS += oOS;
                            }
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

        /// <summary>
        /// Get the statement parameters from the input generic types linked to the CBOMenu.
        /// </summary>
        /// <param name="Menu"></param>
        /// <param name="ParentNode"></param>
        /// <param name="Params"></param>
        /// <param name="GenericKeyData"></param>
        /// <returns></returns>
        private static bool GetStatementParameters(ICBOMenu Menu, CBOMenuNode ParentNode, ParameterCollection Params, Int64 GenericKeyData)
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
        private static bool GetParentGenericType(ParameterCollection Params, int GenericKeyType, CBOMenuNode Node, string ParamName)
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
        private static bool GetGenericType(ParameterCollection Params, int GenericKeyType, Int64 KeyValue, string ParamName)
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
        private static CBOMenuElement FindNodeElement(CBOElementCollection Elements, ICBOMenu Menu)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i].CBOKey == Menu.Key)
                    return Elements[i];
            }
            return null;
        }
    }
}