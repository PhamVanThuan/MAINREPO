using SAHL.Core.Rules;
using System;

namespace SAHL.Core.Specs.RuleSpecs.Fakes
{
    public class FakeDomainRule3 : IDomainRule<FakeRuleModel2>
    {
        private Action action;

        public FakeDomainRule3(Action action)
        {
            this.action = action;
        }

        public void ExecuteRule(SystemMessages.ISystemMessageCollection messages, FakeRuleModel2 ruleModel)
        {
            this.action();
        }
    }
}