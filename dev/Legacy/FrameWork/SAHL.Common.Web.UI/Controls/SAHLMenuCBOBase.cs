using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SAHL.Common.BusinessModel;
using SAHL.Common.Web.UI.Views;
using SAHL.Common.UI;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.X2.BusinessModel;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.X2.BusinessModel.Interfaces;



namespace SAHL.Common.Web.UI.Controls
{
    /// <summary>
    /// Control for displaying the SAHL CBO menus.  This uses two <see cref="SAHLTreeView"/> controls.
    /// </summary>
    public class SAHLMenuCBOBase : SAHLWebControl
    {
        protected SAHLTreeView _treeViewMain;
        protected SAHLTreeView _treeViewContext;
        protected HtmlGenericControl _divContextHeader;
        protected int _menuVersion = -1;
        protected string _contextHeaderText = "";
        protected string _cssClassCBO = "";
        protected string _cssClassContext = "";
        protected string _cssClassContextHeader = "";
        protected IDomainMessageCollection _MC = new DomainMessageCollection();
        protected CBONodeSetType _nodeSet;
        protected CBONode _selectedNode;
        protected CBONode _selectedContextNode;
        protected IViewBase _view;
        private List<string> _expandedContextNodes = new List<string>();

        public event NavigateEventHandler Navigate;

        #region Properties

        /// <summary>
        /// Gets/sets the text to display in the context header.
        /// </summary>
        public string ContextHeaderText
        {
            get
            {
                return _contextHeaderText;
            }
            set
            {
                _contextHeaderText = value;
            }
        }

        /// <summary>
        /// Gets/sets the CSS class applied to the CBO menu (top tree).
        /// </summary>
        public string CssClassCBO
        {
            get
            {
                return _cssClassCBO;
            }
            set
            {
                _cssClassCBO = value;
            }
        }

        /// <summary>
        /// Gets/sets the CSS class applied to the context menu (bottom tree).
        /// </summary>
        public string CssClassContext
        {
            get
            {
                return _cssClassContext;
            }
            set
            {
                _cssClassContext = value;
            }
        }

        /// <summary>
        /// Gets/sets the CSS class applied to the context menu (bottom tree) header .
        /// </summary>
        public string CssClassContextHeader
        {
            get
            {
                return _cssClassContextHeader;
            }
            set
            {
                _cssClassContextHeader = value;
            }
        }

        #endregion

        public SAHLMenuCBOBase()
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _view = Page as IViewBase;
            _nodeSet = _view.CBOManager.GetCurrentNodeSetName(_view.CurrentPrincipal);
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    BaseLoad();
        //}

        //protected override void OnPreRender(EventArgs e)
        //{
        //    BasePreRender(e);
        //}


        protected void BaseInit(EventArgs e)
        {
            // add the trees to the control
            _treeViewMain = new SAHLTreeView();
            this.Controls.Add(_treeViewMain);

            // add the context header
            _divContextHeader = new HtmlGenericControl("div");
            this.Controls.Add(_divContextHeader);

            _treeViewContext = new SAHLTreeView();
            this.Controls.Add(_treeViewContext);

            // register event handlers
            _treeViewMain.NodeSelected += new SAHLTreeNodeEventHandler(_treeViewMain_NodeSelected);
            _treeViewMain.NodeIconClicked += new SAHLTreeNodeEventHandler(_treeViewMain_NodeIconClicked);
            _treeViewContext.NodeSelected += new SAHLTreeNodeEventHandler(_treeViewContext_NodeSelected);
        }

        protected void BaseLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode && !_view.ShouldRunPage) 
                return;

