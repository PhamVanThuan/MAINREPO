using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace LoanServicingTests.Navigation
{
    [RequiresSTA]
    public class ClosedLoanAccountNodeTests : TestBase<BasePage>
    {
        private Automation.DataModels.Account Account;

        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            //set variables
            Account = base.Service<IAccountService>().GetVariableLoanAccountByMainApplicantCount(2, 1, AccountStatusEnum.Closed);
            //login
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            int legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(Account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(Account.AccountKey);
        }

        protected override void OnTestTearDown()
        {
            if (base.Browser != null)
            {
                if (base.Browser.Page<BasePage>().ErrorLabelExists())
                {
                    base.Browser.Page<BasePage>().PleaseStartAgain();
                    base.Browser.Refresh();
                    //over here you need to do whatever is required for your next test to start up
                    base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
                    base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
                    base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
                    int legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(Account.AccountKey);
                    base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(Account.AccountKey);
                }
            }
        }

        #endregion SetupTearDown

        #region NodeClass

        private class ClosedLoanAccountNode
        {
            private readonly List<string> ParentNode = new List<string>();
            private readonly List<string> ParentURL = new List<string>();
            private readonly List<string> ChildNode = new List<string>();
            private readonly List<string> ChildURL = new List<string>();
            private readonly IContextMenuService contextMenuService;

            public ClosedLoanAccountNode()
            {
                contextMenuService = new ContextMenuService();
                var contextMenuItems = contextMenuService.GetContextMenuItemsByCBOKey(11);
                foreach (var item in contextMenuItems)
                {
                    ParentNode.Add(item.ParentNode);
                    ParentURL.Add(item.ParentURL);
                    ChildNode.Add(item.ChildNode);
                    ChildURL.Add(item.ChildURL);
                }
            }
        }

        #endregion NodeClass

        /// <summary>
        /// Ensures that the correct view is loaded when selecting the nodes of the loan account CBO node in Loan Servicing
        /// </summary>
        [Test, Sequential, Description("Ensures that the correct view is loaded when selecting the nodes of the legal entity CBO node in Loan Servicing.")]
        public void ClosedLoanAccountNodeViewLoaded
            (
                [ValueSource(typeof(ClosedLoanAccountNode), "ParentNode")] string ParentNode,
                [ValueSource(typeof(ClosedLoanAccountNode), "ParentURL")] string ParentURL,
                [ValueSource(typeof(ClosedLoanAccountNode), "ChildNode")] string ChildNode,
                [ValueSource(typeof(ClosedLoanAccountNode), "ChildURL")] string ChildURL
            )
        {
            base.Browser.Navigate<LoanServicingCBO>().ClickNodes(ParentNode);
            base.Browser.Page<BasePageAssertions>().AssertViewLoaded(ParentURL);
            //only select the child node if it exists.
            if (!string.IsNullOrEmpty(ChildNode))
            {
                base.Browser.Navigate<LoanServicingCBO>().ClickNodes(ChildNode);
                base.Browser.Page<BasePageAssertions>().AssertViewLoaded(ChildURL);
            }
        }
    }
}