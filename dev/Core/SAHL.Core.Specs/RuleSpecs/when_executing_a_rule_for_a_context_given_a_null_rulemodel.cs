using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Specs.RuleSpecs.Fakes;
using SAHL.Core.SystemMessages;
using System;

namespace SAHL.Core.Specs.RuleSpecs
{
    public class when_executing_a_rule_for_a_context_given_a_null_rulemodel : WithFakes
    {
        private static IDomainRuleManager<FakeRuleModel> domainRuleManager;
        private static Exception exception;

        private Establish context = () =>
        {
            domainRuleManager = new DomainRuleManager<FakeRuleModel>();
            domainRuleManager.RegisterRuleForContext(new FakeDomainRule(null), 2);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => domainRuleManager.ExecuteRulesForContext(new SystemMessageCollection(), null, 2));
        };

        private It should_throw_an_argument_null_execption = () =>
        {
            exception.ShouldBeOfExactType<ArgumentNullException>();
        };

        private It should_indicate_the_null_argument_name = () =>
        {
            ((ArgumentNullException)exception).ParamName.ShouldEqual("ruleModel");
        };
    }
}