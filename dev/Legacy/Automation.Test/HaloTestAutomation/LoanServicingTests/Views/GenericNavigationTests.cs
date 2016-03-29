using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BuildingBlocks;
using WatiN.Core.Logging;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;
using CommonData.Constants;
using CommonData.Enums;

namespace LoanServicingTests
{
    [TestFixture, RequiresSTA]
    public class GenericNavigationTests
    {
        private TestBrowser browser;
        private IEnumerable<SQLQuerying.Models.ContextMenu> _contextMenuItems;
        private int _accountKey;
        #region SetupTearDown
        /// <summary>
        /// Runs when the test suite starts.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSuiteStart()
        {
            Logger.LogWriter = new ConsoleLogWriter();
            BuildingBlocks.Common.WatiNExtensions.KillAllIEProcesses();

        }
        /// <summary>
        /// Runs at the start of each test.
        /// </summary>
        [SetUp]
        public void TestStart()
        {
            Settings.WaitForCompleteTimeOut = 120;
            BuildingBlocks.Common.WatiNExtensions.KillAllIEProcesses();
            if (browser != null)
            {
                BuildingBlocks.Navigation.Helper.CloseLoanNodesFLOBO(browser);
            }
        }
        /// <summary>
        /// Runs at the end of each test.
        /// </summary>
        [TearDown]
        public void TestEnd()
        {
            BuildingBlocks.Common.WatiNExtensions.KillAllIEProcesses();
            browser = null;
        }
        /// <summary>
        /// Runs when the test suite ends.
        /// </summary>
        [TestFixtureTearDown]
        public void TestSuiteEnd()
        {
            BuildingBlocks.Common.WatiNExtensions.KillAllIEProcesses();
        }

        #endregion
        /// <summary>
        /// Ensures that the correct view is loaded when selecting the nodes of the legal entity CBO node in Loan Servicing
        /// </summary>
        [Test, Description("Ensures that the correct view is loaded when selecting the nodes of the legal entity CBO node in Loan Servicing.")]
        public void CheckLegalEntityNodeViewLoaded()
        {
            StartTest(2, AccountStatus.Open);
            ClickNodesAndAssertViewLoaded();
        }
        /// <summary>
        /// Ensures that the correct view is loaded when selecting the nodes of the parent account CBO node in Loan Servicing
        /// </summary>
        [Test, Description("Ensures that the correct view is loaded when selecting the nodes of the legal entity CBO node in Loan Servicing.")]
        public void CheckParentAccountNodeViewLoaded()
        {
            StartTest(5, AccountStatus.Open);
            BuildingBlocks.Navigation.LoanServicingCBO.ParentAccountNode(browser, _accountKey.ToString());
            ClickNodesAndAssertViewLoaded();
        }
        /// <summary>
        /// Ensures that the correct view is loaded when selecting the nodes of the loan account CBO node in Loan Servicing
        /// </summary>
        [Test, Description("Ensures that the correct view is loaded when selecting the nodes of the legal entity CBO node in Loan Servicing.")]
        public void CheckLoanAccountNodeViewLoaded()
        {
            StartTest(6, AccountStatus.Open);    
            BuildingBlocks.Navigation.LoanServicingCBO.LoanAccountNode(browser, _accountKey.ToString());
            ClickNodesAndAssertViewLoaded();
        }
        /// <summary>
        /// Ensures that the correct view is loaded when selecting the nodes of the loan account CBO node in Loan Servicing
        /// </summary>
        [Test, Description("Ensures that the correct view is loaded when selecting the nodes of the legal entity CBO node in Loan Servicing.")]
        public void CheckVariableLoanNodeViewLoaded()
        {
            StartTest(7, AccountStatus.Open);
            BuildingBlocks.Navigation.LoanServicingCBO.VariableLoanNode(browser);
            ClickNodesAndAssertViewLoaded();
        }
        /// <summary>
        /// Ensures that the correct view is loaded when selecting the nodes of the loan account CBO node in Loan Servicing
        /// </summary>
        [Test, Description("Ensures that the correct view is loaded when selecting the nodes of the legal entity CBO node in Loan Servicing.")]
        public void CheckClosedVariableLoanNodeViewLoaded()
        {
            StartTest(8, AccountStatus.Closed);
            BuildingBlocks.Navigation.LoanServicingCBO.VariableLoanNode(browser);
            ClickNodesAndAssertViewLoaded();
        }
        #region Helpers
        /// <summary>
        /// Retrieves the relevant context menu items and logins into the browser.
        /// </summary>
        /// <param name="contextMenuKey"></param>
        private void StartTest(int contextMenuKey, AccountStatus status)
        {
            _contextMenuItems = BuildingBlocks.Common.ContextMenu.GetContextMenuByCBOKey(contextMenuKey);
            browser = BuildingBlocks.Navigation.Helper.StartBrowser(TestUsers.ClintonS, TestUsers.Password_ClintonS);
            BuildingBlocks.Navigation.Helper.Menu(browser);
            BuildingBlocks.Navigation.Helper.LegalEntityMenu(browser);
            BuildingBlocks.Navigation.Helper.CloseLoanNodesCBO(browser);
            _accountKey = BuildingBlocks.Common.Account.GetAccountKeyByMainApplicantCount(1, 1, status);
            int legalEntityKey = browser.Page<Views.ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(_accountKey);
        }
        /// <summary>
        /// Clicks on each of the nodes and asserts that the expected view is loaded.
        /// </summary>
        private void ClickNodesAndAssertViewLoaded()
        {
            foreach (var item in _contextMenuItems)
            {
                BuildingBlocks.Navigation.LoanServicingActions.ClickNodes(browser, item.ParentNode);
                browser.Page<Views.BasePageAssertions>().AssertViewLoaded(item.ParentURL);
                if (!string.IsNullOrEmpty(item.ChildNode))
                {
                    BuildingBlocks.Navigation.LoanServicingActions.ClickNodes(browser, item.ChildNode);
                    browser.Page<Views.BasePageAssertions>().AssertViewLoaded(item.ChildURL);
                }
            }
        }
        #endregion

    }
}
