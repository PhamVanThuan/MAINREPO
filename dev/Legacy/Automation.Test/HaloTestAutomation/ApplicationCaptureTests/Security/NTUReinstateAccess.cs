using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using Common.Constants;
using NUnit.Framework;
using Navigation = BuildingBlocks.Navigation;

namespace ApplicationCaptureTests.Security
{
    [TestFixture, RequiresSTA]
    public sealed class NTUReinstateAccess
    {
        #region PrivateVar

        private TestBrowser browser;
        private int offerKey;

        #endregion PrivateVar

        /// <summary>
        /// Find and NTU an offer to be used in testing.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSuiteStart()
        {
            //Login as registration user to NTU a case
            this.browser = new TestBrowser(TestUsers.RegistrationsLOAAdmin, TestUsers.Password);

            //Search for an application in pipeline and NTU it
            this.browser.Navigate<Navigation.NavigationHelper>().Task();
            this.offerKey = browser.Page<X2Worklist>().SelectFirstOfferFromWorklist(this.browser, WorkflowStates.ApplicationManagementWF.SignedLOAReview);
            browser.ClickAction(WorkflowActivities.ApplicationManagement.NTU);
            browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.ApplicationNTU);

            this.browser.Dispose();
            this.browser = null;
        }

        #region Tests

        /// <summary>
        /// Test that a Branch Consultant can Reinstate an NTU'd offer before LOA
        /// </summary>
        [Test]
        public void _01_BranchConsultantReinstateAccessBeforePipeline()
        {
            //Login as a branch consultant to check security
            this.browser = new TestBrowser(TestUsers.BranchConsultant);
            //Assert
            this.AssertReinstateCanBePerformed(false);
            //Dispose
            this.browser.Dispose();
            this.browser = null;
        }

        /// <summary>
        /// Test that a Branch Admin can Reinstate an NTU'd offer before LOA
        /// </summary>
        [Test]
        public void _02_BranchAdminReinstateAccessBeforePipeline()
        {
            //Login as a branch consultant to check security
            this.browser = new TestBrowser(TestUsers.BranchAdmin);
            //Assert
            this.AssertReinstateCanBePerformed(true);
            //Dispose
            this.browser.Dispose();
            this.browser = null;
        }

        /// <summary>
        /// Test that a Branch Manager can do a reinstate in pipeline
        /// </summary>
        [Test]
        public void _03_ManagerReinstateAccessInPipeline()
        {
            //Accept LOA and NTU offer
            this.browser = new TestBrowser(TestUsers.RegistrationsLOAAdmin);
            this.browser.Navigate<Navigation.NavigationHelper>().Task();
            this.browser.Navigate<Navigation.WorkFlowsNode>().WorkFlows(this.browser);
            browser.Page<WorkflowSuperSearch>().Search(this.offerKey);
            browser.ClickAction(WorkflowActivities.ApplicationManagement.LOAAccepted);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            browser.ClickAction(WorkflowActivities.ApplicationManagement.NTU);
            browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.ApplicationNTU);
            this.browser.Dispose();
            this.browser = null;

            //Login as a branch admin to check security
            this.browser = new TestBrowser(TestUsers.BranchManager, TestUsers.Password);
            //Assert
            this.AssertReinstateCanBePerformed(false);

            this.browser.Dispose();
            this.browser = null;
        }

        /// <summary>
        /// Test that a branch consultant can't reinstate in pipeline
        /// </summary>
        [Test]
        public void _04_BranchConsultantReinstateAccessInPipeline()
        {
            //Login as a branch consultant to check security
            this.browser = new TestBrowser(TestUsers.BranchConsultant);

            //Search for NTUd offer
            this.browser.Navigate<Navigation.NavigationHelper>().Task();
            this.browser.Navigate<Navigation.WorkFlowsNode>().WorkFlows(this.browser);
            browser.Page<WorkflowSuperSearch>().Search(this.offerKey);

            this.AssertCannotReinstateMessageExist();

            //Dispose
            this.browser.Dispose();
            this.browser = null;
        }

        /// <summary>
        /// Test that a branch admin can't reinstate in pipeline
        /// </summary>
        [Test]
        public void _05_BranchAdminReinstateAccessInPipeline()
        {
            //Login as a branch admin to check security
            this.browser = new TestBrowser(TestUsers.BranchAdmin);

            //Search for NTUd offer
            this.browser.Navigate<Navigation.NavigationHelper>().Task();
            this.browser.Navigate<Navigation.WorkFlowsNode>().WorkFlows(this.browser);
            browser.Page<WorkflowSuperSearch>().Search(this.offerKey);

            this.AssertCannotReinstateMessageExist();

            //Dispose
            this.browser.Dispose();
            this.browser = null;
        }

        #endregion Tests

        #region Helpers

        private void AssertCannotReinstateMessageExist()
        {
            browser.ClickAction(WorkflowActivities.ApplicationManagement.ReinstateNTU);
            browser.Page<BasePageAssertions>().AssertFrameContainsText("You cannot Reinstate this NTU (Previous State: Application Check) - please refer to your Manager.");
        }

        private void AssertReinstateCanBePerformed(bool performAction)
        {
            //Search for NTUd offer
            this.browser.Navigate<Navigation.NavigationHelper>().Task();
            this.browser.Navigate<Navigation.WorkFlowsNode>().WorkFlows(this.browser);

            browser.Page<WorkflowSuperSearch>().Search(this.offerKey);
            //Assert that the Reinstate can be done.
            browser.ClickAction(WorkflowActivities.ApplicationManagement.ReinstateNTU);
            browser.Page<WorkflowYesNo>().Confirm(performAction, false);
        }

        #endregion Helpers
    }
}