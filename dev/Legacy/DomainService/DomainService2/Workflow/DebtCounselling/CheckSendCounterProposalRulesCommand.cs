namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckSendCounterProposalRulesCommand : RuleSetDomainServiceCommand
    {
        public int DebtCounsellingKey { get; set; }

        public CheckSendCounterProposalRulesCommand(int debtCounsellingKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.DebtCounsellingSendCounterProposal)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }
    }
}