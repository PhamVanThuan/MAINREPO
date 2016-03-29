using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing.FixedDebitOrders;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public class DeleteFixedDebitOrderTests : TestBase<FixedDebitOrderDelete>
    {
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
        /// When there is no future dated change to be delete then the view does not display a delete button
        /// </summary>
        [Test, Description("When there is no future dated change to be delete then the view does not display a delete button")]
        public void DeleteFixedDebitOrderWithNoFixedDebitOrder()
        {
            //get a test account
            var account = base.Service<IAccountService>().GetVariableLoanAccountByMainApplicantCount(1, 1, AccountStatusEnum.Open);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().ParentAccountNode(account.AccountKey);
            //remove any existing future dated changes
            Service<IFutureDatedChangeService>().DeleteFutureDatedChangeByIdentifierReference(account.AccountKey);
            //navigate
            base.Browser.Navigate<LoanServicingCBO>().FixedDebitOrders(NodeTypeEnum.Delete);
            base.View.AssertDeleteButtonExists(false);
        }

        /// <summary>
        /// This test will find an account that does not have a fixed payment and then add a futrue date fixed debit order. After adding it, the test
        /// will navigate to the Delete Fixed Debit Order node to test that the future dated change can be deleted.
        /// </summary>
        [Test, Description(@"This test will find an account that does not have a fixed payment and then add a futrue date fixed debit order.
        After adding it, the test will navigate to the Delete Fixed Debit Order node to test that the future dated change can be deleted.")]
        public void DeleteFixedDebitOrderRemovesFutureDatedChange()
        {
            var account = base.Service<IAccountService>().GetRandomAccountWithFixedPayment(ProductEnum.NewVariableLoan, AccountStatusEnum.Open, true);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().ParentAccountNode(account.AccountKey);
            //navigate
            base.Browser.Navigate<LoanServicingCBO>().FixedDebitOrders(NodeTypeEnum.Update);
            var newValue = 19999.00;
            base.Browser.Page<FixedDebitOrderUpdate>().UpdateFixedDebitOrder(newValue, true, true);
            //get the future dated change
            var futureDatedChanges = Service<IFutureDatedChangeService>().GetFutureDatedChangeByIdentifierReference(account.AccountKey);
            var datedChanges = futureDatedChanges as IList<Automation.DataModels.FutureDatedChange> ?? futureDatedChanges.ToList();
            var futureDatedChangeDetails = (from f in datedChanges
                                            where f.FutureDatedChangeType == FutureDatedChangeTypeEnum.FixedDebitOrder
                                            select f)
                                                .SelectMany(x => x.FutureDatedChangeDetails)
                                                .Where(b => b.Value == newValue.ToString());
            //check that we have a future dated change
            Assert.That(futureDatedChangeDetails.Any(), "No future dated change exists");
            //to delete it we need the key
            var fdcKey = (from f in datedChanges where f.FutureDatedChangeType == FutureDatedChangeTypeEnum.FixedDebitOrder select f.FutureDatedChangeKey).FirstOrDefault();
            base.Browser.Navigate<LoanServicingCBO>().FixedDebitOrders(NodeTypeEnum.Delete);
            base.View.DeleteFixedDebitOrder(fdcKey);
            //should no longer exist
            futureDatedChanges = Service<IFutureDatedChangeService>().GetFutureDatedChangeByIdentifierReference(account.AccountKey);
            var nullResult = (from f in futureDatedChanges where f.FutureDatedChangeType == FutureDatedChangeTypeEnum.FixedDebitOrder select f).FirstOrDefault();
            Assert.That(nullResult == null);
        }
    }
}