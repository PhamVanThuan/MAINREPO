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
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Administration.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Controls;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Administration
{
    public partial class ContextMenu : SAHLCommonBaseView, SAHL.Web.Views.Administration.Interfaces.IViewContextMenu
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!ShouldRunPage) return;
        //}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!string.IsNullOrEmpty(Request.Form[btnFeature.UniqueID]))
                btnFeature.Text = Request.Form[btnFeature.UniqueID];
            if (!string.IsNullOrEmpty(Request.Form[btnShowParent.UniqueID]))
                btnShowParent.Text = Request.Form[btnShowParent.UniqueID];
            //if (!string.IsNullOrEmpty(Request.Form[txtURL.UniqueID]))
                txtURL.Text = Request.Form[txtURL.UniqueID];
            //if (!string.IsNullOrEmpty(Request.Form[txtSequence.UniqueID]))
                txtSequence.Text = Request.Form[txtSequence.UniqueID];
                txtKey.Text = Request.Form[txtKey.UniqueID];
                txtDesc.Text = Request.Form[txtDesc.UniqueID];
                txtParent.Text = Request.Form[txtParent.UniqueID];
                txtFeature.Text = Request.Form[txtFeature.UniqueID];
        }

        protected void tv_NodeSelected(object source, SAHL.Common.Web.UI.Events.SAHLTreeNodeEventArgs e)
        {
            if (null != OnTreeNodeSelected)
            {
                OnTreeNodeSelected(null, new KeyChangedEventArgs(e.TreeNode.Value));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool VisibleMaint { set { tblMaint.Visible = value; } }

        /// <summary>
        /// 
        /// </summary>
        public bool VisibleContextMenu { set { tblContextMenu.Visible = value; } }
        #region Tree
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Bind"></param>
        /// <param name="TopLevelKey"></param>
        public void BindContextMenu(List<IBindableTreeItem> Bind, int TopLevelKey)
        {
            BindTree(Bind, TopLevelKey, tv);
            BindTree(Bind, TopLevelKey, tvParent);
        }
        
        public void BindFeatures(List<IBindableTreeItem> Bind, int TopLevelKey)
        {
            BindTree(Bind, TopLevelKey, tvFeature);
        }

        private void BindTree(List<IBindableTreeItem> Bind, int TopLevelKey, SAHLTreeView p_tv)
        {
            foreach (IBindableTreeItem f in Bind)
            {
                // add the top level node if no parent else add child
                if (f.ParentKey <= TopLevelKey)
                {
                    AddTopLevelNode(f, p_tv);
                }
                else
                {
                    bool Found = false;
                    foreach (SAHLTreeNode node in p_tv.Nodes)
                    {
                        AddChildNode(f, node, ref Found, p_tv);
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
                    BindTree(f.Children, TopLevelKey, p_tv);
                }
            }
        }

        // marked as static as this method does not use "this"
        private static void AddTopLevelNode(IBindableTreeItem o, SAHLTreeView p_tv)
        {
            SAHLTreeNode tn = new SAHLTreeNode(o.Desc, o.Key.ToString());
            p_tv.Nodes.Add(tn);
        }

        private void AddChildNode(IBindableTreeItem o, SAHLTreeNode node, ref bool Found, SAHLTreeView p_tv)
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
                    AddChildNode(o, Childnode, ref Found, p_tv);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        public void BindSingleContextMenu(BindableContextMenu menu)
        {
            txtKey.Text = menu.Key.ToString();
            txtDesc.Text = menu.Desc;
            txtParent.Text = menu._ParentDesc;
            txtURL.Text = menu._URL;
            txtSequence.Text = menu._Sequence.ToString();
            txtFeature.Text = menu._FeatureDesc;
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnTreeNodeSelected;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitClick;

        protected void tv_ParentNodeSelected(object source, SAHLTreeNodeEventArgs e)
        {
            if (null != OnParentSelected)
            {
                OnParentSelected(null, new KeyChangedEventArgs(e.TreeNode.Value));
            }
        }

        public event EventHandler OnParentClick;
        public string ParentButton { set { btnShowParent.Text = value; } get { return btnShowParent.Text; } }
        public event EventHandler OnParentSelected;
        public string ParentText { set { txtParent.Text = value; }  }
        public bool VisibleParent
        {
            set
            {
                trParentHeader.Visible = value;
                trParentTree.Visible = value;
                tblContextMenu.Visible = !value;
            }
        }
        public bool VisibleFeature
        {
            set
            {
                trFeatureHead.Visible = value;
                trFeatureTree.Visible = value;
                tblContextMenu.Visible = !value;
            }
        }
        public string FeatureButtonText { get { return btnFeature.Text; } set { btnFeature.Text = value; } }
        public string FeatureText { set { txtFeature.Text = value; } }
        public int ContextKey { get { return Convert.ToInt32(txtKey.Text); } }
        public string Description { get { return txtDesc.Text; } }
        public int Sequence { get { return Convert.ToInt32(txtSequence.Text); } }
        public string URL { get { return txtURL.Text; } }
        public string SubmitText { set { btnSubmit.Text = value; } }
        public event EventHandler OnFeatureTreeNodeSelected;
        public event EventHandler OnFeatureButtonClicked;

        // TODO: complete this method and remove supress message when parameters are used
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Method not implemented")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Method not implemented")]
        public void BindFeatures(List<BindableFeature> Bind, int TopLevelKey)
        {
        }

        protected void btnShowParent_Click(object sender, EventArgs e)
        {
            if (null != OnParentClick)
            {
                OnParentClick(null, null);
            }
        }

        protected void tv_FeatureNodeSelected(object source, SAHLTreeNodeEventArgs e)
        {
            if (null != OnFeatureTreeNodeSelected)
            {
                OnFeatureTreeNodeSelected(null, new KeyChangedEventArgs(e.TreeNode.Value));
            }
        }

        protected void btnShowFeature(object sender, EventArgs e)
        {
            if (null != OnFeatureButtonClicked)
            {
                OnFeatureButtonClicked(null, null);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (null != OnSubmitClick)
            {
                OnSubmitClick(null, null);
            }
        }
    }
}
