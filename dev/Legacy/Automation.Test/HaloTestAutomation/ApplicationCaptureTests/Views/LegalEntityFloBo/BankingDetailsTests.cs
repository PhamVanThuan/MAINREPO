using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace ApplicationCaptureTests.Views.LegalEntityFloBo
{
    [RequiresSTA]
    public class BankingDetailsTests : TestBase<BankingDetails>
    {
        #region globalVariables

        /// <summary>
        /// OfferKey for tests
        /// </summary>
        private int _offerKey;

        private int _legalEntityKey;
        /// <summary>
        /// Database Connection to use
        /// </summary>

        private Automation.DataModels.BankAccount _cdvFailureBankAcc = new Automation.DataModels.BankAccount
        {
            ACBBankDescription = "Nedbank",
            ACBBranchCode = "133826",
            ACBTypeDescription = "Current",
            AccountNumber = "2338050053",
            AccountName = "Test"
        };

        private Automation.DataModels.BankAccount bankAccount;
        private int _bankAccountKey;

        #endregion globalVariables

        #region Setup/Teardown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            //create an application
            base.Browser = Helper.CreateApplicationWithBrowser(TestUsers.BranchConsultant10, out _offerKey);
            //remove our invalid CDV account
            base.Service<IBankingDetailsService>().RemoveBankAccount(_cdvFailureBankAcc.AccountNumber);
            bankAccount = Service<IBankingDetailsService>().GetNextUnusedBankAccountDetails();
            bankAccount.AccountName = "Test";
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationCaptureWF.ApplicationCapture, _offerKey);
            _legalEntityKey = base.Service<IApplicationService>().GetFirstApplicantLegalEntityKeyOnOffer(_offerKey);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<LegalEntityNode>().LegalEntity(_offerKey);
        }

        #endregion Setup/Teardown

        #region Tests

        /// <summary>
        /// This test ensures that banking details can be added to a legal entity via the FloBo
        /// </summary>
        [Test, Description("This test ensures that banking details can be added to a legal entity via the FloBo")]
        public void _001_AddBankingDetails()
        {
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            base.View.AddBankingDetails(bankAccount, ButtonTypeEnum.Add);
            _bankAccountKey = BankingDetailsAssertions.AssertBankAccountExists(bankAccount);
            BankingDetailsAssertions.AssertLegalEntityBankAccountByGeneralStatus(_legalEntityKey, _bankAccountKey, GeneralStatusEnum.Active);
        }

        /// <summary>
        /// This test provides invalid banking details that should fail the CDV validation. A warning should be displayed to the user indicating that the
        /// CDV check has failed.
        /// </summary>
        [Test, Description(@"This test provides invalid banking details that should fail the CDV validation. A warning should be displayed to the user indicating that the@
        CDV check has failed.")]
        public void _002_AddBankingDetailsCDVFailed()
        {
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            base.View.AddBankingDetails(_cdvFailureBankAcc, ButtonTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The CDV check for the account number failed. ");
        }

        /// <summary>
        /// This test ensures that the Bank, Branch Code and Account Number fields are not available for the user to update when they navigate to the
        /// Update Banking Details node.
        /// </summary>
        [Test, Description(@"This test ensures that the Bank, Branch Code and Account Number fields are not available for the user to update when they navigate to the
        Update Banking Details node.")]
        public void _003_UpdateBankingDetailsFieldsDisabled()
        {
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Update);
            base.View.AssertUpdateFieldsDisabled();
        }

        /// <summary>
        /// This test ensures that a user can update the banking details on a legal entity
        /// </summary>
        [Test, Description("This test ensures that a user can update the banking details on a legal entity")]
        public void _004_UpdateBankingDetails()
        {
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Update);
            string _accountName = "New Account Name";
            base.View.UpdateBankingDetails(_accountName, null, null);
            _bankAccountKey = BankingDetailsAssertions.AssertBankAccountExists(bankAccount);
            BankingDetailsAssertions.AssertLegalEntityBankAccountByGeneralStatus(_legalEntityKey, _bankAccountKey, GeneralStatusEnum.Active);
        }

        /// <summary>
        /// This test will set up the bank account captured as the debit order bank account. When the user tries to delete it a warning should be
        /// received indicating that the bank account being used as a direct debit cannot be deleted.
        /// </summary>
        [Test, Description(@"This test will set up the bank account captured as the debit order bank account. When the user tries to delete it a warning should be
        received indicating that the bank account being used as a direct debit cannot be deleted.")]
        public void _005_CannotDeleteBankAccountDebitOrder()
        {
            base.Browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            base.Browser.Navigate<LoanDetailsNode>().ClickDebitOrderDetailsNode();
            base.Browser.Navigate<LoanDetailsNode>().ClickUpdateDebitOrderDetailsNode();
            base.Browser.Page<DebitOrderDetailsAppUpdate>().UpdateDebitOrderDetails("Debit Order Payment", 1, 1, ButtonTypeEnum.Update);
            //go to the legal entity node
            base.Browser.Navigate<LegalEntityNode>().LegalEntity(_offerKey);
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Delete);
            base.View.DeleteBankAccount();
            //check that the warning message exists
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cannot deactivate a bank account that is being used as a direct debit.");
            //remove the bank account as a debit order bank account
            base.Service<IDebitOrdersService>().DeleteOfferDebitOrderByBankAccountKey(_bankAccountKey);
        }

        /// <summary>
        /// This test ensures that a legal entity bank account record can be deleted.
        /// </summary>
        [Test, Description("This test ensures that a legal entity bank account record can be deleted.")]
        public void _006_DeleteBankingDetails()
        {
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Delete);
            base.View.DeleteBankAccount();
            BankingDetailsAssertions.AssertLegalEntityBankAccountByGeneralStatus(_legalEntityKey, _bankAccountKey, GeneralStatusEnum.Inactive);
        }

        /// <summary>
        /// This test will update an inactive bank account on a legal entity to be active.
        /// </summary>
        [Test, Description("This test will update an inactive bank account on a legal entity to be active.")]
        public void _007_UpdateBankDetailsToActive()
        {
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Update);
            base.View.UpdateBankingDetails(null, null, GeneralStatusConst.Active);
            BankingDetailsAssertions.AssertLegalEntityBankAccountByGeneralStatus(_legalEntityKey, _bankAccountKey, GeneralStatusEnum.Active);
        }

        #endregion Tests
    }
}