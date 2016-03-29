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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common
{
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Method not implemented")]
    public partial class TransactionAccessMaintenance : SAHLCommonBaseView, ITransactionAccessMaintenance
    {
        #region locals

        private TreeView _tranTree;
        private StringBuilder _treeHtml;
        private StringBuilder _values;
        private StringBuilder _dada;
        private StringBuilder _subRows;
        private int _genTreeCounter;
        private string _postedChecks = "";
        //private string m_SelectedAccessGroup = "";
        private DataTable _transactionTypes;
        private bool _allowUpdate;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage)
                return;
            
            if (Request.Form["ttree_VALUES"] != null)
                _postedChecks = Request.Form["ttree_VALUES"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;

            ClientScript.RegisterClientScriptInclude("TreeScripts", Page.ResolveClientUrl("~/Scripts/HLTree.js"));

            //if (IsPostBack)
            //{
            //    string ctrlname = Page.Request.Params.Get("__EVENTTARGET");
            //    if (ctrlname != null && !String.IsNullOrEmpty(ctrlname))
            //    {
            //        if (ctrlname.EndsWith("TransactionType")) // reset the child ddl to all
            //            _reset = true;
            //        else if (ctrlname.EndsWith("btnNext")) // next button
            //            _paging = Paging.PageRequestType.Next;
            //        else if (ctrlname.EndsWith("btnPrevious")) // previous button
            //            _paging = Paging.PageRequestType.Previous;
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_postedChecks.Length == 0)
            {
                Messages.Add(new Error("Please select at least one transaction type.", "Please select at least one transaction type."));
                return;
            }
            
            OnSubmitButtonClicked(sender, e);
        }

        #region Tree Specific

        private void bindTree()
        {
            buildTree();
            _treeHtml = new StringBuilder();
            _values = new StringBuilder();
            _dada = new StringBuilder();
            _subRows = new StringBuilder();
            _genTreeCounter = 0;

            generateTreeHtml(_tranTree.Nodes);

            _treeHtml.AppendLine("<script language='javascript'>");
            _treeHtml.AppendLine("var oTTree = new HLTree();");

            if (_values.Length > 0)
                _treeHtml.AppendLine("var arValue = new Array(" + _values.ToString().Substring(0, _values.Length - 1) + ");");
            else
                _treeHtml.AppendLine("var arValue = new Array();");

            if (_dada.Length > 0)
                _treeHtml.AppendLine("var arDada = new Array(" + _dada.ToString().Substring(0, _dada.Length - 1) + ");");
            else
                _treeHtml.AppendLine("var arDada = new Array();");

            if (_subRows.Length > 0)
                _treeHtml.AppendLine("var arSubRows = new Array(" + _subRows.ToString().Substring(0, _subRows.Length - 1) + ");");
            else
                _treeHtml.AppendLine("var arSubRows = new Array();");

            _treeHtml.AppendLine("</script>");

            _treeHtml.AppendLine("<input type='hidden' id='ttree_VALUES' name='ttree_VALUES' value='" + _postedChecks + "'>");

            Literal lit = new Literal();
            lit.Text = _treeHtml.ToString();
            TranTreePanel.Controls.Add(lit);
        }

        private void buildTree()
        {
            TreeNode tn = null;
            _tranTree = new TreeView();

            int index = 0;
            // Loop through the data and build the tree by requesting the children of the root items.
            foreach (DataRow row in _transactionTypes.Rows)
            {
                tn = new TreeNode((row["TransactionTypeKey"] + " - " + row["Description"]).Trim(),
                    row["TransactionTypeKey"].ToString() + "|" +
                    row["TransactionType"] + "|" +
                    index++);

                if (Convert.ToBoolean(row["HasAccess"]))
                    tn.Checked = true;

                _tranTree.Nodes.Add(tn);
            }
        }

        private void generateTreeHtml(TreeNodeCollection tnc)
        {
            string dada = "-1";
            foreach (TreeNode tn in tnc)
            {
                _treeHtml.AppendLine("<div valign='center' id='ttree_ROW' name='ttree_ROW'>");
                _treeHtml.AppendLine("    <table border='0' cellspacing='0' cellpadding='0'>");
                _treeHtml.AppendLine("        <tr>");

                //m_TreeHtml.AppendLine(getTreeSpacers(tn.Depth));
                //m_TreeHtml.AppendLine("            <td valign='center'><img id='ttree_PLUS' name='ttree_PLUS' src='../../Images/Dot.png' border='0' style='cursor:hand;width:16px;height:16;'></td>");

                if (_postedChecks.Length > 0)
                {
                    // Check if this item was checked or unchecked client side.
                    if (_postedChecks.IndexOf("[" + _genTreeCounter + "];") > -1)
                        _treeHtml.Append(getTreeCheck(_genTreeCounter, true));
                    else
                        _treeHtml.Append(getTreeCheck(_genTreeCounter, false));
                }
                else
                    _treeHtml.Append(getTreeCheck(_genTreeCounter, tn.Checked));

                _treeHtml.Append("            <td id='ttree_FIELD' name='ttree_FIELD' valign='center' nowrap>");
                _treeHtml.AppendLine(tn.Text);
                _treeHtml.Append("            </td>\n");

                _treeHtml.AppendLine("        </tr>");
                _treeHtml.AppendLine("    </table>");
                _treeHtml.AppendLine("</div>");

                if (tn.Checked)
                    _values.Append("\"" + _genTreeCounter + "\",");
                else
                    _values.Append("\"_\",");

                //if (tn.Parent == null)
                dada = "-1";
                //else
                //{
                //    string[] vals = tn.Parent.Value.Split('|');
                //    dada = vals[7];
                //}

                _dada.Append("\"" + dada + "\",");
                //m_SubRows.Append("\"" + getSubRows(tn.ChildNodes) + "\",");
                _subRows.Append("\"_\",");

                _genTreeCounter++;
                generateTreeHtml(tn.ChildNodes);
            }
        }

        //private string getTreeSpacers(int depth)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    int iC;
        //    for (iC = 0; iC < depth; iC++)
        //    {
        //        sb.Append("<td valign='center'><img src='../../Images/Spacer.png' border='0' width='16' height='1'></td>");
        //    }
        //    return sb.ToString();
        //}

        private string getTreeCheck(int row, bool isChecked)
        {
            string ret = "";

            string simg = "../../Images/TCheckClean.gif";

            if (isChecked)
                simg = "../../Images/TCheck.gif";

            if (_allowUpdate)
                ret = "<td valign='center' onclick=\"oTTree.onHLRCheck(" + row + ",'ttree');setTreeChecks('ttree');\"><img id='ttree_CHECK' name='ttree_CHECK' src='" + simg + "' style='cursor:hand;'></td>";
            else
                ret = "<td valign='center'><img id='ttree_CHECK' name='ttree_CHECK' src='" + simg + "'></td>";

            return ret;
        }

        #endregion

        #region ITransactionAccessMaintenance Members
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groups"></param>
        public void BindGroupDropDown(DataTable groups)
        {
            ddlGroup.DataSource = groups;
            ddlGroup.DataValueField = "ADCredentials";
            ddlGroup.DataTextField = "ADCredentials";
            ddlGroup.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        public void BindUserDropDown(DataTable users)
        {
            ddlAdUser.DataSource = users;
            ddlAdUser.DataTextField = "ADUserName";
            ddlAdUser.DataValueField = "ADUserName";
            ddlAdUser.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionTypes"></param>
        public void BindTransactionAccessTree(DataTable transactionTypes)
        {
            _transactionTypes = transactionTypes;
            bindTree();
        }

        #region Properties


        /// <summary>
        /// 
        /// </summary>
        public bool ShowUser
        {
            set 
            {
                trUser.Visible = value;
                trGroup.Visible = !value; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ShowGroup
        {
            set
            {
                trGroup.Visible = value;
                trUser.Visible = !value;
                tbGroup.Visible = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ShowButton
        {
            set { trButton.Visible = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool AddGroup
        {
            set
            {
                tbGroup.Visible = value;
                ddlGroup.Visible = !value;
                trUser.Visible = !value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GroupSelectedValue
        {
            get { return ddlGroup.SelectedValue; }
            set { ddlGroup.SelectedValue = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserSelectedValue
        {
            get { return ddlAdUser.SelectedValue; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool AllowUpdate
        {
            //get { return _allowUpdate; }
            set { _allowUpdate = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CheckedTransactions
        {
            get { return _postedChecks; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NewGroupDescription
        {
            get { return tbGroup.Text; }
        }

        #endregion

        #endregion
    }
}
