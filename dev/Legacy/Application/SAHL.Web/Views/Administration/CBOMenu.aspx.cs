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
using System.Collections.Generic;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Administration
{
    public partial class CBOMenu : SAHLCommonBaseView, SAHL.Web.Views.Administration.Interfaces.IViewCBOMenu
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!ShouldRunPage) return;
        //}

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (null != OnNextClick)
            {
                OnNextClick(null, null);
            }
        }
        #region IView
        public event EventHandler OnTreeSelected;
        public void BindCBOList(List<IBindableTreeItem> CBONodes, int TopLevelKey)
        {
            foreach (IBindableTreeItem f in CBONodes)
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
                    BindCBOList(f.Children, TopLevelKey);
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
        public bool ShowAllCBO { set { tblCBO.Visible = value; } }
        public void BindCBO(ICBOMenu CBO)
        {
            txtDesc.Text = CBO.Description;
            txtExpandLevel.Text = CBO.ExpandLevel.ToString();
            txtMenuIcon.Text = CBO.MenuIcon;
            txtSequence.Text = CBO.Sequence.ToString();
            txtURL.Text = CBO.URL;
            ddlGenKeyTYpe.SelectedValue = CBO.GenericKeyType.Key.ToString();
            ddlHasOriginationSource.SelectedValue = CBO.HasOriginationSource.ToString();
            ddlIncludeParent.SelectedValue = CBO.IncludeParentHeaderIcons.ToString();
            ddlIsRemovable.SelectedValue = CBO.IsRemovable.ToString();
            ddlNodeType.SelectedValue = CBO.NodeType.ToString();
        }

        public event EventHandler OnNextClick;
        public void BindGenericKeyType(List<BindableGenericKeyType> Bind)
        {
            ddlGenKeyTYpe.DataSource = Bind;
            ddlGenKeyTYpe.DataTextField= "Desc";
            ddlGenKeyTYpe.DataValueField = "Key";
            ddlGenKeyTYpe.DataBind();
        }
        public string Desc { get { return txtDesc.Text; } }
        public string URL { get { return txtURL.Text; } }
        public char NodeType { get { return ddlNodeType.SelectedValue[0]; } }
        public int Sequence { get { return Convert.ToInt32(txtSequence.Text); } }
        public string MenuIcon { get { return txtMenuIcon.Text; } }
        public int GenericKeyTYpe { get { return Convert.ToInt32(ddlGenKeyTYpe.SelectedValue); } }
        public bool HasOriginationSource { get { return Convert.ToBoolean(ddlHasOriginationSource.SelectedItem.Text); } }
        public bool IsRemovable { get { return Convert.ToBoolean(ddlIsRemovable.SelectedItem.Text); } }
        public bool IncludeParentHeaderIcons { get { return Convert.ToBoolean(ddlIncludeParent.SelectedItem.Text); } }
        public string ExpandLevel { get { return txtExpandLevel.Text;} }
        public bool VisibleMaint { set { tblMaint.Visible = value; } }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public string UIStatementName { get { return ddlUIStatement.SelectedItem.Text; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Statements"></param>
        /// <param name="StatementName"></param>
        public void BindUIStatement(System.Collections.Generic.List<BindableUIStatement> Statements, string StatementName)
        {
            ddlUIStatement.DataSource = Statements;
            ddlUIStatement.DataTextField = "StatementName";
            ddlUIStatement.DataValueField = "Key";
            ddlUIStatement.DataBind();
            if (!String.IsNullOrEmpty(StatementName))
            {
                for (int i = 0; i < ddlUIStatement.Items.Count; i++)
                {
                    if (ddlUIStatement.Items[i].Text == StatementName)
                    {
                        ddlUIStatement.SelectedIndex = i;
                        break;
                    }
                }
            }
        }


        protected void tv_NodeSelected(object source, SAHL.Common.Web.UI.Events.SAHLTreeNodeEventArgs e)
        {
            if (null != OnTreeSelected)
            {
                OnTreeSelected(null, new KeyChangedEventArgs(e.TreeNode.Value));
            }
        }
    }
}
