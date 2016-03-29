using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;

namespace ApplicationCaptureTests.Workflow
{
    [RequiresSTA]
    public class BranchConsultantAssignmentTests : ApplicationCaptureTests.TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
        }

        /// <summary>
        /// This test case picks up a lead and ensures that a lead can be reassigned to another branch consultant using
        /// the Reassign Consultant action that is applied at the Manage Lead state.
        /// </summary>
        [Test, Description("Verify that a Branch Consultant can reassign a Lead")]
        public void _01_ReAssignBranchConsultantLead()
        {
            base.GetTestCase("ReAssignConsultantLead", true);
            base.SearchForCase();
            //Step 2: Select the action.
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.ReAssign);
            //Step 3: Select a role and user from the dropdown.
            base.Browser.Page<WF_ReAssign>().SelectUserRoleAndConsultantFromDropdownAndCommit(base.TestCase.Username, base.TestCase.ReAssignedConsultant);
            //Step 4: Assert that the the case is with consultant.
            string adusernamewithoutdomain = base.TestCase.ReAssignedConsultant.RemoveDomainPrefix();
            OfferAssertions.AssertOfferRoleCreatedAndAssignedToADUser(base.TestCase.OfferKey, adusernamewithoutdomain, OfferRoleTypeEnum.BranchConsultantD, 60);
            //Step 5: Assert that the case have the correct worklfow assignment record.
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            //Step 6: Assert that the case have an active worklfow assignment record.
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
        }

        /// <summary>
        /// This test ensures that a branch consultant can pick up an application and correctly assign an admin to the application
        /// using the Assign Admin action.
        /// </summary>
        [Test, Description("Verify that a Branch Consultant can assign an Admin User to an Application")]
        public void _02_AssignAdminToApplication()
        {
            base.GetTestCase("AssignAdminApplication");
            base.SearchForCase();
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.AssignAdmin);
            //Step 3: Select a user from the dropdown
            string adUserName = TestUsers.BranchAdmin;
            base.Browser.Page<App_AssignAdmin>().SelectAdminFromDropdownAndCommit(adUserName.RemoveDomainPrefix());
            //Step 4: Assert that there is active 102 role linked to the above ADUser
            OfferAssertions.AssertOfferRoleCreatedAndAssignedToADUser(base.TestCase.OfferKey, adUserName, OfferRoleTypeEnum.BranchAdminD, 59);
            //STILLTODO: Step 5: Assert that a WFA record exists for our BCuser and our BAuser
            AssertWorkflowAssignment(OfferRoleTypeEnum.BranchAdminD, adUserName);
            //check consultant still assigned
            AssertWorkflowAssignment(OfferRoleTypeEnum.BranchConsultantD, base.TestCase.Username);
        }

        /// <summary>
        /// Verify that a Branch Consultant can assign an Admin User to a Lead
        /// </summary>
        [Test, Description("Verify that a Branch Consultant can assign an Admin User to a Lead")]
        public void _03_AssignAdminToLead()
        {
            base.GetTestCase("AssignAdminLead", true);
            base.SearchForCase();
            //Step 2: Select the action
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.AssignAdmin);
            //Step 3: Select a user from the dropdown
            const string adUserName = "BAUser";
            base.Browser.Page<App_AssignAdmin>().SelectAdminFromDropdownAndCommit(adUserName);
            //Step 4: Assert that there is active 102 role linked to the above ADUser
            OfferAssertions.AssertOfferRoleCreatedAndAssignedToADUser(base.TestCase.OfferKey, adUserName, OfferRoleTypeEnum.BranchAdminD, 59);
            //Step 5: Assert that a WFA record exists for our BCuser and our BAuser
            string username = TestUsers.BranchAdmin;
            AssertWorkflowAssignment(OfferRoleTypeEnum.BranchAdminD, username);
            AssertWorkflowAssignment(OfferRoleTypeEnum.BranchConsultantD, base.TestCase.Username);
        }

        private void AssertWorkflowAssignment(OfferRoleTypeEnum roleType, string user)
        {
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(base.TestCase.OfferKey, roleType);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(base.TestCase.OfferKey, roleType);
            AssignmentAssertions.AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(base.TestCase.OfferKey, roleType, user);
        }
    }
}