using Machine.Fakes;

namespace DomainService2.Specs
{
    public class RuleDomainServiceSpec<T, V> : DomainServiceSpec<T, V>
        where T : RuleDomainServiceCommand
        where V : RuleDomainServiceCommandHandler<T>
    {
        protected static bool ExpectedRuleResult;

        public RuleDomainServiceSpec()
            : base()
        {
            commandHandler = An<ICommandHandler>();
            commandHandler.WhenToldTo(x => x.CheckRule(messages, command))
                .Return(ExpectedRuleResult);
        }
    }
}