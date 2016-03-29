using System;
using System.Collections.Generic;
using System.Text;
using WatiN.Core;
using WatiN.Core.Logging;
using NUnit.Framework;
using BuildingBlocks;
using Navigation = BuildingBlocks.Navigation;
using CommonData.Constants;
using DebtCounsellingTests.View;

namespace DebtCounsellingTests.Views
{
    [TestFixture, RequiresSTA]
#if !DEBUG
	[TestLinkFixture(
		Url = "http://sahls31:8181/testlink/lib/api/xmlrpc.php",
		ProjectName = "HALO Automation",
		TestPlan = "Regression Tests",
		TestSuite = "Debt Counselling Workflow",
		UserId = "admin",
		DevKey = "896f902c0397d7c1849290a44d0f6fb5")]
#endif
    public sealed class ManageLegalEntity
    {
        #region PrivateVar
        private TestBrowser browser;
        #endregion PrivateVar
        
        /// <summary>
        /// Runs at the start of the test suite
        /// </summary>
        [TestFixtureSetUp]
        public void Start()
        {
            try
            {
                BuildingBlocks.Common.WatiNExtensions.CloseAllOpenIEBrowsers();

            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Runs on the startup of each test
        /// </summary>
        [SetUp]
        public void TestStartUp()
        {
            Settings.WaitForCompleteTimeOut = 60;
            BuildingBlocks.Common.WatiNExtensions.KillAllIEProcesses();
            if (browser != null)
            {
                Navigation.Helper.CloseLoanNodesFLOBO(browser);
            }
            browser = Navigation.Helper.StartBrowser(TestUsers.DebtCounsellingConsultant, TestUsers.Password);
        }
        /// <summary>
        /// Runs on the completion of each test
        /// </summary>
        [TearDown]
        public void TestCleanUp()
        {
            if (browser == null)
            {
                return;
            }
            else
            {
                Navigation.Helper.CheckForErrorMessages(browser);
                Navigation.Helper.CloseLoanNodesFLOBO(browser);
                browser.Dispose();
                browser = null;
            }
        }
        /// <summary>
        /// Runs on completion of the test fixture
        /// </summary>
        [TestFixtureTearDown]
        public void TestSuiteCleanUp()
        {
            WorkflowHelper.TestFixtureTearDownForDebtCounselling();
            BuildingBlocks.Common.WatiNExtensions.KillAllIEProcesses();
        }
        [Test]
        public void AddLegalEntity()
        {
            //search for a case at Manage Proposal                        
            string adusername = string.Empty;            
            int debtCounsellingKey = ViewsHelper.GetDebtCounsellingCase(WorkflowStates.DebtCounsellingWF.ManageProposal, out adusername);
            if (debtCounsellingKey == 0)
            {
                debtCounsellingKey = ViewsHelper.GetDebtCounsellingCase(WorkflowStates.DebtCounsellingWF.ReviewNotification, out adusername);
                if (debtCounsellingKey == 0)
                {
                      browser.Dispose();
                      debtCounsellingKey = ViewsHelper.CreateDebtCounsellingCase(WorkflowStates.DebtCounsellingWF.ManageProposal, out browser);
                }
            }
            else
                browser = Navigation.Helper.StartBrowser(TestUsers.DebtCounsellingConsultant, TestUsers.Password);
            
        }
    }
}
