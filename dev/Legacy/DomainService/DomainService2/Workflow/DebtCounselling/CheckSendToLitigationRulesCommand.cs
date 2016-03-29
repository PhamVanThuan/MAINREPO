namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckSendToLitigationRulesCommand : RuleSetDomainServiceCommand
    {
        public int DebtCounsellingKey { get; set; }

        public CheckSendToLitigationRulesCommand(int debtCounsellingKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.DebtCounsellingSendToLitigation)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }
    }
}