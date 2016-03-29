namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckAcceptProposalRulesCommand : RuleSetDomainServiceCommand
    {
        public int DebtCounsellingKey { get; set; }

        public CheckAcceptProposalRulesCommand(int debtCounsellingKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.DebtCounsellingAcceptProposal)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }
    }
}