namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckNoDateNoPayRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckNoDateNoPayRulesCommand>
    {
        public CheckNoDateNoPayRulesCommandHandler(ICommandHandler commandHandler)
            : base(commandHandler)
        {
        }
    }
}