namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckPaymentReceivedRulesCommand : RuleSetDomainServiceCommand
    {
        public int DebtCounsellingKey { get; set; }

        public CheckPaymentReceivedRulesCommand(int debtCounsellingKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.DebtCounsellingPaymentReceived)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }
    }
}