﻿using BuildingBlocks;
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
    public class ClosedParentAcountNodeTests : TestBase<BasePage>
    {
        private Automation.DataModels.Account account;

        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            //set variables
            account = base.Service<IAccountService>().GetVariableLoanAccountByMainApplicantCount(1, 1, AccountStatusEnum.Closed);
            //login
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            int legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().ParentAccountNode(account.AccountKey);
        }

        protected override void OnTestTearDown()
        {
            if (base.Browser != null)
            {
                if (base.View.ErrorLabelExists())
                {
                    base.View.PleaseStartAgain();
                    base.Browser.Refresh();
                    base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
                    base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
                    base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
                    int legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(account.AccountKey);
                    base.Browser.Navigate<LoanServicingCBO>().ParentAccountNode(account.AccountKey);
                }
            }
        }

        #endregion SetupTearDown

        #region NodeClass

        private class ClosedParentAccountNode
        {
            private readonly List<string> ParentNode = new List<string>();
            private readonly List<string> ParentURL = new List<string>();
            private readonly List<string> ChildNode = new List<string>();
            private readonly List<string> ChildURL = new List<string>();
            private readonly IContextMenuService contextMenuService;

            public ClosedParentAccountNode()
            {
                contextMenuService = new ContextMenuService();
                var contextMenuItems = contextMenuService.GetContextMenuItemsByCBOKey(14);
                foreach (var item in contextMenuItems)
                {
                    ParentNode.Add(item.ParentNode);
                    ParentURL.Add(item.ParentURL);
                    ChildNode.Add(item.ChildNode);
                    ChildURL.Add(item.ChildURL);
                }
            }
        }

        /// <summary>
        /// Ensures that the correct view is loaded when selecting the nodes of the parent account CBO node in Loan Servicing
        /// </summary>
        [Test, Sequential, Description("Ensures that the correct view is loaded when selecting the nodes of the legal entity CBO node in Loan Servicing.")]
        public void ClosedParentAccountNodeViewLoaded
            (
                [ValueSource(typeof(ClosedParentAccountNode), "ParentNode")] string parentNode,
                [ValueSource(typeof(ClosedParentAccountNode), "ParentURL")] string parentURL,
                [ValueSource(typeof(ClosedParentAccountNode), "ChildNode")] string childNode,
                [ValueSource(typeof(ClosedParentAccountNode), "ChildURL")] string childURL
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

        #endregion NodeClass
    }
}