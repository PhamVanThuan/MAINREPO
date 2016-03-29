using BuildingBlocks;
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
    public sealed class SendForApproval : DebtCounsellingTests.TestBase<DebtCounsellingAssignSupervisor>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        /// Test the mandatory fields on the DebtCounsellingAssignSupervisor screen
        /// 1. Assert that the User field is mandatory
        /// </summary>
        [Test, Description("Test the mandatory fields on the DebtCounsellingAssignSupervisor screen")]
        public void DebtCounsellingAssignSupervisorMandatoryFields()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            WorkflowHelper.NegotiateProposal(base.Browser, base.TestCase.AccountKey, base.TestCase.InstanceID);
            int proposalKey = Service<IProposalService>().GetProposalKeyByStatusAndAcceptedStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, ProposalAcceptedEnum.False,
                ProposalTypeEnum.Proposal);
            if (proposalKey == 0)
                Service<IProposalService>().InsertActiveProposal(base.TestCase.DebtCounsellingKey, 5, TestUsers.DebtCounsellingAdmin, 1, 1, 0);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendProposalforApproval);
            base.Browser.Page<DebtCounsellingAssignSupervisor>().ClickButton(ButtonTypeEnum.Submit);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please select a User");
        }

        /// <summary>
        /// Test the 'Send proposal to Manager' action
        /// 1. Assert that the case is moved to the 'Decision on Proposal' X2 state
        /// 2. Assert the case is assigned to 'Debt Counselling Supervisor', 'Debt Counselling Manager' or 'Debt Counselling Director' user selected from the 'User' list
        /// 2.1 Check the [2AM]..WorkflowRole records
        /// 2.2 Check the X2.x2.WorkflowRoleAssignment and X2.x2.Worklist records
        ///
        /// </summary>
        [Test, Description("Test the 'Send proposal to Manager' action")]
        public void SendProposalToManager()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            int proposalKey = Service<IProposalService>().GetProposalKeyByStatusAndAcceptedStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, ProposalAcceptedEnum.False,
                ProposalTypeEnum.Proposal);
            if (proposalKey == 0)
                Service<IProposalService>().InsertActiveProposal(base.TestCase.DebtCounsellingKey, 5, TestUsers.DebtCounsellingAdmin, 1, 1, 0);
            WorkflowHelper.SendProposalForApprovalAtManageProposal(base.Browser, base.TestCase.AccountKey, TestUsers.DebtCounsellingSupervisor, base.TestCase.InstanceID, base.TestCase.DebtCounsellingKey);
        }
    }
}