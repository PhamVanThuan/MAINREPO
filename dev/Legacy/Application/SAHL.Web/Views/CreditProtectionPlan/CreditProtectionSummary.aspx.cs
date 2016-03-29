using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.CreditProtectionPlan.Interfaces;

namespace SAHL.Web.Views.CreditProtectionPlan
{
    public partial class CreditProtectionSummary : SAHLCommonBaseView, ICreditProtectionSummary
    {
        public void BindAccountSummary(IAccountCreditProtectionPlan creditProtectionAccount)
        {
            var creditProtectionPlanFinancialServices = creditProtectionAccount.GetFinancialServicesByType(FinancialServiceTypes.SAHLCreditProtectionPlan, new AccountStatuses[] { AccountStatuses.Open, AccountStatuses.Closed }).OrderByDescending(x => x.Key);
            var creditProtectionPlanFinancialService = creditProtectionPlanFinancialServices.First();

            lblPolicyNumber.Text = creditProtectionAccount.Key.ToString();
            lblOpenDate.Text = creditProtectionAccount.OpenDate.Value.ToString(SAHL.Common.Constants.DateFormat);
            lblAccountStatus.Text = creditProtectionAccount.AccountStatus.Description;
            lblLifePolicyPremium.Text = creditProtectionPlanFinancialService.Payment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblSumInsured.Text = creditProtectionPlanFinancialService.FinancialServiceParent.Balance.Amount.ToString(SAHL.Common.Constants.CurrencyFormat);
        }

        public void BindLifePolicyClaimGrid(IList<SAHL.Common.BusinessModel.Interfaces.ILifePolicyClaim> lifePolicyClaims)
        {
            lifePolicyClaims.OrderByDescending(x => x.ClaimDate);

            LifePolicyClaimGrid.AddGridBoundColumn("", "Claim Type", Unit.Percentage(40), HorizontalAlign.Left, true);
            LifePolicyClaimGrid.AddGridBoundColumn("", "Claim Status", Unit.Percentage(40), HorizontalAlign.Left, true);
            LifePolicyClaimGrid.AddGridBoundColumn("", "Claim Date", Unit.Percentage(20), HorizontalAlign.Left, true);

            LifePolicyClaimGrid.DataSource = lifePolicyClaims;
            LifePolicyClaimGrid.DataBind();
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