            LoadMenuNodes(false);
        }

        protected void BasePreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (DesignMode && !_view.ShouldRunPage) 
                return;

            // the code below is only really required when we have flushed the spc
            // it re-adds the nodesets after they have been cleared-out via the flush
            _view.CBOManager.CreateNodeSets();

            LoadMenuNodes(true);
            SetCss();
        }

        protected void SetCss()
        {
            // set the CSS classes
            _treeViewMain.CssClass = _cssClassCBO;
            _divContextHeader.Attributes.Add("class", CssClassContextHeader);
            _divContextHeader.InnerHtml = "<span>" + ContextHeaderText + "";
            _treeViewContext.CssClass = CssClassContext;
        }


        /// <summary>
        /// Reloads both trees using the <see cref="CBOManager"/>.
        /// </summary>
        protected void LoadMenuNodes(bool selectAndExpand)
        {
            // get the currently selected nodes
            //clear the Menu and reload all the nodes
            _treeViewMain.Nodes.Clear();
            _treeViewContext.Nodes.Clear();

            List<CBONode> cboNodes = _view.CBOManager.GetMenuNodes(_view.CurrentPrincipal, _nodeSet);
            _selectedNode = _view.CBOManager.UpdateNodeSelection(_view.CurrentPrincipal, _nodeSet);
            AddNodes(_treeViewMain, cboNodes, null, selectAndExpand);
          
            List<CBONode> contextNodes = _view.CBOManager.GetContextNodes(_view.CurrentPrincipal, _nodeSet);

            if (selectAndExpand)
            {
                if (!_view.IsMenuPostBack)
                    ResetSelectedContextNode(_view.ViewName, contextNodes);
                
                _selectedContextNode = _view.CBOManager.UpdateContextNodeSelection(_view.CurrentPrincipal, _nodeSet);

                _expandedContextNodes.Clear();

                if (_selectedContextNode != null)
                {
                    _expandedContextNodes.Add(_selectedContextNode.NodePath);
                    CBONode nodeParent = _selectedContextNode.ParentNode;
                    while (nodeParent != null)
                    {
                        _expandedContextNodes.Add(nodeParent.NodePath);
                        nodeParent = nodeParent.ParentNode;
                    }
                }
            }

            // reset the context node at this stage if the selected node doesn't match the view name
            //string selectedContextURL = (_selectedContextNode == null ? "" : _selectedContextNode.URL);
           
            //if (_view != null && _view.ShouldRunPage && _view.ViewName != selectedContextURL)
            //    ResetSelectedContextNode(_view.ViewName, contextNodes);

            AddNodes(_treeViewContext, contextNodes, null, selectAndExpand);
        }

        /// <summary>
        /// Internal method used to loop through the context nodes and ensure the correct one is displayed.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contextNodes"></param>
        /// <returns></returns>
        protected bool ResetSelectedContextNode(string url, List<CBONode> contextNodes)
        {
            foreach (CBOContextNode node in contextNodes)
            {
                if (node.URL == _view.ViewName)
                {
                    _view.CBOManager.SetCurrentContextNode(_view.CurrentPrincipal, node, _nodeSet);
                    _selectedContextNode = node;
                    return true;
                }
                
                // run through the children of the node
                if (ResetSelectedContextNode(url, node.ChildNodes))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Add a list of nodes to a treeview
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="cboNodes"></param>
        /// <param name="parentNode"></param>
        /// <param name="selectAndExpand"></param>
        protected void AddNodes(SAHLTreeView treeView, List<CBONode> cboNodes, SAHLTreeNode parentNode, bool selectAndExpand)
        {
            if (cboNodes == null)
                return;

            List<string> nodePaths = new List<string>();

            foreach (CBONode node in cboNodes)
            {
                //make sure we don't add the same node more than once
                if (nodePaths.Contains(node.NodePath))
                    continue;
                else
                    nodePaths.Add(node.NodePath);

                SAHLTreeNode treeNode = new SAHLTreeNode(node.Description, node.NodePath);
                treeNode.ToolTipText = node.LongDescription;

                if (!String.IsNullOrEmpty(node.URL))
                {
                    treeNode.NavigateValue = node.URL;
                    treeNode.OnClientClick = "if (window.masterNavigate) return masterNavigate()";
                }
                else
                {
                    treeNode.AutoPostBack = false;
                }

                if (node.MenuIcon != null)
                {
                    SAHLTreeNodeIcon icon = new SAHLTreeNodeIcon(Page.ResolveClientUrl("~/Images/" + node.MenuIcon));

                    if (node.IsRemovable)
                    {
                        icon.AutoPostBack = true;
                        icon.HoverIcon = Page.ResolveClientUrl("~/Images/delete.png");
                    }

                    treeNode.Icons.Add(icon);
                }

                // add the origination source icon
                if (!String.IsNullOrEmpty(node.OriginationSourceIcon))
                {
                    SAHLTreeNodeIcon osIcon = new SAHLTreeNodeIcon(Page.ResolveClientUrl(node.OriginationSourceIcon));
                    treeNode.Icons.Add(osIcon);
                }

                if (selectAndExpand)
                {
                    if (treeView == _treeViewContext)
                    {
                        if (_expandedContextNodes.Contains(node.NodePath))
                            treeNode.Expanded = true;
                        else
                            treeNode.Expanded = false;

                        if (_selectedContextNode != null && node.NodePath == _selectedContextNode.NodePath)
                            treeView.SelectedNode = treeNode;
                    }
                    else
                    {
                        treeNode.Expanded = true;

                        if (_selectedNode != null && node.NodePath == _selectedNode.NodePath)
                            treeView.SelectedNode = treeNode;
                    }
                }

                if (parentNode == null)
                    treeView.Nodes.Add(treeNode);
                else
                    parentNode.Nodes.Add(treeNode);

                if ((node.ChildNodes != null)&&(node.ChildNodes.Count > 0))
                    AddNodes(treeView, node.ChildNodes, treeNode, selectAndExpand);
            }
        }

        /// <summary>
        /// Called when an item on the main menu is clicked.  By default, this will navigate to the view defined 
        /// in <c>e</c>.  This can be overridden for additional functionality.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected virtual void OnMenuNodeSelected(object source, SAHLTreeNodeEventArgs e)
        {
            //CBOMenuNode node = _view.CBOManager.GetCBOMenuNodeByKey(_view.CurrentPrincipal, e.TreeNode.Value.ToString(), _nodeSet);
            //_view.CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, node, _nodeSet);
            //List<CBONode> contextNodes = _view.CBOManager.GetContextNodes(_view.CurrentPrincipal, _nodeSet);
            //string navigateTo = e.TreeNode.NavigateValue;

            //if (_nodeSet == CBONodeSetType.CBO)
            //{
            //    // check to see if there are context nodes - if there are, then we need to navigate to the URL 
            //    // of the first node, otherwise we bypass feature security - if there are no nodes then just go 
            //    // to the URL specified on the treenode
            //    if (contextNodes.Count > 0)
            //        navigateTo = contextNodes[0].URL;
            //}
            //else if (_nodeSet == CBONodeSetType.X2)
            //{
            //    //must select the default form for an instance node
            //    if (node is InstanceNode)
            //    {
            //        for (int i = contextNodes.Count - 1; i > -1; i--)
            //        {
            //            if (contextNodes[i].Description == "Forms")
            //            {
            //                _view.CBOManager.SetCurrentContextNode(_view.CurrentPrincipal, contextNodes[i] as CBOContextNode, _nodeSet);
            //                //_selectedContextNode = contextNodes[i];
            //                break;
            //            }
            //        }

            //        InstanceNode iNode = node as InstanceNode;
            //        //change the following when InstanceNode gets updated to the new format
            //        IX2Service x2Service = ServiceFactory.GetService<IX2Service>();
            //        IEventList<IForm> forms = x2Service.GetFormsForInstance(iNode.InstanceID);

            //        if (forms != null && forms.Count > 0)
            //            navigateTo = forms[0].Name;
            //    }
            //}

            //DoNavigate(navigateTo);
            ////DoNavigate(e.TreeNode.NavigateValue);
        }

        /// <summary>
        /// Called when an item on the context menu is clicked.  By default, this will navigate to the view defined 
        /// in <c>e</c>.  This can be overridden for additional functionality.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected virtual void OnContextNodeSelected(object source, SAHLTreeNodeEventArgs e)
        {
            //CBOContextNode contextNode = _view.CBOManager.GetCBOContextNodeByKey(_view.CurrentPrincipal, e.TreeNode.Value.ToString(), _nodeSet);
            //_view.CBOManager.SetCurrentContextNode(_view.CurrentPrincipal, contextNode, _nodeSet);
            ////_selectedContextNode = contextNode;
            //string navigateTo = e.TreeNode.NavigateValue;

            //if (_nodeSet == CBONodeSetType.X2 && contextNode != null)
            //{
            //    IX2Service x2Service = ServiceFactory.GetService<IX2Service>();

            //    if (contextNode.IsActivity)
            //        x2Service.StartActivity(_view.CurrentPrincipal, contextNode.GenericKey, contextNode.Description, null, false);

            //    if (contextNode.URL != null && contextNode.URL != "")
            //        navigateTo = contextNode.URL;
            //    else
            //    {
            //        x2Service.CompleteActivity(_view.CurrentPrincipal, null, false);
            //        //you will need to do a refresh, if the state is still on the persons todo list, you'll show the first form on the new state, if its not you navigate to the todo list
            //        //you'll basically have to look at the instance id and worklist after navigating, if the instance is still in the worklist its no problem, else goto todo
            //        //don't forget to remove it from the cbo if its not

            //        InstanceNode node = _view.CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, _nodeSet) as InstanceNode;
            //        IWorkList iwl = x2Service.GetWorkListItemByInstanceID(_view.CurrentPrincipal, node.GenericKey);

            //        if (iwl != null)
            //        {
            //            _view.CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, node, _nodeSet);
            //            _view.CBOManager.SetCurrentContextNode(_view.CurrentPrincipal, null, _nodeSet);
            //            //_selectedNode = node;
            //            //_selectedContextNode = null;
            //            navigateTo = x2Service.GetURLForCurrentState(node.GenericKey);
            //        }
            //        else
            //        {
            //            //remove from cbo, navigate to worklist
            //            _view.CBOManager.RemoveCBOMenuNode(_view.CurrentPrincipal, node.NodePath, _nodeSet);
            //            navigateTo = "X2WorkList";
            //        }
            //    }
            //}

            //DoNavigate(navigateTo);
        }

        /// <summary>
        /// Navigates to a url, but also flags the page as being a menu postback.  This also handles configuration 
        /// errors and raises them as a normal application error so the user doesn't get a white screen.  When a 
        /// configuration error occurs, the user's menu settings are cleared from the CBO service.
        /// </summary>
        /// <param name="navigateUrl"></param>
        protected void DoNavigate(string navigateUrl)
        {
            if (_view != null)
            {
                try
                {
                    _view.Navigator.Navigate(navigateUrl);
                    _view.IsMenuPostBack = true;
                }
                catch (System.Configuration.ConfigurationException ce)
                {
                    //CBONodeSetType nodeSet = _view.CBOManager.GetCurrentNodeSetName(_view.CurrentPrincipal);
                    _view.CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, null, _nodeSet);
                    throw new Exception(navigateUrl + " is not a valid configuration value.", ce);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Navigate"/> event.
        /// </summary>
        /// <param name="navigateValue"></param>
        public void OnNavigate(string navigateValue)
        {
            if (Navigate != null)
                Navigate(this, new SAHL.Common.Web.UI.Events.NavigateEventArgs(navigateValue));
        }


        #region Event Handlers

        protected void _treeViewMain_NodeIconClicked(object source, SAHLTreeNodeEventArgs e)
        {
            if (e.IconIndex == 0)
            {
                _view.CBOManager.RemoveCBOMenuNode(_view.CurrentPrincipal, e.TreeNode.Value.ToString(), _nodeSet);
                //_view.CBOManager.ClearContextNodes(_view.CurrentPrincipal, _nodeSet);

                // navigate back to the default page
                List<CBONode> nodes = _view.CBOManager.GetMenuNodes(_view.CurrentPrincipal, _nodeSet);

                if (nodes.Count > 0)
                    DoNavigate(nodes[0].URL);

            }
        }

        protected void _treeViewMain_NodeSelected(object source, SAHLTreeNodeEventArgs e)
        {
            OnMenuNodeSelected(source, e);
        }

        protected void _treeViewContext_NodeSelected(object source, SAHLTreeNodeEventArgs e)
        {
            OnContextNodeSelected(source, e);
        }

        #endregion

    }

}
