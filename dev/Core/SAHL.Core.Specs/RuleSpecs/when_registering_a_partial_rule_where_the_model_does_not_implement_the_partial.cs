using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Specs.RuleSpecs.Fakes;
using System;

namespace SAHL.Core.Specs.RuleSpecs
{
    public class when_registering_a_partial_rule_where_the_model_does_not_implement_the_partial : WithFakes
    {
        private static IDomainRuleManager<FakeRuleModel2> domainRuleManager;
        private static Exception exception;

        private Establish context = () =>
        {
            domainRuleManager = new DomainRuleManager<FakeRuleModel2>();
            domainRuleManager.RegisterRule(new FakeDomainRule3(null));
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => domainRuleManager.RegisterPartialRule<IFakePartialRuleItem>(new FakePartialDomainRule(null)));
        };

        private It should_throw_an_argument_null_execption = () =>
        {
            exception.ShouldBeOfExactType<ArgumentException>();
        };

        It should_set_the_correct_error_message = () =>
        {
            exception.Message.ShouldStartWith("Partial rules must be implemented as an interface on the rule model");
        };
    }
}