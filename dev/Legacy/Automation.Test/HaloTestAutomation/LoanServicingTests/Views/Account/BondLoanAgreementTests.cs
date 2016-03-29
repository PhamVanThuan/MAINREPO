using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public class BondLoanAgreementTests : TestBase<BondLoanAgreement>
    {
        private int _legalEntityKey;
        private Automation.DataModels.Account Account;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            Account = base.Service<IAccountService>().GetVariableLoanAccountByMainApplicantCount(1, 1, Common.Enums.AccountStatusEnum.Open);
            _legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(Account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().VariableLoanNode(Account.AccountKey);
        }

        /// <summary>
        /// Asserts that the Update Bond presenter is loaded correctly.
        /// </summary>
        [Test, Description("Asserts that the Update Bond presenter is loaded correctly.")]
        public void UpdateBondPresenterLoaded()
        {
            base.Browser.Navigate<LoanServicingCBO>().UpdateBond();
            var bond = base.Service<IBondLoanAgreementService>().GetAccountBonds(Account.AccountKey).FirstOrDefault();
            base.View.SelectBond(bond.BondRegistrationNumber);
            base.View.AssertDeedsOffices();
            base.View.AssertAttorneysBySelectedDeedsOffice();
            base.View.AssertUpdatePresenterState();
            base.View.AssertFieldValues(bond);
        }

        /// <summary>
        /// Tests that a bond record can be updated.
        /// </summary>
        [Test, Description("Tests that a bond record can be updated.")]
        public void UpdateBondRecord()
        {
            base.Browser.Navigate<LoanServicingCBO>().UpdateBond();
            //get the bond record
            var bond = base.Service<IBondLoanAgreementService>().GetAccountBonds(Account.AccountKey)
                .FirstOrDefault();
            base.View.SelectBond(bond.BondRegistrationNumber);
            //we need the bondKey
            int key = bond.BondKey;
            var random = new Random();
            string newRegNumber = string.Format("TestBond/{0}", random.Next(0, 150000));
            double newAmount = bond.BondRegistrationAmount + 1;
            //change the values
            bond.BondRegistrationNumber = newRegNumber;
            bond.BondRegistrationAmount = newAmount;
            base.View.UpdateBond(bond);
            var updatedBond = (from bonds in base.Service<IBondLoanAgreementService>().GetAccountBonds(Account.AccountKey)
                               where bonds.BondKey == key
                               select bonds).FirstOrDefault();
            Assert.That(bond.BondRegistrationNumber == newRegNumber && bond.BondRegistrationAmount == newAmount, "Bond record was not updated correctly");
        }

        /// <summary>
        /// Asserts that the mandatory field validation is running on the Update Bond screen.
        /// </summary>
        [Test, Description("Asserts that the mandatory field validation is running on the Update Bond screen.")]
        public void UpdateBondFieldValidation()
        {
            var bond = base.Service<IBondLoanAgreementService>().GetAccountBonds(Account.AccountKey).FirstOrDefault();
            base.Browser.Navigate<LoanServicingCBO>().UpdateBond();
            base.View.SelectBond(bond.BondRegistrationNumber);
            bond.BondRegistrationAmount = 0.00;
            bond.BondRegistrationNumber = string.Empty;
            base.View.Populate(bond);
            base.View.ClickSubmit();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Bond Registration Amount must be between R 0.01 and R 999,999,999.99.",
                "The Bond Registration Number must be entered.");
        }

        /// <summary>
        /// This test will ensure that changing the deeds office correctly reloads the attorneys dropdown to those that are linked to the selected deeds office.
        /// </summary>
        [Test, Description(@"This test will ensure that changing the deeds office correctly reloads the attorneys dropdown to those that are linked to the
        selected deeds office.")]
        public void UpdateBondDeedsOfficeReloadsAttorneyList()
        {
            base.Browser.Navigate<LoanServicingCBO>().UpdateBond();
            base.View.AssertAttorneysBySelectedDeedsOffice();
            base.View.ChangeDeedsOffice();
        }

        /// <summary>
        /// Tests that the add loan agreement presenter has the correct fields enabled and existing when loaded.
        /// </summary>
        [Test, Description("Tests that the add loan agreement presenter has the correct fields enabled and existing when loaded.")]
        public void AddLoanAgreementViewLoaded()
        {
            base.Browser.Navigate<LoanServicingCBO>().AddLoanAgreement();
            base.View.AssertAddPresenterState();
        }

        /// <summary>
        /// Test ensures that the mandatory data rules fire correctly when trying to add a loan agreement
        /// </summary>
        [Test, Description("Test ensures that the mandatory data rules fire correctly when trying to add a loan agreement")]
        public void AddLoanAgreementMandatoryFields()
        {
            var bonds = base.Service<IBondLoanAgreementService>().GetAccountBonds(Account.AccountKey);
            var bondRegAmount = (from b in bonds where b.BondKey == bonds.Max(w => w.BondKey) select b)
                    .First()
                    .BondRegistrationAmount;
            base.Browser.Navigate<LoanServicingCBO>().AddLoanAgreement();
            base.View.ClearAddLoanAgreementFields();
            base.View.ClickSubmit();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please enter a valid Loan Agreement date.");
            base.View.ClickSubmit();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                string.Format(@"Loan Agreement Amount must be between R 0.01 and R {0}.00.", bondRegAmount.ToString()));
        }

        /// <summary>
        /// You are not allowed to add a loan agreement that does not have a value that is less than the account's current balance.
        /// </summary>
        [Test, Description(@"You are not allowed to add a loan agreement that does not have a value that is less than the account's current balance.")]
        public void AddLoanAgreementLessThanCurrentBalance()
        {
            EnsureTestCaseHasOnlyOneBond();
            //go to add loan agreement
            base.Browser.Navigate<LoanServicingCBO>().AddLoanAgreement();
            //add a loan agreement less than the value of the current balance, we need the account
            var account = base.Service<IAccountService>().GetAccountByKey(Account.AccountKey);
            decimal accountBalance = Math.Round(account.CurrentBalance, 2);
            base.View.AddLoanAgreement((accountBalance - 5000).ToString());
            //This fails if the loan agreement amount ends with a zer (e.g xxxx.40) as it only shows xxxx.4 but the assert message shows the 0.
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                    string.Format(@"The Total Loan Agreement cannot be less than the loan current balance - R {0}.", accountBalance.ToString())
                );
        }

        /// <summary>
        /// You should not be allowed to add a loan agreement that exceeds the registered bond amount.
        /// </summary>
        [Test, Description("You should not be allowed to add a loan agreement that exceeds the registered bond amount.")]
        public void AddLoanAgreementGreaterThanBond()
        {
            base.Browser.Navigate<LoanServicingCBO>().AddLoanAgreement();
            var bonds = base.Service<IBondLoanAgreementService>().GetAccountBonds(Account.AccountKey);
            var bondRegAmount = (from b in bonds where b.BondKey == bonds.Max(w => w.BondKey) select b)
                    .First()
                    .BondRegistrationAmount;
            base.View.AddLoanAgreement((bondRegAmount + 25000).ToString());
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The Loan Agreement Amount cannot be greater than the Bond Amount.");
        }

        /// <summary>
        /// Ensures our test case only has a single bond record.
        /// </summary>
        private void EnsureTestCaseHasOnlyOneBond()
        {
            IEnumerable<Automation.DataModels.Bond> bonds = null;
            bool searched = false;
            //we need someone with a single bond
            while (bonds == null)
            {
                bonds = base.Service<IBondLoanAgreementService>().GetAccountBonds(Account.AccountKey);
                if (bonds.Count() > 1)
                {
                    bonds = null;
                    Account = base.Service<IAccountService>().GetVariableLoanAccountByMainApplicantCount(1, 1, Common.Enums.AccountStatusEnum.Open);
                    searched = true;
                }
            }
            if (searched)
            {
                base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
                base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
                _legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(Account.AccountKey);
                base.Browser.Navigate<LoanServicingCBO>().VariableLoanNode(Account.AccountKey);
            }
        }
    }
}