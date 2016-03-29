using BuildingBlocks.Assertions;
using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using ObjectMaps.Presenters.PersonalLoans;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Presenters.PersonalLoans
{
    public class PersonalLoanCalculator : PersonalLoanCalculatorControls
    {
        private readonly ICommonService commonService;
        private decimal initiationFee = 1140.00M;
        private decimal monthlyServiceFee = 57.00M;
        private decimal lifePremiumFactor = 0.00375M;

        public PersonalLoanCalculator()
        {
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        /// <summary>
        /// Checks that the presenter loads correctly
        /// </summary>
        public void CheckDefaultState()
        {
            Assert.That(base.chkLifePolicy.Checked);
            Assert.That(base.txtLoanAmountCents.Exists);
            Assert.That(base.txtLoanAmountRands.Exists);
            Assert.That(base.txtLoanTerm.Exists);
            Assert.That(!base.btnCreateApplication.Enabled);
        }

        /// <summary>
        /// Checks that the default behaviour after supplying the amount and clicking calculate
        /// </summary>
        /// <param name="amount"></param>
        public void CheckDefaultCalculations(decimal amount)
        {
            WatiNAssertions.AssertCurrencyLabel(base.lblInitiationFee, initiationFee);
            WatiNAssertions.AssertCurrencyLabel(base.lblMonthlyFee, monthlyServiceFee);
            var lifePremium = (amount + initiationFee) * lifePremiumFactor;
            WatiNAssertions.AssertCurrencyLabel(base.lblCreditLifePremium, lifePremium);
            //check that we have added the correct options
            var list = new List<int> { 6, 12, 18, 24, 30, 36, 42, 48 };
            var option = (from l in list where base.gridOptionExists(l) == false select l).FirstOrDefault();
            Assert.That(option == 0, string.Format("Default option for term {0} was missing", option));
            //button should still be disable
            Assert.That(!base.btnCreateApplication.Enabled);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="term"></param>
        /// <param name="amount"></param>
        public void Calculate(int term, decimal amount)
        {
            string rands, cents;
            if (term > 0)
                base.txtLoanTerm.Value = term.ToString();
            commonService.SplitRandsCents(out rands, out cents, amount.ToString());
            base.txtLoanAmountRands.Value = rands;
            base.txtLoanAmountCents.Value = cents;
            base.btnCalculate.Click();
        }

        /// <summary>
        /// Checks that an option with the term specified has been added to the grid
        /// </summary>
        /// <param name="term"></param>
        public void AssertOptionExists(int term)
        {
            Assert.That(base.gridOptionExists(term));
        }

        /// <summary>
        /// Checks the value of the credit life premium
        /// </summary>
        /// <param name="expectedValue"></param>
        public void AssertCreditLifePremium(decimal expectedValue)
        {
            WatiNAssertions.AssertCurrencyLabel(base.lblCreditLifePremium, expectedValue);
        }

        /// <summary>
        /// Sets the state of the checkbox and recalculates
        /// </summary>
        /// <param name="state"></param>
        public void SelectLifePremiumAndRecalc(bool state)
        {
            base.chkLifePolicy.Checked = state;
            base.btnCalculate.Click();
        }

        /// <summary>
        /// Selects the option from the grid when provided with the term
        /// </summary>
        /// <param name="term"></param>
        public void SelectOptionFromGrid(int term)
        {
            var row = base.GetTableRowByTermValue(term);
            row.Click();
        }

        /// <summary>
        /// Checks the state of the create application button.
        /// </summary>
        /// <param name="p"></param>
        public void AssertCreateApplicationButton(bool p)
        {
            Assert.That(base.btnCreateApplication.Enabled == p);
        }

        /// <summary>
        /// Selects the create application button
        /// </summary>
        /// <param name="p"></param>
        public void CreateApplication()
        {
            btnCreateApplication.Click();
        }

        public void UncheckCreditLifePremium()
        {
            base.chkLifePolicy.Checked = false;
        }

        /// <summary>
        ///
        /// </summary>
        public void UpdateApplication()
        {
            base.btnUpdateApplication.Click();
        }
    }
}