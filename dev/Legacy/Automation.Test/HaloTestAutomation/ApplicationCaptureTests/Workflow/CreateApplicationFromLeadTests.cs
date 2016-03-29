using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace ApplicationCaptureTests.Workflow
{
    [RequiresSTA]
    public class CreateApplicationFromLeadTests : ApplicationCaptureTests.TestBase<CommonReasonCommonDecline>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
        }

        /// <summary>
        /// Ensures that a Branch Consultant user can create an application from a lead
        /// </summary>
        [Test, Description("Ensures that a Branch Consultant user can create an application from a lead")]
        public void CreateApplicationFromLead()
        {
            base.GetTestCase("CreateApplicationFromLead", true);
            SearchForCase();
            //Step 2: Select the action
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.CreateApplication);
            //Create the application
            Browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_NewPurchase(Products.Edge, "900000", "200000",
                EmploymentType.Salaried, "240", "35000", ButtonTypeEnum.CreateApplication);

            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.OfferKey, Workflows.ApplicationCapture, WorkflowStates.ApplicationCaptureWF.ApplicationCapture);

            //assert that the case is now in Application Capture state
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.ApplicationCapture);
            //assert the case is still owned by the correct user
            //we need to check the 101 OfferRole
            AssignmentAssertions.AssertOfferRoleRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD, base.TestCase.Username);
            //we need to check the 101 WFA record
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD, base.TestCase.Username);
        }
    }
}