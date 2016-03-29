using BuildingBlocks;
using BuildingBlocks.Navigation;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing.ManualDebitOrders;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

using System;
using System.Linq;

namespace PersonalLoansTests.Views
{
    [RequiresSTA]
    public class ManualDebitOrderTests : PersonalLoansWorkflowTestBase<ManualDebitOrderAddView>
    {
        private Automation.DataModels.Account Account;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.PersonalLoanClientServiceUser, TestUsers.Password);

            base.Browser.Navigate<NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();

            Account = base.Service<IAccountService>().GetPersonalLoanAccount();
            //we always need a bank account so insert one.
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Service<IManualDebitOrderService>().DeleteManualDebitOrders(Account.AccountKey);
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            base.Browser.Dispose();
        }

        [Test]
        public void AddManualDebitOrder()
        {
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(Account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().PersonalLoansNode(Account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().ManualDebitOrders(NodeTypeEnum.Add);

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
    }
}