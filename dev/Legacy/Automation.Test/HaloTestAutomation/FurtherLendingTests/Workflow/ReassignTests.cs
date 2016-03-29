using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Presenters.Origination.FurtherLending;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Linq;

namespace FurtherLendingTests.Workflow
{
    [RequiresSTA]
    public class ReassignTests : TestBase<BasePage>
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
        /// This test reassigns a Readvance application on an account that has other open FL applications. Once the readvance application has been
        /// reassigned the other open applications against the account should also be reassigned to the same user. A new offer role record, as well
        /// as Workflow Assignment records should be inserted when reassigning the application.
        /// </summary>
        [Test, Description("Reassigning a FL case with associated applications on the same account should reassign all the associated applications"), Category("All")]
        public void _01_ReassignMultipleFurtherLendingApplications()
        {
            var results = Service<IFurtherLendingService>().ReassignMultipleApps();
            //login
            int offerKey = (from r in results
                            where r.Column("name").Value == WorkflowStates.ApplicationManagementWF.ManageApplication
                            select r.Column("offerkey").GetValueAs<int>()).FirstOrDefault();
            base.FLSupervisorBrowser = new TestBrowser(TestUsers.FLSupervisor);
            base.FLSupervisorBrowser.Page<WorkflowSuperSearch>().Search(base.FLSupervisorBrowser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            string flAppProcUser = base.FLSupervisorBrowser.Page<ApplicationSummaryFurtherLending>().GetApplicationProcessor();
            string user = Service<IADUserService>().GetADUserPlayingOfferRole(OfferRoleTypeEnum.FLProcessorD, flAppProcUser);
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ApplicationManagement.ReassignUser);
            base.FLSupervisorBrowser.Page<WF_ReAssign>().SelectUserRoleAndConsultantFromDropdownAndCommit(flAppProcUser, user);
            //we need to run the workflow assignment assertions for each of the related offers
            foreach (QueryResultsRow row in results.RowList)
            {
                offerKey = row.Column("offerkey").GetValueAs<int>();
                //the readvance case should have complete workflow assignment records
                if (row.Column("name").Value == WorkflowStates.ApplicationManagementWF.ManageApplication)
                {
                    AssignmentAssertions.AssertWorkflowAssignment(user, offerKey, OfferRoleTypeEnum.FLProcessorD);
                }
                //we need to check for offer roles
                AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.FLProcessorD);
                //check that the records are assigned to our user
                AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.FLProcessorD, user);
            }
            results.Dispose();
        }

        /// <summary>
        /// A FL Supervisor should be able to Reassign a workflow case to another FL Processor. This test case is a simple test
        /// and does not ensure that any other related FL applications are also reassigned at the same time. This test does ensure
        /// that the Workflow Assignment records have been updated correctly after the case has been reassigned.
        /// </summary>
        [Test, Description("A FL Supervisor can reassign an workflow case"), Category("Readvances")]
        public void _02_ReassignUser()
        {
            //search for the case
            base.FLSupervisorBrowser = new TestBrowser(TestUsers.FLSupervisor);
            var offerKey = Helper.GetApplicationForFurtherLendingTest(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement);
            base.FLSupervisorBrowser.Page<WorkflowSuperSearch>().Search(base.FLSupervisorBrowser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //perform the action
            string adUserName = base.FLSupervisorBrowser.Page<ApplicationSummaryFurtherLending>().GetApplicationProcessor();
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ApplicationManagement.ReassignUser);
            string reassignTo = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.FLProcessorD,
                RoundRobinPointerEnum.FLProcessor);
            base.FLSupervisorBrowser.Page<WF_ReAssign>().SelectUserRoleAndConsultantFromDropdownAndCommit(adUserName, reassignTo);
            //assert the case has been reassigned correctly
            AssignmentAssertions.AssertWorkflowAssignment(reassignTo, offerKey, OfferRoleTypeEnum.FLProcessorD);
        }

        /// <summary>
        /// This test ensures that A Further Lending Processor cannot reassign a case in the Application Management workflow.
        /// </summary>
        [Test, Description("A Further Lending Processor cannot reassign a case in the Application Management workflow."), Category("Further Advances")]
        public void _03_FLProcessorReassignDisallowed()
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.QA, Workflows.ApplicationManagement, OfferTypeEnum.FurtherAdvance,
                "FLAutomation");
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.QA);
            //the reassign user action should not exist
            bool actionExists = base.Browser.ActionExists(WorkflowActivities.ApplicationManagement.ReassignUser);
            Assert.False(actionExists, "The Reassign User action is being incorrectly given to FL Processors");
        }

        /// <summary>
        /// This test case will pick up an application in the Readvance Payments workflow and reassign the Further Lending Processor role on the
        /// application.
        /// </summary>
        [Test, Description(@"This test case will pick up an application in the Readvance Payments workflow and reassign the Further Lending Processor role on the
        application.")]
        public void _04_FLProcessorReassignUserReadvancePayments()
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ReadvancePaymentsWF.SetupPayment, Workflows.ReadvancePayments,
                                                                OfferTypeEnum.Readvance, "FLAutomation");
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ReadvancePaymentsWF.SetupPayment);
            //perform the action
            string adUserName = base.Browser.Page<ApplicationSummaryFurtherLending>().GetApplicationProcessor();
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ReassignUser);
            string reassignTo = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.FLProcessorD,
                RoundRobinPointerEnum.FLProcessor);
            base.Browser.Page<WF_ReAssign>().SelectUserRoleAndConsultantFromDropdownAndCommit(adUserName, reassignTo);
            //assert the case has been reassigned correctly
            AssignmentAssertions.AssertWorkflowAssignment(reassignTo, offerKey, OfferRoleTypeEnum.FLProcessorD);
        }
    }
}