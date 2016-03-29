using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace ApplicationCaptureTests.Workflow
{
    [RequiresSTA]
    public class EscalateToManagerTests : ApplicationCaptureTests.TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
        }

        [Test, Sequential]
        public void _01_EscalateToManager([Values("EscalateToManager")] string identifier)
        {
            base.GetTestCase(identifier);
            base.SearchForCase();
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.EscalatetoManager);
            Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //Assert Branch Manager D offerrole exists
            AssignmentAssertions.AssertOfferRoleRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchManagerD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchManagerD);
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchManagerD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchManagerD);
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.ManagerReview);
        }
    }
}