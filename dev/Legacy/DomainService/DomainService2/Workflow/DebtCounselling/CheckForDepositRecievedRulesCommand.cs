namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckForDepositRecievedRulesCommand : RuleDomainServiceCommand
    {
        public CheckForDepositRecievedRulesCommand(int debtDounsellingKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.DebtCounsellingDepositCheck)
        {
            this.DebtCounsellingKey = debtDounsellingKey;
        }

        public int DebtCounsellingKey
        {
            get;
            protected set;
        }

        public object[] RuleParameters
        {
            get { return new object[] { this.DebtCounsellingKey }; }
        }
    }
}