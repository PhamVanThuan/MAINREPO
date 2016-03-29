using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Specs.RuleSpecs.Fakes;
using System;

namespace SAHL.Core.Specs.RuleSpecs
{
    public class when_registering_a_partial_rule_with_partial_that_is_not_an_interface : WithFakes
    {
        private static IDomainRuleManager<FakeRuleModel> domainRuleManager;
        private static Exception exception;

        private Establish context = () =>
        {
            domainRuleManager = new DomainRuleManager<FakeRuleModel>();
            domainRuleManager.RegisterRule(new FakeDomainRule(null));
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => domainRuleManager.RegisterPartialRule(new FakeBrokenPartialDomainRule()));
        };

        private It should_throw_an_argument_null_execption = () =>
        {
            exception.ShouldBeOfExactType<ArgumentException>();
        };

        It should_set_the_correct_error_message = () =>
            {
                exception.Message.ShouldStartWith("Partial rules must be defined as an interface");
            };
    }
}