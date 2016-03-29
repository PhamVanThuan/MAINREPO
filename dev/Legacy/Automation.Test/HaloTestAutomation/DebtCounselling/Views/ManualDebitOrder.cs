using Automation.DataModels;
using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing.ManualDebitOrders;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DebtCounsellingTests.Views
{
    [RequiresSTA]
    public sealed class ManualDebitOrderTests : DebtCounsellingTests.TestBase<ManualDebitOrderAddRecurringView>
    {
        private Account Account;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            //search for a case at Pend Proposal
            base.StartTest(WorkflowStates.DebtCounsellingWF.ReviewNotification, TestUsers.DebtCounsellingConsultant);
            Account = base.Service<IAccountService>().GetAccountByKey(base.TestCase.AccountKey);
            //Navigate the the add manual debit order screen.
            base.Browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            base.Browser.Navigate<LoanDetailsNode>().ClickManualDebitOrdersNode();
        }

        #region Tests

        /// <summary>
        /// A debt counselling user should be able to add multiple occurences of a manual debit order when the case is under debt counselling.
        /// </summary>
        [Test, Description("A debt counselling user should be able to add multiple occurences of a manual debit order when the case is under debt counselling.")]
        public void AddManualDebitOrderWithMultipleOccurencesForDebtCounselling()
        {
            int noOfRecurrences = 5;
            var manualDebitOrder = AddRecurringManualDebitOrder(noOfRecurrences);
            //fetch them
            var manualDebitOrders = base.Service<IAccountService>().GetManualDebitOrders(Account.AccountKey);
            var count = (from mdo in manualDebitOrders
                         where mdo.BankAccountKey == manualDebitOrder.BankAccountKey
                            && mdo.Notes == manualDebitOrder.Notes
                            && mdo.Amount == manualDebitOrder.Amount
                         select mdo).Count();
            Assert.That(count == noOfRecurrences);
        }

        /// <summary>
        /// Once added to the grid, the user should be able to select a single occurence and update its details.
        /// </summary>
        [Test, Description("Once added to the grid, the user should be able to select a single occurence and update its details.")]
        public void UpdateManualDebitOrderWithMultipleOccurencesForDebtCounselling()
        {
            int noOfRecurrences = 5;
            var mdo = AddRecurringManualDebitOrder(noOfRecurrences);
            IEnumerable<ManualDebitOrder> manualDebitOrders = base.Service<IAccountService>().GetManualDebitOrders(Account.AccountKey);
            var manualDebitOrderKey = (from m in manualDebitOrders
                                       where m.Notes == mdo.Notes
                                       select m.manualDebitOrderKey).FirstOrDefault();
            base.Browser.Navigate<LoanDetailsNode>().ClickUpdateManualDebitOrderNode();
            //select the one from the grid
            base.Browser.Page<ManualDebitOrderBaseView>().SelectManualDebitOrder(manualDebitOrderKey);
            //update it
            int newDebitOrderAmount = 9999;
            string reference = "newReference";
            base.Browser.Page<ManualDebitOrderUpdateView>().UpdateManualDebitOrderAmount(newDebitOrderAmount, reference, Account.AccountKey.ToString(), false);
            //get them again
            manualDebitOrders = base.Service<IAccountService>().GetManualDebitOrders(Account.AccountKey);
            //find our new one
            var debitOrders = manualDebitOrders as IList<ManualDebitOrder> ?? manualDebitOrders.ToList();
            var updated = (from m in debitOrders where m.Notes == reference && m.Amount == newDebitOrderAmount select m).FirstOrDefault();
            Assert.IsNotNull(updated);
            //old one inactive
            var inactive = (from m in debitOrders
                            where m.GeneralStatusKey == (int)GeneralStatusEnum.Inactive
                                && m.manualDebitOrderKey == manualDebitOrderKey
                            select m).FirstOrDefault();
            Assert.IsNotNull(inactive);
        }

        /// <summary>
        /// Deleting a manual debit order that has multiple occurences
        /// </summary>
        [Test]
        public void DeleteManualDebitOrderWithMultipleOccurencesForDebtCounselling()
        {
            int noOfRecurrences = 5;
            var mdo = AddRecurringManualDebitOrder(noOfRecurrences);
            IEnumerable<ManualDebitOrder> manualDebitOrders = base.Service<IAccountService>().GetManualDebitOrders(Account.AccountKey);
            var manualDebitOrderKey = (from m in manualDebitOrders
                                       where m.Notes == mdo.Notes
                                       select m.manualDebitOrderKey).FirstOrDefault();
            base.Browser.Navigate<LoanDetailsNode>().ClickDeleteManualDebitOrderNode();
            //select the one from the grid
            base.Browser.Page<ManualDebitOrderBaseView>().SelectManualDebitOrder(manualDebitOrderKey);
            base.Browser.Page<ManualDebitOrderDeleteView>().ClickDelete(false);
            manualDebitOrders = base.Service<IAccountService>().GetManualDebitOrders(Account.AccountKey);
            var inactive = (from m in manualDebitOrders
                            where m.GeneralStatusKey == (int)GeneralStatusEnum.Inactive
                                && m.manualDebitOrderKey == manualDebitOrderKey
                            select m).FirstOrDefault();
            Assert.That(inactive != null);
        }

        /// <summary>
        /// There should be a rule running when another user accesses the delete manual debit order for an account under debt counselling.
        /// </summary>
        [Test, Description("There should be a rule running when another user accesses the delete manual debit order for an account under debt counselling.")]
        public void AccountUnderDebtCounsellingRuleTest()
        {
            int noOfRecurrences = 5;
            var mdo = AddRecurringManualDebitOrder(noOfRecurrences);
            IEnumerable<ManualDebitOrder> manualDebitOrders = base.Service<IAccountService>().GetManualDebitOrders(Account.AccountKey);
            var manualDebitOrderKey = (from m in manualDebitOrders select m.manualDebitOrderKey).FirstOrDefault();
            //logoff
            //login as HaloUser
            var haloUserbrowser = new TestBrowser(TestUsers.HaloUser);
            //goto the delete screen
            haloUserbrowser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(haloUserbrowser);
            haloUserbrowser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            haloUserbrowser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(Account.AccountKey);
            haloUserbrowser.Navigate<LoanServicingCBO>().VariableLoanNode(Account.AccountKey);
            haloUserbrowser.Navigate<LoanDetailsNode>().ClickManualDebitOrdersNode();
            haloUserbrowser.Navigate<LoanDetailsNode>().ClickDeleteManualDebitOrderNode();
            haloUserbrowser.Page<ManualDebitOrderBaseView>().SelectManualDebitOrder(manualDebitOrderKey);
            haloUserbrowser.Page<ManualDebitOrderDeleteView>().ClickDelete(false);
            haloUserbrowser.Page<BasePageAssertions>().AssertValidationMessageExists(
                @"Account is under debt counselling. This manual debit order was created for Debt Counselling, please contact SAHL\DCCUser to update this manual debit order."
                );
            haloUserbrowser.Dispose();
        }

        /// <summary>
        /// Using the screen provided in debt counselling you cannot add a manual debit order with entering a value for the number of payments
        /// </summary>
        [Test]
        public void AddManualDebitOrderWithoutNoOfPaymentsThrowsValidationMessage()
        {
            //Navigate the the add manual debit order screen.
            base.Browser.Navigate<LoanDetailsNode>().ClickAddManualDebitOrderNode();
            //add a new manual debit order
            var manualDebitOrder = base.Browser.Page<ManualDebitOrderBaseView>().PopulateManualDebitOrderDetails(Account);
            base.View.ClickAdd();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please enter the number of payments.");
        }

        #endregion Tests

        #region HelperMethods

        /// <summary>
        /// Performs the steps to add a recurring manual debit order
        /// </summary>
        /// <param name="noOfRecurrences"></param>
        /// <returns></returns>
        private ManualDebitOrder AddRecurringManualDebitOrder(int noOfRecurrences)
        {
            //remove any existing
            base.Service<IManualDebitOrderService>().DeleteManualDebitOrders(Account.AccountKey);
            //goto add
            base.Browser.Navigate<LoanDetailsNode>().ClickAddManualDebitOrderNode();
            //add a new manual debit order
            var manualDebitOrder = base.Browser.Page<ManualDebitOrderBaseView>().PopulateManualDebitOrderDetails(Account);
            //add recurring payments
            base.View.AddNumberOfPayments(noOfRecurrences);
            base.View.ClickAdd();
            return manualDebitOrder;
        }

        #endregion HelperMethods
    }
}