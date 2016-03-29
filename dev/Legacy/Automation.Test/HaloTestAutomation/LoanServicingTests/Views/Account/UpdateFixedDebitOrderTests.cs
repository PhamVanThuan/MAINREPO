using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing.FixedDebitOrders;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public class UpdateFixedDebitOrderTests : TestBase<FixedDebitOrderUpdate>
    {
        private int _legalEntityKey;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            //remove any nodes from CBO
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            // navigate to ClientSuperSearch
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().Menu();
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
        }

        /// <summary>
        /// The effective date must be a business day when adding a future dated fixed debit order
        /// </summary>
        [Test, Description("The effective date must be a business day when adding a future dated fixed debit order")]
        public void EffectiveDateMustBeABusinessDay()
        {
            var account = GetFixedDebitOrderTestCase(false);
            base.Browser.Navigate<LoanServicingCBO>().FixedDebitOrders(NodeTypeEnum.Update);
            base.View.UpdateFixedDebitOrder(1525, false, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The Effective Date must be a business day");
        }

        /// <summary>
        /// The fixed debit order value has to be greater than the total amount due on the loan.
        /// </summary>
        [Test, Description("The fixed debit order value has to be greater than the total amount due on the loan.")]
        public void FixedDebitOrderAmountMustBeGreaterThanTotalAmountDue()
        {
            var account = GetFixedDebitOrderTestCase(false);
            base.Browser.Navigate<LoanServicingCBO>().FixedDebitOrders(NodeTypeEnum.Update);
            //add one that is less than the account instalment
            var value = Service<IAccountService>().GetTotalInstalment(account.AccountKey);
            value = value - 0.01;
            base.View.UpdateFixedDebitOrder(value, true, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("The fixed debit order amount must be at least the total amount due");
            value = value + 0.01;
            base.View.UpdateFixedDebitOrder(value, DateTime.Now);
            base.Browser.Page<BasePageAssertions>().AssertNoValidationMessageExists();
            //get the account details
            account = base.Service<IAccountService>().GetAccountByKey(account.AccountKey);
            Assert.AreEqual(value, account.FixedPayment);
        }

        /// <summary>
        /// When adding a fixed debit order, the effective date can be today's date
        /// </summary>
        [Test, Description("When adding a fixed debit order, the effective date can be today's date")]
        public void EffectiveDateCanBeTodaysDate()
        {
            var account = GetFixedDebitOrderTestCase(false);
            base.Browser.Navigate<LoanServicingCBO>().FixedDebitOrders(NodeTypeEnum.Update);
            //add today's date as the effective date
            var value = Service<IAccountService>().GetTotalInstalment(account.AccountKey);
            value += 0.01;
            base.View.UpdateFixedDebitOrder(value, DateTime.Now);
            base.Browser.Page<BasePageAssertions>().AssertNoValidationMessageExists();
            //get the account details
            account = base.Service<IAccountService>().GetAccountByKey(account.AccountKey);
            Assert.AreEqual(value, account.FixedPayment);
        }

        /// <summary>
        /// When adding a fixed debit order, the effective date cannot be blank
        /// </summary>
        [Test, Description("When adding a fixed debit order, the effective date cannot be blank")]
        public void EffectiveDateCannotBeBlank()
        {
            var account = GetFixedDebitOrderTestCase(false);
            base.Browser.Navigate<LoanServicingCBO>().FixedDebitOrders(NodeTypeEnum.Update);
            //add today's date as the effective date
            base.View.UpdateFixedDebitOrder(30000, null);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Effective Date must be greater than today's date");
        }

        /// <summary>
        /// This test will ensure that adding a future dated fixed debit order does not change the account's existing fixed payment.
        /// </summary>
        [Test, Description("This test will ensure that adding a future dated fixed debit order does not change the account's existing fixed payment.")]
        public void AddFutureDatedFixedDebitOrderDoesNotChangeExistingFixedPayment()
        {
            var account = GetFixedDebitOrderTestCase(true);
            base.Browser.Navigate<LoanServicingCBO>().FixedDebitOrders(NodeTypeEnum.Update);
            //change it
            var prevValue = account.FixedPayment;
            var newValue = 30000.00;
            base.View.UpdateFixedDebitOrder(newValue, true, true);
            //get the account details
            account = base.Service<IAccountService>().GetAccountByKey(account.AccountKey);
            Assert.AreEqual(prevValue, account.FixedPayment);
        }

        /// <summary>
        /// This test ensures that adding a future dated fixed debit order adds a future dated change.
        /// </summary>
        [Test, Description("This test ensures that adding a future dated fixed debit order adds a future dated change.")]
        public void AddFutureDatedFixedDebitOrderDoesNotChangeExistingFixedPaymentCreatesFutureDatedChange()
        {
            var account = GetFixedDebitOrderTestCase(true);
            base.Browser.Navigate<LoanServicingCBO>().FixedDebitOrders(NodeTypeEnum.Update);
            //change it
            var newValue = 30000.00;
            base.View.UpdateFixedDebitOrder(newValue, true, true);
            //get the future dated change
            var futureDatedChanges = Service<IFutureDatedChangeService>().GetFutureDatedChangeByIdentifierReference(account.AccountKey);
            var futureDatedChangeDetails = (from f in futureDatedChanges
                                            where f.FutureDatedChangeType == FutureDatedChangeTypeEnum.FixedDebitOrder
                                            select f)
                                                .SelectMany(x => x.FutureDatedChangeDetails)
                                                .Where(b => b.Value == newValue.ToString());
            Assert.That(futureDatedChangeDetails.Count() > 0);
        }

        /// <summary>
        /// This test ensures that when we add a fixed debit order for an account that has a subsidy, that the fixed debit order and the subsidy amount are
        /// greater than the total amount due on the account.
        /// </summary>
        [Test, Description(@"This test ensures that when we add a fixed debit order for an account that has a subsidy, that the fixed debit order and the
        subsidy amount are greater than the total amount due on the account.")]
        public void FixedDebitOrderWithSubsidyMustBeGreaterThanTotalAmountDue()
        {
            var account = GetFixedDebitOrderTestCase(true, true);
            base.Browser.Navigate<LoanServicingCBO>().FixedDebitOrders(NodeTypeEnum.Update);
            //update it to be less than the total instalment minus the stop order amounts
            var newValue = account.TotalInstalment - account.SubsidyAmount - 100;
            base.View.UpdateFixedDebitOrder(newValue, true, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("The fixed debit order amount must be at least the total amount due");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("for subsidy client");
        }

        #region HelperMethods

        /// <summary>
        /// Gets a test case for the fixed debit order tests.
        /// </summary>
        /// <param name="hasFixedPayment"></param>
        /// <returns></returns>
        private Automation.DataModels.Account GetFixedDebitOrderTestCase(bool hasFixedPayment, bool subsidyAccount = false)
        {
            Automation.DataModels.Account account = null;
            if (!subsidyAccount)
            {
                account = hasFixedPayment ? base.Service<IAccountService>().GetRandomAccountWithFixedPayment(ProductEnum.NewVariableLoan,
                    AccountStatusEnum.Open, false) : base.Service<IAccountService>().GetRandomAccountWithFixedPayment(ProductEnum.NewVariableLoan, AccountStatusEnum.Open, true);
            }
            else
            {
                account = hasFixedPayment ? base.Service<IAccountService>().GetOpenAccountWithSubsidyStopOrder(true) : base.Service<IAccountService>().GetOpenAccountWithSubsidyStopOrder(false);
            }
            _legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().ParentAccountNode(account.AccountKey);
            return account;
        }

        #endregion HelperMethods
    }
}