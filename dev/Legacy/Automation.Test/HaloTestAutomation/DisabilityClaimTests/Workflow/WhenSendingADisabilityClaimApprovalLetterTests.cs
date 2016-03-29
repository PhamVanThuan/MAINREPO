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
    public class WhenSendingADisabilityClaimApprovalLetterTests : DisabilityClaimsWorkflowTestBase<SendDisabilityClaimLetter>
    {
        private DisabilityClaim TestCase;

        protected override void OnTestStart()
        {
            base.OnTestStart();
            TestCase = Service<IX2WorkflowService>().GetDisabilityClaimAtState(WorkflowStates.DisabilityClaimWF.SendApprovalLetter);
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.DisabilityClaimWF.SendApprovalLetter, TestCase.LoanAccountKey);
            base.Browser.Navigate<DisabilityClaimNode>().ClickSendApprovalLetter();
        }

        [Test]
        public void when_sending_a_disability_claim_approval_letter_by_email()
        {
            base.View.SelectCorrespondenceOption(Common.Enums.CorrespondenceMediumEnum.Email);
            base.View.SendCorrespondence(Common.Enums.CorrespondenceMediumEnum.Email, TestCase.LegalEntityKey);
            var disabilityClaim = base.GetDisabilityClaim(TestCase.DisabilityClaimKey);
            Assert.That(disabilityClaim.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Approved));
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(disabilityClaim.DisabilityClaimKey, CorrespondenceReports.DisabilityClaimApprovalLetter, CorrespondenceMedium.Email);
            CorrespondenceAssertions.AssertImageIndex(disabilityClaim.DisabilityClaimKey.ToString(), CorrespondenceReports.DisabilityClaimApprovalLetter, CorrespondenceMedium.Email, disabilityClaim.AccountKey, disabilityClaim.DisabilityClaimKey);
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(disabilityClaim.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.ApprovedClaim);
            StageTransitionAssertions.AssertStageTransitionCreated(disabilityClaim.DisabilityClaimKey, StageDefinitionStageDefinitionGroupEnum.DisabilityClaim_ApprovalLetterSent);
        }

        [Test]
        public void when_sending_a_disability_claim_approval_letter_by_post()
        {
            base.View.SelectCorrespondenceOption(Common.Enums.CorrespondenceMediumEnum.Post);
            base.View.SendCorrespondence(Common.Enums.CorrespondenceMediumEnum.Post, TestCase.LegalEntityKey);
            var disabilityClaim = base.GetDisabilityClaim(TestCase.DisabilityClaimKey);
            Assert.That(disabilityClaim.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Approved));
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(disabilityClaim.DisabilityClaimKey, CorrespondenceReports.DisabilityClaimApprovalLetter, CorrespondenceMedium.Post);
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(disabilityClaim.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.ApprovedClaim);
            StageTransitionAssertions.AssertStageTransitionCreated(disabilityClaim.DisabilityClaimKey, StageDefinitionStageDefinitionGroupEnum.DisabilityClaim_ApprovalLetterSent);
        }

        [Test]
        public void when_sending_a_disability_claim_approval_letter_and_not_selecting_a_correspondence_option()
        {
            base.View.SendCorrespondence(Common.Enums.CorrespondenceMediumEnum.None, TestCase.LegalEntityKey);
            Assert.That(TestCase.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Approved));
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select at least one Correspondence Option");
            CorrespondenceAssertions.AssertCorrespondenceRecordNotAdded(TestCase.DisabilityClaimKey, CorrespondenceReports.DisabilityClaimApprovalLetter, CorrespondenceMedium.Email);
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(TestCase.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.SendApprovalLetter);
        }

        [Test]
        public void when_cancelling_before_sending_a_disability_claim_approval_letter()
        {
            base.View.SelectCorrespondenceOption(Common.Enums.CorrespondenceMediumEnum.Post);
            base.View.CancelSendingCorrespondenceBeforeSubmission();
            Assert.That(TestCase.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Approved));
            CorrespondenceAssertions.AssertCorrespondenceRecordNotAdded(TestCase.DisabilityClaimKey, CorrespondenceReports.DisabilityClaimApprovalLetter, CorrespondenceMedium.Email);
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(TestCase.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.SendApprovalLetter);
        }
    }
}