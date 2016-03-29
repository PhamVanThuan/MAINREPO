namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckAttorneyToOpposeRulesCommand : RuleSetDomainServiceCommand
    {
        public int DebtCounsellingKey { get; set; }

        public CheckAttorneyToOpposeRulesCommand(int debtCounsellingKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.DebtCounsellingAttorneyToOppose)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }
    }
}