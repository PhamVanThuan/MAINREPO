using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination.FurtherLending;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FurtherLendingTests.Rules
{
    [TestFixture, RequiresSTA]
    public class DebtCounsellingRulesTests : TestBase<BasePage>
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
        /// This test will find a legal entity that plays a role in 2 accounts that both qualify for further lending. It will then create a further lending application for
        /// each of these accounts but place only one of them under debt counselling. When loading up the application that is not linked to the account under debt counselling
        /// a warning for the common legal entity between the accounts who is under debt counselling should be displayed.
        /// </summary>
        [Test, Description(@"This test will find a legal entity that plays a role in 2 accounts that both qualify for further lending. It will then create a further lending application for
        each of these accounts but place only one of them under debt counselling. When loading up the application that is not linked to the account under debt counselling
        a warning for the common legal entity between the accounts who is under debt counselling should be displayed.")]
        public void _01_RelatedLegalEntityUnderDebtCounsellingFurtherLendingApplicationSummary()
        {
            var insertedKeys = new List<int>();
            try
            {
                var legalEntities = Service<IFurtherLendingService>().GetLegalEntityWhoQualifiesForMoreThanOneFLApplication();
                //details for the first account
                var accountKey = legalEntities.Rows(0).Column("AccountKey").GetValueAs<int>();
                var relatedAccountKey = legalEntities.Rows(1).Column("AccountKey").GetValueAs<int>();
                //details for the second account
                decimal amount = Decimal.Round(legalEntities.Rows(0).Column("FurtherAdvanceAmount").GetValueAs<decimal>(), 2);
                decimal relatedAccountAmount = Decimal.Round(legalEntities.Rows(1).Column("FurtherAdvanceAmount").GetValueAs<decimal>(), 2);
                //create a further lending case
                Helper.NavigateToFurtherLendingCalculator(accountKey, base.Browser);
                base.Browser.Page<FurtherLendingCalculator>().CreateApplication(amount.ToString(), CorrespondenceMedium.Fax, OfferTypes.FurtherAdvance, accountKey);
                //navigate to the further lending calculator for the other case
                Helper.NavigateToFurtherLendingCalculator(relatedAccountKey, base.Browser);
                base.Browser.Page<FurtherLendingCalculator>().CreateApplication(relatedAccountAmount.ToString(), CorrespondenceMedium.Fax, OfferTypes.FurtherAdvance,
                    accountKey);
                //now put one account in debtcounselling
                insertedKeys = Service<IDebtCounsellingService>().AddAccountUnderDebtCounselling(accountKey);
                //load the related case onto the FloBO
                int offerKey = base.Service<IAccountService>().GetOfferForAccount(relatedAccountKey, OfferTypeEnum.FurtherAdvance, OfferStatusEnum.Open);
                //add the case
                base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.AwaitingApplication);
                DebtCounsellingAssertions.AssertRelatedAccountDebtCounsellingRule(base.Browser, accountKey, relatedAccountKey, offerKey);
            }
            finally
            {
                Service<IDebtCounsellingService>().RemoveDebtCounsellingCase(insertedKeys[0], insertedKeys[1]);
            }
        }

        /// <summary>
        /// This test will find a legal entity who is related to 2 accounts that both qualify for a further lending application. It will then create a further lending
        /// application for one of these accounts and navigate to the further lending calculator for the other account ensuring that a warning is displayed and that the
        /// further lending application cannot be created.
        /// </summary>
        [Test, Description(@"This test will find a legal entity who is related to 2 accounts that both qualify for a further lending application. It will then create a further lending
        application for one of these accounts and navigate to the further lending calculator for the other account ensuring that a warning is displayed and that the
        further lending application cannot be created.")]
        public void _02_RelatedLegalEntityUnderDebtCounsellingFurtherLendingCalculatorCreate()
        {
            var insertedKeys = new List<int>();
            try
            {
                var legalEntities = Service<IFurtherLendingService>().GetLegalEntityWhoQualifiesForMoreThanOneFLApplication();
                //details for the first account
                var accountKey = legalEntities.Rows(0).Column("AccountKey").GetValueAs<int>();
                var relatedAccountKey = legalEntities.Rows(1).Column("AccountKey").GetValueAs<int>();
                //details for the second account
                decimal amount = Decimal.Round(legalEntities.Rows(0).Column("FurtherAdvanceAmount").GetValueAs<decimal>(), 2);
                //create a further lending case
                Helper.NavigateToFurtherLendingCalculator(accountKey, base.Browser);
                base.Browser.Page<FurtherLendingCalculator>().CreateApplication(amount.ToString(), CorrespondenceMedium.Fax, OfferTypes.FurtherAdvance, accountKey);
                //now put one account in debtcounselling
                insertedKeys = Service<IDebtCounsellingService>().AddAccountUnderDebtCounselling(accountKey);
                //navigate to the further lending calculator for the other case
                Helper.NavigateToFurtherLendingCalculator(relatedAccountKey, base.Browser);
                //There should be warnings
                DebtCounsellingAssertions.AssertRelatedAccountDebtCounsellingRule(base.Browser, accountKey, relatedAccountKey, null);
                base.Browser.Page<FurtherLendingCalculator>().AssertNextButtonState(false);
            }
            finally
            {
                Service<IDebtCounsellingService>().RemoveDebtCounsellingCase(insertedKeys[0], insertedKeys[1]);
            }
        }

        /// <summary>
        /// This test will take an account that has an open further lending application and then put the account into debt counselling. It will then load the
        /// case onto the FloBO to ensure that the warning message indicating that the account is under debt counselling is displayed.
        /// </summary>
        [Test, Description(@"This test will take an account that has an open further lending application and then put the account into debt counselling. It will then load the
        case onto the FloBO to ensure that the warning message indicating that the account is under debt counselling is displayed.")]
        public void _03_AccountUnderDebtCounsellingExistingFurtherLendingApplication()
        {
            var insertedKeys = new List<int>();
            try
            {
                base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
                int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                    OfferTypeEnum.FurtherAdvance, "FLAutomation");
                var accountKey = base.Service<IApplicationService>().GetOfferAccountKey(offerKey);
                //put this account under debt counselling
                insertedKeys = Service<IDebtCounsellingService>().AddAccountUnderDebtCounselling(accountKey);
                base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
                base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("This Account is undergoing Debt Counselling.");
            }
            finally
            {
                Service<IDebtCounsellingService>().RemoveDebtCounsellingCase(insertedKeys[0], insertedKeys[1]);
            }
        }

        #endregion Tests
    }
}