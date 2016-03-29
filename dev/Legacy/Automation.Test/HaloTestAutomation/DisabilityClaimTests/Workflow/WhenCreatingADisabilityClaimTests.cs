using Automation.DataAccess;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.DisabilityClaim;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace DisabilityClaimTests.Workflow
{
    [RequiresSTA]
    public class WhenCreatingADisabilityClaimTests : DisabilityClaimsWorkflowTestBase<CreateDisabilityClaim>
    {
        private int AccountKey;
        private int LegalEntityKey;

        protected override void OnTestStart()
        {
            base.OnTestStart();
            QueryResults getOpenLifeAccountWithAssuredLife = GetOpenLifeAccountWithAssuredLife();
            AccountKey = Convert.ToInt32(getOpenLifeAccountWithAssuredLife.Rows(0).Columns[0].Value);
            LegalEntityKey = Convert.ToInt32(getOpenLifeAccountWithAssuredLife.Rows(0).Columns[1].Value);
        }

        [Test]
        public void when_creating_a_disability_claim()
        {
            base.CreateDisabilityClaim(AccountKey);
            base.View.SelectDisabilityClaimant(LegalEntityKey);
            base.View.ClickCreateClaimButton();
            var disabilityClaim = base.GetDisabilityClaimByLegalEntityAndAccountKey(LegalEntityKey, AccountKey);
            Assert.That(disabilityClaim.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Pending));
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(disabilityClaim.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.ClaimDetails);
            StageTransitionAssertions.AssertStageTransitionCreated(disabilityClaim.DisabilityClaimKey, StageDefinitionStageDefinitionGroupEnum.DisabilityClaim_Created);
        }

        [Test]
        public void when_creating_a_disability_claim_without_selecting_a_claimant()
        {
            base.CreateDisabilityClaim(AccountKey);
            base.View.ClickCreateClaimButton();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select a Claimant.");
        }
    }
}