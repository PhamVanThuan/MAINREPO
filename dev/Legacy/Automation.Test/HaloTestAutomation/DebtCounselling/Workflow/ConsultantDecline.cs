using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public class ConsultantDeclineTests : DebtCounsellingTests.TestBase<CommonReasonCommonDecline>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        /// This test performs the Consultant Decline action on a draft proposal. It ensures that the case remains at the Manage Proposal state, while the expected
        /// stage transition and reason is written against the case and proposal.
        /// </summary>
        [Test, Description(@"This test performs the Consultant Decline action on a draft proposal. It ensures that the case remains at the Manage Proposal state, while the
		expected stage transition and reason is written against the case and proposal.")]
        public void ConsultantDeclineDraftProposal()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            Service<IProposalService>().InsertProposal(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft, 4, base.TestCase.AssignedUser, 0, 0);
            var reasonDescriptions = Service<IReasonService>().GetReasonDescriptionsByReasonType(ReasonType.ConsultantDeclined, true);
            string reasonDescription = String.Empty;
            foreach (KeyValuePair<int, string> description in reasonDescriptions)
            {
                reasonDescription = description.Value;
                break;
            }
            int proposalKey = Service<IProposalService>().GetProposalKeyByStatusAndAcceptedStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft, ProposalAcceptedEnum.False,
                ProposalTypeEnum.Proposal);
            WorkflowHelper.ConsultantDecline(base.Browser, base.TestCase.AccountKey, DebtCounsellingProposalStatus.Draft, reasonDescription, base.TestCase.DebtCounsellingKey, base.TestCase.InstanceID, proposalKey);
        }

        /// <summary>
        /// This test performs the Consultant Decline action on an active proposal. It ensures that the case remains at the Manage Proposal state, while the expected
        /// stage transition and reason is written against the case and proposal.
        /// </summary>
        [Test, Description(@"This test performs the Consultant Decline action on an active proposal. It ensures that the case remains at the Manage Proposal state, while
		the expected stage transition and reason is written against the case and proposal.")]
        public void ConsultantDeclineActiveProposal()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            Service<IProposalService>().InsertProposal(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, 4, base.TestCase.AssignedUser, 0, 0);
            var reasonDescriptions = Service<IReasonService>().GetReasonDescriptionsByReasonType(ReasonType.ConsultantDeclined, true);
            string reasonDescription = String.Empty;
            foreach (KeyValuePair<int, string> description in reasonDescriptions)
            {
                reasonDescription = description.Value;
                break;
            }
            int proposalKey = Service<IProposalService>().GetProposalKeyByStatusAndAcceptedStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, ProposalAcceptedEnum.False,
                ProposalTypeEnum.Proposal);
            WorkflowHelper.ConsultantDecline(base.Browser, base.TestCase.AccountKey, DebtCounsellingProposalStatus.Active, reasonDescription, base.TestCase.DebtCounsellingKey, base.TestCase.InstanceID, proposalKey);
        }

        /// <summary>
        /// This test ensures that it is mandatory to select a proposal to decline before capturing the required reasons.
        /// </summary>
        [Test, Description("This test ensures that it is mandatory to select a proposal to decline before capturing the required reasons.")]
        public void ConsultantDeclineValidation()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            Service<IProposalService>().InsertProposal(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, 4, base.TestCase.AssignedUser, 0, 0);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.ConsultantDecline);
            base.Browser.Page<ConsultantDecline>().ClickSubmit();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select a proposal.");
        }
    }
}