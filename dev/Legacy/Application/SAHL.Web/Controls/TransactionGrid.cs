using SAHL.Common.Web.UI.Controls;
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Common Transactions grid view
    /// </summary>
    public class TransactionGrid : SAHLGridView
    {
        /// <summary>
        /// Defines all columns used in the <see cref="TransactionGrid"/>.
        /// </summary>
        public enum GridColumns
        {
            Description = 0,
            Service,
            Amount,
            InsertDate,
            EffectiveDate,
            NewBalance,
            AccountCurrentBalance,
            Rate,
            CanRollback,
            UserID,
            Reference,
            Number,
            HTMLColour,
            ForeColour
        }

        public TransactionGrid()
        {
            AutoGenerateColumns = false;
            FixedHeader = false;
            EnableViewState = false;
            HeaderCaption = "Loan Transactions";
            EmptyDataSetMessage = "There are no Loan Transactions.";
            NullDataSetMessage = EmptyDataSetMessage;
            EmptyDataText = EmptyDataSetMessage;
            RowStyle.CssClass = "TableRowA";
            GridHeight = Unit.Pixel(388); 
            GridWidth = Unit.Pixel(770);
            Width = Unit.Pixel(1850);
            ShowFooter = false;
            ScrollX = true;
            PostBackType = GridPostBackType.None;

            if (!DesignMode)
            {
                this.AddGridBoundColumn("Description", "Transaction Type", Unit.Percentage(15), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("HTMLColour", "", Unit.Percentage(0), HorizontalAlign.Left, false);
                this.AddGridBoundColumn("ForeColour", "", Unit.Percentage(0), HorizontalAlign.Left, false);
                // common columns
                this.AddGridBoundColumn("Amount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amount", false, Unit.Percentage(6), HorizontalAlign.Right, true);
                this.AddGridBoundColumn("InsertDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Insert Date", false, Unit.Percentage(6), HorizontalAlign.Center, true);
                this.AddGridBoundColumn("EffectiveDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(6), HorizontalAlign.Center, true);
                this.AddGridBoundColumn("NewBalance", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Balance", false, Unit.Percentage(7), HorizontalAlign.Right, true);
                this.AddGridBoundColumn("AccountCurrentBalance", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Account Balance", false, Unit.Percentage(7), HorizontalAlign.Right, true);
                this.AddGridBoundColumn("Rate", "Rate", Unit.Percentage(3), HorizontalAlign.Right, true);
                //end common
                this.AddGridBoundColumn("CanRollBack", "Rollback", Unit.Percentage(1), HorizontalAlign.Center, true);
                this.AddGridBoundColumn("Service", "Service", Unit.Percentage(7), HorizontalAlign.Left, true);
                //common
                this.AddGridBoundColumn("UserID", "Changed By", Unit.Percentage(6), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("Reference", "Reference", Unit.Percentage(32), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("Number", "Number", Unit.Percentage(4), HorizontalAlign.Left, true);
                //end common
            }
        }

        public void BindArrearTransactions(DataTable transactions)
        {
            List<GridEntity> gridEntities = new List<GridEntity>();
            foreach (DataRow dr in transactions.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                    gridEntities.Add(
                        new GridEntity(dr["TransactionTypeLoanDescription"].ToString(),
                            dr["TransactionTypeHTMLColour"].ToString(),
                            "",
                            dr["ArrearTransactionAmount"].ToString(),
                            dr["ArrearTransactionInsertDate"].ToString(),
                            dr["ArrearTransactionEffectiveDate"].ToString(),
                            dr["ArrearTransactionNewBalance"].ToString(),
                            dr["LoanAccountArrearBalance"].ToString(),
                            dr["ArrearTransactionRate"].ToString(),
                            dr["CanRollBack"].ToString(),
                            dr["Service"].ToString(),
                            dr["ArrearTransactionUserID"].ToString(),
                            dr["ArrearTransactionReference"].ToString(),
                            dr["ArrearTransactionNumber"].ToString()
                        )
                    );
            }

            DataSource = gridEntities;
            DataBind();
        }

        public void BindTransactions(DataTable transactions)
        {
            List<GridEntity> gridEntities = new List<GridEntity>();
            foreach (DataRow dr in transactions.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                    gridEntities.Add(
                        new GridEntity(dr["TransactionTypeLoanDescription"].ToString(),
                            dr["TransactionTypeHTMLColour"].ToString(),
                            "",
                            dr["LoanTransactionAmount"].ToString(),
                            dr["LoanTransactionInsertDate"].ToString(),
                            dr["LoanTransactionEffectiveDate"].ToString(),
                            dr["LoanTransactionNewBalance"].ToString(),
                            dr["LoanAccountCurrentBalance"].ToString(),
                            dr["LoanTransactionRate"].ToString(),
                            dr["CanRollBack"].ToString(),
                            dr["Service"].ToString(),
                            dr["LoanTransactionUserID"].ToString(),
                            dr["LoanTransactionReference"].ToString(),
                            dr["LoanTransactionNumber"].ToString()
                        )
                    );
            }
            
            DataSource = gridEntities;
            DataBind();
        }

        #region Private Classes

        /// <summary>
        /// Internal class to make binding to the grid a little easier as we have to support different data source entities.
        /// </summary>
        private class GridEntity
        {
            private string _description;
            private string _htmlColour;
            private string _foreColour;
            private string _amount;
            private string _insertDate;
            private string _effectiveDate;
            private string _newBalance;
            private string _accountCurrentBalance;
            private string _rate;
            private string _canRollback;
            private string _service;
            private string _userID;
            private string _reference;
            private string _number;

            public GridEntity(string description, string htmlColour, string foreColour, string amount, string insertDate, string effectiveDate, string newBalance, string accountCurrentBalance, string rate, string canRollback, string service, string userID, string reference, string number)
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
            }

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
            public string InsertDate
            {
                get { return _insertDate; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string EffectiveDate
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
        }
        #endregion
    }
}
