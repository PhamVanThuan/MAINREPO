namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckFortyFiveDayReminderRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckFortyFiveDayReminderRulesCommand(int debtCounsellingKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.DebtCounselling45DayReminder)
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