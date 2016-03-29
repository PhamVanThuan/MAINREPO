using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Specs.RuleSpecs.Fakes;
using SAHL.Core.SystemMessages;

namespace SAHL.Core.Specs.RuleSpecs
{
    public class when_executing_a_rule_given_there_are_more_than_one_rule_without_context : WithFakes
    {
        private static IDomainRuleManager<FakeRuleModel> domainRuleManager;
        private static ISystemMessageCollection messages;
        private static int executionCount;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            executionCount = 0;

            domainRuleManager = new DomainRuleManager<FakeRuleModel>();
            domainRuleManager.RegisterRule(new FakeDomainRule(() => executionCount++));
            domainRuleManager.RegisterRule(new FakeDomainRule2(() => executionCount++));
        };

        private Because of = () =>
        {
            domainRuleManager.ExecuteRules(messages, new FakeRuleModel("sfdf"));
        };

        private It should_execute_all_the_rules = () =>
        {
            executionCount.ShouldEqual(2);
        };
    }
}