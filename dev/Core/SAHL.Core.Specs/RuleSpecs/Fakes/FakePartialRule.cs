using SAHL.Core.Rules;
using System;

namespace SAHL.Core.Specs.RuleSpecs.Fakes
{
    public class FakePartialRule : IDomainRule<FakeRuleModel>
    {
        private Action action;

        public FakePartialRule(Action action)
        {
            this.action = action;
        }

        public void ExecuteRule(SystemMessages.ISystemMessageCollection messages, FakeRuleModel ruleModel)
        {
            this.action();
        }
    }
}