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
    [TestFixture, RequiresSTA]
    public class AdministrationNodeTests : TestBase<BasePage>
    {
        private Automation.DataModels.Account _accountKey;

        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            //set variables
            _accountKey = base.Service<IAccountService>().GetVariableLoanAccountByMainApplicantCount(1, 1, AccountStatusEnum.Open);
            //login
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
        }

        #endregion SetupTearDown

        #region NodesClass

        private class AdminNodes
        {
            private readonly List<string> ParentNode = new List<string>();
            private readonly List<string> ParentURL = new List<string>();
            private readonly List<string> ChildNode = new List<string>();
            private readonly List<string> ChildURL = new List<string>();
            private readonly IContextMenuService contextMenuService;

            public AdminNodes()
            {
                contextMenuService = new ContextMenuService();
                var contextMenuItems = contextMenuService.GetContextMenuItemsByCBOKey(25);
                foreach (var item in contextMenuItems)
                {
                    ParentNode.Add(item.ParentNode);
                    ParentURL.Add(item.ParentURL);
                    ChildNode.Add(item.ChildNode);
                    ChildURL.Add(item.ChildURL);
                }
            }
        }

        #endregion NodesClass

        /// <summary>
        /// Ensures that the correct view is loaded when selecting the nodes of the loan account CBO node in Loan Servicing
        /// </summary>
        [Test, Sequential, Description("Ensures that the correct view is loaded when selecting the nodes of the legal entity CBO node in Loan Servicing.")]
        public void AdministrationNodeViewLoaded
            (
                [ValueSource(typeof(AdminNodes), "ParentNode")] string parentNode,
                [ValueSource(typeof(AdminNodes), "ParentURL")] string parentURL,
                [ValueSource(typeof(AdminNodes), "ChildNode")] string childNode,
                [ValueSource(typeof(AdminNodes), "ChildURL")] string childURL
            )
        {
            base.Browser.Navigate<LoanServicingCBO>().ClickNodes(parentNode);
            base.Browser.Page<BasePageAssertions>().AssertViewLoaded(parentURL);
            //only select the child node if it exists.
            if (!string.IsNullOrEmpty(childNode))
            {
                base.Browser.Navigate<LoanServicingCBO>().ClickNodes(childNode);
                base.Browser.Page<BasePageAssertions>().AssertViewLoaded(childURL);
            }
        }
    }
}