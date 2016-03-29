using BuildingBlocks.Assertions;
using BuildingBlocks.Services.Contracts;
using ObjectMaps.Presenters.PersonalLoans;
using System;

namespace BuildingBlocks.Presenters.PersonalLoans
{
    public class PersonalLoanChangeInstalment : PersonalLoanChangeInstalmentControls
    {
        private static IWatiNService watinService;

        public PersonalLoanChangeInstalment()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public void ChangeInstalment(string comment)
        {
            PopulateComment(comment);
            ClickCalculate();
            ClickChangeInstalment();
        }

        public void AssertInstalmentBreakdown(double loanInstalment, double serviceFee, double creditLifePremium)
        {
            WatiNAssertions.AssertCurrencyLabel(base.lblNewLoanInstalment, Convert.ToDecimal(loanInstalment));
            WatiNAssertions.AssertCurrencyLabel(base.lblMonthlyServiceFee, Convert.ToDecimal(serviceFee));
            WatiNAssertions.AssertCurrencyLabel(base.lblCreditLifePremium, Convert.ToDecimal(creditLifePremium));
        }

        public void PopulateComment(string comment)
        {
            base.txtComments.Clear();
            base.txtComments.Value = comment.ToString();
        }

        public void ClickCalculate()
        {
            base.btnCalculate.Click();
        }

        public void ClickChangeInstalment()
        {
            watinService.HandleConfirmationPopup(base.btnChangeInstalment);
        }
    }
}