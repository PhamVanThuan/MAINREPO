using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;

namespace FurtherLendingTests.Rules
{
    [TestFixture, RequiresSTA]
    public class BaselRulesTests : TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.FLProcessor3);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            if (base.Browser != null)
            {
                base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            }
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
        }

        #region Tests

        /// <summary>
        /// This test case is used to ensure that the High Risk Basel II rule is correctly run when the FL Processor user
        /// accesses the Further Lending Calculator. It will ensure that the validation message is displayed on the screen to the
        /// user
        /// </summary>
        [Test, Description(@"This test case is used to ensure that the High Risk Basel II rule is correctly run when the FL Processor user
		accesses the Further Lending Calculator. It will ensure that the validation message is displayed on the screen to the
		user")]
        public void BaselIIHighRisk()
        {
            const string identifier = FurtherLendingTestCases.Basel2HighRisk;
            QueryResults r = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            Helper.GoToFurtherLendingCalculatorAndCheckIfMessageExists(identifier, r.Rows(0).Column("AccountKey").GetValueAs<int>(), base.Browser,
                @"Refer to our guidance on acceptable Behavioural score (Basel II). A score less than or equal to 671 represents a higher risk and typically a decline for further credit.", true);
        }

        /// <summary>
        /// This test case is used to ensure that the Moderate Risk Basel II rule is correctly run when the FL Processor user
        /// accesses the Further Lending Calculator. It will ensure that the validation message is displayed on the screen to the
        /// user
        /// </summary>
        [Test, Sequential, Description(@"This test case is used to ensure that the Moderate Risk Basel II rule is correctly run when the FL Processor user
		accesses the Further Lending Calculator. It will ensure that the validation message is displayed on the screen to the ")]
        public void BaselIIModerateRisk([Values(FurtherLendingTestCases.Basel2ModerateRiskLowerBound, FurtherLendingTestCases.Basel2ModerateRiskUpperBound)] string identifier)
        {
            QueryResults r = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            Helper.GoToFurtherLendingCalculatorAndCheckIfMessageExists(identifier, r.Rows(0).Column("AccountKey").GetValueAs<int>(), base.Browser,
                @"Refer to our guidance on acceptable Behavioural score (Basel II). A score between 672 and 695 inclusive suggests some risk & a cautious approach.", true);
        }

        /// <summary>
        /// If an account has a high enough behavioral score for Basel II then no validation warning should be displayed to the
        /// FL Processor when they access the Further Lending Calculator. This test case will navigate to the FL Calculator for
        /// one of these cases and checks for the non-existence of the Validation Summary.
        /// </summary>
        [Test, Description(@"If an account has a high enough behavioral score for Basel II then no validation warning should be displayed to the
		FL Processor when they access the Further Lending Calculator. This test case will navigate to the FL Calculator for
		one of these cases and checks for the non-existence of the Validation Summary.")]
        public void BaselIILowRisk()
        {
            const string identifier = FurtherLendingTestCases.Basel2LowRisk;
            QueryResults r = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            Helper.GoToFurtherLendingCalculatorAndCheckIfMessageExists(identifier, r.Rows(0).Column("AccountKey").GetValueAs<int>(), base.Browser, "Refer to our guidance on acceptable Behavioural score (Basel II).", false);
        }

        #endregion Tests
    }
}