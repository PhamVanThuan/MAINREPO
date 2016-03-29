using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    internal class DocumentCheckTests : PersonalLoansWorkflowTestBase<WorkflowYesNo>
    {
        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.ManageApplication, WorkflowRoleTypeEnum.PLConsultantD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DocumentCheck);
        }

        /// <summary>
        /// When performing the document check action the case should be round robin assigned to the next personal loan admin.
        /// </summary>
        [Test]
        public void when_performing_document_check_should_round_robin_to_personal_loan_admin()
        {
            string expectedPLAdmin = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey,
                WorkflowRoleTypeEnum.PLAdminD, RoundRobinPointerEnum.PLAdmin);
            base.View.ClickYes();
            bool offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.DocumentCheck);
            Assert.That(offerExists);
            StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_DocumentCheck);
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLAdminD, expectedPLAdmin, WorkflowStates.PersonalLoansWF.DocumentCheck, Workflows.PersonalLoans);
        }
    }
}