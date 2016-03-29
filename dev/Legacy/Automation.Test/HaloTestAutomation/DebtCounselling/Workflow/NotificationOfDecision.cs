using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public sealed class NotificationOfDecision : DebtCounsellingTests.TestBase<CorrespondenceProcessingMultipleWorkflowDebtCounsellor>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        /// This test will ensure that the Debt Counsellor is correct displayed on the Correspondence Screen when the Notification of Decision action
        /// is selected. The Debt Counsellor is displayed due to the Proposal Acceptance or the Court Order Acceptance reason being added when the Proposal is accepted.
        /// </summary>
        [Test, Sequential, Description(@"This test will ensure that the Debt Counsellor is correct displayed on the Correspondence Screen when the Notification of Decision
        action is selected. The Debt Counsellor is displayed due to the Proposal Acceptance or the Court Order Acceptance reason being added when the Proposal is accepted.")]
        public void NotificationOfDecisionProposalAcceptance([Values(ReasonDescription.ProposalAcceptance, ReasonDescription.CourtOrderAcceptance)]  string reasonDescription)
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.AcceptedProposal, TestUsers.DebtCounsellingConsultant);
            //we need the active proposal key
            int proposalKey = Service<IProposalService>().GetProposalKeyByStatusAndAcceptedStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, ProposalAcceptedEnum.True,
                ProposalTypeEnum.Proposal);
            //we need to change the reason on the case
            Service<IReasonService>().RemoveReasonsAgainstGenericKeyByReasonType(proposalKey, GenericKeyTypeEnum.Proposal_ProposalKey, ReasonTypeEnum.ProposalAccepted);
            Service<IReasonService>().InsertReason(proposalKey, reasonDescription, ReasonTypeEnum.ProposalAccepted, GenericKeyTypeEnum.Proposal_ProposalKey);
            var externalRole = Service<IExternalRoleService>().GetFirstActiveExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.DebtCounsellor);

            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.NotificationofDecision);
            base.View.AssertCorrespondenceRecipientExists(externalRole.LegalEntityKey);
            base.View.SelectCorrespondenceRecipient(externalRole.LegalEntityKey);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(WorkflowActivities.DebtCounselling.NotificationofDecision, base.TestCase.InstanceID, 1);
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.PendPayment);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, true, true);
        }
    }
}