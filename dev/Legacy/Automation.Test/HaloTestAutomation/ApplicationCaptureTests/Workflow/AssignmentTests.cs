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
    public class ConsultantAssignmentTests : ApplicationCaptureTests.TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchAdmin);
        }

        /// <summary>
        /// This test ensures that a branch admin user can pickup an application and assign a branch consultant user
        /// to the application using the Assign Consultant action in Application Capture
        /// </summary>
        [Test, Description(@"This test ensures that a branch admin user can pickup an application and assign a branch consultant user
		to the application using the Assign Consultant action in Application Capture")]
        public void _01_AssignConsultant()
        {
            base.GetTestCase("AssignConsultant");
            base.SelectCaseFromWorklist(WorkflowStates.ApplicationCaptureWF.ConsultantAssignment);
            var consultant = TestUsers.BranchConsultant;
            base.Browser.Page<AssignInitialConsultant>().AssignConsultant(consultant, ButtonTypeEnum.Submit);
            //commissionable consultant
            AssignmentAssertions.AssertOfferRoleRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.CommissionableConsultant);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.CommissionableConsultant);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(base.TestCase.OfferKey, OfferRoleTypeEnum.CommissionableConsultant, consultant);
            //branch consultant
            AssertAssignment(OfferRoleTypeEnum.BranchConsultantD, consultant);
            //branch admin
            AssertAssignment(OfferRoleTypeEnum.BranchAdminD, base.TestCase.Username);
        }

        /// <summary>
        /// Verify that a Branch Admin can reassign a Lead
        /// </summary>
        [Test, Description("Verify that a Branch Admin can reassign a Lead")]
        public void _02_ReAssignBranchAdminLead()
        {
            base.GetTestCase("ReAssignAdminLead", true);
            SearchForCase();
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ReAssign);
            //Step 3: Select a role and user from the dropdown.
            Browser.Page<WF_ReAssign>().SelectUserRoleAndConsultantFromDropdownAndCommit(base.TestCase.Username, base.TestCase.ReAssignedConsultant);
            //Step 4: Assert that the the case is with consultant.
            string adusernamewithoutdomain = base.TestCase.ReAssignedConsultant.RemoveDomainPrefix();
            OfferAssertions.AssertOfferRoleCreatedAndAssignedToADUser(base.TestCase.OfferKey, adusernamewithoutdomain, OfferRoleTypeEnum.BranchAdminD, 59);
            //Step 5: Assert that the case have the correct worklfow assignment record.
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchAdminD);
            //Step 6: Assert that the case have an active worklfow assignment record.
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchAdminD);
            //Update the ReAssignedConsultant in the BranchConsultantLeads.csv file.
        }

        private void AssertAssignment(OfferRoleTypeEnum roleType, string user)
        {
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(base.TestCase.OfferKey, roleType);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(base.TestCase.OfferKey, roleType);
            AssignmentAssertions.AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(base.TestCase.OfferKey, roleType, user);
            AssignmentAssertions.AssertOfferRoleRecordExists(base.TestCase.OfferKey, roleType);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(base.TestCase.OfferKey, roleType);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(base.TestCase.OfferKey, roleType, user);
            AssignmentAssertions.AssertOfferExistsOnWorkList(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.ApplicationCapture, user, Workflows.ApplicationCapture);
        }
    }
}