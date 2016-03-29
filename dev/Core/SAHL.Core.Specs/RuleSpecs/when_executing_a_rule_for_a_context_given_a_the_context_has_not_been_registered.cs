using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Specs.RuleSpecs.Fakes;
using SAHL.Core.SystemMessages;
using System;

namespace SAHL.Core.Specs.RuleSpecs
{
    public class when_executing_a_rule_for_a_context_given_a_the_context_has_not_been_registered : WithFakes
    {
        private static IDomainRuleManager<FakeRuleModel> domainRuleManager;
        private static Exception exception;

        private Establish context = () =>
        {
            domainRuleManager = new DomainRuleManager<FakeRuleModel>();
            domainRuleManager.RegisterRuleForContext<int>(new FakeDomainRule(null), 2);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => domainRuleManager.ExecuteRulesForContext(new SystemMessageCollection(), new FakeRuleModel("fdgfd"), 3));
        };

        private It should_throw_an_argument_execption = () =>
        {
            exception.ShouldBeOfExactType<ArgumentException>();
        };

        private It should_indicate_the_argument_name = () =>
        {
            ((ArgumentException)exception).ParamName.ShouldEqual("context");
        };

        private It should_indicate_that_the_context_already_exists = () =>
        {
            exception.Message.ToLower().ShouldContain("does not exist");
        };
    }
}