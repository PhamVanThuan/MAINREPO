namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckNoDateNoPayRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckNoDateNoPayRulesCommand(int debtCounsellingKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.DebtCounsellingCheckNoDateNoPay)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }

        public int DebtCounsellingKey
        {
            get;
            protected set;
        }

        public override object[] RuleParameters
        {
            get { return new object[] { this.DebtCounsellingKey }; }
        }
    }
}