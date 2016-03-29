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
using SAHL.Web.Views.QuickCash.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.QuickCash
{
    public partial class QuickCashThirdPartyPayments : SAHLCommonBaseView, IQuickCashThirdPartyPayments
    {
        ILookupRepository lookups;
        double totalAmt;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;

            lookups = RepositoryFactory.GetRepository<ILookupRepository>();
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            txtAmount.Attributes["onkeyup"] = "calculatePortionToClient()" + txtAmount.Attributes["onkeyup"];
        }

        public IApplicationExpense GetUpdatedApplicationExpense(IApplicationExpense appExpense)
        {
            if (ddlExpenseType.SelectedValue != "-select-")
                appExpense.ExpenseType = lookups.ExpenseTypes.ObjectDictionary[ddlExpenseType.SelectedValue];
            appExpense.ExpenseAccountName = txtExpenseName.Text;
            appExpense.ExpenseAccountNumber = txtExpenseAccountNumber.Text;
            appExpense.TotalOutstandingAmount = Convert.ToDouble(txtAmount.Amount);

            return appExpense;
        }

        public void BindThirdPartyPaymentDetails(IApplicationExpense appThirdPartyExpense)
        {
            ddlExpenseType.SelectedValue = appThirdPartyExpense.ExpenseType.Key.ToString();
            txtExpenseName.Text = appThirdPartyExpense.ExpenseAccountName;
            txtExpenseAccountNumber.Text = appThirdPartyExpense.ExpenseAccountNumber;
            txtAmount.Amount = Convert.ToDouble(appThirdPartyExpense.TotalOutstandingAmount);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        
        public void BindExpenseTypes(IEventList<IExpenseType> expenseType)
        {
            ddlExpenseType.DataTextField = "Description";
            ddlExpenseType.DataValueField = "Key";
            ddlExpenseType.DataSource = expenseType;
            ddlExpenseType.DataBind();
        }

        public void BindThirdPartyPaymentsGrid(IEventList<IApplicationExpense> applicationExpenses,bool autoPostBack)
        {
            grdQuickCashThirdPartyPayments.AddGridBoundColumn("", "Expense Type", Unit.Percentage(10), HorizontalAlign.Left, true);
            grdQuickCashThirdPartyPayments.AddGridBoundColumn("", "Bank Account", Unit.Percentage(35), HorizontalAlign.Left, true);
            grdQuickCashThirdPartyPayments.AddGridBoundColumn("", "Expense Name", Unit.Percentage(15), HorizontalAlign.Left, true);
            grdQuickCashThirdPartyPayments.AddGridBoundColumn("", "Expense Account No", Unit.Percentage(15), HorizontalAlign.Left, true);
            grdQuickCashThirdPartyPayments.AddGridBoundColumn("", "Reference", Unit.Percentage(14), HorizontalAlign.Left, true);
            grdQuickCashThirdPartyPayments.AddGridBoundColumn("", "Amount", Unit.Percentage(10), HorizontalAlign.Left, true);

            grdQuickCashThirdPartyPayments.DataSource = applicationExpenses;
            if (autoPostBack)
                grdQuickCashThirdPartyPayments.PostBackType = GridPostBackType.SingleClick;
            grdQuickCashThirdPartyPayments.DataBind();
        }

        public void BindQuickCashDetails(double amtAvailable, double amtToClient)
        {
            txtAmtRequested.Amount = amtAvailable;
            txtAvailableAmount.Amount = amtToClient;
            txtPortionToClient.Amount = amtToClient;
        }

        protected void grdQuickCashThirdPartyPayments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            IApplicationExpense appExpense = e.Row.DataItem as IApplicationExpense;

            if (e.Row.DataItem != null)
            {
                cells[0].Text = appExpense.ExpenseType.Description;
                if (appExpense.ApplicationDebtSettlements != null && appExpense.ApplicationDebtSettlements.Count > 0)
                    cells[1].Text = appExpense.ApplicationDebtSettlements[0].BankAccount.AccountName + " " +
                        appExpense.ApplicationDebtSettlements[0].BankAccount.ACBBranch.ACBBank.ACBBankDescription + " " +
                        appExpense.ApplicationDebtSettlements[0].BankAccount.ACBBranch.ACBBranchDescription + appExpense.ApplicationDebtSettlements[0].BankAccount.AccountNumber;
                else
                    cells[1].Text = "";
                cells[2].Text = appExpense.ExpenseAccountName;
                cells[3].Text = appExpense.ExpenseAccountNumber;
                cells[4].Text = appExpense.ExpenseReference;
                cells[5].Text = appExpense.TotalOutstandingAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                totalAmt += Convert.ToDouble(appExpense.TotalOutstandingAmount);
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                cells[0].Text = "Total";
                cells[5].Text = totalAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        protected void grdQuickCashThirdPartyPayments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onGridQuickCashPaymentSelectedIndexChanged != null)
                onGridQuickCashPaymentSelectedIndexChanged(sender, new KeyChangedEventArgs(grdQuickCashThirdPartyPayments.SelectedIndex));
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            OnSubmitButtonClicked(sender, e);
        }

        public event EventHandler OnSubmitButtonClicked;

        public event EventHandler OnCancelButtonClicked;

        public event KeyChangedEventHandler onGridQuickCashPaymentSelectedIndexChanged;

    }
}
