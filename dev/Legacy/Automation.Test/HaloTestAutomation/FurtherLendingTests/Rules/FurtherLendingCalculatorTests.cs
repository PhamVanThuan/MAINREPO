using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination.FurtherLending;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace FurtherLendingTests.Rules
{
    [RequiresSTA]
    public class FurtherLendingCalculatorTests : TestBase<BasePage>
    {
        private static IFurtherLendingService flService;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.FLProcessor);
            flService = ServiceLocator.Instance.GetService<IFurtherLendingService>();
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

        #region tests

        /// <summary>
        /// If an account already has existing Further Lending applications then they need to access those cases in the workflow and not try and create further applications
        /// using the Further Lending Calculator. This test will ensure that the warning message is displayed on the screen when the user tries to access
        /// the Further Lending Calculator for this account.
        /// </summary>
        [Test, Description("A warning is displayed on the FL Calculator for accounts that already have further lending applications in progress"), Category("All")]
        public void _037_FurtherLendingProgressMessageDisplayedOnCalculator()
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            var offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.QA, Workflows.ApplicationManagement,
                OfferTypeEnum.FurtherAdvance, "FLAutomation");
            string msg = String.Format(@"Further lending was initiated by");
            var accountKey = Service<IApplicationService>().GetOfferAccountKey(offerKey);
            var newBrowser = new TestBrowser(TestUsers.FLProcessor3);
            //navigate to the client search
            newBrowser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(newBrowser);
            newBrowser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            //load the client into the CBO
            newBrowser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            newBrowser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            newBrowser.Navigate<LoanServicingCBO>().LoanAdjustments();
            newBrowser.Navigate<LoanServicingCBO>().FurtherLendingCalculator();
            newBrowser.Page<FurtherLendingPreCheck>().Next();
            newBrowser.Page<BasePageAssertions>().AssertFrameContainsText(msg);
            newBrowser.Dispose();
            newBrowser = null;
        }

        /// <summary>
        /// A warning message should be displayed when the FL Calculator is loaded if the mortgage loan account has certain details types loaded against it.
        /// This test ensures that the generic warning message displays correctly as well as a list of the detail types that are preventing the
        /// further lending from taking place.
        /// </summary>
        [Test, Description("An account undergoing cancellation cannot have a further lending application created against it."), Category("Rule")]
        public void _040_UnderCancellationMessageDisplayedOnCalculator()
        {
            //search for the case
            int accountKey = Service<IDetailTypeService>().GetOpenAccountWithDetailType(DetailTypeEnum.CancellationRegistered);
            //start the browser
            //navigate to the client search
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            //load the client into the CBO

            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            base.Browser.Navigate<LoanServicingCBO>().FurtherLendingCalculator();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cancellation Registered");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Further Lending can not be processed because of Detail Types.");
        }

        [Test, Description("An account with Bond variation in progress on the loan cannot have a further lending application created against it."), Category("Rule")]
        public void _041_BondVariationInProgressMessageDisplayedOnCalculator()
        {
            //search for the case
            int accountKey = Service<IDetailTypeService>().GetOpenAccountWithDetailType(DetailTypeEnum.BondVariationInProgress);
            //start the browser
            //navigate to the client search
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            //load the client into the CBO

            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            base.Browser.Navigate<LoanServicingCBO>().FurtherLendingCalculator();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Bond Variation In Progress");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Further Lending can not be processed because of Detail Types.");
        }

        [Test, Description("An account with No Readvances or Further Loans detail type loaded should display a warning to the user."), Category("Rule")]
        public void _042_NoReadvancesOrFurtherLoansMessageDisplayedOnCalculator()
        {
            //search for the case
            int accountKey = Service<IDetailTypeService>().GetOpenAccountWithDetailType(DetailTypeEnum.NoReadvancesorFurtherLoans);
            //start the browser
            //navigate to the client search
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            //load the client into the CBO

            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            base.Browser.Navigate<LoanServicingCBO>().FurtherLendingCalculator();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("No Readvances or Further Loans");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The following detail types are loaded against the loan.");
        }

        [Test, Description("An account with Varifix Opt-Out 90 Day Pending detail type loaded should display a warning to the user."), Category("Rule")]
        public void _043_VarifixOptOut90DayPendingMessageDisplayedOnCalculator()
        {
            //search for the case
            int accountKey = Service<IDetailTypeService>().GetOpenAccountWithDetailType(DetailTypeEnum.VariFixOpt_Out90DayPending);
            //start the browser
            //navigate to the client search
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            //load the client into the CBO

            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            base.Browser.Navigate<LoanServicingCBO>().FurtherLendingCalculator();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Varifix Opt-Out 90 Day Pending");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The following detail types are loaded against the loan.");
        }

        [Test, Description("An account with Varifix Opt-Out 90 Day Pending detail type loaded and Application Amount + Current Balance combined is greater than the Loan Agreement Amount cannot have a further lending application created against it."), Category("Rule")]
        [Ignore]
        public void _044_VarifixOptOut90DayPendingApplicationAmountExceedsLoanAgreementAmountMessageDisplayedOnCalculator()
        {
            var results = flService.GetFLAutomationRowByTestIdentifier("VarifixOptOut90DayPendingAndAmountExceedsLoanAgreementAmount");
            var accountKey = results.Rows(0).Column("AccountKey").GetValueAs<int>();
            //navigate to the client search
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            //load the client into the CBO
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            base.Browser.Navigate<LoanServicingCBO>().FurtherLendingCalculator();
            base.Browser.Page<FurtherLendingPreCheck>().Next();
            base.Browser.Page<FurtherLendingCalculator>().PopulateFLValuesFromTestData(results, 0, 0);
            base.Browser.Page<FurtherLendingCalculator>().ClickCalculate();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Varifix opt out 90 day pending hold is loaded on this loan. The application amount must be lower than the loan agreement amount.");
        }

        /// <summary>
        /// A warning should appear on the FL Calculator if the effective LTP is greater than 85%
        /// </summary>
        [Test, Description("A warning should appear on the FL Calculator if the effective LTP is greater than 85%"), Category("Rule")]
        public void LoanValueToPurchaserPriceGreaterThan85Percent()
        {
            int accountKey = Service<IFurtherLendingService>().GetAccountForLTPTest();
            //navigate to the client search
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            //load the client into the CBO
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);

            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            base.Browser.Navigate<LoanServicingCBO>().FurtherLendingCalculator();
            base.Browser.Page<FurtherLendingPreCheck>().Next();
            //need to enter a further lending amount and then hit calculate and the warning will appear
            bool update = base.Browser.Page<FurtherLendingCalculator>().PopulateFLValuesFromRequiredValues(OfferTypeEnum.Readvance);
            if (!update)
            {
                update = base.Browser.Page<FurtherLendingCalculator>().PopulateFLValuesFromRequiredValues(OfferTypeEnum.FurtherAdvance);
            }
            if (!update)
            {
                update = base.Browser.Page<FurtherLendingCalculator>().PopulateFLValuesFromRequiredValues(OfferTypeEnum.FurtherLoan);
            }
            base.Browser.Page<FurtherLendingCalculator>().ClickCalculate();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The LTP is greater than 0.85%");
        }

        [Test, Description("Asserts that the expected validation messages are thrown when accessing an account which has an active subsidy and an accepted offer with loan conditions 222 or 223")]
        public void AssertExpectedValidationMessagesAppearWhenViewingAccountsWithActiveSubsidyAndAcceptedOfferWithLoanConditions222Or223()
        {
            int accountKey = Service<IAccountService>().GetAccountsWithActiveSubsidyAndAcceptedOfferWithLoanConditions222Or223().AccountKey;
            
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            
            base.Browser.Navigate<LoanServicingCBO>().ParentAccountNode(accountKey);
            AssertValidationMessage();
            
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            AssertValidationMessage();

            base.Browser.Navigate<LoanServicingCBO>().FurtherLendingCalculator();
            AssertValidationMessage();
        }

        private void AssertValidationMessage()
        {
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("It is a condition of the loan that the instalment shall be paid by way of a salary stop order.");
        }

        #endregion tests
    }
}