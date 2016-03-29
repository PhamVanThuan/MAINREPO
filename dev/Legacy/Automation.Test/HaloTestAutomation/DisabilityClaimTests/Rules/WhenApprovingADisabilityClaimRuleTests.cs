using Automation.DataModels;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.DisabilityClaim;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DisabilityClaim;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using System.Linq;

namespace DisabilityClaimTests.Rules
{
    [RequiresSTA]
    public class WhenApprovingADisabilityClaimRuleTests : DisabilityClaimsWorkflowTestBase<ApproveDisabilityClaim>
    {
        private DisabilityClaim DisabilityClaim;

        protected override void OnTestStart()
        {
            base.OnTestStart();
            DisabilityClaim = Service<IX2WorkflowService>().GetDisabilityClaimAtState(WorkflowStates.DisabilityClaimWF.AssessClaim);
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.DisabilityClaimWF.AssessClaim, DisabilityClaim.LoanAccountKey);
        }

        [Test]
        public void when_approving_a_disability_claim_and_the_number_of_instalments_is_greater_than_99()
        {
            base.Browser.Navigate<DisabilityClaimNode>().ClickApprove();
            base.View.AddNumberOfInstalments(100);
            base.View.SubmitDisabilityClaim();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("No. of Authorised Instalments cannot be greater than 99.");
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(DisabilityClaim.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.AssessClaim);
            Assert.That(base.GetDisabilityClaim(DisabilityClaim.DisabilityClaimKey).DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Pending));
        }

        [Test]
        public void when_approving_a_disability_claim_and_the_number_of_instalments_exceeds_the_bond_term()
        {
            int financialServiceKey = Service<IAccountService>().GetAccountByKey(DisabilityClaim.LoanAccountKey).FinancialServices.
                Where(x => x.FinancialServiceTypeKey.Equals(1)).FirstOrDefault().FinancialServiceKey;
            int remainingInstalments = Service<ILoanTransactionService>().GetRemainingInstalmentsOnLoan(financialServiceKey);
            Service<ILoanTransactionService>().UpdateRemainingInstalmentsOnLoan(financialServiceKey, 1);
            base.Browser.Navigate<DisabilityClaimNode>().ClickApprove();
            base.View.AddNumberOfInstalments(10);
            base.View.SubmitDisabilityClaim();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("No. of Authorised Instalments exceeds the Term of the Bond.");
            X2Assertions.AssertCurrentX2State(base.GetInstanceID(DisabilityClaim.DisabilityClaimKey), WorkflowStates.DisabilityClaimWF.AssessClaim);
            Assert.That(base.GetDisabilityClaim(DisabilityClaim.DisabilityClaimKey).DisabilityClaimStatusKey.Equals((int)Common.Enums.DisabilityClaimStatusEnum.Pending));
            Service<ILoanTransactionService>().UpdateRemainingInstalmentsOnLoan(financialServiceKey, remainingInstalments);
        }
    }
}