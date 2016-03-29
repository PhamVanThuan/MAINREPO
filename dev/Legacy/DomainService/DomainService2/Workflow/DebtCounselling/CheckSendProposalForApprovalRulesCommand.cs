namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckSendProposalForApprovalRulesCommand : RuleSetDomainServiceCommand
    {
        public int DebtCounsellingKey { get; set; }

        public CheckSendProposalForApprovalRulesCommand(int debtCounsellingKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.DebtCounsellingApproveProposal)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }
    }
}