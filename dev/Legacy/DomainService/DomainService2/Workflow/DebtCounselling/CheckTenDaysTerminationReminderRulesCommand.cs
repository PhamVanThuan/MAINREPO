namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckTenDaysTerminationReminderRulesCommand : RuleSetDomainServiceCommand
    {
        public int DebtCounsellingKey { get; set; }

        public CheckTenDaysTerminationReminderRulesCommand(int debtCounsellingKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.DebtCounselling10DayTerminationReminder)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }
    }
}