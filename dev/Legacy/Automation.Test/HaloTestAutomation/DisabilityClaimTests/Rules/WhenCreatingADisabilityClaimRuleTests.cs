using BuildingBlocks.Presenters.DisabilityClaim;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;

namespace DisabilityClaimTests.Rules
{
    [RequiresSTA]
    public class WhenCreatingADisabilityClaimRuleTests : DisabilityClaimsWorkflowTestBase<CreateDisabilityClaim>
    {
        [Test]
        public void when_creating_a_disability_claim_and_the_client_already_has_a_pending_or_approved_disability_claim()
        {
            var disabilityClaim = Service<IX2WorkflowService>().GetDisabilityClaimAtState(WorkflowStates.DisabilityClaimWF.AssessClaim);            
            base.CreateDisabilityClaim(disabilityClaim.AccountKey);
            base.View.SelectDisabilityClaimant(disabilityClaim.LegalEntityKey);
            base.View.ClickCreateClaimButton();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The client already has a pending or approved disability claim.");
        }
    }
}