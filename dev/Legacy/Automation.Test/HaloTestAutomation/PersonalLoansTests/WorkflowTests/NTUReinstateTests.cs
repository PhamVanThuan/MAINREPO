using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class NTUReinstateTests : PersonalLoansWorkflowTestBase<CommonReasonCommonDecline>
    {
        private string expectedUser;

        /// <summary>
        /// NTU's a case from Manage Application and ensures the case is correctly assigned to the consultant when reinstated.
        /// </summary>
        [Test]
        public void NTUAndReinstateFromManageApplication()
        {
            var state = WorkflowStates.PersonalLoansWF.ManageApplication;
            var role = WorkflowRoleTypeEnum.PLConsultantD;
            base.FindCaseAtState(state, role);
            NTUCase();
            ReinstateCase(role, state);
        }

        /// <summary>
        /// NTU's a case from Disbursementand ensures the case is correctly assigned to the supervisor when reinstated.
        /// </summary>
        [Test]
        public void NTUAndReinstateFromDisbursements()
        {
            var state = WorkflowStates.PersonalLoansWF.Disbursement;
            var role = WorkflowRoleTypeEnum.PLSupervisorD;
            base.FindCaseAtState(state, role);
            NTUCase();
            ReinstateCase(role, state);
        }

        /// <summary>
        /// NTU's a case from Legal Agreements and ensures the case is correctly assigned to the admin user when reinstated.
        /// </summary>
        [Test]
        public void NTUAndReinstateFromLegalAgreements()
        {
            var state = WorkflowStates.PersonalLoansWF.LegalAgreements;
            var role = WorkflowRoleTypeEnum.PLConsultantD;
            base.FindCaseAtState(state, role);
            NTUCase();
            ReinstateCase(role, state);
        }

        /// <summary>
        /// NTU's the case
        /// </summary>
        private void NTUCase()
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.NTU);
            //we need the consultant
            this.expectedUser = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey, WorkflowRoleTypeEnum.PLConsultantD,
                RoundRobinPointerEnum.PLConsultant);
            string selectedReason = base.View.SelectReasonAndSubmit(ReasonType.PersonalLoanNTU);
            //case moves to NT
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.NTU);
            Assert.That(offerExists);
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLConsultantD, expectedUser,
                WorkflowStates.PersonalLoansWF.NTU, Workflows.PersonalLoans);
            //reason added
            ReasonAssertions.AssertReason(selectedReason, ReasonType.PersonalLoanNTU, base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey, true);
            base.Browser.Dispose();
        }

        /// <summary>
        /// Reinstates the case
        /// </summary>
        /// <param name="expectedRoleOnReinstate">Expected role of the user who should be given the case on reinstate</param>
        /// <param name="expectedStateOnReinstate">The state that it is reinstated to</param>
        private void ReinstateCase(WorkflowRoleTypeEnum expectedRoleOnReinstate, string expectedStateOnReinstate)
        {
            var pointer = RoundRobinPointerEnum.PLConsultant;
            switch (expectedRoleOnReinstate)
            {
                case WorkflowRoleTypeEnum.PLConsultantD:
                    pointer = RoundRobinPointerEnum.PLConsultant;
                    break;

                case WorkflowRoleTypeEnum.PLSupervisorD:
                    pointer = RoundRobinPointerEnum.PLSupervisor;
                    break;

                case WorkflowRoleTypeEnum.PLAdminD:
                    pointer = RoundRobinPointerEnum.PLAdmin;
                    break;
            }
            base.Browser = new BuildingBlocks.TestBrowser(expectedUser, TestUsers.Password);
            this.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            this.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(this.Browser);
            this.Browser.Page<X2Worklist>().SelectCaseFromWorklist(this.Browser, WorkflowStates.PersonalLoansWF.NTU, this.GenericKey);
            this.Browser.ClickAction(WorkflowActivities.PersonalLoans.ReinstateNTU);
            string reinstateUser = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey,
                    expectedRoleOnReinstate, pointer);
            this.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            //reinstated
            var caseExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, expectedStateOnReinstate);
            Assert.That(caseExists);
            //case should be back with to the role who has access at the state from which it was NTU'd
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, expectedRoleOnReinstate, reinstateUser,
                expectedStateOnReinstate, Workflows.PersonalLoans);
            //check transition
            StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_ReinstateNTU);
        }
    }
}