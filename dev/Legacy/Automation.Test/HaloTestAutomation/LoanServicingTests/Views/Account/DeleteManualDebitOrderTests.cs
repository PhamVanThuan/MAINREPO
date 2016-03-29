using BuildingBlocks;
using BuildingBlocks.Assertions;
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
    public class DeleteManualDebitOrderTests : TestBase<ManualDebitOrderDeleteView>
    {
        #region PrivateVariables

        private Automation.DataModels.Account Account;
        private int _legalEntityKey;
        private Automation.DataModels.ManualDebitOrder ManualDebitOrder = null;
        private int instanceID = 0;

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

        #endregion SetupTearDown

        #region Tests

        /// <summary>
        /// A user should be able to delete a manual debit order if they were the original user that inserted the manual debit order.
        /// </summary>
        [Test, Description("A user should be able to delete a manual debit order if they were the original user that inserted the manual debit order.")]
        public void _01_DeleteManualDebitOrder()
        {
            ManualDebitOrder = base.Service<IManualDebitOrderService>().InsertManualDebitOrder(Account, TestUsers.HaloUser).FirstOrDefault();
            base.Browser.Navigate<LoanServicingCBO>().ManualDebitOrders(NodeTypeEnum.Delete);
            int manualDebitOrderKey = ManualDebitOrder.manualDebitOrderKey;
            base.Browser.Page<ManualDebitOrderBaseView>().SelectManualDebitOrder(ManualDebitOrder.BankAccountKey);
            //delete it
            base.View.ClickDelete(false);
            //should now be set to inactive
            ManualDebitOrder = base.Service<IAccountService>().GetManualDebitOrders(Account.AccountKey)
                .Where(a => a.manualDebitOrderKey == manualDebitOrderKey).FirstOrDefault();
            Assert.That(ManualDebitOrder.GeneralStatusKey == (int)GeneralStatusEnum.Inactive);
            base.Service<IManualDebitOrderService>().DeleteManualDebitOrders(Account.AccountKey);
        }

        [Ignore] //functionality has been removed from HALO
        /// <summary>
        /// If a user tries to delete a manual debit order that they did not initially insert then a Delete Debit Order request should be created.
        /// </summary>
        [Test, Description("If a user tries to delete a manual debit order that they did not initially insert then a Delete Debit Order request should be created.")]
        public void _02_DeleteManualDebitOrderCreatesDebitOrderDeleteRequest()
        {
            ManualDebitOrder = base.Service<IManualDebitOrderService>().InsertManualDebitOrder(Account, TestUsers.HaloUser).FirstOrDefault();
            base.Browser.Navigate<LoanServicingCBO>().ManualDebitOrders(NodeTypeEnum.Delete);
            int manualDebitOrderKey = ManualDebitOrder.manualDebitOrderKey;
            base.Browser.Page<ManualDebitOrderBaseView>().SelectManualDebitOrder(manualDebitOrderKey);
            //delete it
            base.View.ClickDelete(true);
            instanceID = X2Assertions.AssertCurrentDeleteDebitOrderX2State(Account.AccountKey, "Process Delete Request");
        }

        #endregion Tests
    }
}