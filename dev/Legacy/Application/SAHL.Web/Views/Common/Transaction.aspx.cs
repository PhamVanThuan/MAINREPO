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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Web.Controls;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.DomainMessages;
using DevExpress.Web.ASPxGridView;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common
{
    public partial class Transaction : SAHLCommonBaseView, ITransaction
    {
        private bool _gridCreated;
        private bool _finTranOnly;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (IsMenuPostBack)
            {
                grdTransaction.ClearSort();

                foreach (GridViewDataColumn col in grdTransaction.GetGroupedColumns())
                {
                    grdTransaction.UnGroup(col);
                }


            }

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
        protected void btnRollback_Click(object sender, EventArgs e)
        {
            List<object> selectedValues = grdTransaction.GetSelectedFieldValues("CanRollback");
            if (selectedValues != null && selectedValues.Count == 1)
                if (selectedValues[0].ToString() != "1")
                {
                    Messages.Add(new Error("This transaction can not be rolled back.", "This transaction can not be rolled back."));
                    return;
                }

            OnRollbackButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRollbackConfirm_Click(object sender, EventArgs e)
        {
            OnRollbackConfirmButtonClicked(sender, e);
        }

        #region ITransaction Members

        public event EventHandler OnCancelButtonClicked;

        public event EventHandler OnRollbackButtonClicked;

        public event EventHandler OnRollbackConfirmButtonClicked;

        public event EventHandler OnTransactionTypeSelectedIndexChanged;


        private void AddGridColumn(string fieldName, string caption, Unit width, GridFormatType formatType, string format, HorizontalAlign align, bool visible)
        {
            AddGridColumn(fieldName, caption, width, formatType, format, align, GridViewColumnFixedStyle.None, visible);
        }
        private void AddGridColumn(string fieldName, string caption, Unit width, GridFormatType formatType, string format, HorizontalAlign align, GridViewColumnFixedStyle fixedStyle, bool visible)
        {
            DXGridViewFormattedTextColumn col = new DXGridViewFormattedTextColumn();
            col.FieldName = fieldName;
            if (fieldName == "Number")
                col.Settings.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
            col.Caption = caption;

            col.Width = width;
            col.FixedStyle = fixedStyle;
            col.Format = formatType;
            if (!String.IsNullOrEmpty(format))
                col.FormatString = format;
            col.Visible = visible;
            col.CellStyle.HorizontalAlign = align;
            grdTransaction.Columns.Add(col);
        }

        public void BindTransactions(DataTable Transactions, bool isArrears)
        {
            SetupGrid(isArrears);

            grdTransaction.Settings.ShowGroupPanel = true;

            grdTransaction.SettingsPager.PageSize = 15;


            if (isArrears)
                BindArrearTransactions(Transactions);
            else
                BindLoanTransactions(Transactions);

        }

        protected void LoanTranGrid_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            if (e.Column.FieldName == "Number")
            {
                e.Handled = true;
                int num1 = Convert.ToInt32(e.Value1);
                int num2 = Convert.ToInt32(e.Value2);
                if (num1 < num2)
                    e.Result = -1;
                else
                    e.Result = 1;

            }
        }



        private void SetupGrid(bool isArrears)
        {
            if (!_gridCreated)
            {
                AddGridColumn("Description", "Transaction Type", Unit.Pixel(300), GridFormatType.GridString, null, HorizontalAlign.Left, GridViewColumnFixedStyle.Left, true);
                AddGridColumn("HTMLColour", "", Unit.Pixel(0), GridFormatType.GridString, null, HorizontalAlign.Left, false);
                AddGridColumn("ForeColour", "", Unit.Pixel(0), GridFormatType.GridString, null, HorizontalAlign.Left, false);
                AddGridColumn("Service", "Service", Unit.Pixel(80), GridFormatType.GridString, null, HorizontalAlign.Left, true);
                AddGridColumn("TransactionGroup", "Transaction Group", Unit.Pixel(100), GridFormatType.GridString, null, HorizontalAlign.Left, true);
                AddGridColumn("Amount", "Amount", Unit.Pixel(100), GridFormatType.GridNumber, SAHL.Common.Constants.CurrencyFormat, HorizontalAlign.Right, true);
                AddGridColumn("InsertDate", "Insert Date", Unit.Pixel(100), GridFormatType.GridDate, null, HorizontalAlign.Center, true);
                AddGridColumn("EffectiveDate", "Effective Date", Unit.Pixel(100), GridFormatType.GridDate, SAHL.Common.Constants.DateFormat, HorizontalAlign.Center, true);
                AddGridColumn("NewBalance", "Balance", Unit.Pixel(100), GridFormatType.GridNumber, SAHL.Common.Constants.CurrencyFormat, HorizontalAlign.Right, true);
                AddGridColumn("AccountCurrentBalance", "Account Balance", Unit.Pixel(100), GridFormatType.GridNumber, SAHL.Common.Constants.CurrencyFormat, HorizontalAlign.Right, true);
                if (!isArrears)
                    AddGridColumn("Rate", "Rate", Unit.Pixel(50), GridFormatType.GridNumber, SAHL.Common.Constants.RateFormat, HorizontalAlign.Right, true);
                AddGridColumn("CanRollback", "RollBack", Unit.Pixel(40), GridFormatType.GridString, null, HorizontalAlign.Center, true);
                AddGridColumn("UserID", "Changed By", Unit.Pixel(100), GridFormatType.GridString, null, HorizontalAlign.Left, true);
                AddGridColumn("Reference", "Reference", Unit.Pixel(200), GridFormatType.GridString, null, HorizontalAlign.Left, true);
                AddGridColumn("Number", "Number", Unit.Pixel(50), GridFormatType.GridString, null, HorizontalAlign.Left, true);
                _gridCreated = true;


            }
        }

        public void BindRollbackTransactions(DataTable Transactions, bool isArrears)
        {
            if (isArrears)
            {
                grdRollbackConfirm.AddGridBoundColumn("ArrearTransactionKey", "Number", Unit.Percentage(15), HorizontalAlign.Left, true);
                grdRollbackConfirm.AddGridBoundColumn("ServiceDescription", "Service", Unit.Percentage(20), HorizontalAlign.Left, true);
                grdRollbackConfirm.AddGridBoundColumn("TransactionTypeDescription", "Transaction Type", Unit.Percentage(20), HorizontalAlign.Left, true);
                grdRollbackConfirm.AddGridBoundColumn("InsertDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Insert Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
                grdRollbackConfirm.AddGridBoundColumn("EffectiveDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
                grdRollbackConfirm.AddGridBoundColumn("Amount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amount", false, Unit.Percentage(15), HorizontalAlign.Right, true);
            }
            else
            {
                grdRollbackConfirm.AddGridBoundColumn("FinancialTransactionKey", "Number", Unit.Percentage(15), HorizontalAlign.Left, true);
                grdRollbackConfirm.AddGridBoundColumn("ServiceDescription", "Service", Unit.Percentage(20), HorizontalAlign.Left, true);
                grdRollbackConfirm.AddGridBoundColumn("TransactionTypeDescription", "Transaction Type", Unit.Percentage(20), HorizontalAlign.Left, true);
                grdRollbackConfirm.AddGridBoundColumn("InsertDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Insert Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
                grdRollbackConfirm.AddGridBoundColumn("EffectiveDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
                grdRollbackConfirm.AddGridBoundColumn("Amount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amount", false, Unit.Percentage(15), HorizontalAlign.Right, true);;
            }

            grdRollbackConfirm.DataSource = Transactions;
            grdRollbackConfirm.DataBind();

            if (Transactions.Rows.Count > 0)
                ConfirmTable.Attributes.Add("style", "display: inline");

            if (Transactions.Rows.Count > 1)
                ConfirmMessage.InnerText = "The transaction you selected to rollback has other transactions that are directly related.<br />" +
                                            "All the following transactions will be rolled back.";
            else
                ConfirmMessage.InnerText = "The following transaction will be rolled back.";
        }

        protected void LoanTranGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "CanRollback")
            {
                if (e.CellValue.Equals("1"))
                {
                    Image img = new Image();
                    img.ImageUrl = "~/Images/Check.gif";

                    e.Cell.Text = "";
                    e.Cell.Controls.Add(img);
                }
                else
                {
                    e.Cell.Text = "&nbsp;";
                }
            }

        }

        protected void LoanTranGrid_RowDataBound(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data && e.Row.Cells.Count > 1)
            {
                if (!String.IsNullOrEmpty(e.GetValue("HTMLColour").ToString()))
                {
                    if (String.IsNullOrEmpty(e.GetValue("ForeColour").ToString()))
                        e.Row.Cells[(int)TransactionGrid.GridColumns.Description].Attributes.Add("style", "color: #FFFFFF; background-color:" + e.GetValue("HTMLColour").ToString());
                    else
                        e.Row.Cells[(int)TransactionGrid.GridColumns.Description].Attributes.Add("style", "color:" + e.GetValue("ForeColour").ToString() + "; background-color:" + e.GetValue("HTMLColour").ToString());
                }
            }
        }


        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public int FinancialServicesSelectedValue
        {
            get
            {
                if (FinancialServices.SelectedValue.Length == 0)
                    return 0;
                else
                    return Convert.ToInt32(FinancialServices.SelectedValue);
            }
            set
            {
                FinancialServices.SelectedValue = value.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TransactionDisplayTypeSelectedValue
        {
            get
            {
                if (FinancialServices.SelectedValue.Length == 0)
                    return 0;
                else
                    return Convert.ToInt32(TransactionType.SelectedValue);
            }
            set
            {
                TransactionType.SelectedValue = value.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TransactionDescriptionSelectedValue
        {
            get
            {
                if (TransactionTypeDesc.SelectedValue.Length == 0)
                    return "All";
                else
                    return TransactionTypeDesc.SelectedValue;
            }
            set { TransactionTypeDesc.SelectedValue = value; }
        }

        public bool FinancialTransactionsOnly
        {
            get { return _finTranOnly; }
            set { _finTranOnly = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ButtonStatus.Display ButtonCancel
        {
            set
            {
                switch (value)
                {
                    case ButtonStatus.Display.Hidden:
                        btnCancel.Visible = false;
                        break;
                    case ButtonStatus.Display.Disabled:
                        btnCancel.Visible = true;
                        btnCancel.Enabled = false;
                        break;
                    case ButtonStatus.Display.Enabled:
                        btnCancel.Visible = true;
                        btnCancel.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ButtonStatus.Display ButtonRollback
        {
            set
            {
                switch (value)
                {
                    case ButtonStatus.Display.Hidden:
                        btnRollback.Visible = false;
                        break;
                    case ButtonStatus.Display.Disabled:
                        btnRollback.Visible = true;
                        btnRollback.Enabled = false;
                        break;
                    case ButtonStatus.Display.Enabled:
                        btnRollback.Visible = true;
                        btnRollback.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ButtonStatus.Display ButtonRollbackConfirm
        {
            set
            {
                switch (value)
                {
                    case ButtonStatus.Display.Hidden:
                        btnRollbackConfirm.Visible = false;
                        break;
                    case ButtonStatus.Display.Disabled:
                        btnRollbackConfirm.Visible = true;
                        btnRollbackConfirm.Enabled = false;
                        break;
                    case ButtonStatus.Display.Enabled:
                        btnRollbackConfirm.Visible = true;
                        btnRollbackConfirm.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RollbackTransactionNumber
        {
            get
            {
                List<object> selectedValues = grdTransaction.GetSelectedFieldValues(grdTransaction.KeyFieldName);
                if (selectedValues != null && selectedValues.Count == 1)
                    return int.Parse(selectedValues[0].ToString());

                return 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public GridPostBackType GridViewPostBackType
        {
            set { grdTransaction.PostBackType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ShowTransactions
        {
            set
            {
                grdTransaction.Visible = value;
                DisplayTable.Visible = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ShowRollbackTransactions
        {
            set
            {
                grdRollbackConfirm.Visible = value;
            }
        }
        /// <summary>
        /// Use this to hide empty lists and buttons when no account exists
        /// </summary>
        public bool NoTransactions
        {
            set
            {
                if (value == true)
                {
                    //btnNext.Visible = false;
                    //btnPrevious.Visible = false;
                    DisplayTable.Visible = false;
                }
            }
        }

        #endregion

        #endregion

        #region TransactionGrid Classes/Methods

        public void BindArrearTransactions(DataTable transactions)
        {
            List<GridEntity> gridEntities = new List<GridEntity>();
            foreach (DataRow dr in transactions.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                    gridEntities.Add(
                        new GridEntity(dr["TransactionTypeDescription"].ToString(),
                            dr["TransactionTypeHTMLColour"].ToString(),
                            "",
                            dr["Amount"].ToString(),
                            (DateTime)dr["InsertDate"],
							(DateTime)dr["EffectiveDate"],
                            dr["Balance"].ToString(),
                            dr["AccountBalance"].ToString(),
                            "",
                            dr["CanRollBack"].ToString(),
                            dr["FinancialService"].ToString(),
                            dr["UserID"].ToString(),
                            dr["Reference"].ToString(),
                            dr["ArrearTransactionKey"].ToString(),
                            dr["TransactionGroup"].ToString()
                        )
                    );
            }

            grdTransaction.DataSource = gridEntities;
            grdTransaction.DataBind();
        }

        public void BindLoanTransactions(DataTable transactions)
        {
            List<GridEntity> gridEntities = new List<GridEntity>();
            foreach (DataRow dr in transactions.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                    gridEntities.Add(
                        new GridEntity(dr["TransactionTypeDescription"].ToString(),
                            dr["TransactionTypeHTMLColour"].ToString(),
                            "",
                            dr["Amount"].ToString(),
                            (DateTime)dr["InsertDate"],
							(DateTime)dr["EffectiveDate"],
                            dr["Balance"].ToString(),
                            dr["AccountBalance"].ToString(),
                            dr["InterestRate"].ToString(),
                            dr["CanRollBack"].ToString(),
                            dr["FinancialService"].ToString(),
                            dr["UserID"].ToString(),
                            dr["Reference"].ToString(),
                            dr["FinancialTransactionKey"].ToString(),
                            dr["TransactionGroup"].ToString()
                        )
                    );
            }

            grdTransaction.DataSource = gridEntities;
            grdTransaction.DataBind();
        }


        /// <summary>
        /// Internal class to make binding to the grid a little easier as we have to support different data source entities.
        /// </summary>
        private class GridEntity
        {
            #region private variables
            private string _description;
            private string _htmlColour;
            private string _foreColour;
            private string _amount;
            private DateTime _insertDate;
			private DateTime _effectiveDate;
            private string _newBalance;
            private string _accountCurrentBalance;
            private string _rate;
            private string _canRollback;
            private string _service;
            private string _userID;
            private string _reference;
            private string _number;
            private string _transactiongroup;
            #endregion

			public GridEntity(string description, string htmlColour, string foreColour, string amount, DateTime insertDate, DateTime effectiveDate, string newBalance, string accountCurrentBalance, string rate, string canRollback, string service,
                              string userID, string reference, string number, string transactiongroup)
            {
                _description = description;
                _htmlColour = htmlColour;
                _foreColour = foreColour;
                _amount = amount;
                _insertDate = insertDate;
                _effectiveDate = effectiveDate;
                _newBalance = newBalance;
                _accountCurrentBalance = accountCurrentBalance;
                _rate = rate;
                _canRollback = canRollback;
                _service = service;
                _userID = userID;
                _reference = reference;
                _number = number;
                _transactiongroup = transactiongroup;
            }

            #region public properties
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string Description
            {
                get { return _description; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string HTMLColour
            {
                get { return _htmlColour; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string ForeColour
            {
                get { return _foreColour; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string Amount
            {
                get { return _amount; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public DateTime InsertDate
            {
                get { return _insertDate; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public DateTime EffectiveDate
            {
                get { return _effectiveDate; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string NewBalance
            {
                get { return _newBalance; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string AccountCurrentBalance
            {
                get { return _accountCurrentBalance; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string Rate
            {
                get { return _rate; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string CanRollback
            {
                get { return _canRollback; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string Service
            {
                get { return _service; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string UserID
            {
                get { return _userID; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string Reference
            {
                get { return _reference; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string Number
            {
                get { return _number; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string TransactionGroup { get { return _transactiongroup; } }
            #endregion
        }

        #endregion
    }
}
