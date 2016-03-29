using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan
{
    public partial class PersonalLoanSAHLLifePolicySummary : SAHLCommonBaseView, IPersonalLoanSAHLLifePolicySummary
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindAccountSummary(IAccountCreditProtectionPlan creditProtectionAccount)
        {
            var creditProtectionPlanFinancialServices = creditProtectionAccount.GetFinancialServicesByType(FinancialServiceTypes.SAHLCreditProtectionPlan, new AccountStatuses[] { AccountStatuses.Open, AccountStatuses.Closed }).OrderByDescending(x => x.Key);
            var creditProtectionPlanFinancialService = creditProtectionPlanFinancialServices != null ? creditProtectionPlanFinancialServices.First() : null;
            if (creditProtectionPlanFinancialService != null)
            {
                lblPolicyNumber.Text = creditProtectionAccount.Key.ToString();
                lblCommencementDate.Text = creditProtectionAccount.OpenDate.Value.ToString(SAHL.Common.Constants.DateFormat);
                lblStatus.Text = creditProtectionAccount.AccountStatus.Description;
                lblLifePolicyPremium.Text = creditProtectionPlanFinancialService.Payment.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblSumInsured.Text = creditProtectionPlanFinancialService.FinancialServiceParent.Balance.Amount.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }


        public void BindInsurerName(string insurerName)
        {
            lblInsurer.Text = insurerName;
        }
    }
}