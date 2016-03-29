using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Specs.RuleSpecs.Fakes;
using System;

namespace SAHL.Core.Specs.RuleSpecs
{
    public class when_registering_a_new_rule_without_context_that_already_exists : WithFakes
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
                domainRuleManager.RegisterRule(new FakeDomainRule(null));
                domainRuleManager.RegisterRule(new FakeDomainRule2(null));
            });
        };

        private It should_not_throw_an_argument_execption = () =>
        {
            exception.ShouldBeNull();
        };
    }
}