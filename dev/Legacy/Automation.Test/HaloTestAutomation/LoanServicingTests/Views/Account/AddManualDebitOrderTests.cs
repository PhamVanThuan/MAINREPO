using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing.ManualDebitOrders;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public class AddManualDebitOrderTests : TestBase<ManualDebitOrderAddView>
    {
        #region PrivateVariables

        private Automation.DataModels.Account Account;
        private int _legalEntityKey;
        private Random random = new Random();

        #endregion PrivateVariables

        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            Account = base.Service<IAccountService>().GetVariableLoanAccountByMainApplicantCount(1, 1, AccountStatusEnum.Open);
            _legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(Account.AccountKey);
            //we always need a bank account so insert one.
            base.Service<ILegalEntityService>().InsertLegalEntityBankAccount(_legalEntityKey);
            base.Browser.Navigate<LoanServicingCBO>().VariableLoanNode(Account.AccountKey);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Service<IManualDebitOrderService>().DeleteManualDebitOrders(Account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().ManualDebitOrders(NodeTypeEnum.Add);
        }

        #endregion SetupTearDown

        #region Validation

        [Test]
        public void ValidateMandatoryFields()
        {
            base.View.ClearReference();
            base.View.ClickAdd(true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                "Effective Date must be greater than today, or if today's date it must be captured before 14h00",
                "Bank Account is a mandatory field for Manual Debit Order Payments.",
                "Amount must be greater than 0.",
                "Reference is a mandatory field");
        }

        [Test]
        public void ValidateReferenceField()
        {
            base.View.AssertReferenceEqualsAccountKey(Account.AccountKey);
        }

        [Test]
        public void ValidateBankAccountSelectList()
        {
            var bankAccounts = base.Service<IAccountService>().GetBankAccountRecordsForAccount(Account.AccountKey);
            var expectedList = base.Service<IBankingDetailsService>().GetBankAccountStringList(bankAccounts);
            base.View.AssertBankAccountListContents(expectedList);
        }

        #endregion Validation

        #region AddManualDebitOrder

        /// <summary>
        /// This test ensures that a manual debit order can be saved using the Add Manual Debit Order screen
        /// </summary>
        [Test, Description("This test ensures that a manual debit order can be saved using the Add Manual Debit Order screen")]
        public void AddManualDebitOrder()
        {
            var manualDebitOrder = base.Browser.Page<ManualDebitOrderBaseView>().PopulateManualDebitOrderDetails(Account);
            base.View.ClickAdd(false);
            var manualDebitOrders = base.Service<IAccountService>().GetManualDebitOrders(Account.AccountKey);
            var savedTransaction = (from t in manualDebitOrders
                                    where t.Amount == manualDebitOrder.Amount
                                        && t.GeneralStatusKey == 1
                                        && t.ActionDate.Date == manualDebitOrder.ActionDate.Date
                                        && t.Notes == manualDebitOrder.Notes
                                        && t.Reference == manualDebitOrder.Reference
                                    select t).FirstOrDefault();
            Assert.That(savedTransaction != null);
        }

        /// <summary>
        /// A manual debit order cannot be saved without a reference
        /// </summary>
        [Test, Description("A manual debit order cannot be saved without a reference")]
        public void AddWithBlankReferenceGeneratesWarning()
        {
            var manualDebitOrder = new Automation.DataModels.ManualDebitOrder
            {
                Amount = random.Next(4000, 6000),
                Reference = String.Empty,
                ActionDate = DateTime.Now.AddDays(1),
                Notes = string.Format(@"Add Manual Debit Order Test-{0}", random.Next(0, 1000000)),
                TransactionType = TransactionTypeEnum.ManualDebitOrderPayment,
                GeneralStatusKey = 1
            };
            manualDebitOrder = base.Browser.Page<ManualDebitOrderBaseView>().PopulateManualDebitOrderDetails(Account, manualDebitOrder);
            base.View.ClickAdd(true);
            base.Browser.Page<BasePageAssertions>()
                .AssertValidationMessageExists("Reference is a mandatory field");
        }

        /// <summary>
        /// A bank account is required in order for a manual debit order to be saved.
        /// </summary>
        [Test, Description("A bank account is required in order for a manual debit order to be saved.")]
        public void AddBankAccountMandatory()
        {
            var manualDebitOrder = new Automation.DataModels.ManualDebitOrder
            {
                Amount = random.Next(4000, 6000),
                Reference = Account.AccountKey.ToString(),
                ActionDate = DateTime.Now.AddDays(1),
                Notes = string.Format(@"Add Manual Debit Order Test-{0}", random.Next(0, 1000000)),
                TransactionType = TransactionTypeEnum.ManualDebitOrderPayment,
                GeneralStatusKey = 1
            };
            manualDebitOrder = base.Browser.Page<ManualDebitOrderBaseView>().PopulateManualDebitOrderDetails(Account, manualDebitOrder);
            base.Browser.Page<ManualDebitOrderBaseView>().SelectNoBankAccount();
            base.View.ClickAdd(false);
            base.Browser.Page<BasePageAssertions>().
                AssertValidationMessageExists("Bank Account is a mandatory field for Manual Debit Order Payments.");
        }

        /// <summary>
        /// An effective date is required in order for a manual debit order to be saved
        /// </summary>
        [Test, Description("An effective date is required in order for a manual debit order to be saved")]
        public void AddEffectiveDateRequired()
        {
            var manualDebitOrder = new Automation.DataModels.ManualDebitOrder
            {
                Amount = random.Next(4000, 6000),
                Reference = Account.AccountKey.ToString(),
                ActionDate = DateTime.Now.AddDays(-1),
                Notes = string.Format(@"Add Manual Debit Order Test-{0}", random.Next(0, 1000000)),
                TransactionType = TransactionTypeEnum.ManualDebitOrderPayment,
                GeneralStatusKey = 1
            };
            manualDebitOrder = base.Browser.Page<ManualDebitOrderBaseView>().PopulateManualDebitOrderDetails(Account, manualDebitOrder);
            base.View.ClickAdd(false);
            base.Browser.Page<BasePageAssertions>().
                AssertValidationMessageExists("Effective Date must be greater than today, or if today's date it must be captured before 14h00");
        }

        /// <summary>
        /// A manual debit order cannot be added with an amount of zero
        /// </summary>
        [Test, Description("A manual debit order cannot be added with an amount of zero")]
        public void AddAmountNotGreaterThanZero()
        {
            var manualDebitOrder = new Automation.DataModels.ManualDebitOrder
            {
                Amount = 0,
                Reference = Account.AccountKey.ToString(),
                ActionDate = DateTime.Now.AddDays(-1),
                Notes = string.Format(@"Add Manual Debit Order Test-{0}", random.Next(0, 1000000)),
                TransactionType = TransactionTypeEnum.ManualDebitOrderPayment,
                GeneralStatusKey = 1
            };
            manualDebitOrder = base.Browser.Page<ManualDebitOrderBaseView>().PopulateManualDebitOrderDetails(Account, manualDebitOrder);
            base.View.ClickAdd(false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Amount must be greater than 0.");
        }

        /// <summary>
        /// Ensures that the arrear balance is correctly displayed on the screen
        /// </summary>
        [Test, Description("Ensures that the arrear balance is correctly displayed on the screen")]
        public void AddDisplaysCorrectArrearBalance()
        {
            var arrearTransactionBalance = Service<ILoanTransactionService>().GetLatestArrearBalanceAmount(Account.AccountKey);
            base.View.AssertArrearBalance(arrearTransactionBalance);
        }

        /// <summary>
        /// A manual debit order can be added with a reference other than the account number, but the user is required to confirm this first.
        /// </summary>
        [Test, Description("A manual debit order can be added with a reference other than the account number, but the user is required to confirm this first.")]
        public void AddWithReferenceNotEqualToAccountNumberShouldSaveManualDebitOrder()
        {
            var manualDebitOrder = new Automation.DataModels.ManualDebitOrder
            {
                Amount = 5000,
                Reference = "NotTheAccountNumber",
                ActionDate = DateTime.Now.AddDays(+1),
                Notes = string.Format(@"Add Manual Debit Order Test-{0}", random.Next(0, 1000000)),
                TransactionType = TransactionTypeEnum.ManualDebitOrderPayment,
                GeneralStatusKey = 1
            };
            manualDebitOrder = base.Browser.Page<ManualDebitOrderBaseView>().PopulateManualDebitOrderDetails(Account, manualDebitOrder);
            base.View.ClickAdd(true);
            var manualDebitOrders = base.Service<IAccountService>().GetManualDebitOrders(Account.AccountKey);
            var savedTransaction = (from t in manualDebitOrders
                                    where t.Amount == manualDebitOrder.Amount
                                        && t.GeneralStatusKey == 1
                                        && t.ActionDate.Date == manualDebitOrder.ActionDate.Date
                                        && t.Notes == manualDebitOrder.Notes
                                        && t.Reference == manualDebitOrder.Reference
                                    select t).FirstOrDefault();
            Assert.That(savedTransaction != null);
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void AddWithNoNotesSavesManualDebitOrder()
        {
            var manualDebitOrder = new Automation.DataModels.ManualDebitOrder
            {
                Amount = 5000,
                Reference = Account.AccountKey.ToString(),
                ActionDate = DateTime.Now.AddDays(+1),
                Notes = string.Empty,
                TransactionType = TransactionTypeEnum.ManualDebitOrderPayment,
                GeneralStatusKey = 1
            };
            manualDebitOrder = base.Browser.Page<ManualDebitOrderBaseView>().PopulateManualDebitOrderDetails(Account, manualDebitOrder);
            base.View.ClickAdd(false);
            var manualDebitOrders = base.Service<IAccountService>().GetManualDebitOrders(Account.AccountKey);
            var savedTransaction = (from t in manualDebitOrders
                                    where t.Amount == manualDebitOrder.Amount
                                        && t.GeneralStatusKey == 1
                                        && t.ActionDate.Date == manualDebitOrder.ActionDate.Date
                                        && t.Reference == manualDebitOrder.Reference
                                    select t).FirstOrDefault();
            Assert.That(savedTransaction != null);
        }

        #endregion AddManualDebitOrder
    }
}