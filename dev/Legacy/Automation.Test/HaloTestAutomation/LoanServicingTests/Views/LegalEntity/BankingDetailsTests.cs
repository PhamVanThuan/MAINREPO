using BuildingBlocks;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;

namespace LoanServicingTests.Views.LegalEntity
{
    [RequiresSTA]
    public class BankingDetailsTests : TestBase<BankingDetails>
    {
        #region PrivateVariables

        private Automation.DataModels.Account Account;
        private int _legalEntityKey;

        #endregion PrivateVariables

        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            //// open browser with test user
            base.Browser = new TestBrowser(TestUsers.HaloUser, TestUsers.Password);
            //remove any nodes from CBO
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            // navigate to ClientSuperSearch
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().Menu();
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            Account = base.Service<IAccountService>().GetVariableLoanAccountByMainApplicantCount(1, 1, Common.Enums.AccountStatusEnum.Open);
            _legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(Account.AccountKey);
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            //remove any nodes from CBO
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            // navigate to ClientSuperSearch
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().Menu();
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
        }

        #endregion SetupTearDown

        #region View Tests

        /// <summary>
        /// Verify that it is possible to navigate to the BankingDetailsDisplay view.
        /// Assert that the number of LegalEntityBankAccount records equals the number of records displayed in the Bank Details grid
        /// </summary>
        [Test]
        public void ViewBankingDetails_Navigation()
        {
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.View);
            //Assertions
            List<string> accountNumbers = (from r in Service<ILegalEntityBankAccountService>().GetBankAccountsLinkedToALegalEntities(_legalEntityKey)
                                           select r.AccountNumber).ToList<string>();
            base.View.AssertBankingDetailsDisplayed(accountNumbers);
        }

        #endregion View Tests

        #region Add Tests

        /// <summary>
        /// Verify that it is possible to navigate to the BankDetailsAdd view.
        /// Assert the expected controls exist on the BankDetailsAdd view
        /// </summary>
        [Test]
        public void AddBankingDetails_Navigation()
        {
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            //Assertions
            base.View.AssertBankingDetailsAddControlsExist();
        }

        /// <summary>
        /// Check all mandatory fields on the AddBankingDetails view
        /// Assert that an appropriate error message is displayed if a mandatory field is not populated
        /// </summary>
        [Test]
        public void AddBankingDetails_MandatoryFields()
        {
            var bankAcc = Service<IBankingDetailsService>().GetNextUnusedBankAccountDetails();
            string prevAccountNumber = bankAcc.AccountNumber;
            bankAcc.AccountName = "Test";
            //Bank
            bankAcc.ACBBankDescription = null;
            bankAcc.ACBBranchCode = null;
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            base.View.AddBankingDetails(bankAcc, ButtonTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A bank and branch must be specified");
            //Branch
            bankAcc.ACBBankDescription = "ABSA";
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            base.View.AddBankingDetails(bankAcc, ButtonTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A bank and branch must be specified");
            //Account Type
            bankAcc.ACBTypeDescription = null;
            bankAcc.ACBBranchCode = "632005";
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            base.View.AddBankingDetails(bankAcc, ButtonTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Account type must be specified");
            //Account Number
            bankAcc.AccountNumber = null;
            bankAcc.ACBTypeDescription = Common.Constants.ACBType.Current;
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            base.View.AddBankingDetails(bankAcc, ButtonTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Account Number is a mandatory field");
            //Account Name
            bankAcc.AccountNumber = prevAccountNumber;
            bankAcc.AccountName = null;
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            base.View.AddBankingDetails(bankAcc, ButtonTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Account Name is a mandatory field");
        }

        /// <summary>
        /// Check mandatory fields when searching for a bank account on the AddBankingDetails view
        /// Assert that an appropriate error message is displayed if a mandatory field is not populated
        /// </summary>
        [Test]
        public void AddBankingDetails_Search_MandatoryFields()
        {
            var bankAccount = Service<IBankingDetailsService>().GetNextUnusedBankAccountDetails();
            string validationMessage = "Bank, Branch, and Account Number must be specified for a search";
            //Bank
            string bankAccountNumber = bankAccount.AccountNumber;
            bankAccount.ACBBankDescription = null;
            bankAccount.ACBBranchCode = null;
            bankAccount.ACBTypeDescription = null;
            bankAccount.AccountName = null;
            bankAccount.AccountNumber = null;
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            base.View.AddBankingDetails(bankAccount, ButtonTypeEnum.Search);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(validationMessage);
            //Branch
            bankAccount.ACBBankDescription = "ABSA";
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            base.View.AddBankingDetails(bankAccount, ButtonTypeEnum.Search);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(validationMessage);
            //Account Number
            bankAccount.ACBBranchCode = "632005";
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            base.View.AddBankingDetails(bankAccount, ButtonTypeEnum.Search);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(validationMessage);
        }

        /// <summary>
        /// Check that the correct options are available for selection from the Account Type dropdowm lists
        /// </summary>
        [Test]
        public void AddBankingDetails_CheckSelectLists()
        {
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            base.View.AssertAccountTypeOptions();
        }

        #endregion Add Tests

        #region Update Tests

        /// <summary>
        /// Verify that it is possible to navigate to the BankDetailsUpdate view.
        /// Assert the expected controls exist on the BankDetailsUpdate view
        /// </summary>
        [Test]
        public void UpdateBankingDetails_Navigation()
        {
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Update);

            //Assertions
            base.View.AssertUpdateFieldsDisabled();
        }

        /// <summary>
        /// Check all mandatory fields on the UpdateBankingDetails view
        /// Assert that an appropriate error message is displayed if a mandatory field is not populated
        /// </summary>
        [Test]
        public void UpdateBankingDetails_MandatoryFields()
        {
            var bankAccount = Service<IBankingDetailsService>().GetNextUnusedBankAccountDetails();

            //Account Type
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Update);
            base.View.UpdateBankingDetails(null, 0, null);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Account type must be specified");
            //Account Name
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Update);
            base.View.UpdateBankingDetails("", ACBType.Current, null);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Account Name is a mandatory field");
        }

        /// <summary>
        /// Check that correct options are available for selection from the Account Type and Status dropdowm lists
        /// </summary>
        [Test]
        public void UpdateBankingDetails_CheckSelectLists()
        {
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Update);
            base.View.AssertAccountTypeOptions();
            base.View.AssertStatusOptions();
        }

        #endregion Update Tests

        #region Delete Tests

        /// <summary>
        /// Verify that it is possible to navigate to the BankDetailsDelete view.
        /// Assert the expected controls exist on the BankDetailsDelete view
        /// </summary>
        [Test]
        public void DeleteBankingDetails_Navigation()
        {
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Delete);

            //Assertions
            base.View.AssertBankingDetailsDeleteControlsExist();
        }

        #endregion Delete Tests

        #region Search Tests

        /// <summary>
        /// Search for an Account Number that is not on record
        /// Assert that the BankDetailssearchLegalEntity view is displayed with no results
        /// </summary>
        [Test]
        public void BankingDetailsSearchLegalEntity_UnusedBankAccount()
        {
            var bankAccount = Service<IBankingDetailsService>().GetNextUnusedBankAccountDetails();
            bankAccount.ACBTypeDescription = null;
            bankAccount.AccountName = null;
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            base.View.AddBankingDetails(bankAccount, ButtonTypeEnum.Search);
            //Assertion
            base.Browser.Page<BankingDetailsSearchLegalEntity>().AssertTextMessageExists("There are no other related Clients.");
        }

        /// <summary>
        /// Verify that it is possible to search for a bank account that exists on record for other legal entities
        /// Assert that all Legal Entities that are linked to the bank account number are displayed in the search results on the BankingDetailsSearchLegalEntity view
        /// </summary>
        [Test]
        public void BankingDetailsSearchLegalEntity_ExistingBankAccount()
        {
            var legalEntityBankAccount = base.Service<IBankingDetailsService>().GetRandomLegalEntityBankAccountByBankAccountKey();
            var bankAccount = base.Service<IBankingDetailsService>().GetBankAccountByBankAccountKey(legalEntityBankAccount.BankAccountKey);
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            bankAccount.ACBTypeDescription = null;
            bankAccount.AccountName = null;
            base.View.AddBankingDetails(bankAccount, ButtonTypeEnum.Search);

            //Assertions
            List<string> legalEntities = (from r in Service<ILegalEntityBankAccountService>().GetLegalEntitiesLinkedToABankAccount(bankAccount.BankAccountKey)
                                          select r.IdNumber).ToList<string>();
            base.Browser.Page<BankingDetailsSearchLegalEntity>().AssertLegalEntityDisplayed(legalEntities);
        }

        /// <summary>
        /// Verify that it is possible to search for an existing bank account and link it to the legal entity
        /// Assert that the bank account is linked to the legal entity and displayed in the Bank Details grid on the ViewBankingDetails view
        /// </summary>
        [Test]
        public void BankingDetailsSearchLegalEntity_UseLegalEntityBankAccount()
        {
            var legalEntityBankAccount = base.Service<IBankingDetailsService>().GetRandomLegalEntityBankAccountByBankAccountKey();
            var bankAccount = base.Service<IBankingDetailsService>().GetBankAccountByBankAccountKey(legalEntityBankAccount.BankAccountKey);
            bankAccount.ACBTypeDescription = null;
            bankAccount.AccountName = null;
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            base.View.AddBankingDetails(bankAccount, ButtonTypeEnum.Search);

            var legalEntity = (from r in Service<ILegalEntityBankAccountService>().GetLegalEntitiesLinkedToABankAccount(bankAccount.BankAccountKey)
                               select r).FirstOrDefault<Automation.DataModels.LegalEntity>();

            base.Browser.Page<BankingDetailsSearchLegalEntity>().UseLegalEntityBankAccount(legalEntity.IdNumber, ButtonTypeEnum.Use);

            //Assertions
            List<string> accountNumber = new List<string> { bankAccount.AccountNumber };
            base.View.AssertBankingDetailsDisplayed(accountNumber);
        }

        #endregion Search Tests
    }
}