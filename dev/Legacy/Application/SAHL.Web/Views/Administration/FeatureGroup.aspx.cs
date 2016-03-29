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
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Administration
{
    public partial class FeatureGroup : SAHLCommonBaseView, IViewFeatureGroup
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!ShouldRunPage) return;
        //}

        #region IView
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FeatureGroups"></param>
        public void BindADUserGroup(List<BindableFeatureGroup> FeatureGroups)
        {
            ddlFeatureGroup.DataSource = FeatureGroups;
            ddlFeatureGroup.DataTextField = "ADUserGroup";
            ddlFeatureGroup.DataValueField = "ADUserGroup";
            ddlFeatureGroup.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnFeatureGroupChanged;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitClick;
        /// <summary>
        /// 
        /// </summary>
        public bool VisibleSummaryAccess { set { tblSummary.Visible = value; } }
        /// <summary>
        /// 
        /// </summary>
        public bool VisibleTree
        {
            set { tblTree.Visible = value; }

        }
        #region Treebind
        public void BindFeatureTree(List<IBindableTreeItem> Features)
        {
            // loop through all the features and add them as we go along. Once we add them, add their kids if they have

            foreach (IBindableTreeItem f in Features)
            {
                // add the top level node if no parent else add child
                if (f.ParentKey < 0)
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
                }

                // now we have added the parent we can add their kids by recursing back in here.
                if (f.Children.Count > 0)
                {
                    BindFeatureTree(f.Children);
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

        #endregion
        #endregion
        protected void ddlFeature_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null != OnFeatureGroupChanged)
            {
                OnFeatureGroupChanged(null, new KeyChangedEventArgs(ddlFeatureGroup.SelectedValue));
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
