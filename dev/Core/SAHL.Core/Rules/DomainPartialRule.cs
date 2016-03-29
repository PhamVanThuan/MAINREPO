using System;

namespace SAHL.Core.Rules
{
    public interface IDomainPartialRule
    {
        Type GetRuleType();
    }

    public class DomainPartialRule<T, U> : IDomainRule<T>, IDomainPartialRule
        where T : class
        where U : class
    {
        public IDomainRule<U> PartialRule { get; protected set; }

        public DomainPartialRule(IDomainRule<U> partialRule)
        {
            this.PartialRule = partialRule;
        }

        public void ExecuteRule(SystemMessages.ISystemMessageCollection messages, T ruleModel)
        {
            this.PartialRule.ExecuteRule(messages, ruleModel as U);
        }

        public Type GetRuleType()
        {
            return this.PartialRule.GetType();
        }
    }
}