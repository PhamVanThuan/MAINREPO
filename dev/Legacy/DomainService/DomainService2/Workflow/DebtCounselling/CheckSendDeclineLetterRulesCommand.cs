namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckSendDeclineLetterRulesCommand : RuleSetDomainServiceCommand
    {
        public int DebtCounsellingKey { get; set; }

        public CheckSendDeclineLetterRulesCommand(int debtCounsellingKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.DebtCounsellingProposalDecline)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }
    }
}