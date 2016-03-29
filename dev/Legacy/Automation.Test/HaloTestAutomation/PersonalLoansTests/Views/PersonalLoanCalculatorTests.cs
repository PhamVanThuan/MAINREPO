using BuildingBlocks.Presenters.PersonalLoans;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.Views
{
    [RequiresSTA]
    public class PersonalLoanCalculatorTests : PersonalLoansWorkflowTestBase<PersonalLoanCalculator>
    {
        private decimal initiationFee = 1140;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.ManageLead, WorkflowRoleTypeEnum.PLConsultantD, true);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.CalculateApplication);
        }

        /// <summary>
        /// Ensures that the screen loads correctly by default
        /// </summary>
        [Test, Description("Ensures that the screen loads correctly by default")]
        public void PersonalLoanCalculatorDefaultState()
        {
            base.View.CheckDefaultState();
        }

        /// <summary>
        /// Ensures that the screen loads correctly after performing the default calculation by entering a loan amount, no term
        /// and then pressing calculate.
        /// </summary>
        [Test, Description(@"Ensures that the screen loads correctly after performing the default calculation by entering a loan amount, no term
        and then pressing calculate.")]
        public void PersonalLoanCalculatorDefaultCalculations()
        {
            decimal loanAmount = 25000;
            base.View.Calculate(0, loanAmount);
            base.View.CheckDefaultCalculations(loanAmount);
        }

        /// <summary>
        /// The calculator should bring back a set of default options that the user can then add their own options to for comparison
        /// purposes.
        /// </summary>
        [Test, Description(@"The calculator should bring back a set of default options that the user can then add their own options to for comparison
        purposes.")]
        public void CalculatingWithNewTermAddsTheOptionToGrid()
        {
            decimal loanAmount = 25000;
            int term = 7;
            base.View.Calculate(0, loanAmount);
            base.View.Calculate(term, loanAmount);
            base.View.AssertOptionExists(term);
            int newterm = 19;
            base.View.Calculate(newterm, loanAmount);
            base.View.AssertOptionExists(newterm);
            base.View.AssertOptionExists(term);
        }

        /// <summary>
        /// Checking and unchecking the life premium checkbox should change the calculated life premium accordingly.
        /// </summary>
        [Test, Description("Checking and unchecking the life premium checkbox should change the calculated life premium accordingly.")]
        public void AddingRemovingLifePremiumRecalculatesPremium()
        {
            decimal loanAmount = 25000;
            base.View.Calculate(0, loanAmount);
            decimal lifePremium = (loanAmount + initiationFee) * 0.00375M;
            base.View.SelectLifePremiumAndRecalc(false);
            base.View.AssertCreditLifePremium(0.00M);
            base.View.SelectLifePremiumAndRecalc(true);
            base.View.AssertCreditLifePremium(lifePremium);
        }

        /// <summary>
        /// Once an option is selected from the grid, the create application button should be enabled allowing the user to create the
        /// application.
        /// </summary>
        [Test, Description(@"Once an option is selected from the grid, the create application button should be enabled allowing the user to create the
        application.")]
        public void SelectingOptionEnablesCreateApplicationButton()
        {
            var loanAmount = 25000;
            base.View.Calculate(0, loanAmount);
            int term = 18;
            base.View.SelectOptionFromGrid(term);
            base.View.AssertCreateApplicationButton(true);
        }

        /// <summary>
        /// Validates that a rule message is displayed if the minimum term for a personal application is breached
        /// </summary>
        [Test, Description("Validates that a rule message is displayed if the minimum term for a personal application is breached")]
        public void CheckMinTermRule()
        {
            decimal loanAmount = 25000.00M;
            var term = 5;
            base.View.Calculate(term, loanAmount);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Loan Term should not be less than 6 months.");
        }

        /// <summary>
        /// Validates that a rule message is displayed if the max term for a personal application is breached
        /// </summary>
        [Test, Description("Validates that a rule message is displayed if the max term for a personal application is breached")]
        public void CheckMaxTermRule()
        {
            decimal loanAmount = 25000.00M;
            var term = 49;
            base.View.Calculate(term, loanAmount);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Loan Term should not be greater than 48 months.");
        }

        /// <summary>
        /// Validates that a rule message is displayed if the minimum loan amount for a personal application is breached
        /// </summary>
        [Test, Description("Validates that a rule message is displayed if the minimum loan amount for a personal application is breached")]
        public void CheckMinLoanValueRule()
        {
            decimal loanAmount = 9999.99M;
            var term = 0;
            base.View.Calculate(term, loanAmount);
            base.Browser.Page<BasePageAssertions>()
                .AssertValidationMessageExists("Personal Loan amount is too low. Loan amount is R 9999.99 . The minimum allowed is: R 10000.");
        }

        /// <summary>
        /// Validates that a rule message is displayed if the max loan amount for a personal application is breached
        /// </summary>
        [Test, Description("Validates that a rule message is displayed if the max loan amount for a personal application is breached")]
        public void CheckMaxLoanValueRule()
        {
            decimal loanAmount = 70000.01M;
            var term = 0;
            base.View.Calculate(term, loanAmount);
            base.Browser.Page<BasePageAssertions>()
                .AssertValidationMessageExists("Personal Loan amount is too high. Loan amount is R 70000.01 . The maximum allowed is: R 70000.");
        }
    }
}