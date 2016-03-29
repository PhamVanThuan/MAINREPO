using BuildingBlocks.Assertions;
using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using ObjectMaps.Presenters.PersonalLoans;
using System;

namespace BuildingBlocks.Presenters.PersonalLoans
{
    public class PersonalLoanChangeTerm : PersonalLoanChangeTermControls
    {
        private static IWatiNService watinService;

        public PersonalLoanChangeTerm()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public void ChangeTerm(int newTerm, string comment)
        {
            base.txtNewRemainingTerm.Clear();
            base.txtNewRemainingTerm.Value = newTerm.ToString();
            base.txtComments.Clear();
            base.txtComments.Value = comment.ToString();
            base.btnCalculate.Click();
            watinService.HandleConfirmationPopup(base.btnProcessTermChange);
        }

        public void PopulateFields(int newTerm, string comment)
        {
            base.txtNewRemainingTerm.Clear();
            base.txtNewRemainingTerm.Value = newTerm.ToString();
            base.txtComments.Clear();
            base.txtComments.Value = comment.ToString();
        }

        public void ClickCalculate()
        {
            base.btnCalculate.Click();
        }

        public void ClickProcessTermChange()
        {
            watinService.HandleConfirmationPopup(base.btnProcessTermChange);
        }

        public void AssertInstalmentBreakdown(double loanInstalment, double serviceFee, double creditLifePremium)
        {
            WatiNAssertions.AssertCurrencyLabel(base.lblNewLoanInstalment, Convert.ToDecimal(loanInstalment));
            WatiNAssertions.AssertCurrencyLabel(base.lblMonthlyServiceFee, Convert.ToDecimal(serviceFee));
            WatiNAssertions.AssertCurrencyLabel(base.lblCreditLifePremium, Convert.ToDecimal(creditLifePremium));
        }

        public void AssertMaxTermAllowed(int maxAllowedRemainingTerm)
        {
            Assert.That(base.lblMaxRemainingTerm.Text.Contains(maxAllowedRemainingTerm.ToString()));
        }
    }
}