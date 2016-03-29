using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Rules
{
    /// <summary>
    /// Domain Rule Context class
    /// </summary>
    /// <typeparam name="T">Rule Model Type</typeparam>
    public class DomainRuleManager<T> : IDomainRuleManager<T>
        where T : class
    {
        protected internal Dictionary<Type, Dictionary<object, List<IDomainRule<T>>>> rulesWithContext;
        protected internal Dictionary<Type, List<IDomainRule<T>>> rulesWithoutContext;

        public DomainRuleManager()
        {
            this.rulesWithContext = new Dictionary<Type, Dictionary<object, List<IDomainRule<T>>>>();
            this.rulesWithoutContext = new Dictionary<Type, List<IDomainRule<T>>>();
        }

        /// <summary>
        /// Registers a rule without a context for a model.
        /// </summary>
        /// <param name="rule">The rule to register.</param>
        public void RegisterRule(IDomainRule<T> rule)
        {
            if (rule == null)
            {
                throw new ArgumentNullException("rule");
            }

            this.AddRuleWithoutContext(rule);
        }

        public void RegisterPartialRule<U>(IDomainRule<U> rule) where U : class
        {
            if (!typeof(U).IsInterface)
            {
                throw new ArgumentException(string.Format("Partial rules must be defined as an interface, {0} is not an interface", typeof(U).FullName));
            }

            if (!typeof(U).IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException(string.Format("Partial rules must be implemented as an interface on the rule model, {0} is not implemented by {1}", typeof(U).FullName, typeof(T)));
            }

            IDomainRule<T> partialRule = new DomainPartialRule<T, U>(rule);
            this.AddRuleWithoutContext(partialRule);
        }

        private void AddRuleWithoutContext(IDomainRule<T> rule)
        {
            Type modelType = typeof(T);

            // check if a rule has been registered for this model type before
            if (this.rulesWithoutContext.ContainsKey(modelType))
            {
                // if the modeltype exists check if the rule has been added before
                var ruleType = GetRuleType(rule);

                bool ruleExists = this.rulesWithoutContext[modelType].Any(existingRule => ruleType == GetRuleType(existingRule));

                if (ruleExists)
                {
                    throw new ArgumentException(string.Format("Rule of type {0} has already been registered without a context", rule.GetType()), "rule");
                }
            }
            else
            {
                this.rulesWithoutContext.Add(modelType, new List<IDomainRule<T>>());
            }

            this.rulesWithoutContext[modelType].Add(rule);
        }

        private Type GetRuleType(IDomainRule<T> ruleToGetTypeFrom)
        {
            var domainPartialRule = ruleToGetTypeFrom as IDomainPartialRule;
            return domainPartialRule != null ? domainPartialRule.GetRuleType() : ruleToGetTypeFrom.GetType();
        }

        /// <summary>
        /// Registers a rule for a given context, you can register more than one rule for a model to a context.
        /// However you cannot add the same rule for a given model more than once to a context.
        /// </summary>
        /// <param name="rule">The rule to register.</param>
        /// <param name="context">The rule context.</param>
        public void RegisterRuleForContext<V>(IDomainRule<T> rule, V context) where V : struct
        {
            if (rule == null)
            {
                throw new ArgumentNullException("rule");
            }

            Type contextType = typeof(V);
            if (!this.rulesWithContext.ContainsKey(contextType))
            {
                this.rulesWithContext.Add(contextType, new Dictionary<object, List<IDomainRule<T>>>());
            }

            if (!this.rulesWithContext[contextType].ContainsKey(context))
            {
                this.rulesWithContext[contextType].Add(context, new List<IDomainRule<T>>());
            }

            // check if the rule has already ben added for the context
            if (this.rulesWithContext[contextType][context].Any(x => x.GetType() == rule.GetType()))
            {
                throw new ArgumentException(string.Format("Rule of type {0} has already been registered for context {1}", rule.GetType(), context), "rule");
            }

            this.rulesWithContext[contextType][context].Add(rule);
        }

        /// <summary>
        /// Executes one or more rules that have been registered for a model.
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="ruleModel"></param>
        public void ExecuteRules(SystemMessages.ISystemMessageCollection messages, T ruleModel)
        {
            if (messages == null)
            {
                throw new ArgumentNullException("messages");
            }

            if (ruleModel == null)
            {
                throw new ArgumentNullException("ruleModel");
            }

            Type modelType = typeof(T);

            foreach (var rule in this.rulesWithoutContext[modelType])
            {
                rule.ExecuteRule(messages, ruleModel);
            }
        }

        /// <summary>
        /// Executes one or more rules that have been registered for a model.
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="ruleModel"></param>
        /// <param name="context"></param>
        public void ExecuteRulesForContext<V>(SystemMessages.ISystemMessageCollection messages, T ruleModel, V context) where V : struct
        {
            if (messages == null)
            {
                throw new ArgumentNullException("messages");
            }

            if (ruleModel == null)
            {
                throw new ArgumentNullException("ruleModel");
            }

            Type contextType = typeof(V);
            if (!this.rulesWithContext.ContainsKey(contextType))
            {
                throw new ArgumentException(string.Format("Rule context {0} for model {1} does not exist, register the context before attempting to execute the rule", 
                                            context, typeof(T).Name), "context");
            }

            if (!this.rulesWithContext[contextType].ContainsKey(context))
            {
                throw new ArgumentException(string.Format("Rule context {0} for model {1} does not exist, register the context before attempting to execute the rule", 
                                            context, typeof(T).Name), "context");
            }

            foreach (var rule in this.rulesWithContext[contextType][context])
            {
                rule.ExecuteRule(messages, ruleModel);
            }
        }
    }
}