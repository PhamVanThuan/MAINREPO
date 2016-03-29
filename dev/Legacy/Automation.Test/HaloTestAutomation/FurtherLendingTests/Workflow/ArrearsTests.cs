using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination.FurtherLending;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Threading;

namespace FurtherLendingTests.Workflow
{
    [RequiresSTA]
    public class ArrearsTests : TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.FLProcessor3);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            if (base.Browser != null)
            {
                base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            }
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
        }

        /// <summary>
        /// A FL Processor has the option to send a FL application to a Collections Admin user for a comment on the Arrears status
        /// of the Mortgage Loan by performing the Require Arrear Comment action. After the completion of this action the test ensures
        /// that the following has occurred:
        /// <list type="bullet">
        /// <item>
        /// <description>The case is moved to the Arrears state</description>
        /// </item>
        /// <item>
        /// <description>The case is assigned in the Worklist to FL Collections Admin Static Role</description>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description("A FL Processor can send a case to the Collections Admin user for an Arrears Comment"), Category("Readvances")]
        public void _01_RequireArrearComment()
        {
            //search for the case
            int offerKey = Helper.GetApplicationForFurtherLendingTest(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //perform the action
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.RequireArrearComment);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            //Assert the case movement
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.Arrears);
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.Arrears, Workflows.ApplicationManagement, "FL Collections Admin");
        }

        /// <summary>
        /// Once the case has been sent to the Arrears state the Collections Admin user can move the case back to the Manage Application
        /// state by using the Note Comment action and adding a Memo record to the Application. Once completed the test ensures that
        /// the following has occurred:
        /// <list type="bullet">
        /// <item>
        /// <description>The case is moved to the Manage Application state</description>
        /// </item>
        /// <item>
        /// <description>The case is assigned to the FL Processor who sent the case to the Arrears state</description>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description(@"A Collect Admin User can perform the Note Comment action, add a Memo record and send the case back
			to the Manage Application state"), Category("Readvances")]
        public void _02_NoteComment()
        {
            var collectadminbrowser = new TestBrowser(TestUsers.CollectAdminUser);
            int offerKey = Helper.GetApplicationForFurtherLendingTest(WorkflowStates.ApplicationManagementWF.Arrears, Workflows.ApplicationManagement);
            collectadminbrowser.Page<WorkflowSuperSearch>().Search(collectadminbrowser, offerKey, WorkflowStates.ApplicationManagementWF.Arrears);
            //perform the action
            string adUserName = collectadminbrowser.Page<ApplicationSummaryFurtherLending>().GetApplicationProcessor();
            collectadminbrowser.ClickAction(WorkflowActivities.ApplicationManagement.NoteComment);
            collectadminbrowser.Page<GenericMemoAdd>().AddMemoRecord(MemoStatus.UnResolved, "Arrear Comment");
            //assert the movement
            Thread.Sleep(3000);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //assert the WFA
            AssignmentAssertions.AssertWorkflowAssignment(adUserName, offerKey, OfferRoleTypeEnum.FLProcessorD);
            collectadminbrowser.Dispose();
        }
    }
}