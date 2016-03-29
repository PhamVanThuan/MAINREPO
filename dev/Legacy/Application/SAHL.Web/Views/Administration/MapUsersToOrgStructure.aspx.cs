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
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration
{
    public partial class MapUsersToOrgStructure : SAHLCommonBaseView, IViewMapUsersToOrgStructure
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!ShouldRunPage) return;
        //}

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (null != OnAddClick)
            {
                OnAddClick(sender, new KeyChangedEventArgs(liNotIn.SelectedItem.Value));
            }
        }

        protected void btnREmove_Click(object sender, EventArgs e)
        {
            if (null != OnRemoveClick)
            {
                OnRemoveClick(sender, new KeyChangedEventArgs(liIn.SelectedItem.Value));
            }
        }

        protected void tv_NodeSelected(object source, SAHL.Common.Web.UI.Events.SAHLTreeNodeEventArgs e)
        {
            if (null != OnTreeNodeSeleced)
            {
                int SelectedKey = Convert.ToInt32(e.TreeNode.Value);
                OnTreeNodeSeleced(source, new KeyChangedEventArgs(SelectedKey));
            }
        }
        #region View Members
        #region tree
        /// <summary>
        /// Binds a list of ORganisationSTructures to a tree view. The TopLevel key is the the parentkey value that the
        /// view will accept as top level tree nodes.
        /// </summary>
        /// <param name="OrganisationStructure"></param>
        /// <param name="TopLevelKey"></param>
        public void BindOrganisationStructure(System.Collections.Generic.List<IBindableTreeItem> OrganisationStructure, int TopLevelKey)
        {
            foreach (IBindableTreeItem f in OrganisationStructure)
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
                    BindOrganisationStructure(f.Children, TopLevelKey);
                }
            }
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

        //private void SelectLastSelectedNode(SAHLTreeNodeCollection nodes, SAHLTreeNode tnSelected)
        //{
        //    foreach (SAHLTreeNode tnTmp in nodes)
        //    {
        //        // check to see if this node has children
        //        if (tnTmp.Nodes.Count > 0)
        //        {
        //            SelectLastSelectedNode(tnTmp.Nodes, tnSelected);
        //        }
        //        // continue looping throug the children of this node.
        //        if (tnTmp.Value == tnSelected.Value)
        //        {
        //            // we have the node to select
        //            break;
        //        }
        //    }
        //}
        #endregion
        /// <summary>
        /// Fires when a node in the tree is seleced
        /// </summary>
        public event EventHandler OnTreeNodeSeleced;
        /// <summary>
        /// Sets the add remove section of the page to visible / not
        /// </summary>
        public bool VisibleMaint { set { tblMaint.Visible = value; } }
        /// <summary>
        /// Sets the add remove buttons to be visible or not
        /// </summary>
        public bool VisibleButtons { set { btnAdd.Visible = value; btnREmove.Visible = value; } }
        /// <summary>
        /// Fires when an ADUser is added to an org structure
        /// </summary>
        public event EventHandler OnAddClick;
        /// <summary>
        /// Fires when an ADUser is removed from an organisation structure
        /// </summary>
        public event EventHandler OnRemoveClick;

        public void BindMapping(List<ADUserBind> In, List<ADUserBind> NotIn)
        {
            liIn.DataSource = In;
            liIn.DataTextField = "ADUserName";
            liIn.DataValueField = "AdUserKey";
            liIn.DataBind();

            liNotIn.DataSource = NotIn;
            liNotIn.DataTextField = "ADUserName";
            liNotIn.DataValueField = "AdUserKey";
            liNotIn.DataBind();
        }
        #endregion
    }
}
