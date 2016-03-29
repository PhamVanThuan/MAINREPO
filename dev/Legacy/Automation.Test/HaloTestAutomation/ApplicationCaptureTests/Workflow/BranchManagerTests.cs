using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;
using System;

namespace ApplicationCaptureTests.Workflow
{
    [RequiresSTA]
    public class BranchManagerTests : ApplicationCaptureTests.TestBase<ReassignOriginatingBranchConsultant>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.GetTestCase("ManagerSubmitSwitchEdgeApplication");
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationCapture, WorkflowAutomationScripts.ApplicationCapture.EscalateToManager, base.TestCase.OfferKey);
            base.GetTestCase("ManagerReassign");
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationCapture, WorkflowAutomationScripts.ApplicationCapture.EscalateToManager, base.TestCase.OfferKey);
            base.GetTestCase("ManagerArchive");
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationCapture, WorkflowAutomationScripts.ApplicationCapture.EscalateToManager, base.TestCase.OfferKey);
            base.GetTestCase("RefreshApplicationTimeout");
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationCapture, WorkflowAutomationScripts.ApplicationCapture.EscalateToManager, base.TestCase.OfferKey);
            base.Browser = new TestBrowser(TestUsers.BranchManager);
        }

        /// <summary>
        /// Verify that a Branch Manager can reassign the Commission Earning consultant on an application
        /// </summary>
        [Test, Description("Verify that a Branch Manager can reassign the Commission Earning consultant on an application")]
        public void _01_ReAssignCommissionEarningConsultant()
        {
            base.GetTestCase("ReAssignCommissionEarningConsultant");
            string reassignedUser = TestUsers.BranchConsultant1;
            SearchForCase();
            //select the action
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ReAssignCommissionConsultant);
            //select a new consultant
            base.View.ReassignCommissionConsultant(reassignedUser.RemoveDomainPrefix(), false);
            //assert that the 100 OfferRole has changed, there is no WFA change
            AssignmentAssertions.AssertOfferRoleRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.CommissionableConsultant);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.CommissionableConsultant);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(base.TestCase.OfferKey, OfferRoleTypeEnum.CommissionableConsultant, reassignedUser);
        }

        /// <summary>
        /// Verify that a Branch Manager can reassign the Commission Earning and the Branch Consultand D Role on an application
        /// </summary>
        [Test, Description("Verify that a Branch Manager can reassign the Commission Earning and the Branch Consultand D Role on an application")]
        public void _02_ReAssignCommissionEarningAndBranchConsultant()
        {
            base.GetTestCase("ReAssignCommissionEarningAndBranchConsultant");
            string reassignedUser = TestUsers.BranchConsultant1;
            base.SearchForCase();
            //select the action
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ReAssignCommissionConsultant);
            //select a new consultant
            base.View.ReassignCommissionConsultant(reassignedUser.RemoveDomainPrefix(), true);
            //assert that the 100 OfferRole has changed, there is no WFA change
            AssignmentAssertions.AssertOfferRoleRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.CommissionableConsultant);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.CommissionableConsultant);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(base.TestCase.OfferKey, OfferRoleTypeEnum.CommissionableConsultant, reassignedUser);
            //we need to check the 101 OfferRole
            AssignmentAssertions.AssertOfferRoleRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD, reassignedUser);
            //we need to check the 101 WFA record
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD, reassignedUser);
        }

        /// <summary>
        /// Verify that a Branch Manager can perform the 'Manager Reassign' action at 'Manager Review' state,
        /// which moves the case to 'Application Capture' state and reassigns the case to the selected use
        /// </summary>
        [Test, Description(@"Verify that a Branch Manager can perform the 'Manager Reassign' action at 'Manager Review' state,
			which moves the case to 'Application Capture' state and reassigns the case to the selected user")]
        public void _03_ManagerReassign()
        {
            base.GetTestCase("ManagerReassign");
            //select ADUSer to reassign offer to.  Ensure it is different to the active user
            string reassignToUser = base.TestCase.Username == TestUsers.BranchConsultant ? TestUsers.BranchConsultant1 : TestUsers.BranchConsultant;
            base.SearchForCase();
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ManagerReassign);
            Browser.Page<WF_ReAssign>().SelectRoleAndConsultantFromDropdownAndCommit(reassignToUser, OfferRoleTypes.BranchConsultantD);
            //Assert the Branch Consultant D offerrole is correctly assigned
            AssignmentAssertions.AssertOfferRoleRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD, reassignToUser);
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD, reassignToUser);
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.ApplicationCapture);
        }

        /// <summary>
        /// Verify that a Branch Manager can perform the Estate Agent Assignment action
        /// </summary>
        [Test, Description("Verify that a Branch Manager can perform the Estate Agent Assignment action")]
        public void _04_EstateAgentAssign()
        {
            base.GetTestCase("ManagerArchive");
            base.SearchForCase();
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.EstateAgentAssignment);
            //look for and agency in the database
            string estateAgencyName = Service<ILegalEntityService>().GetActiveEstateAgencyTradingName();
            //select and assign the Estate Agent
            Browser.Page<App_AssignEstateAgent>().CaptureEstateAgency(estateAgencyName);
            //assert that the Estate Agent offerrole exists
            AssignmentAssertions.AssertOfferRoleRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.EstateAgentChannel);
            //assert that the case is still at current state
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.ManagerReview);
        }

        /// <summary>
        /// Verify that a Branch Manager user can refresh the application timeout value for a case in Application Capture
        /// </summary>
        [Test, Description("Verify that a Branch Manager user can refresh the application timeout value for a case in Application Capture")]
        public void _05_RefreshApplicationTimeout()
        {
            base.GetTestCase("RefreshApplicationTimeout");
            base.SearchForCase();
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.RefreshApplicationTimeout);
            Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //assert that the scheduled activity has been updated
            X2Assertions.AssertScheduleActivitySetup(base.TestCase.OfferKey.ToString(), ScheduledActivities.ApplicationCapture._15DayAutoReminder, 15, 0, 0);
        }

        /// <summary>
        /// Verify that a Branch Manager can perform the Archive Lead action
        /// </summary>
        [Test, Description("Verify that a Branch Manager can perform the Archive Lead action")]
        public void _06_ArchiveLead()
        {
            base.GetTestCase("RefreshApplicationTimeout");
            base.SearchForCase();
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ArchiveLead);
            //confirm the action
            Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //assert that the case is sent to the archive state
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.ApplicationNotAccepted);
            //assert that the offerstatus is set to NTU
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.NTU);
            //assert that the offerenddate is set
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, DateTime.Now, 0, false);
        }

        /// <summary>
        /// Verify that a Branch Manager can perform the 'Manager Submit Application' action at 'Manager Review' state, which moves the case to the next state depending on the LoanType
        /// </summary>
        [Test, Sequential, Description("Verify that a Branch Manager can perform the 'Manager Submit Application' action at 'Manager Review' state, which moves the case to the next state depending on the LoanType")]
        public void _07_ManagerSubmitApplication()
        {
            base.GetTestCase("ManagerSubmitSwitchEdgeApplication");
            base.SearchForCase();
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ManagerSubmitApplication);
            Browser.Page<WorkflowYesNo>().Confirm(true, true);
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.OfferKey, Workflows.ApplicationManagement
                                                                , WorkflowStates.ApplicationManagementWF.ManageApplication);
            X2Assertions.AssertCurrentAppManX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
        }

        /// <summary>
        /// Verify that a Branch Manager can archive an application that has been escalated to their worklist
        /// </summary>
        [Test, Description("Verify that a Branch Manager can archive an application that has been escalated to their worklist")]
        public void _08_ManagerArchive()
        {
            base.GetTestCase("ManagerArchive");
            base.SearchForCase();
            //Perform Manager Archive
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ManagerArchive);
            Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //Assert that the Offer Status is NTU
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.NTU);
            //Assert that the case is in the Archive
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.ApplicationNotAccepted);
            //Assert the End Date has been populated with today's date
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, DateTime.Now, 0, false);
        }
    }
}