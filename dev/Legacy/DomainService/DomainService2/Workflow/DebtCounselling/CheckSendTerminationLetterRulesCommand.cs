namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckSendTerminationLetterRulesCommand : RuleSetDomainServiceCommand
    {
        public int DebtCounsellingKey { get; set; }

        public CheckSendTerminationLetterRulesCommand(int debtCounsellingKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.DebtCounsellingSendTerminationLetter)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }
    }
}