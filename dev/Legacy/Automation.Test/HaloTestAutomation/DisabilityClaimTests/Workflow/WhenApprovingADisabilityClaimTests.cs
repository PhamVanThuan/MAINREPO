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
using System.Linq;

namespace DisabilityClaimTests.Workflow
{
    [RequiresSTA]
    public class WhenApprovingADisabilityClaimTests : DisabilityClaimsWorkflowTestBase<ApproveDisabilityClaim>
    {
        private DisabilityClaim TestCase;
        private Random r = new Random();

        protected override void OnTestStart()
        {
            base.OnTestStart();
            TestCase = Service<IX2WorkflowService>().GetDisabilityClaimAtState(WorkflowStates.DisabilityClaimWF.AssessClaim);
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.DisabilityClaimWF.AssessClaim, TestCase.LoanAccountKey);            
        }

        [Test]
        public void when_approving_a_disability_claim_and_no_exclusions_exist()
        {
            base.Browser.Navigate<DisabilityClaimNode>().ClickApprove();
            int numberOfInstalments = r.Next(1, 25);
            base.View.AddNumberOfInstalments(numberOfInstalments);
            base.View.SubmitDisabilityClaim();
            var disabilityClaim = base.GetDisabilityClaim(TestCase.DisabilityClaimKey);
            var disabilityPayments = (from dp in Service<IDisabilityClaimService>().GetDisabilityPaymentSchedule(TestCase.DisabilityClaimKey)
                                      where dp.DisabilityPaymentStatusKey == (int)DisabilityPaymentStatusEnum.Active
                                      select dp);
            Assert.That(disabilityClaim.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Approved));
            Assert.That(disabilityPayments.Count().Equals(numberOfInstalments));
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(disabilityClaim.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.SendApprovalLetter);
            StageTransitionAssertions.AssertStageTransitionCreated(disabilityClaim.DisabilityClaimKey, StageDefinitionStageDefinitionGroupEnum.DisabilityClaim_Approved);
            StageTransitionAssertions.AssertStageTransitionCreated(disabilityClaim.DisabilityClaimKey, StageDefinitionStageDefinitionGroupEnum.DisabilityClaim_ContainsNoExclusions);
        }

        [Test]
        public void when_approving_a_disability_claim_and_exclusions_exist()
        {
            int financialServiceKey = Service<IAccountService>().GetAccountByKey(TestCase.LoanAccountKey).FinancialServices.
                Where(x => x.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan).FirstOrDefault().FinancialServiceKey;
            Service<ILoanTransactionService>().pProcessTran(financialServiceKey, TransactionTypeEnum.Readvance, 10000M, "Readvance", @"SAHL\HaloUser");
            base.Browser.Navigate<DisabilityClaimNode>().ClickApprove();
            int numberOfInstalments = r.Next(1, 25);
            base.View.AddNumberOfInstalments(numberOfInstalments);
            base.View.SubmitDisabilityClaim();
            BuildingBlocks.Timers.GeneralTimer.Wait(3000);
            var disabilityClaim = base.GetDisabilityClaim(TestCase.DisabilityClaimKey);
            var disabilityPayments = (from dp in Service<IDisabilityClaimService>().GetDisabilityPaymentSchedule(TestCase.DisabilityClaimKey)
                                      where dp.DisabilityPaymentStatusKey == (int)DisabilityPaymentStatusEnum.Active
                                      select dp);
            Assert.That(disabilityClaim.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Approved));
            Assert.That(disabilityPayments.Count().Equals(numberOfInstalments));
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(disabilityClaim.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.ApprovedClaim);
            StageTransitionAssertions.AssertStageTransitionCreated(disabilityClaim.DisabilityClaimKey, StageDefinitionStageDefinitionGroupEnum.DisabilityClaim_Approved);
            StageTransitionAssertions.AssertStageTransitionCreated(disabilityClaim.DisabilityClaimKey, StageDefinitionStageDefinitionGroupEnum.DisabilityClaim_ManualApprovalLetter);
        }

        [Test]
        public void when_approving_a_disability_claim_and_required_fields_are_empty()
        {
            base.Browser.Navigate<DisabilityClaimNode>().ClickApprove();
            base.View.SubmitDisabilityClaim();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("No. of Authorised Disability Instalments must be entered.");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Disability Payment End Date cannot be empty.");
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(TestCase.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.AssessClaim);
            Assert.That(TestCase.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Pending));
        }

        [Test]
        public void when_cancelling_before_approving_a_disability_claim()
        {
            base.Browser.Navigate<DisabilityClaimNode>().ClickApprove();
            int numberOfInstalments = r.Next(1, 25);
            base.View.AddNumberOfInstalments(numberOfInstalments);
            base.View.CancelDisabilityClaimBeforeSubmission();
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(TestCase.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.AssessClaim);
            Assert.That(TestCase.DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Pending));
        }
    }
}