using SAHL.Core.Rules;

namespace SAHL.Core.Specs.RuleSpecs.Fakes
{
    public class FakeBrokenPartialDomainRule : IDomainRule<FakeRuleModel2>
    {
        public void ExecuteRule(SystemMessages.ISystemMessageCollection messages, FakeRuleModel2 ruleModel)
        {
            //Fake Broken Partial Domain Rule, empty by design
        }
    }
}