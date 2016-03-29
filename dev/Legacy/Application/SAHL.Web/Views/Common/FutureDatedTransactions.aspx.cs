using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Authentication;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class FutureDatedTransactions : SAHLCommonBaseView, IFutureDatedTransactions
    {
        #region Private Variables

        private bool _setEffectiveDateToCurrentDate;

        private bool _showButtons;
        private bool _displayControls;
        private bool _showLabels;
        private double _arrearBalance;
        private bool _captureMultipleDebitOrders;

        private IEventList<IManualDebitOrder> _lstManualDebitOrders;

        private IList<ILegalEntityBankAccount> _legalEntityBankAccounts = new List<ILegalEntityBankAccount>();

        private enum GridColumns
        {
            Key = 0,
            BankKey,
            InsertDate,
            ActionDate,
            Amount,
            Bank,
            BankAccountNumber,
            UserID,
            TransactionType,
            Status
        }

        #endregion Private Variables

        #region Protected Functions Section

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;

            BindOrdersToGrid(_lstManualDebitOrders);

            if (RecordsGrid.PostBackType != GridPostBackType.None && !IsPostBack && RecordsGrid.Rows.Count > 0 && RecordsGrid.SelectedIndex > -1)
            {
                BindControls();
            }
            BindBankAccountControl();
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
            if (!ShouldRunPage) return;

            trNoOfPayments.Visible = _captureMultipleDebitOrders;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        protected void RecordsGrid_SelectedIndexChanged(Object s, EventArgs e)
        {
            BindControls();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            OnAddButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            OnUpdateButtonClicked(sender, SelectedItemEventArgs);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            OnDeleteButtonClicked(sender, SelectedItemEventArgs);
        }

        /// <summary>
        ///
        /// </summary>
        private KeyChangedEventArgs SelectedItemEventArgs
        {
            get
            {
                return new KeyChangedEventArgs(RecordsGrid.Rows[RecordsGrid.SelectedIndex].Cells[0].Text);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RecordsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                IManualDebitOrder rt = e.Row.DataItem as IManualDebitOrder;
                if (rt != null)
                {
                    if (DateTime.Parse(e.Row.Cells[(int)GridColumns.ActionDate].Text) > DateTime.Now)
                    {
                        e.Row.ForeColor = Color.Red;
                    }

                    e.Row.Cells[(int)GridColumns.Bank].Text = GetBankDescription(rt);

                    e.Row.Cells[(int)GridColumns.BankAccountNumber].Text = rt.BankAccount == null ? "-" : rt.BankAccount.AccountNumber;

                    e.Row.Cells[(int)GridColumns.BankKey].Text = (rt.BankAccount == null ? "-" : rt.BankAccount.Key.ToString());

                    if (rt.TransactionType != null)
                    {
                        if (rt.TransactionType.Key == (int)SAHL.Common.Globals.TransactionTypes.MonthlyServiceFee)
                        {
                            e.Row.Cells[(int)GridColumns.TransactionType].Text = "Service Fee";
                        }
                        else
                        {
                            e.Row.Cells[(int)GridColumns.TransactionType].Text = "Debit Order";
                        }
                    }

                    e.Row.Cells[(int)GridColumns.Status].Text = System.Convert.IsDBNull(rt.Active) == true ? "Inactive" : rt.Active == false ? "Inactive" : "Active";
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rt"></param>
        /// <returns></returns>
        private static string GetBankDescription(IManualDebitOrder rt)
        {
            StringBuilder sb = new StringBuilder();
            IBankAccount bankAccount = rt.BankAccount;
            if (bankAccount != null)
            {
                IACBBranch acbBranch = bankAccount.ACBBranch;
                if (acbBranch != null)
                {
                    if (acbBranch.ACBBank != null)
                        sb.Append(acbBranch.ACBBank.ACBBankDescription);
                    else
                        sb.Append("Unknown Bank");
                    sb.Append(" - " + acbBranch.Key.ToString());
                    sb.Append(" " + acbBranch.ACBBranchDescription);
                    if (bankAccount.ACBType != null)
                        sb.Append(" - " + bankAccount.ACBType.ACBTypeDescription);
                }
            }
            if (sb.Length == 0)
                sb.Append("-");
            return sb.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            CancelButton.Visible = _showButtons;

            dateEffectiveDateUpdate.Visible = !_showLabels;
            lblEffectiveDate.Visible = _showLabels;

            txtNoteUpdate.Visible = !_showLabels;
            NotePanel.Visible = _showLabels;

            txtReferenceUpdate.Visible = !_showLabels;
            lblReference.Visible = _showLabels;

            txtAmountUpdate.Visible = !_showLabels;
            lblAmount.Visible = _showLabels;

            ddlBankUpdate.Visible = !_showLabels;
            lblBank.Visible = _showLabels;

            DisplayData.Visible = _displayControls;

            if (ArrearBalanceRowVisible)
                lblArrearBalance.Text = ArrearBalance.ToString(SAHL.Common.Constants.CurrencyFormat);

            if (GridPostbackType != GridPostBackType.None)
            {
                if (_setEffectiveDateToCurrentDate)
                {
                    DateTime current = DateTime.Now;
                    if (current.TimeOfDay.Hours < 15)
                    {
                        dateEffectiveDateUpdate.Date = DateTime.Today;
                    }
                    else
                    {
                        dateEffectiveDateUpdate.Date = DateTime.Today.Add(new TimeSpan(24, 0, 0));
                    }
                }

                if (RecordsGrid.Rows.Count > 0)
                {
                    for (int x = 0; x < ddlBankUpdate.Items.Count; x++)
                    {
                        if (ddlBankUpdate.Items[x].Value.ToString() == RecordsGrid.Rows[RecordsGrid.SelectedIndex].Cells[1].Text)
                        {
                            ddlBankUpdate.SelectedIndex = x;
                            break;
                        }
                    }
                }

                if (RecordsGrid.Rows.Count < 1)
                    btnDelete.Enabled = false;
            }
        }

        /// <summary>
        /// Duplication of the main grid.
        /// Shows historical information i.e. Inactive Transactions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RecordsGridPrv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                IManualDebitOrder rt = e.Row.DataItem as IManualDebitOrder;
                if (rt != null)
                {
                    if (DateTime.Parse(e.Row.Cells[(int)GridColumns.ActionDate].Text) > DateTime.Now)
                        e.Row.ForeColor = Color.Red;

                    e.Row.Cells[(int)GridColumns.Bank].Text = GetBankDescription(rt);

                    e.Row.Cells[(int)GridColumns.BankAccountNumber].Text = rt.BankAccount == null ? "-" : rt.BankAccount.AccountNumber;

                    e.Row.Cells[(int)GridColumns.BankKey].Text = (rt.BankAccount == null ? "-" : rt.BankAccount.Key.ToString());

                    if (rt.TransactionType != null)
                    {
                        if (rt.TransactionType.Key == (int)SAHL.Common.Globals.TransactionTypes.MonthlyServiceFee)
                            e.Row.Cells[(int)GridColumns.TransactionType].Text = "Service Fee";
                        else
                            e.Row.Cells[(int)GridColumns.TransactionType].Text = "Debit Order";
                    }

                    e.Row.Cells[(int)GridColumns.Status].Text = System.Convert.IsDBNull(rt.Active) == true ? "Inactive" : rt.Active == false ? "Inactive" : "Active";
                }
            }
        }

        #endregion Protected Functions Section

        #region IFutureDatedTransactions Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="manualDebitOrders"></param>
        public void BindOrdersToGrid(SAHL.Common.Collections.Interfaces.IEventList<SAHL.Common.BusinessModel.Interfaces.IManualDebitOrder> manualDebitOrders)
        {
            _lstManualDebitOrders = manualDebitOrders;

            if (!DesignMode)
            {
                RecordsGrid.Columns.Clear();

                RecordsGrid.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
                RecordsGrid.AddGridBoundColumn("", "BankKey", Unit.Percentage(0), HorizontalAlign.Left, false);
                RecordsGrid.AddGridBoundColumn("InsertDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Insert Date", false, Unit.Percentage(7), HorizontalAlign.Center, true);
                RecordsGrid.AddGridBoundColumn("ActionDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(7), HorizontalAlign.Center, true);
                RecordsGrid.AddGridBoundColumn("Amount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amount", false, Unit.Percentage(12), HorizontalAlign.Right, true);
                RecordsGrid.AddGridBoundColumn("", "Bank", Unit.Percentage(40), HorizontalAlign.Left, true);
                RecordsGrid.AddGridBoundColumn("", "Bank Account", Unit.Percentage(10), HorizontalAlign.Left, true);
                RecordsGrid.AddGridBoundColumn("UserID", "Captured By", Unit.Percentage(7), HorizontalAlign.Left, true);
                RecordsGrid.AddGridBoundColumn("", "Transaction Type", Unit.Percentage(10), HorizontalAlign.Center, true);
                RecordsGrid.AddGridBoundColumn("Active", "Status", Unit.Percentage(10), HorizontalAlign.Left, true);
            }

            RecordsGrid.DataSource = manualDebitOrders;
            RecordsGrid.DataBind();
        }

        /// <summary>
        /// Duplication of the main grid.
        /// Shows historical information i.e. Inactive Transactions
        /// </summary>
        /// <param name="manualDebitOrdersPrv"></param>
        public void BindOrdersToPreviousGrid(SAHL.Common.Collections.Interfaces.IEventList<SAHL.Common.BusinessModel.Interfaces.IManualDebitOrder> manualDebitOrdersPrv)
        {
            if (!DesignMode)
            {
                RecordsGridPrv.Columns.Clear();

                RecordsGridPrv.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
                RecordsGridPrv.AddGridBoundColumn("", "BankKey", Unit.Percentage(0), HorizontalAlign.Left, false);
                RecordsGridPrv.AddGridBoundColumn("InsertDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Insert Date", false, Unit.Percentage(7), HorizontalAlign.Center, true);
                RecordsGridPrv.AddGridBoundColumn("ActionDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(7), HorizontalAlign.Center, true);
                RecordsGridPrv.AddGridBoundColumn("Amount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amount", false, Unit.Percentage(12), HorizontalAlign.Right, true);
                RecordsGridPrv.AddGridBoundColumn("", "Bank", Unit.Percentage(40), HorizontalAlign.Left, true);
                RecordsGridPrv.AddGridBoundColumn("", "Bank Account", Unit.Percentage(10), HorizontalAlign.Left, true);
                RecordsGridPrv.AddGridBoundColumn("UserID", "Captured By", Unit.Percentage(7), HorizontalAlign.Left, true);
                RecordsGridPrv.AddGridBoundColumn("", "Transaction Type", Unit.Percentage(10), HorizontalAlign.Center, true);
                RecordsGridPrv.AddGridBoundColumn("Active", "Status", Unit.Percentage(10), HorizontalAlign.Left, true);
            }

            RecordsGridPrv.DataSource = manualDebitOrdersPrv;
            RecordsGridPrv.DataBind();
        }

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnAddButtonClicked;

        /// <summary>
        ///
        /// </summary>
        public event KeyChangedEventHandler OnDeleteButtonClicked;

        /// <summary>
        ///
        /// </summary>
        public event KeyChangedEventHandler OnUpdateButtonClicked;

        /// <summary>
        ///
        /// </summary>
        public void BindControls()
        {
            if (RecordsGrid.Rows[RecordsGrid.SelectedIndex].Cells[7].Text == "Service Fee")
            {
                _showLabels = true;
            }

            if (RecordsGrid.Rows.Count > 0 && RecordsGrid.SelectedIndex > -1)
            {
                IManualDebitOrder manualDebitOrder = _lstManualDebitOrders[RecordsGrid.SelectedIndex];

                if (!_showLabels)
                {
                    txtNoteUpdate.Text = manualDebitOrder.Memo != null ? manualDebitOrder.Memo.Description : String.Empty;
                    if (manualDebitOrder.ActionDate != null)
                    {
                        dateEffectiveDateUpdate.Date = manualDebitOrder.ActionDate;
                    }
                    txtAmountUpdate.Amount = manualDebitOrder.Amount;
                    txtReferenceUpdate.Text = manualDebitOrder.Reference;
                }
                else
                {
                    lblNote.Text = manualDebitOrder.Memo != null ? manualDebitOrder.Memo.Description : String.Empty;
                    lblEffectiveDate.Text = manualDebitOrder.ActionDate.ToString(SAHL.Common.Constants.DateFormat);
                    lblAmount.Text = manualDebitOrder.Amount.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblReference.Text = manualDebitOrder.Reference.ToString();

                    if (_legalEntityBankAccounts.Count > 0)
                    {
                        for (int y = 0; y < _legalEntityBankAccounts.Count; y++)
                        {
                            IBankAccount leBankAccount = _legalEntityBankAccounts[y].BankAccount;
                            IBankAccount rtBankAccount = manualDebitOrder.BankAccount;
                            if (rtBankAccount != null && leBankAccount.Key == rtBankAccount.Key)
                            {
                                lblBank.Text = leBankAccount.GetDisplayName(BankAccountNameFormat.Full);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool ShowButtons
        {
            get { return _showButtons; }
            set { _showButtons = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool ArrearBalanceRowVisible
        {
            get { return ArrearBalanceRow.Visible; }
            set { ArrearBalanceRow.Visible = value; }
        }

        /// <summary>
        /// Gets/sets the visibility of the Add button.
        /// </summary>
        public bool ButtonAddVisible
        {
            get
            {
                return btnAdd.Visible;
            }
            set
            {
                btnAdd.Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets the visibility of the Update button.
        /// </summary>
        public bool ButtonUpdateVisible
        {
            get
            {
                return btnUpdate.Visible;
            }
            set
            {
                btnUpdate.Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets the visibility of the Delete button.
        /// </summary>
        public bool ButtonDeleteVisible
        {
            get
            {
                return btnDelete.Visible;
            }
            set
            {
                btnDelete.Visible = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool ControlsVisible
        {
            set { _displayControls = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public GridPostBackType GridPostbackType
        {
            get { return RecordsGrid.PostBackType; }
            set { RecordsGrid.PostBackType = value; }
        }

        public bool ShowLabels
        {
            set { _showLabels = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int RecordsGridSelectedIndex
        {
            get
            {
                return RecordsGrid.SelectedIndex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string DeleteButtonText
        {
            set
            {
                btnDelete.Text = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string DeleteButtonOnClientClick
        {
            set
            {
                btnDelete.OnClientClick = value;
            }
        }

        /// <summary>
        /// Sets the visibility of the tdRequestedBy TD.
        /// </summary>
        public bool tdRequestedByVisible
        {
            set
            {
                tdRequestedBy.Visible = value;
            }
        }

        /// <summary>
        /// Sets the visibility of the lblRequestedBy Label.
        /// </summary>
        public bool lblRequestedByVisible
        {
            set
            {
                lblRequestedBy.Visible = value;
            }
        }

        /// <summary>
        /// Sets the text of the lblRequestedBy Label.
        /// </summary>
        public string lblRequestedByText
        {
            set
            {
                lblRequestedBy.Text = value;
            }
        }

        /// <summary>
        /// Sets the visibility of the tdProcessedBy TD.
        /// </summary>
        public bool tdProcessedByVisible
        {
            set
            {
                tdProcessedBy.Visible = value;
            }
        }

        /// <summary>
        /// Sets the visibility of the lblProcessedBy Label.
        /// </summary>
        public bool lblProcessedByVisible
        {
            set
            {
                lblProcessedBy.Visible = value;
            }
        }

        /// <summary>
        /// Sets the text of the lblProcessedBy Label.
        /// </summary>
        public string lblProcessedByText
        {
            set
            {
                lblProcessedBy.Text = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void BindBankAccountControl()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            // StringBuilder sb;
            for (int x = 0; x < _legalEntityBankAccounts.Count; x++)
            {
                IBankAccount bankAccount = _legalEntityBankAccounts[x].BankAccount;
                if (!dic.ContainsKey(bankAccount.Key))
                {
                    dic.Add(bankAccount.Key, bankAccount.GetDisplayName(BankAccountNameFormat.Full));
                }
            }
            ddlBankUpdate.DataSource = dic;
            ddlBankUpdate.DataValueField = "Key";
            ddlBankUpdate.DataTextField = "Description";
            ddlBankUpdate.DataBind();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="gridRowIndex"></param>
        public int GetFSRTransactionKey(int gridRowIndex)
        {
            if (gridRowIndex > -1)
            {
                if (RecordsGrid.Rows.Count > 0)
                {
                    return string.IsNullOrEmpty(RecordsGrid.Rows[gridRowIndex].Cells[0].Text) == true ? 0 : int.Parse(RecordsGrid.Rows[gridRowIndex].Cells[0].Text);
                }
            }
            return 0;
        }

        /// <summary>
        ///
        /// </summary>
        public int? SelectedBankAccountKey
        {
            get
            {
                if (ddlBankUpdate.SelectedValue == SAHLDropDownList.PleaseSelectValue)
                    return new int?();
                else
                    return new int?(int.Parse(ddlBankUpdate.SelectedValue));
            }
        }

        /// <summary>
        /// Gets/sets the Arrears Balance to be displayed on the screen if <see cref="ArrearBalanceRowVisible"/> is set to true.
        /// </summary>
        public double ArrearBalance
        {
            get { return _arrearBalance; }
            set { _arrearBalance = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? EffectiveDate
        {
            get { return dateEffectiveDateUpdate.Date; }
        }

        /// <summary>
        ///
        /// </summary>
        public double? Amount
        {
            get
            {
                return txtAmountUpdate.Amount;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string AccountKey
        {
            get { return hidAccountNo.Value; }
            set { hidAccountNo.Value = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string Reference
        {
            get { return txtReferenceUpdate.Text; }
            set { txtReferenceUpdate.Text = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string Note
        {
            get
            {
                return txtNoteUpdate.Text;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool SetEffectiveDateToCurrentDate
        {
            set { _setEffectiveDateToCurrentDate = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public IList<ILegalEntityBankAccount> LegalEntityBankAccounts
        {
            get { return _legalEntityBankAccounts; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool RecordsGridPrvVisible
        {
            set { tblGridPrv.Visible = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool CaptureMultipleDebitOrders
        {
            set { _captureMultipleDebitOrders = value; }
            get { return _captureMultipleDebitOrders; }
        }

        /// <summary>
        ///
        /// </summary>
        public int NoOfPayments
        {
            get
            {
                if (!string.IsNullOrEmpty(txtNoOfPayments.Text))
                    return Convert.ToInt32(txtNoOfPayments.Text);
                else
                    return -1;
            }
        }

        #endregion IFutureDatedTransactions Members
    }
}