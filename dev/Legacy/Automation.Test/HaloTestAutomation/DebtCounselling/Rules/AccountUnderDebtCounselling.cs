using System.Collections.Generic;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination.FurtherLending;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core;
using WatiN.Core.Logging;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Rules
{
    [RequiresSTA]
    [Category("Rules")]
    public sealed class AccountUnderDebtCounselling : DebtCounsellingTests.TestBase<BasePageAssertions>
    {
        private TestBrowser browser;

        protected override void OnTestFixtureSetup()
        {
            Logger.LogWriter = new ConsoleLogWriter();
            Service<IWatiNService>().KillAllIEProcesses();
        }

        protected override void OnTestStart()
        {
            Settings.WaitForCompleteTimeOut = 120;
            Service<IWatiNService>().KillAllIEProcesses();
        }

        protected override void OnTestTearDown()
        {
            Service<IWatiNService>().KillAllIEProcesses();
            browser = null;
        }

        protected override void OnTestFixtureTearDown()
        {
            Service<IWatiNService>().KillAllIEProcesses();
        }

        /// <summary>
        /// A warning should be displayed when a user tries to access the Further Lending Calculator using an account that currently has an active debt counselling
        /// case.
        /// </summary>
        [Test, Description(@"A warning should be displayed when a user tries to access the Further Lending Calculator using an account that currently has an active debt counselling
        case.")]
        public void AccountUnderDebtCounsellingFurtherLending()
        {
            //get account with open dc
            int accountKey = Service<IDebtCounsellingService>().GetRandomDebtCounsellingAccount(DebtCounsellingStatusEnum.Open);
            browser = new TestBrowser(TestUsers.FLProcessor3);
            LoadAccountAndNavigateToAccountSummary(browser, accountKey);
            browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            browser.Navigate<LoanServicingCBO>().FurtherLendingCalculator();
            browser.Page<FurtherLendingPreCheck>().Next();
            browser.Page<FurtherLendingCalculator>().AssertNextButtonState(false);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("This Account is undergoing Debt Counselling.");
            //check each LE
            DebtCounsellingAssertions.AssertLegalEntitiesOnAccountUnderDebtCounsellingRule(browser, accountKey);
        }

        /// <summary>
        /// When loading the account summary for an account that is under debt counselling a warning should be displayed to the user. Each Legal Entity who is under
        /// debt counselling on the debt counselling case should be listed in the validation summary.
        /// </summary>
        [Test, Description(@"When loading the account summary for an account that is under debt counselling a warning should be displayed to the user. Each Legal Entity who
        is under debt counselling on the debt counselling case should be listed in the validation summary.")]
        public void AcccountUnderDebtCounsellingAccountSummary()
        {
            //get account with open dc
            int accountKey = Service<IDebtCounsellingService>().GetRandomDebtCounsellingAccount(DebtCounsellingStatusEnum.Open);
            browser = new TestBrowser(TestUsers.FLProcessor3);
            LoadAccountAndNavigateToAccountSummary(browser, accountKey);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("This Account is undergoing Debt Counselling.");
            //check each LE
            DebtCounsellingAssertions.AssertLegalEntitiesOnAccountUnderDebtCounsellingRule(browser, accountKey);
        }

        /// <summary>
        ///
        /// </summary>
        [Test, Description("")]
        public void RelatedLegalEntityUnderDebtCounsellingAccountSummary()
        {
            List<int> accountList = Service<ILegalEntityService>().GetTwoOpenMortgageLoanAccountsForSameLegalEntity();
            int accountKey = accountList[0];
            int relatedAccountKey = accountList[1];
            List<int> insertedKeys = new List<int>();
            //put the one under debt counselling
            try
            {
                //put the other one under DC
                insertedKeys = Service<IDebtCounsellingService>().AddAccountUnderDebtCounselling(accountKey);
                browser = new TestBrowser(TestUsers.HaloUser);
                LoadAccountAndNavigateToAccountSummary(browser, relatedAccountKey);
                //Assert warnings
                DebtCounsellingAssertions.AssertRelatedAccountDebtCounsellingRule(browser, accountKey, relatedAccountKey, null);
            }
            finally
            {
                Service<IDebtCounsellingService>().RemoveDebtCounsellingCase(insertedKeys[0], insertedKeys[1]);
            }
        }

        /// <summary>
        /// Loading the personal loan account summary where the account is under debt counselling should display a warning message.
        /// </summary>
        [Test, Description("Loading the personal loan account summary where the account is under debt counselling should display a warning message.")]
        public void AccountUnderDebtCounsellingPersonLoanSummary()
        {
            //fetch open personal loan account that is under debt counselling
            var account = Service<IAccountService>().GetPersonalLoanAccount();
            WorkflowHelper.CreateCase(account.AccountKey);
            //naivgate to the personal loan account summary
            browser = new TestBrowser(TestUsers.HaloUser, TestUsers.Password);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(browser);
            browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(account.AccountKey);
            browser.Navigate<LoanServicingCBO>().PersonalLoansNode(account.AccountKey);
            //assert the expected warning messages are displayed
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("This Account is undergoing Debt Counselling.");
            DebtCounsellingAssertions.AssertLegalEntitiesOnAccountUnderDebtCounsellingRule(browser, account.AccountKey);
        }

        #region Helper

        private void LoadAccountAndNavigateToAccountSummary(TestBrowser browser, int accountKey)
        {
            //navigate to the client search
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            //load the client into the CBO
            browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
        }

        #endregion Helper
    }
}