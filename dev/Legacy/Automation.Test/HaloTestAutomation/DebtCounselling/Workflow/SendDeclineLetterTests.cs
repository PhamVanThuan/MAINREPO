using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Linq;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public class SendDeclineLetterTests : TestBase<BasePageAssertions>
    {
        #region CommonSendDeclineLetter

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        /// This test will ensure that the decline letter cannot be sent if the case has not yet been declined.
        /// </summary>
        [Test, Description(@"This test will ensure that the decline letter cannot be sent if the case has not yet been declined.")]
        public void SendDeclineLetterCaseNotDeclined()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            //perform the action
            WorkflowHelper.NegotiateProposal(base.Browser, base.TestCase.AccountKey, base.TestCase.InstanceID);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendDeclineLetter);
            base.View.AssertNotification("Decline Reasons must be captured in order for this document to be sent.");
        }

        /// <summary>
        /// If the selected decline reason is Court Order With Appeal then the decline letter is not allowed to be sent
        /// </summary>
        [Test, Description("If the selected decline reason is Court Order With Appeal then the decline letter is not allowed to be sent")]
        public void SendDeclineLetterCourtOrderWithAppeal()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //we need a proposal
            Service<IProposalService>().InsertProposal(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, 5, TestUsers.DebtCounsellingConsultant, 1, 1);
            WorkflowHelper.SendProposalForApprovalAtManageProposal(base.Browser, base.TestCase.AccountKey, TestUsers.DebtCounsellingSupervisor, base.TestCase.InstanceID, base.TestCase.DebtCounsellingKey);
            //supervisor declines with Court order with Appeal
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.DeclineProposalCourtOrderWithAppeal, base.TestCase.DebtCounsellingKey);
            //should not be able to send decline letter.
            base.LoadCase(WorkflowStates.DebtCounsellingWF.ManageProposal);
            base.View.AssertValidationMessageExists("Proposal has been declined with the Court Order with Appeal reason.");
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendDeclineLetter);
            base.View.AssertNotification("The Debt Counselling Case has a Court Order with Appeal decline reason.");
        }

        /// <summary>
        /// Once a case has been consultant declined the decline letter should be allowed to be sent.
        /// </summary>
        [Test, Description("Once a case has been consultant declined the decline letter should be allowed to be sent.")]
        public void SendDeclineLetterConsultantDecline()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //we need a proposal
            Service<IProposalService>().InsertProposal(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft, 5, TestUsers.DebtCounsellingConsultant, 1, 1);
            int proposalKey = Service<IProposalService>().GetProposalKeyByStatusAndAcceptedStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft,
                    ProposalAcceptedEnum.False, ProposalTypeEnum.Proposal);
            //get a decline reason
            var reasonDescriptions = Service<IReasonService>().GetReasonDescriptionsByReasonType(ReasonType.ConsultantDeclined, true);
            string reasonDescription = (from r in reasonDescriptions select r.Value).FirstOrDefault();
            //decline the case
            WorkflowHelper.ConsultantDecline(base.Browser, base.TestCase.AccountKey, DebtCounsellingProposalStatus.Draft, reasonDescription, base.TestCase.DebtCounsellingKey, base.TestCase.InstanceID,
                proposalKey);
            //perform the action
            SendDeclineLetter();
            //case remains at state
            X2Assertions.AssertCurrentX2State(base.TestCase.InstanceID, WorkflowStates.DebtCounsellingWF.ManageProposal);
        }

        /// <summary>
        /// Once a case has been declined by the supervisor at Decision on Proposal the decline letter should be allowed to be sent.
        /// </summary>
        [Test, Description("Once a case has been declined by the supervisor at Decision on Proposal the decline letter should be allowed to be sent.")]
        public void SendDeclineLetterSupervisorDecline()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //we need a proposal
            Service<IProposalService>().InsertProposal(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, 5, TestUsers.DebtCounsellingConsultant, 1, 1);
            WorkflowHelper.SendProposalForApprovalAtManageProposal(base.Browser, base.TestCase.AccountKey, TestUsers.DebtCounsellingSupervisor, base.TestCase.InstanceID, base.TestCase.DebtCounsellingKey);
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.DeclineProposal, base.TestCase.DebtCounsellingKey);
            base.LoadCase(WorkflowStates.DebtCounsellingWF.ManageProposal);
            //perform the action
            SendDeclineLetter();
            //case remains at state
            X2Assertions.AssertCurrentX2State(base.TestCase.InstanceID, WorkflowStates.DebtCounsellingWF.ManageProposal);
        }

        #endregion CommonSendDeclineLetter

        private void SendDeclineLetter()
        {
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendDeclineLetter);
            //get the debt counsellor
            var externalRole = Service<IExternalRoleService>().GetFirstActiveExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                ExternalRoleTypeEnum.DebtCounsellor);
            //send correspondence
            base.Browser.Page<CorrespondenceProcessingMultipleWorkflowDebtCounsellor>().SelectCorrespondenceRecipient(externalRole.LegalEntityKey);
            //check the document has been sent
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.TestCase.DebtCounsellingKey, CorrespondenceReports.DebtCounsellingDeclineLetter,
                 CorrespondenceMedium.Post);
            CorrespondenceAssertions.AssertImageIndex(base.TestCase.DebtCounsellingKey.ToString(), CorrespondenceReports.DebtCounsellingDeclineLetter,
                CorrespondenceMedium.Post, base.TestCase.AccountKey, base.TestCase.DebtCounsellingKey);
            //check transition
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_SendDeclineLetter);
        }
    }
}