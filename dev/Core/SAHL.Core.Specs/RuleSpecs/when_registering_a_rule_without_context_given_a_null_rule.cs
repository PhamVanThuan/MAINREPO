using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Specs.RuleSpecs.Fakes;
using System;

namespace SAHL.Core.Specs.RuleSpecs
{
    public class when_registering_a_rule_without_context_given_a_null_rule : WithFakes
    {
        private static IDomainRuleManager<FakeRuleModel> domainRuleManager;
        private static Exception exception;

        private Establish context = () =>
        {
            domainRuleManager = new DomainRuleManager<FakeRuleModel>();
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => domainRuleManager.RegisterRule(null));
        };

        private It should_throw_an_argument_null_execption = () =>
        {
            exception.ShouldBeOfExactType<ArgumentNullException>();
        };
    }
}