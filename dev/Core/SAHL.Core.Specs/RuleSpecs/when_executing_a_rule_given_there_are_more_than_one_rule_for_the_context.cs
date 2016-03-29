using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Specs.RuleSpecs.Fakes;
using SAHL.Core.SystemMessages;

namespace SAHL.Core.Specs.RuleSpecs
{
    public class when_executing_a_rule_given_there_are_more_than_one_rule_for_the_context : WithFakes
    {
        private static IDomainRuleManager<FakeRuleModel> domainRuleManager;
        private static ISystemMessageCollection messages;
        private static int executionCount;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            executionCount = 0;

            domainRuleManager = new DomainRuleManager<FakeRuleModel>();
            domainRuleManager.RegisterRuleForContext(new FakeDomainRule(() => executionCount++), 2);
            domainRuleManager.RegisterRuleForContext(new FakeDomainRule2(() => executionCount++), 2);
        };

        private Because of = () =>
        {
            domainRuleManager.ExecuteRulesForContext(messages, new FakeRuleModel("sfdf"), 2);
        };

        private It should_execute_all_the_rules = () =>
        {
            executionCount.ShouldEqual(2);
        };
    }
}