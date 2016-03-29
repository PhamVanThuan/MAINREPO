using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing.ManualDebitOrders;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Linq;

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public class UpdateManualDebitOrderTests : TestBase<ManualDebitOrderUpdateView>
    {
        #region PrivateVariables

        private Automation.DataModels.Account Account;
        private int _legalEntityKey;
        private Automation.DataModels.ManualDebitOrder ManualDebitOrder = null;

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
            ManualDebitOrder = base.Service<IManualDebitOrderService>().InsertManualDebitOrder(Account, TestUsers.HaloUser).FirstOrDefault();
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<LoanServicingCBO>().ManualDebitOrders(NodeTypeEnum.Update);
        }

        protected override void OnTestFixtureTearDown()
        {
            base.Service<IManualDebitOrderService>().DeleteManualDebitOrders(Account.AccountKey);
            base.OnTestFixtureTearDown();
        }

        #endregion SetupTearDown

        #region Tests

        /// <summary>
        /// Ensures that a manual debit order can be updated.
        /// </summary>
        [Test, Description("Ensures that a manual debit order can be updated.")]
        public void UpdateManualDebitOrder()
        {
            int manualDebitOrderKey = ManualDebitOrder.manualDebitOrderKey;
            base.Browser.Page<ManualDebitOrderBaseView>().SelectManualDebitOrder(manualDebitOrderKey);
            int newAmt = 9999;
            string newNotes = "UpdatedNotes";
            base.View.UpdateManualDebitOrderAmount(newAmt, newNotes, Account.AccountKey.ToString(), false);
            //fetch the d/o again
            ManualDebitOrder = base.Service<IAccountService>().GetManualDebitOrders(Account.AccountKey)
                .Where(a => a.manualDebitOrderKey != manualDebitOrderKey).FirstOrDefault();
            Assert.That(ManualDebitOrder.Amount == newAmt && ManualDebitOrder.Notes == newNotes);
        }

        /// <summary>
        /// Ensures that the validation is running on the update manual debit order screen
        /// </summary>
        [Test, Description("Ensures that the validation is running on the update manual debit order screen")]
        public void UpdateWithZeroValueDisplaysWarning()
        {
            int manualDebitOrderKey = ManualDebitOrder.manualDebitOrderKey;
            base.Browser.Page<ManualDebitOrderBaseView>().SelectManualDebitOrder(manualDebitOrderKey);
            int newAmt = 0;
            string newNotes = "UpdatedNotes";
            base.View.UpdateManualDebitOrderAmount(newAmt, newNotes, Account.AccountKey.ToString(), false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Amount must be greater than 0.");
        }

        #endregion Tests
    }
}