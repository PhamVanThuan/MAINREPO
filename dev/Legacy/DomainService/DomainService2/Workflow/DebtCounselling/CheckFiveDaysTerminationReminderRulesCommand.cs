namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckFiveDaysTerminationReminderRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckFiveDaysTerminationReminderRulesCommand(int debtCounsellingKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.DebtCounselling5DayTerminationReminder)
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