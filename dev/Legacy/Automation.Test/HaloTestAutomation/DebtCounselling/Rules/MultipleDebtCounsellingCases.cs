using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using WatiN.Core;

namespace DebtCounsellingTests.Rules
{
    /// <summary>
    /// This testsuite will tested that a rule fires when there is more than one active debtcounselling case
    /// for the same account or when more than one DC Group exists for Legal Entities on the case.
    /// </summary>
    [TestFixture, RequiresSTA]
    public sealed class MultipleDebtCounsellingCases : DebtCounsellingTests.TestBase<ClientSuperSearch>
    {
        #region PrivateVar

        private TestBrowser browser;

        #endregion PrivateVar

        #region FixtureSetup

        protected override void OnTestFixtureSetup()
        {
        }

        protected override void OnTestFixtureTearDown()
        {
        }

        protected override void OnTestStart()
        {
        }

        protected override void OnTestTearDown()
        {
        }

        #endregion FixtureSetup

        #region Tests

        /// <summary>
        /// Test that the MultipleDebtCounsellingCases rules fires
        /// </summary>
        [Test]
        public void MultipleDebtCounsellingCasesRule()
        {
            int legalentityKey = 0;
            // Step01:
            // Get a legalentity that plays a role on three open accounts.
            string legalentityIdNumber = Service<IDebtCounsellingService>().GetLegalEntityIDNumberForDCCreate(false, 3, false);

            // Step02:
            // Get a list of all the accounts that the legalentity in Step01 play a role in.
            List<int> accountList = Service<IAccountService>().GetMortgageAccountsByLegalEntity(ref legalentityKey, ref legalentityIdNumber);

            // Step03: Get a legalentity that does not play a role in any of the above accounts.
            string legalentityIdNotOnAccount = Service<ILegalEntityService>().GetLegalEntityIDNumberNotLinkedToAccount(accountList.ToArray());

            // Step04:
            // Take the first account from the list of accounts and capture the legalentity from Step03 as a suretor on the account.
            int suretorAccountKey = accountList[0];
            this.AddSuretorToAccount(legalentityIdNotOnAccount, suretorAccountKey);

            // Step05:
            // Create a debt counselling case for the account with the suretor on on it!
            this.browser = new TestBrowser(TestUsers.DebtCounsellingAdmin, TestUsers.Password);
            WorkflowHelper.CreateCase(suretorAccountKey, legalentityIdNotOnAccount, true, this.browser);

            // Step06:
            // Create DebtCounselling cases for each of the accounts.
            foreach (int accountkey in accountList)
                WorkflowHelper.CreateCase(accountkey, legalentityIdNumber, true, this.browser);
            int accountKeyToAssert = suretorAccountKey;

            // Step07:
            // Navigate, search and load cases and then assert that both messages display on the summary screen.
            WorkflowHelper.NavigateSearchAndLoadDebtCounsellingCase(this.browser, accountKeyToAssert, "Review Notification");
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("More than one DC Group exists for Legal Entities on this case.");
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("More than one active Case exists for this Account. Please investigate and resolve.");
            this.browser.Dispose();
            this.browser = null;
        }

        #endregion Tests

        #region Helpers

        /// <summary>
        /// Search and load account using the ClientSuperSearch screen then add the provided legalentity as a suretor
        /// </summary>
        /// <param name="legalEntityIdNumber"></param>
        /// <param name="accountKey"></param>
        private void AddSuretorToAccount(string legalEntityIdNumber, int accountKey)
        {
            this.browser = new TestBrowser(TestUsers.FLManager, TestUsers.Password);
            this.browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(this.browser);
            this.browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            this.browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(this.browser);

            QueryResultsRow legalentityRow = Service<ILegalEntityService>().GetFirstLegalEntityOnAccount(accountKey);

            //Search and load account
            this.browser.Page<ClientSuperSearch>().PopulateSearch("", "", "", "", accountKey.ToString());
            this.browser.Page<ClientSuperSearch>().PerformSearch();

            if (legalentityRow.Column("legalentitytypekey").GetValueAs<int>() == (int)LegalEntityTypeEnum.Company
                || legalentityRow.Column("legalentitytypekey").GetValueAs<int>() == (int)LegalEntityTypeEnum.CloseCorporation
                || legalentityRow.Column("legalentitytypekey").GetValueAs<int>() == (int)LegalEntityTypeEnum.Trust)
                this.browser.Page<ClientSuperSearch>().SelectByLegalName(legalentityRow.Column("RegistrationNumber").Value);
            if (legalentityRow.Column("legalentitytypekey").GetValueAs<int>() == (int)LegalEntityTypeEnum.NaturalPerson)
                this.browser.Page<ClientSuperSearch>().SelectByIDNumber(legalentityRow.Column("idnumber").Value);

            //navigate to loan account.
            browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            browser.Navigate<LoanServicingCBO>().AddSuretor();

            //Search and add suretor
            this.browser.Page<ClientSuperSearch>().PopulateSearch("", "", legalEntityIdNumber, "", "");
            this.browser.Page<ClientSuperSearch>().PerformSearch();
            this.browser.Page<ClientSuperSearch>().SelectByIDNumber(legalEntityIdNumber);
            this.browser.Page<LegalEntityDetailsSuretorAddExisting>().AddSuretor();
            this.browser.Dispose();
            this.browser = null;
        }

        #endregion Helpers
    }
}