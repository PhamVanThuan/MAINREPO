using SAHL.Core.Rules;
using System;

namespace SAHL.Core.Specs.RuleSpecs.Fakes
{
    public class FakeDomainRule : IDomainRule<FakeRuleModel>
    {
        private Action action;

        public FakeDomainRule(Action action)
        {
            this.action = action;
        }

        public void ExecuteRule(SystemMessages.ISystemMessageCollection messages, FakeRuleModel ruleModel)
        {
            this.action();
        }
    }
}