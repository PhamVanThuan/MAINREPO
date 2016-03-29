using System;
using System.Linq;
using System.Web.UI.WebControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;

namespace SAHL.Web.Views.PersonalLoan
{
    public partial class UnsecuredLoanSummary : SAHLCommonBaseView, IUnsecuredLoanSummary
    {
        public void BindAccountSummary(IAccountPersonalLoan accountPersonalLoan)
        {
            var personalLoanFinancialServices = accountPersonalLoan.GetFinancialServicesByType(FinancialServiceTypes.PersonalLoan, new AccountStatuses[] { AccountStatuses.Open, AccountStatuses.Closed }).OrderByDescending(x => x.Key);
            //there should only be one personal financial service
            var personalLoanFinancialService = personalLoanFinancialServices.First();
            summaryHeading.InnerText = string.Format("{0} Summary", accountPersonalLoan.Product.Description);
            lblAccountNumber.Text = accountPersonalLoan.Key.ToString();
            lblLoanAmount.Text = personalLoanFinancialService.Balance.LoanBalance.InitialBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblIncome.Text = accountPersonalLoan.GetHouseholdIncome().ToString(SAHL.Common.Constants.CurrencyFormat);
            lblDebitOrderDay.Text = personalLoanFinancialService.CurrentBankAccount.DebitOrderDay.ToString();
            lblAccountStatus.Text = accountPersonalLoan.AccountStatus.Description;

            if (personalLoanFinancialService.CurrentBankAccount != null)
            {
                var debitOrderDay = personalLoanFinancialService.CurrentBankAccount.DebitOrderDay;
                var settlementDate = new DateTime(accountPersonalLoan.OpenDate.Value.Year, accountPersonalLoan.OpenDate.Value.Month, debitOrderDay);
                settlementDate = settlementDate.AddMonths(1);
                settlementDate = settlementDate.AddMonths(personalLoanFinancialService.Balance.LoanBalance.Term);
                lblSettlementDate.Text = settlementDate.ToString(SAHL.Common.Constants.DateFormat);
            }

            lblOpenDate.Text = accountPersonalLoan.OpenDate.Value.ToString(SAHL.Common.Constants.DateFormat);

            lblCurrentBalance.Text = accountPersonalLoan.InstallmentSummary.CurrentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);

            lblArrearBalance.Text = accountPersonalLoan.InstallmentSummary.TotalArrearsBalance.ToString(SAHL.Common.Constants.CurrencyFormat);

            lblRemainingTerm.Text = personalLoanFinancialService.Balance.LoanBalance.RemainingInstalments.ToString() + " months";
            lblInterestRate.Text = personalLoanFinancialService.Balance.LoanBalance.InterestRate.ToString(SAHL.Common.Constants.RateFormat);

            lblPersonalLoanInstalment.Text = accountPersonalLoan.InstallmentSummary.TotalLoanInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblMonthlyServiceFee.Text = accountPersonalLoan.InstallmentSummary.MonthlyServiceFee.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblTotalInstalment.Text = accountPersonalLoan.InstallmentSummary.TotalAmountDue.ToString(SAHL.Common.Constants.CurrencyFormat);

            double interestMonthToDate = 0;
            double totalInterest = 0;

            lblCreditLifePremium.Text = accountPersonalLoan.InstallmentSummary.TotalPremium.ToString(SAHL.Common.Constants.CurrencyFormat);

            // Calculate interest is only valid on an open personal loan account
            if (accountPersonalLoan.AccountStatus.Key == (int)AccountStatuses.Open)
            {
                accountPersonalLoan.CalculateInterest(out interestMonthToDate, out totalInterest);
            }

            lblInterestMonthToDate.Text = interestMonthToDate.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblInterestForMonth.Text = totalInterest.ToString(SAHL.Common.Constants.CurrencyFormat);
        }

        public void BindLifePolicyClaimPending(string claimDate)
        {
            pendingLifePolicyClaimHeading.InnerText = string.Format("A pending Life Policy Claim exists - Claim Date: {0}", claimDate);
        }

        protected void LifePolicyClaimGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            SAHL.Common.BusinessModel.Interfaces.ILifePolicyClaim lifePolicyClaim = e.Row.DataItem as SAHL.Common.BusinessModel.Interfaces.ILifePolicyClaim;

            if (e.Row.DataItem != null)
            {
                cells[0].Text = lifePolicyClaim.ClaimType == null ? " " : lifePolicyClaim.ClaimType.Description;
                cells[1].Text = lifePolicyClaim.ClaimStatus == null ? " " : lifePolicyClaim.ClaimStatus.Description;
                cells[2].Text = lifePolicyClaim.ClaimDate == null ? " " : lifePolicyClaim.ClaimDate.ToShortDateString();
            }
        }
    }
}