using Machine.Fakes;

namespace DomainService2.Specs
{
    public class RuleSetDomainServiceSpec<T, V> : DomainServiceSpec<T, V>
        where T : RuleSetDomainServiceCommand
        where V : RuleSetDomainServiceCommandHandler<T>
    {
        protected static bool ExpectedRuleResult;

        public RuleSetDomainServiceSpec()
            : base()
        {
            commandHandler = An<ICommandHandler>();
            commandHandler.WhenToldTo(x => x.CheckRules(messages, command))
                .Return(ExpectedRuleResult);
        }
    }
}