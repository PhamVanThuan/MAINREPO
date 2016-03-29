using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class ApproveApplicationTests : PersonalLoansWorkflowTestBase<WorkflowYesNo>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.Credit, WorkflowRoleTypeEnum.PLCreditAnalystD);
        }

        /// <summary>
        /// This test will complete the Approve action and ensure that the case is moved to the legal agreements stage, assigned to the
        /// PL Admin user. The offer information should also be marked as accepted.
        /// </summary>
        [Test]
        public void ApproveApplication()
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.Approve);
            //get expected user
            string expectedUser = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey,
                WorkflowRoleTypeEnum.PLConsultantD, RoundRobinPointerEnum.PLConsultant);
            base.View.Confirm(true, true);
            //check case movement
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.LegalAgreements);
            Assert.That(offerExists);
            //check transition
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey,
                StageDefinitionStageDefinitionGroupEnum.PersonalLoans_Approve);
            //check the assignment
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLConsultantD, expectedUser,
                WorkflowStates.PersonalLoansWF.LegalAgreements, Workflows.PersonalLoans);
            //offer information should have marked as accepted
            OfferAssertions.OfferInformationUpdated(base.GenericKey, OfferInformationTypeEnum.AcceptedOffer, ProductEnum.PersonalLoan);
        }
    }
}