using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class ReturnToLegalAgreementsTests : PersonalLoansWorkflowTestBase<WorkflowYesNo>
    {
        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.VerifyDocuments, WorkflowRoleTypeEnum.PLAdminD, true);
        }

        /// <summary>
        /// This test will determine that an application successfully moves from state 'Verify Documents' to state 'Legal Agreements' when the PL Admin user performs the 'Return to Legal Agreements' action.
        /// </summary>
        [Test, Description("This test will determine that an application successfully moves from state 'Verify Documents' to state 'Legal Agreements' when the PL Admin user performs the 'Return to Legal Agreements' action.")]
        public void ReturnToLegalAgreements()
        {
            // Send application back to Legal Agreements by performing the 'Return to Legal Agreements' action.
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ReturntoLegalAgreements);
            base.View.Confirm(true, true);
            // Get the expected user
            string expectedUser = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey, WorkflowRoleTypeEnum.PLConsultantD, RoundRobinPointerEnum.PLConsultant);
            // Assert that the case has returned to state 'Legal Agreements'
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.LegalAgreements);
            Assert.That(offerExists);
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_ReturnToLegalAgreements);
            // Check that the assignment has been successfull
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLConsultantD, expectedUser, WorkflowStates.PersonalLoansWF.LegalAgreements, Workflows.PersonalLoans);
        }
    }
}