using SAHL.Core.Rules;
using System;

namespace SAHL.Core.Specs.RuleSpecs.Fakes
{
    public class FakePartialDomainRule : IDomainRule<IFakePartialRuleItem>
    {
        private Action action;

        public FakePartialDomainRule(Action action)
        {
            this.action = action;
        }

        public void ExecuteRule(SystemMessages.ISystemMessageCollection messages, IFakePartialRuleItem ruleModel)
        {
            this.action();
        }
    }
}