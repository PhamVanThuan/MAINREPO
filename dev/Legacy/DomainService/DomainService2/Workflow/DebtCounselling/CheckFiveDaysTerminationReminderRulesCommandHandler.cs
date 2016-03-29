namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckFiveDaysTerminationReminderRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckFiveDaysTerminationReminderRulesCommand>
    {
        public CheckFiveDaysTerminationReminderRulesCommandHandler(ICommandHandler commandHandler)
            : base(commandHandler)
        {
        }
    }
}