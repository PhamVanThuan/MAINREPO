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
using SAHL.Web.Views.Life.Interfaces;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.Life
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FAIS : SAHLCommonBaseView,IFAIS
    {
        private bool _confirmationRequired;
        private string _contactNumber;
        private bool _confirmationMode;
        private bool _activityDone;
        private bool _allOptionsChecked;
        private bool _contactOptionError;
        private bool _contactNumberError;
        private int _lifePolicyTypeKey;

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

            if (_confirmationMode == true)
            {
                pnlConfirm.Visible = false;
                lblPageHeader.Text += " ( CONFIRMATION )";

                tblText.Rows[5].Visible = true; // FAIS 5 - Policy Document
                tblText.Rows[6].Visible = false;  // FAIS 6 - Choice
                tblText.Rows[7].Visible = false;  // FAIS 9 - Confirm
                tblText.Rows[8].Visible = false;  // FAIS 10 - Contact
                tblText.Rows[9].Visible = false;   // FAIS 11 - Protection
                tblText.Rows[10].Visible = false; // FAIS 12 - Commencement
                tblText.Rows[11].Visible = false; // FAIS 13 - Readvance
                tblText.Rows[12].Visible = true; // FAIS 14 - Complete
                tblText.Rows[13].Visible = true; // FAIS 15 - Contacted

                if (_lifePolicyTypeKey==(int)SAHL.Common.Globals.LifePolicyTypes.AccidentOnlyCover)
                    tblText.Rows[14].Visible = false; // FAIS 16 - Free Cover


                txtPhone.Visible = false;        
            }
            else
            {
                pnlConfirm.Visible = true;
                tblText.Rows[13].Visible = false; // FAIS 15 - Contacted

                if (rbnsConfirmation.Items[1].Selected) // Yes
                {
                    tblText.Rows[7].Visible = true;  // FAIS 9 - Confirm
                    tblText.Rows[8].Visible = true;  // FAIS 10 - Contact
                    tblText.Rows[9].Visible = true;  // FAIS 11 - Protection
                    tblText.Rows[10].Visible = false; // FAIS 12 - Free Cover
                    tblText.Rows[11].Visible = false; // FAIS 13 - Readvance
                    tblText.Rows[12].Visible = false; // FAIS 14 - Complete
                    if (_lifePolicyTypeKey == (int)SAHL.Common.Globals.LifePolicyTypes.AccidentOnlyCover)
                        tblText.Rows[14].Visible = true; // FAIS 16 - Free Cover

                }
                else if (rbnsConfirmation.Items[0].Selected) // No
                {
                    tblText.Rows[7].Visible = false;  // FAIS 9 - Confirm
                    tblText.Rows[8].Visible = false;  // FAIS 10 - Contact
                    tblText.Rows[9].Visible = false;  // FAIS 11 - Protection
                    tblText.Rows[10].Visible = true; // FAIS 12 - Free Cover
                    tblText.Rows[11].Visible = true; // FAIS 13 - Readvance
                    tblText.Rows[12].Visible = true; // FAIS 14 - Complete
                    if (_lifePolicyTypeKey == (int)SAHL.Common.Globals.LifePolicyTypes.AccidentOnlyCover)
                        tblText.Rows[14].Visible = false; // FAIS 16 - Free Cover
                }
                else
                {
                    tblText.Rows[7].Visible = false;  // FAIS 9 - Confirm
                    tblText.Rows[8].Visible = false;  // FAIS 10 - Contact
                    tblText.Rows[9].Visible = false;  // FAIS 11 - Protection
                    tblText.Rows[10].Visible = false; // FAIS 12 - Free Cover
                    tblText.Rows[11].Visible = false; // FAIS 13 - Readvance
                    tblText.Rows[12].Visible = false; // FAIS 14 - Complete
                    if (_lifePolicyTypeKey == (int)SAHL.Common.Globals.LifePolicyTypes.AccidentOnlyCover)
                        tblText.Rows[14].Visible = false; // FAIS 16 - Free Cover
                }
            }
        }

        #region IFAIS Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnNextButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public bool ConfirmationRequired
        {
            get { return _confirmationRequired; }
            set { _confirmationRequired = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ContactNumber
        {
            get { return _contactNumber; }
            set { _contactNumber = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ConfirmationMode
        {
            get { return _confirmationMode; }
            set { _confirmationMode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ActivityDone
        {
            get { return _activityDone; }
            set { _activityDone = value; }
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
        /// 
        /// </summary>
        public bool ContactOptionError
        {
            get { return _contactOptionError; }
            set { _contactOptionError = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ContactNumberError
        {
            get { return _contactNumberError; }
            set { _contactNumberError = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int LifePolicyTypeKey
        {
            get { return _lifePolicyTypeKey; }
            set { _lifePolicyTypeKey = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textItems"></param>
        public void BindFAIS(IReadOnlyEventList<ITextStatement> textItems)
        {
            if (textItems.Count > 0)
            {
                TableRow spacer = new TableRow();
                spacer.Height = new Unit(5);
                tblText.Rows.Add(spacer);

                for (int i = 0; i < textItems.Count; i++)
                {
                    if (textItems[i].StatementTitle == "FAIS 7 - Confirm Choice 1" || textItems[i].StatementTitle == "FAIS 8 - Confirm Choice 2")
                    {
                        if (textItems[i].StatementTitle == "FAIS 7 - Confirm Choice 1")
                            rbnsConfirmation.Items[0].Text = textItems[i].Statement;
                        else
                            rbnsConfirmation.Items[1].Text = textItems[i].Statement;

                        continue;
                    }

                    TableRow row = new TableRow();
                    row.Cells.Add(new TableCell());
                    row.Cells.Add(new TableCell());

                    CheckBox cbx = new CheckBox();
                    cbx.ID = "cbx" + i.ToString();

                    if (_activityDone)
                    {
                        cbx.Checked = true;
                        cbx.Enabled = false;
                        if (_confirmationMode == false)
                        {
                            rbnsConfirmation.Enabled = false;
                            if (_confirmationRequired) 
                            {
                                rbnsConfirmation.Items[1].Selected = true;
                                txtPhone.Text = _contactNumber;
                                txtPhone.Enabled = false;
                            }
                            else
                            {
                                rbnsConfirmation.Items[0].Selected = true;
                            }
                        }
                    }

                    Label lbl = new Label();
                    lbl.ID = "lbl" + i.ToString();
                    lbl.Text = textItems[i].Statement;
                    lbl.Font.Size = new FontUnit(FontSize.Small);

                    // add checkbox control to table cell - not for 'FAIS 6 - Choice'
                    if (textItems[i].StatementTitle != "FAIS 6 - Choice")
                        row.Cells[0].Controls.Add(cbx);

                    row.Cells[0].VerticalAlign = VerticalAlign.Top;
                    row.Cells[1].Controls.Add(lbl);
                    row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                    row.Cells[1].VerticalAlign = VerticalAlign.Top;
                    row.Height = new Unit(40);

                    if (_confirmationMode==false)
                    {
                        switch (textItems[i].StatementTitle)
                        {
                            case "FAIS 6 - Choice" : 
                                tblText.Rows.Add(spacer);
                                row.Cells[1].Controls.Add(pnlConfirm);
                                break;
                            case "FAIS 10 - Contact":
                                txtPhone.Style.Add("margin-left", "10px");
                                row.Cells[1].Controls.Add(txtPhone);
                                break;
                            default:
                                break;
                        }
                    }

                    tblText.Rows.Add(row);
                }
            }

            StringBuilder JS = new StringBuilder();
            JS.AppendLine("function ValidateCheckBoxes()");
            JS.AppendLine("{");
            JS.AppendLine("var form = document.getElementById('aspnetForm');");
            JS.AppendLine("var parent = document.getElementById('tblTextStatements');");
            JS.AppendLine("var elements = parent.getElementsByTagName('input');");
            JS.AppendLine("var hiddenfield = form['" + AllChecked.UniqueID + "'];");
            JS.AppendLine("hiddenfield.value = true;");
            JS.AppendLine("for (var i = 0; i < elements.length; i++)");
            JS.AppendLine("{");
            JS.AppendLine("var ctl = elements[i];");
            JS.AppendLine("if (ctl.tagName.toLowerCase() == 'input' && ctl.getAttribute('type').toLowerCase() == 'checkbox')");
            JS.AppendLine("{");
            JS.AppendLine("if (!ctl.checked)");
            JS.AppendLine("{");
            JS.AppendLine("hiddenfield.value = false;");
            JS.AppendLine("break;");
            JS.AppendLine("}");
            JS.AppendLine("}");
            JS.AppendLine("}");
            JS.AppendLine("}");
            if (!Page.ClientScript.IsClientScriptBlockRegistered("ValidateCheckBoxes"))
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ValidateCheckBoxes", JS.ToString(), true);

        }


        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            _allOptionsChecked = true;
            _contactOptionError = false;
            _contactNumberError = false;

            if (_confirmationMode == false && rbnsConfirmation.Items[1].Selected && txtPhone.Text.Trim().Length < 1)
            {
                _contactNumberError = true;
            }
            else if (AllChecked.Value == "false")
            {
                _allOptionsChecked = false;
            }
            else if (_confirmationMode == false && rbnsConfirmation.SelectedIndex < 0)
            {
                _contactOptionError = true;
            }
            else
            {
                if (_confirmationMode == false)
                {
                    if (rbnsConfirmation.Items[1].Selected)
                    {
                        _confirmationRequired = true;
                        _contactNumber = txtPhone.Text.Trim();
                    }
                    else
                    {
                        _confirmationRequired = false;
                        _contactNumber = "";
                    }
                }

                _activityDone = true;
            }

            OnNextButtonClicked(sender, e);
        }
    }
}
