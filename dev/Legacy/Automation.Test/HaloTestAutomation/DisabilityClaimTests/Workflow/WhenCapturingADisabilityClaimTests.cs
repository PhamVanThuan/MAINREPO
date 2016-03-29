using Automation.DataModels;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.DisabilityClaim;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DisabilityClaim;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace DisabilityClaimTests.Workflow
{
    [RequiresSTA]
    public class WhenCapturingADisabilityClaimTests : DisabilityClaimsWorkflowTestBase<CaptureDisabilityClaim>
    {
        private DisabilityClaim TestCase;

        protected override void OnTestStart()
        {
            base.OnTestStart();
            TestCase = Service<IX2WorkflowService>().GetDisabilityClaimAtState(WorkflowStates.DisabilityClaimWF.ClaimDetails);
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.DisabilityClaimWF.ClaimDetails, TestCase.LoanAccountKey);
            base.Browser.Navigate<DisabilityClaimNode>().ClickCaptureDetails();
        }

        [Test]
        public void when_capturing_a_disability_claim()
        {
            base.View.CaptureClaimDetails(DateTime.Today.AddDays(-15), "Other", "This is an additional comment", "Systems Tester", DateTime.Today.AddDays(-5), DateTime.Today.AddMonths(12));
            base.View.SubmitCapturedClaimDetails();
            var disabilityClaim = base.GetDisabilityClaim(TestCase.DisabilityClaimKey);
            Assert.That(disabilityClaim.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Pending));
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(disabilityClaim.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.AssessClaim);
            StageTransitionAssertions.AssertStageTransitionCreated(disabilityClaim.DisabilityClaimKey, StageDefinitionStageDefinitionGroupEnum.DisabilityClaim_Captured);
        }

        [Test]
        public void when_capturing_a_disability_claim_and_not_entering_required_values()
        {
            base.View.ClearAllDetails();
            base.View.SubmitCapturedClaimDetails();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Date of Diagnosis must be entered.");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Nature of the Disability must be selected.");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Occupation must be entered.");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Last Date Worked must be entered.");
            Assert.That(TestCase.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Pending));
        }

        [Test]
        public void when_cancelling_before_capturing_a_disability_claim()
        {
            base.View.CaptureClaimDetails(DateTime.Today.AddDays(-17), "Other", "This is an additional comment", "Systems Tester", DateTime.Today.AddDays(-7), DateTime.Today.AddMonths(12));
            base.View.CancelClaimBeforeSubmission();
            Assert.That(TestCase.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Pending));
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(TestCase.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.ClaimDetails);
        }
    }
}