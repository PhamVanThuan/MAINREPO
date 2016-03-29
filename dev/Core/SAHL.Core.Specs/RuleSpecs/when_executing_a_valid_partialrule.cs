using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Specs.RuleSpecs.Fakes;
using SAHL.Core.SystemMessages;
using System;

namespace SAHL.Core.Specs.RuleSpecs
{
    public class when_executing_a_valid_partialrule : WithFakes
    {
        private static IDomainRuleManager<FakeRuleModel> domainRuleManager;
        private static Exception exception;
        private static bool partialWasExecuted = false;

        private Establish context = () =>
        {
            domainRuleManager = new DomainRuleManager<FakeRuleModel>();
            domainRuleManager.RegisterRule(new FakeDomainRule(() => { }));
            domainRuleManager.RegisterPartialRule(new FakePartialDomainRule(() => { partialWasExecuted = true; }));
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => domainRuleManager.ExecuteRules(new SystemMessageCollection(), new FakeRuleModel("hi")));
        };

        private It should_throw_an_argument_null_execption = () =>
        {
            exception.ShouldBeNull();
        };

        private It should_have_executed_the_partial_rule = () =>
        {
            partialWasExecuted.ShouldBeTrue();
        };
    }
}