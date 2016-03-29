using System;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;


namespace SAHL.Web.Views.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CATSDisbursement : SAHLCommonBaseView, ICATSDisbursement
    {
        #region Local Variables
        List<IBankAccount> _bankAccountList;
        private bool _displayCancelConfirmationMessage;
        private bool _displaySubmitConfirmationMessage;
        private bool _displaySaveConfirmationMessage;
        private bool _displayDeleteConfirmationMessage;

        private string _cancelConfirmationMessage;
        private string _submitConfirmationMessage;
        private string _saveConfirmationMessage;
        private string _deleteConfirmationMessage;


        #endregion


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
            if (!ShouldRunPage)
                return;

            //setup confirmation warning messages if required
            if (_displayCancelConfirmationMessage)
                CancelButton.Attributes.Add("onclick", "return confirm('" + _cancelConfirmationMessage + "')");
            if (_displaySubmitConfirmationMessage)
                SubmitButton.Attributes.Add("onclick", "return confirm('" + _submitConfirmationMessage + "')");
            if (_displayDeleteConfirmationMessage)
                DeleteDisbursement.Attributes.Add("onclick", "return confirm('" + _deleteConfirmationMessage + "')");
            if (_displaySaveConfirmationMessage)
                SaveButton.Attributes.Add("onclick", "return confirm('" + _saveConfirmationMessage + "')");
        }




        /// <summary>
        /// 
        /// </summary>
        public bool DisplayControlsVisible
        {
            set
            {
                tblDisplay.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DisbursementTypeEnabled
        {
            set
            {
                ddlDisbursementType.Enabled = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int DisbursementTypeSelectedValue
        {
            set
            {
                ddlDisbursementType.SelectedValue = value.ToString();
            }
            get
            {
                if (!String.IsNullOrEmpty(ddlDisbursementType.SelectedValue) && ddlDisbursementType.SelectedValue != "-select-" && Convert.ToInt32(ddlDisbursementType.SelectedValue) > 0)
                    return Convert.ToInt32(ddlDisbursementType.SelectedValue);
                if (Request.Form[ddlDisbursementType.UniqueID] != null && Request.Form[ddlDisbursementType.UniqueID] != "-select-")
                    return Convert.ToInt32(Request.Form[ddlDisbursementType.UniqueID]);
                return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DisbursementTypeLabelText
        {
            set
            {
                lblTransactionType.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TotalAmountLabelText
        {
            set
            {
                lblDisbursementsTotal.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DisbursementGridVisible
        {
            set
            {
                tblGrid.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AddControlsVisible
        {
            set
            {
                tblAdd1.Visible = value;
                tblAdd2.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool RollbackControlsVisible
        {
            set
            {
                tblRollback.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CancelButtonVisible
        {
            set
            {
                CancelButton.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SubmitButtonVisible
        {
            set
            {
                SubmitButton.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SaveButtonVisible
        {
            set
            {
                SaveButton.Visible = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string SubmitButtonText
        {
            set
            {
                SubmitButton.Text = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool SubmitButtonEnabled
        {
            set
            {
                SubmitButton.Enabled = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DeleteButtonEnabled
        {
            set
            {
                DeleteDisbursement.Enabled = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SaveButtonEnabled
        {
            set
            {
                SaveButton.Enabled = value;
                SubmitButton.Enabled = !value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DisbursementTypeLableVisible
        {
            set
            {
                lblddlTransactionType.Visible = value;
                ddlDisbursementType.Visible = !value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public GridPostBackType DisbursementGridPostBackType
        {
            set
            {
                grdDisbursements.PostBackType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double DisbursementAmount
        {
            get
            {
                if (String.IsNullOrEmpty(txtDisbursementAmount.Text))
                    return 0;
                if (Convert.ToDouble(txtDisbursementAmount.Text) > 0)
                    return Convert.ToDouble(txtDisbursementAmount.Text);
                if (!String.IsNullOrEmpty(Request.Form[txtDisbursementAmount.UniqueID]))
                    return Convert.ToDouble(Request.Form[txtDisbursementAmount.UniqueID]);
                return 0;
            }
            set
            {
                txtDisbursementAmount.Text = value.ToString(SAHL.Common.Constants.NumberFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double DisbursementTotalAmount
        {
            get
            {
                if (!String.IsNullOrEmpty(txtTotalAmount.Text) && Convert.ToDouble(txtTotalAmount.Text) > 0)
                        return Convert.ToDouble(txtTotalAmount.Text);

                if (!String.IsNullOrEmpty(Request.Form[txtTotalAmount.UniqueID]))
                    return Convert.ToDouble(Request.Form[txtTotalAmount.UniqueID]);
                
                return 0;
            }
            set
            {
                txtTotalAmount.Text = value.ToString(SAHL.Common.Constants.NumberFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DisbursementReference
        {
            get
            {
                if (Request.Form[txtDisbursementReference.UniqueID] != null)
                    return Request.Form[txtDisbursementReference.UniqueID];
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SelectedBankAccount
        {
            get
            {
                if (ddlBankDetails.SelectedValue != "-select-" && !String.IsNullOrEmpty(ddlBankDetails.SelectedValue) && Convert.ToInt32(ddlBankDetails.SelectedValue) > 0)
                    return Convert.ToInt32(ddlBankDetails.SelectedValue);

                if (Request.Form[ddlBankDetails.UniqueID] != null && Request.Form[ddlBankDetails.UniqueID] != "-select-")
                    return Convert.ToInt32(Request.Form[ddlBankDetails.UniqueID]);

                return -1;

            }
            set
            {
                ddlBankDetails.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int LoanTransactionSelectedIndex
        {
            get
            {
                return grdLoanTransactions.SelectedIndexInternal;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AddControlsEnabled
        {
            set
            {
                txtTotalAmount.Enabled = value;
                txtDisbursementReference.Enabled = value;
                ddlBankDetails.Enabled = value;
                txtDisbursementAmount.Enabled = value;
            }
        }

        public bool AddButtonEnabled
        {
            set { AddDisbursement.Enabled = value; }
        }

        public bool DisplayCancelConfirmationMessage
        {
            set { _displayCancelConfirmationMessage = value; }
        }

        public bool DisplaySubmitConfirmationMessage
        {
            set { _displaySubmitConfirmationMessage = value; }
        }
        public bool DisplaySaveConfirmationMessage
        {
            set { _displaySaveConfirmationMessage = value; }
        }
        public bool DisplayDeleteConfirmationMessage
        {
            set { _displayDeleteConfirmationMessage = value; }
        }

        public string CancelConfirmationMessage
        {
            get { return _cancelConfirmationMessage; }
            set { _cancelConfirmationMessage = value; }
        }

        public string SubmitConfirmationMessage
        {
            get { return _submitConfirmationMessage; }
            set { _submitConfirmationMessage = value; }
        }

        public string SaveConfirmationMessage
        {
            get { return _saveConfirmationMessage; }
            set { _saveConfirmationMessage = value; }
        }        
        
        public string DeleteConfirmationMessage
        {
            get { return _deleteConfirmationMessage; }
            set { _deleteConfirmationMessage = value; }
        }

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnLoanTransactionGridSelectedIndexChanged;

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
        public event EventHandler OnAddDisbursementClicked;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnDeleteDisbursementClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSaveButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnDisbursementTypeSelectedIndexChanged;

        #endregion

        #region Protected Functions Section

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDisbursements_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                IDisbursement disbursement = e.Row.DataItem as IDisbursement;
                if (disbursement != null)
                {
                    if (disbursement.PreparedDate.HasValue)
                        e.Row.Cells[0].Text = disbursement.PreparedDate.Value.ToString(SAHL.Common.Constants.DateFormat);

                    e.Row.Cells[1].Text = disbursement.ACBBank.ACBBankDescription + "," + disbursement.ACBBranch.Key + "," + disbursement.ACBBranch.ACBBranchDescription + "," + disbursement.ACBType.ACBTypeDescription + "," + disbursement.AccountNumber + "," + disbursement.AccountName;

                    if (disbursement.Amount.HasValue)
                        e.Row.Cells[2].Text = disbursement.Amount.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdLoanTransactions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                if (e.Row.Cells[1].Text == "1")
                {
                    Image img = new Image();
                    img.ImageUrl = "~/Images/Check.gif";
                    e.Row.Cells[1].Controls.Add(img);
                }
                else
                    e.Row.Cells[1].Text = "";

                e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text).ToString(SAHL.Common.Constants.DateFormat);

                e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToString(SAHL.Common.Constants.DateFormat);

                if (String.IsNullOrEmpty(e.Row.Cells[5].Text))
                    e.Row.Cells[5].Text = "R0.00";
                else
                    e.Row.Cells[5].Text = Convert.ToDouble(e.Row.Cells[5].Text).ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDisbursementTransactions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                e.Row.Cells[0].Text = Convert.ToDateTime(e.Row.Cells[0].Text).ToString(SAHL.Common.Constants.DateFormat);

                if (e.Row.Cells[1].Text != "&nbsp;")
                    e.Row.Cells[1].Text = Convert.ToDateTime(e.Row.Cells[1].Text).ToString(SAHL.Common.Constants.DateFormat);

                for (int bankIndex = 0; bankIndex < _bankAccountList.Count; bankIndex++)
                {
                    if (_bankAccountList[bankIndex].AccountNumber == e.Row.Cells[2].Text)
                    {
                        e.Row.Cells[2].Text = _bankAccountList[bankIndex].GetDisplayName(BankAccountNameFormat.Full);
                        break;
                    }
                }

                e.Row.Cells[3].Text = Convert.ToDouble(e.Row.Cells[3].Text).ToString(SAHL.Common.Constants.CurrencyFormat);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddDisbursement_Click(object sender, EventArgs e)
        {
            if (DisbursementTypeSelectedValue == -1)
            {
                Messages.Add(new SAHL.Common.DomainMessages.Error("Please select the Disbursement Type.", "Please select the Disbursement Type."));
                return;
            }
            if (OnAddDisbursementClicked != null)
            {
                OnAddDisbursementClicked(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteDisbursement_Click(object sender, EventArgs e)
        {
            if (OnDeleteDisbursementClicked != null)
                OnDeleteDisbursementClicked(sender, new KeyChangedEventArgs(grdDisbursements.SelectedIndexInternal));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (OnSaveButtonClicked != null)
            {
                OnSaveButtonClicked(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
            {
                OnCancelButtonClicked(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
            {
                OnSubmitButtonClicked(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdLoanTransactions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdLoanTransactions.SelectedRow != null)
            {
                KeyChangedEventArgs args = new KeyChangedEventArgs(grdLoanTransactions.SelectedIndexInternal);
                if (OnLoanTransactionGridSelectedIndexChanged != null)
                {
                    OnLoanTransactionGridSelectedIndexChanged(sender, args);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDisbursementType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (Request.Form[ddlDisbursementType.UniqueID] != null)
                ddlDisbursementType.SelectedValue = Request.Form[ddlDisbursementType.UniqueID];

            if (OnDisbursementTypeSelectedIndexChanged != null)
                OnDisbursementTypeSelectedIndexChanged(sender, e);
        }


        #endregion

        #region ICATSDisbursement Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disbursementTypeList"></param>
        public void BindDisbursementTypes(IReadOnlyEventList<IDisbursementTransactionType> disbursementTypeList)
        {
            ddlDisbursementType.DataSource = disbursementTypeList;
            ddlDisbursementType.DataValueField = "Key";
            ddlDisbursementType.DataTextField = "Description";
            ddlDisbursementType.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        public void BindBankAccounts()
        {
            ddlBankDetails.Items.Clear();

            ddlBankDetails.Items.Add(new ListItem("-Please select-", "-1"));
            for (int i = 0; i < _bankAccountList.Count; i++)
            {
                ddlBankDetails.Items.Add(new ListItem
                    (
                        _bankAccountList[i].GetDisplayName(BankAccountNameFormat.Full),
                        _bankAccountList[i].Key.ToString()
                    )
                    );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disbursements"></param>
        public void BindGridDisbursements(IList<IDisbursement> disbursements)
        {
            grdDisbursements.Columns.Clear();
            grdDisbursements.AddGridBoundColumn("PreparedDate", "Prepared Date", Unit.Percentage(20), HorizontalAlign.Left, true);
            grdDisbursements.AddGridBoundColumn("AccountNumber", "Bank Details", Unit.Percentage(60), HorizontalAlign.Left, true);
            grdDisbursements.AddGridBoundColumn("Amount", "Amount", Unit.Percentage(20), HorizontalAlign.Left, true);
            grdDisbursements.DataSource = disbursements;
            grdDisbursements.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loanTransactions"></param>
        public void BindLoanTransactions(DataTable loanTransactions)
        {
            grdLoanTransactions.Columns.Clear();
            grdLoanTransactions.AddGridBoundColumn("LoanTransactionNumber", "LoanTransactionNumber", Unit.Percentage(25), HorizontalAlign.Left, false);
            grdLoanTransactions.AddGridBoundColumn("CanRollBack", "Rollback", Unit.Percentage(10), HorizontalAlign.Left, true);
            grdLoanTransactions.AddGridBoundColumn("LoanTransactionInsertDate", "Insert Date", Unit.Percentage(10), HorizontalAlign.Left, true);
            grdLoanTransactions.AddGridBoundColumn("LoanTransactionEffectiveDate", "Effective Date", Unit.Percentage(10), HorizontalAlign.Left, true);
            grdLoanTransactions.AddGridBoundColumn("TransactionTypeLoanDescription", "Transaction Type", Unit.Percentage(20), HorizontalAlign.Left, true);
            grdLoanTransactions.AddGridBoundColumn("LoanTransactionAmount", "Amount", Unit.Percentage(12), HorizontalAlign.Left, true);
            grdLoanTransactions.AddGridBoundColumn("LoanTransactionUserID", "Changed By", Unit.Percentage(12), HorizontalAlign.Left, true);
            grdLoanTransactions.AddGridBoundColumn("DisbursementStatusDescription", "Status", Unit.Percentage(25), HorizontalAlign.Left, true);
            grdLoanTransactions.DataSource = loanTransactions;
            grdLoanTransactions.DataBind();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disbursementTransactions"></param>
        public void BindDisbursementTransactions(DataTable disbursementTransactions)
        {
            grdDisbursementTransactions.Columns.Clear();
            grdDisbursementTransactions.AddGridBoundColumn("PreparedDate", "Prepared Date", Unit.Percentage(15), HorizontalAlign.Left, true);
            grdDisbursementTransactions.AddGridBoundColumn("ActionDate", "Action Date", Unit.Percentage(15), HorizontalAlign.Left, true);
            grdDisbursementTransactions.AddGridBoundColumn("AccountNumber", "Bank Details", Unit.Percentage(50), HorizontalAlign.Left, true);
            grdDisbursementTransactions.AddGridBoundColumn("Amount", "Amount", Unit.Percentage(20), HorizontalAlign.Left, true);
            grdDisbursementTransactions.DataSource = disbursementTransactions;
            grdDisbursementTransactions.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BankAccountList"></param>
        public void SetBankAccounts(List<IBankAccount> BankAccountList)
        {
            _bankAccountList = BankAccountList;
        }

        #endregion
    }
}
