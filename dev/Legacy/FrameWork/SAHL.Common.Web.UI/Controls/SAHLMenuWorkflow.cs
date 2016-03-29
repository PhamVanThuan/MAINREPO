using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Views;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace SAHL.Common.Web.UI.Controls
{
    /// <summary>
    /// Control for displaying the SAHL workflow menus.  This uses two <see cref="SAHLTreeView"/> controls.
    /// </summary>
    public class SAHLMenuWorkflow : SAHLMenuCBOBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (_nodeSet == CBONodeSetType.X2)
                BaseInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (_nodeSet == CBONodeSetType.X2)
                BaseLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (_nodeSet == CBONodeSetType.X2)
                BasePreRender(e);
        }

        protected override void OnMenuNodeSelected(object source, SAHLTreeNodeEventArgs e)
        {
            if (_nodeSet == CBONodeSetType.X2)
            {
                CBOMenuNode node = _view.CBOManager.GetCBOMenuNodeByKey(_view.CurrentPrincipal, e.TreeNode.Value.ToString(), _nodeSet);
                _view.CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, node, _nodeSet);
                _selectedContextNode = _view.CBOManager.UpdateContextNodeSelection(_view.CurrentPrincipal, _nodeSet);

                string navigateTo = e.TreeNode.NavigateValue;
                //must select the default form for an instance node

                if (node is InstanceNode)
                {
                    if (_selectedContextNode != null)
                        navigateTo = _selectedContextNode.URL;
                }

                DoNavigate(navigateTo);
            }
        }

        protected override void OnContextNodeSelected(object source, SAHLTreeNodeEventArgs e)
        {
            if (_nodeSet == CBONodeSetType.X2)
            {
                CBOContextNode contextNode = _view.CBOManager.GetCBOContextNodeByKey(_view.CurrentPrincipal, e.TreeNode.Value.ToString(), _nodeSet);
                _view.CBOManager.SetCurrentContextNode(_view.CurrentPrincipal, contextNode, _nodeSet);
                string navigateTo = e.TreeNode.NavigateValue;

                if (contextNode != null)
                {
                    IX2Service x2Service = ServiceFactory.GetService<IX2Service>();

                    try
                    {
                        if (contextNode.IsActivity)
                            x2Service.StartActivity(_view.CurrentPrincipal, contextNode.GenericKey, contextNode.Description, null, false);
                    }
                    catch (X2ActivityStartFailedException)
                    {
                        _view.CBOManager.SetCurrentContextNode(_view.CurrentPrincipal, null, _nodeSet);
                        throw;
                    }

                    if (!String.IsNullOrEmpty(contextNode.URL))
                        navigateTo = contextNode.URL;
                    else
                    {
                        x2Service.CompleteActivity(_view.CurrentPrincipal, null, false);
                        //you will need to do a refresh, if the state is still on the persons todo list, you'll show the first form on the new state, if its not you navigate to the todo list
                        //you'll basically have to look at the instance id and worklist after navigating, if the instance is still in the worklist its no problem, else goto todo
                        //don't forget to remove it from the cbo if its not
                        InstanceNode node = _view.CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, _nodeSet) as InstanceNode;
                        node.Refresh();

                        IWorkList iwl = x2Service.GetWorkListItemByInstanceID(_view.CurrentPrincipal, node.GenericKey);

                        if (iwl != null)
                        {
                            //_view.CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, node, _nodeSet);
                            _view.CBOManager.SetCurrentContextNode(_view.CurrentPrincipal, null, _nodeSet);
                            navigateTo = x2Service.GetURLForCurrentState(node.GenericKey);
                        }
                        else
                        {
                            //remove from cbo, navigate to worklist
                            _view.CBOManager.RemoveCBOMenuNode(_view.CurrentPrincipal, node.NodePath, _nodeSet);
                            navigateTo = "X2WorkList";
                        }
                    }
                }

                DoNavigate(navigateTo);
            }
        }
    }
}