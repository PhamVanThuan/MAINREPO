using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Views;

namespace SAHL.Common.Web.UI.Controls
{
    /// <summary>
    /// Control for displaying the SAHL CBO menus.  This uses two <see cref="SAHLTreeView"/> controls.
    /// </summary>
    public class SAHLMenuCBO : SAHLMenuCBOBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (_nodeSet == CBONodeSetType.CBO)
                BaseInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (_nodeSet == CBONodeSetType.CBO)
            {
                if (_selectedNode == null)
                {
                    List<CBONode> nodes = _view.CBOManager.GetMenuNodes(_view.CurrentPrincipal, _nodeSet);

                    // iterate the top level nodes to check for a match to the currently loading page
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        if (nodes[i].URL == _view.ViewName)
                        {
                            _view.CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, nodes[i] as CBOMenuNode, _nodeSet);
                            _selectedNode = nodes[i];
                            break;
                            //return;
                        }
                    }
                }

                BaseLoad(e);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (_nodeSet == CBONodeSetType.CBO)
                BasePreRender(e);
        }

        protected override void OnMenuNodeSelected(object source, SAHLTreeNodeEventArgs e)
        {
            if (_nodeSet == CBONodeSetType.CBO)
            {
                CBOMenuNode node = _view.CBOManager.GetCBOMenuNodeByKey(_view.CurrentPrincipal, e.TreeNode.Value.ToString(), _nodeSet);
                _view.CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, node, _nodeSet);
                List<CBONode> contextNodes = _view.CBOManager.GetContextNodes(_view.CurrentPrincipal, _nodeSet);

                if (contextNodes.Count > 0)
                    DoNavigate(contextNodes[0].URL);
                else
                    DoNavigate(e.TreeNode.NavigateValue);
            }
        }

        protected override void OnContextNodeSelected(object source, SAHLTreeNodeEventArgs e)
        {
            if (_nodeSet == CBONodeSetType.CBO)
            {
                CBOContextNode node = _view.CBOManager.GetCBOContextNodeByKey(_view.CurrentPrincipal, e.TreeNode.Value.ToString(), _nodeSet);
                _view.CBOManager.SetCurrentContextNode(_view.CurrentPrincipal, node, _nodeSet);
                DoNavigate(e.TreeNode.NavigateValue);
            }
        }
    }
}