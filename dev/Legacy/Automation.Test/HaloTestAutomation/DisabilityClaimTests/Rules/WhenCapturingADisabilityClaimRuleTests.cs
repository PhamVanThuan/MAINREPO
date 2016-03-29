using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.DisabilityClaim;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DisabilityClaim;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using System;

namespace DisabilityClaimTests.Rules
{
    [RequiresSTA]
    public class WhenCapturingADisabilityClaimRuleTests : DisabilityClaimsWorkflowTestBase<CaptureDisabilityClaim>
    {
        [Test]
        public void when_capturing_a_disability_claim_and_expected_return_to_work_date_is_before_claimants_last_date_worked()
        {
            var disabilityClaim = Service<IX2WorkflowService>().GetDisabilityClaimAtState(WorkflowStates.DisabilityClaimWF.ClaimDetails);
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.DisabilityClaimWF.ClaimDetails, disabilityClaim.LoanAccountKey);
            base.Browser.Navigate<DisabilityClaimNode>().ClickCaptureDetails();
            base.View.CaptureClaimDetails(DateTime.Today.AddDays(-10), "Other", "This is an additional comment", "Systems Tester", DateTime.Today.AddDays(-1), DateTime.Today.AddDays(-2));
            base.View.SubmitCapturedClaimDetails();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Expected Return to Work Date cannot be before Claimant's Last Date Worked.");
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(disabilityClaim.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.ClaimDetails);
            Assert.That(base.GetDisabilityClaim(disabilityClaim.DisabilityClaimKey).DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Pending));
        }
    }
}