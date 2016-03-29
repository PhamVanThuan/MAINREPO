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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration
{
    public partial class OrganisationStructure : SAHLCommonBaseView, IViewOrganisationStructure
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!ShouldRunPage) return;
        //}

        #region IViewOrganisationStructure Members
        public event EventHandler OnTreeNodeSeleced;

        public event EventHandler OnClearClicked;

        public event EventHandler OnSubmitClick;

        public void BindParentOrganisationStructure(BindOrganisationStructure OS, int SelectedValue)
        {
            // use Key to go find selected pupppy
            // SAHLTreeNode node = tv.FindNode(SelectedValue.ToString());
#warning Check this out
            //tv.SelectNode(node);

            if (OS.Key > 0)
            {
                hdParentKey.Value = OS.Key.ToString();
            }
        }

        public void BindSingleOrganisationStructure(BindOrganisationStructure OS, int SelectedValue)
        {
            // use Key to go find selected pupppy
            // SAHLTreeNode node = tv.FindNode(SelectedValue.ToString());
#warning Check this out
            //tv.SelectNode(node);

            hdKey.Value = OS.Key.ToString();
            txtDesc.Text = OS.Desc;
            if (OS.ParentKey > 0)
            {
                //txtParent.Text = OS.ParentDescription;
                hdParentKey.Value = OS.ParentKey.ToString();
            }
            // do the types
            ddlGeneralStatus.SelectedValue = OS.GeneralStatusKey.ToString();
            ddlOSType.SelectedValue = OS.OrganisationTypeKey.ToString();
        }
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

        public void BindLookups(IEventList<IGeneralStatus> status, IEventList<IOrganisationType> osType)
        {
            ddlGeneralStatus.DataSource = status.BindableDictionary;
            ddlGeneralStatus.DataTextField = "Description";
            ddlGeneralStatus.DataValueField = "Key";
            ddlGeneralStatus.DataBind();

            ddlOSType.DataSource = osType.BindableDictionary;
            ddlOSType.DataTextField = "Description";
            ddlOSType.DataValueField = "Key";
            ddlOSType.DataBind();
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

        protected void tv_NodeSelected(object source, SAHL.Common.Web.UI.Events.SAHLTreeNodeEventArgs e)
        {
            if (null != OnTreeNodeSeleced)
            {
                int SelectedKey = Convert.ToInt32(e.TreeNode.Value);
                OnTreeNodeSeleced(source, new KeyChangedEventArgs(SelectedKey));
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            //txtParent.Text = "";
            //hdParentKey.Value = "";            
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (null != OnSubmitClick)
            {
                OnSubmitClick(null, null);
            }
        }

        public string Desc { get { return txtDesc.Text; } }
        public int ParentKey { get { return Convert.ToInt32(hdParentKey.Value); } }
        public int Key { get { return Convert.ToInt32(hdKey.Value);} }
        public int OSType { get { return Convert.ToInt32(ddlOSType.SelectedItem.Value); } }
        public int GeneralStatusKey { get { return Convert.ToInt32(ddlGeneralStatus.Text); } }
        public bool VisibleMaint { set { tblMaint.Visible = value; } }
    }
}
