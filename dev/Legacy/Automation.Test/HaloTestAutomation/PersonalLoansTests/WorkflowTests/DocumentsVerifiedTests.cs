using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    internal class DocumentsVerifiedTests : PersonalLoansWorkflowTestBase<WorkflowYesNo>
    {
        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.VerifyDocuments, WorkflowRoleTypeEnum.PLAdminD, true);
        }

        /// <summary>
        /// Performs the documents verified activity, ensuring that the case is sent to the Disbursement state and assigned to the PL Supervisor
        /// </summary>
        [Test]
        public void DocumentsVerified()
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DocumentsVerified);
            //get expected user
            string expectedUser = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey,
                WorkflowRoleTypeEnum.PLSupervisorD, RoundRobinPointerEnum.PLSupervisor);
            base.View.Confirm(true, true);
            //check case movement
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.Disbursement);
            Assert.That(offerExists);
            //check transition
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey,
                StageDefinitionStageDefinitionGroupEnum.PersonalLoans_DocumentsVerified);
            //check the assignment
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLSupervisorD, expectedUser,
                WorkflowStates.PersonalLoansWF.Disbursement, Workflows.PersonalLoans);
        }
    }
}