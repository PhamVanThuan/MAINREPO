using Automation.DataModels;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.DisabilityClaim;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DisabilityClaim;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace DisabilityClaimTests.Workflow
{
    [RequiresSTA]
    public class WhenRepudiatingADisabilityClaimTests : DisabilityClaimsWorkflowTestBase<RepudiateDisabilityClaim>
    {
        private DisabilityClaim TestCase;

        protected override void OnTestStart()
        {
            base.OnTestStart();
            TestCase = Service<IX2WorkflowService>().GetDisabilityClaimAtState(WorkflowStates.DisabilityClaimWF.AssessClaim);
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.DisabilityClaimWF.AssessClaim, TestCase.LoanAccountKey);
            base.Browser.Navigate<DisabilityClaimNode>().ClickRepudiate();
        }

        [Test]
        public void when_repudiating_a_disability_claim()
        {
            base.View.SelectRepudiationReason(DisabilityClaimRepudiationReasons.Alcohol);
            base.View.SubmitDisabilityClaimRepudiation();
            var disabilityClaim = base.GetDisabilityClaim(TestCase.DisabilityClaimKey);
            Assert.That(disabilityClaim.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Repudiated));
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(disabilityClaim.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.ArchiveRepudiated);
            StageTransitionAssertions.AssertStageTransitionCreated(disabilityClaim.DisabilityClaimKey, StageDefinitionStageDefinitionGroupEnum.DisabilityClaim_Repudiated);
        }

        [Test]
        public void when_cancelling_before_repudiating_a_disability_claim()
        {
            base.View.SelectRepudiationReason(DisabilityClaimRepudiationReasons.Alcohol);
            base.View.SelectRepudiationReason(DisabilityClaimRepudiationReasons.SelfInflictedInjuryOrSuicideAttempt);
            base.View.CancelDisabilityClaimBeforeRepudiation();
            Assert.That(TestCase.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Pending));
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(TestCase.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.AssessClaim);
        }

        [Test]
        public void when_repudiating_a_disability_claim_without_selecting_a_repudiation_reason()
        {
            string message = base.View.SubmitDisabilityClaimRepudiationWithoutSelectingAReason();
            Assert.That(message.Equals("Please select at least one Reason"));
            Assert.That(TestCase.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Pending));
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(TestCase.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.AssessClaim);
        }
    }
}