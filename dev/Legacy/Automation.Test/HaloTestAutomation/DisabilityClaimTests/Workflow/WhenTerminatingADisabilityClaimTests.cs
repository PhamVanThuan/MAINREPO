using Automation.DataModels;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.DisabilityClaim;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DisabilityClaim;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Linq;

namespace DisabilityClaimTests.Workflow
{
    [RequiresSTA]
    public class WhenTerminatingADisabilityClaimTests : DisabilityClaimsWorkflowTestBase<TerminateDisabilityClaim>
    {
        private DisabilityClaim TestCase;

        protected override void OnTestStart()
        {
            base.OnTestStart();            
            TestCase = Service<IX2WorkflowService>().GetDisabilityClaimAtState(WorkflowStates.DisabilityClaimWF.ApprovedClaim);
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.DisabilityClaimWF.ApprovedClaim, TestCase.LoanAccountKey);
            base.Browser.Navigate<DisabilityClaimNode>().ClickTerminate();
        }

        [Test]
        public void when_terminating_a_disability_claim()
        {
            base.View.SelectTerminationReason(DisabilityClaimTerminationReasons.ClientReturnedToWork);
            base.View.SubmitDisabilityClaimTermination();
            var disabilityClaim = base.GetDisabilityClaim(TestCase.DisabilityClaimKey);
            var disabilityPayments = (from dp in Service<IDisabilityClaimService>().GetDisabilityPaymentSchedule(TestCase.DisabilityClaimKey)
                                      where dp.DisabilityPaymentStatusKey == (int)DisabilityPaymentStatusEnum.Active
                                      select dp);
            Assert.That(disabilityClaim.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Terminated));
            Assert.That(disabilityPayments.Count().Equals(0));
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(disabilityClaim.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.ArchiveTerminated);
            StageTransitionAssertions.AssertStageTransitionCreated(disabilityClaim.DisabilityClaimKey, StageDefinitionStageDefinitionGroupEnum.DisabilityClaim_Terminated);
        }

        [Test]
        public void when_cancelling_before_terminating_a_disability_claim()
        {
            base.View.SelectTerminationReason(DisabilityClaimTerminationReasons.DeathClaimReceived);
            base.View.CancelDisabilityClaimBeforeTermination();
            var disabilityClaim = base.GetDisabilityClaim(TestCase.DisabilityClaimKey);
            Assert.That(disabilityClaim.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Approved));
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(disabilityClaim.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.ApprovedClaim);
        }

        [Test]
        public void when_terminating_a_disability_claim_without_selecting_a_termination_reason()
        {
            string message = base.View.SubmitDisabilityClaimTerminationWithoutSelectingAReason();
            var disabilityClaim = base.GetDisabilityClaim(TestCase.DisabilityClaimKey);
            Assert.That(disabilityClaim.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Approved));
            Assert.That(message.Equals("Please select at least one Reason"));
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(disabilityClaim.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.ApprovedClaim);
        }
    }
}