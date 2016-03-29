using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Specs.RuleSpecs.Fakes;
using System;

namespace SAHL.Core.Specs.RuleSpecs
{
    public class when_registering_a_rule_for_a_context_that_already_exists : WithFakes
    {
        private static IDomainRuleManager<FakeRuleModel> domainRuleManager;
        private static Exception exception;

        private Establish context = () =>
        {
            domainRuleManager = new DomainRuleManager<FakeRuleModel>();
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() =>
            {
                domainRuleManager.RegisterRuleForContext(new FakeDomainRule(null), 2);
                domainRuleManager.RegisterRuleForContext(new FakeDomainRule(null), 2);
            });
        };

        private It should_throw_an_argument_execption = () =>
        {
            exception.ShouldBeOfExactType<ArgumentException>();
        };

        private It should_indicate_the_rule_argument_name = () =>
        {
            ((ArgumentException)exception).ParamName.ShouldEqual("rule");
        };

        private It should_indicate_that_the_rule_has_already_been_registered_for_the_context = () =>
        {
            exception.Message.ToLower().ShouldContain("has already been registered");
        };
    }
}