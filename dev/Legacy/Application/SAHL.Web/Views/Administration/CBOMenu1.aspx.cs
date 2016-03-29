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
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration
{
    public partial class CBOMenu2 : SAHLCommonBaseView, IViewCBOMenu1
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void tv_NodeSelected(object source, SAHL.Common.Web.UI.Events.SAHLTreeNodeEventArgs e)
        {
            int Key = Convert.ToInt32(e.TreeNode.Value);
            if (null != OnTreeSelected)
            {
                OnTreeSelected(null, new KeyChangedEventArgs(Key));
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (null != OnNextClick)
            {
                OnNextClick(null, null);
            }
        }

        public event EventHandler OnNextClick;
        public event EventHandler OnTreeSelected;

        #region Tree
        public void BindFeatureList(List<IBindableTreeItem> Features, int TopLevelKey, int Selected)
        {
            foreach (IBindableTreeItem f in Features)
            {
                // add the top level node if no parent else add child
                if (f.ParentKey <= TopLevelKey)
                {
                    AddTopLevelNode(f);
                }
                else
                {
                    bool Found = false;
                    foreach (SAHLTreeNode node in tv.Nodes)
                    {
                        AddChildNode(f, node, ref Found);
                        if (Found)
                            break;
                    }
                    if (!Found)
                    {
                        // string WTF = "?";
                    }
                }

                // now we have added the parent we can add their kids by recursing back in here.
                if (f.Children.Count > 0)
                {
                    BindFeatureList(f.Children, TopLevelKey, Selected);
                }
            }
#warning select the node
        }
        private void AddTopLevelNode(IBindableTreeItem o)
        {
            SAHLTreeNode tn = new SAHLTreeNode(o.Desc, o.Key.ToString());
            tv.Nodes.Add(tn);
        }

        private void AddChildNode(IBindableTreeItem o, SAHLTreeNode node, ref bool Found)
        {
            // if we found it inside the recurse then get outta here.
            if (Found) return;
            // loop through all the nodes that have been added so far and find the one that is 
            // the parent of the node we are trying to add. Once we find it add it to the newly
            // found nodes children.

            // recurse the children
            foreach (SAHLTreeNode Childnode in node.Nodes)
            {
                if (Childnode.Nodes.Count > 0)
                {
                    AddChildNode(o, Childnode, ref Found);
                }
                else
                {
                    if (Childnode.Value == o.ParentKey.ToString())
                    {
                        Childnode.Nodes.Add(new SAHLTreeNode(o.Desc, o.Key.ToString()));
                        Found = true;
                        return;
                    }
                }
                //node.Nodes ??? somehow we GetDataItem this rught
            }
            if (node.Value == o.ParentKey.ToString())
            {
                node.Nodes.Add(new SAHLTreeNode(o.Desc, o.Key.ToString()));
                Found = true;
                return;
            }

        }
        #endregion

    }
}
