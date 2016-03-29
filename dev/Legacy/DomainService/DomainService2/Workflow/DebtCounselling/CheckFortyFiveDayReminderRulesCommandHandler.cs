namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckFortyFiveDayReminderRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckFortyFiveDayReminderRulesCommand>
    {
        public CheckFortyFiveDayReminderRulesCommandHandler(ICommandHandler commandHandler)
            : base(commandHandler,true,true)
        {
        }
    }
}