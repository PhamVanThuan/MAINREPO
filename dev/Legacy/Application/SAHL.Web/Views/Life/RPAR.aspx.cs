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

using Microsoft.ApplicationBlocks.UIProcess;

using SAHL.Common;
using System.Text;
using System.Web.Configuration;
using SAHL.Common.Authentication;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Diagnostics.CodeAnalysis;


namespace SAHL.Web.Views.Life
{
    public partial class RPAR : SAHLCommonBaseView, IRPAR
    {
        private bool _showReplacePolicyConditions;
        private bool _otherInsurerVisible;
        private bool _allOptionsChecked;
        private string _otherInsurerName = "";
        private string _rparPolicyNumber = "";
        private int _rparInsurerKey = -1;
        private string _rparInsurer = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!this.ShouldRunPage)
                return;
            
            lblAssurer.Visible = _showReplacePolicyConditions;
            lblPolicyNumber.Visible = _showReplacePolicyConditions;
            lblAware.Visible = _showReplacePolicyConditions;
            lblContinue.Visible = _showReplacePolicyConditions;
            ddlAssurer.Visible = _showReplacePolicyConditions;
            txtPolicyNumber.Visible = _showReplacePolicyConditions;
            tblText.Visible = _showReplacePolicyConditions;
            btnNTU.Visible = _showReplacePolicyConditions;
            btnConsider.Visible = _showReplacePolicyConditions;

            if (_showReplacePolicyConditions)
                btnNext.Text = "Accept/Next";
            else
                btnNext.Text = "Next";

            if (String.Compare(ddlAssurer.SelectedItem.Text, "other", true) == 0)
            {
                lblOther.Visible = true;
                txtOther.Visible = true;
            }
            else
            {
                lblOther.Visible = false;
                txtOther.Visible = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblYesNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnRBNYesNoSelectedIndexChanged != null)
                OnRBNYesNoSelectedIndexChanged(sender, new KeyChangedEventArgs(rblYesNo.Items[0].Selected));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            _allOptionsChecked = true;
            if (rblYesNo.Items[0].Selected && HiddenField1.Value == "false" && rblYesNo.Enabled == true)
                _allOptionsChecked = false;

            if (_otherInsurerVisible)
                _otherInsurerName = txtOther.Text;
            else
                _otherInsurerName = "";

            if (rblYesNo.SelectedIndex == 0) // Yes
            {
                _rparPolicyNumber = txtPolicyNumber.Text;
                _rparInsurerKey = Convert.ToInt32(ddlAssurer.SelectedItem.Value);
            }
            else
            {
                _rparPolicyNumber = null;
                _rparInsurerKey = 0;
            }

            OnNextButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNTU_Click(object sender, EventArgs e)
        {
            OnNTUButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConsider_Click(object sender, EventArgs e)
        {
            OnConsideringButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlAssurer_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnDDLAssurerSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlAssurer.SelectedItem.Text));
        }

        #region IRPAR Members

        /// <summary>
        /// implements <see cref="IRPAR.OnNTUButtonClicked"/>
        /// </summary>
        public event EventHandler OnNTUButtonClicked;

        /// <summary>
        /// implements <see cref="IRPAR.OnConsideringButtonClicked"/>
        /// </summary>
        public event EventHandler OnConsideringButtonClicked;

        /// <summary>
        /// implements <see cref="IRPAR.OnNextButtonClicked"/>
        /// </summary>
        public event EventHandler OnNextButtonClicked;

        /// <summary>
        /// implements <see cref="IRPAR.OnDDLAssurerSelectedIndexChanged"/>
        /// </summary>
        public event KeyChangedEventHandler OnDDLAssurerSelectedIndexChanged;


        /// <summary>
        /// implements <see cref="IRPAR.OnRBNYesNoSelectedIndexChanged"/>
        /// </summary>
        public event KeyChangedEventHandler OnRBNYesNoSelectedIndexChanged;

        /// <summary>
        /// implements <see cref="IRPAR.ReplacePolicyControlsVisible"/>
        /// </summary>
        public bool ReplacePolicyControlsVisible
        {
            set { _showReplacePolicyConditions = value; }
            get { return _showReplacePolicyConditions; }
        }

        /// <summary>
        /// implements <see cref="IRPAR.OtherInsurerVisible"/>
        /// </summary>
        public bool OtherInsurerVisible
        {
            set { _otherInsurerVisible = value; }
            get { return _otherInsurerVisible ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AllOptionsChecked
        {
            get { return _allOptionsChecked; }
            set { _allOptionsChecked = value; }
        }

        /// <summary>
        /// implements <see cref="IRPAR.BindReplacePolicyConditions"/>
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="rparDone"></param>
        public void BindReplacePolicyConditions(IReadOnlyEventList<ITextStatement> conditions, bool rparDone)
        {
            _showReplacePolicyConditions = rparDone;

            if (_showReplacePolicyConditions)
            {
                rblYesNo.SelectedValue = "Yes";
                rblYesNo.Enabled = false;
                ddlAssurer.Enabled = false;
                txtOther.Enabled = false;
                txtPolicyNumber.Enabled = false;
            }

            txtPolicyNumber.Text = _rparPolicyNumber;
            ddlAssurer.SelectedValue = _rparInsurerKey.ToString();
            txtOther.Text = _otherInsurerName;

            StringBuilder JS = new StringBuilder();
            JS.AppendLine("function allChecked()");
            JS.AppendLine("{");
            JS.AppendLine("var form = document.getElementById('aspnetForm');");
            JS.AppendLine("var hid = form['" + HiddenField1.UniqueID + "'];");
            JS.AppendLine("hid.value = true;");
            JS.AppendLine("var cbx = null;");

            if (conditions.Count > 0)
            {
                TableRow spacer = new TableRow();
                spacer.Height = new Unit(5);
                tblText.Rows.Add(spacer);

                for (int i = 0; i < conditions.Count; i++)
                {
                    TableRow row = new TableRow();
                    row.Cells.Add(new TableCell());
                    row.Cells.Add(new TableCell());

                    CheckBox cbx = new CheckBox();
                    cbx.ID = "cbx" + i.ToString();

                    if (rparDone)
                    {
                        cbx.Checked = true;
                        cbx.Enabled = false;
                    }

                    Label lbl = new Label();
                    lbl.ID = "lbl" + i.ToString();
                    lbl.Text = conditions[i].Statement;

                    lbl.Font.Size = new FontUnit(FontSize.Small);

                    row.Cells[0].Controls.Add(cbx);
                    row.Cells[0].VerticalAlign = VerticalAlign.Top;
                    row.Cells[1].Controls.Add(lbl);
                    row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                    row.Cells[1].VerticalAlign = VerticalAlign.Top;
                    row.Height = new Unit(40);
                    tblText.Rows.Add(row);

                    cbx.Attributes.Add("onclick", "allChecked();");

                    JS.AppendLine("cbx = document.getElementById('" + cbx.ClientID + "');");
                    JS.AppendLine("if (cbx.checked == false) hid.value = false;");
                }
            }

            JS.AppendLine("}");
            if (!Page.ClientScript.IsClientScriptBlockRegistered("allChecked"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "allChecked", JS.ToString(), true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="insurers"></param>
        public void BindInsurers(IDictionary<string, string> insurers)
        {
            ddlAssurer.DataSource = insurers;
            ddlAssurer.DataBind();
        }
   

        /// <summary>
        /// 
        /// </summary>
        public string OtherInsurerName
        {
            get { return _otherInsurerName; }
            set { _otherInsurerName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RPARPolicyNumber
        {
            set { _rparPolicyNumber = value; }
            get { return _rparPolicyNumber; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int RPARInsurerKey
        {
            set { _rparInsurerKey = value; }
            get { return _rparInsurerKey; }

        }
        /// <summary>
        /// 
        /// </summary>
        public string RPARInsurer
        {
            set { _rparInsurer = value; }
            get { return _rparInsurer; }

        }
        #endregion
    }
}