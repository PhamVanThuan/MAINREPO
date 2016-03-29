using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public class DeclineProposalTests : DebtCounsellingTests.TestBase<CommonReasonCommonDecline>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingSupervisor);
        }

        /// <summary>
        /// Test the 'Decline' action
        /// 1 Assert that a Reason record is created with a comment
        /// 2 Assert the case is assigned to the last active Debt Counselling Consultant user associated with the case
        /// 2.1 Check the [2AM]..WorkflowRole records
        /// 2.2 Check the X2.x2.WorkflowRoleAssignment and X2.x2.Worklist records
        /// 3 Assert StageTransition created
        /// </summary>
        [Test, Description("This test will decline a case at the Decision on Proposal case.")]
        public void DeclineAtDecisionOnProposal()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.DecisiononProposal, TestUsers.DebtCounsellingSupervisor);
            //Get a dictionary containing the reason and the keys provided the reasontype
            Dictionary<int, string> reasonDescriptions = Service<IReasonService>().GetReasonDescriptionsByReasonType(ReasonType.ProposalDeclined, true);
            string reasonDescription = String.Empty;
            reasonDescription = (from r in reasonDescriptions where r.Value != ReasonDescription.CourtOrderwithAppeal select r.Value).FirstOrDefault();
            int proposalKey = Service<IProposalService>().GetProposalKeyByStatusAndAcceptedStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, ProposalAcceptedEnum.False,
                ProposalTypeEnum.Proposal);
            //Cater for bad migration data
            if (proposalKey == 0)
            {
                proposalKey = Service<IProposalService>().GetProposalKeyByStatusAndAcceptedStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, ProposalAcceptedEnum.True,
                ProposalTypeEnum.Proposal);
            }
            //Cater for complete garbage
            if (proposalKey == 0)
            {
                Service<IProposalService>().InsertActiveProposal(base.TestCase.DebtCounsellingKey, 5, TestUsers.DebtCounsellingSupervisor, 1, 1, 0);
                proposalKey = Service<IProposalService>().GetProposalKeyByStatusAndAcceptedStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, ProposalAcceptedEnum.False,
                    ProposalTypeEnum.Proposal);
            }
            //Perform decline
            DeclineProposal(reasonDescription, proposalKey);
        }

        private void DeclineProposal(string reasonDescription, int proposalKey)
        {
            string date = DateTime.Now.ToString(Formats.DateTimeFormatSQL);
            string DCCUser = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, Workflows.DebtCounselling,
                    DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, base.TestCase.DebtCounsellingKey);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.Decline);
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAddCommentAndSubmit(ReasonType.ProposalDeclined, reasonDescription, "Comment",
                ButtonTypeEnum.Submit);
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.DebtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.ManageProposal);
            //Assert Reasons added
            ReasonAssertions.AssertReason(reasonDescription, ReasonType.ProposalDeclined, proposalKey, GenericKeyTypeEnum.Proposal_ProposalKey, true);
            //Assert WorkflowRoleAssignment
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, DCCUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, true, true);
            //Assert StageTransition created
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_DeclineProposal);
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(base.TestCase.DebtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, DCCUser);
        }
    }
}