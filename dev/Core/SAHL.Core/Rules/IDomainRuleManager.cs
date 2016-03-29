using SAHL.Core.SystemMessages;

namespace SAHL.Core.Rules
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T">The domain rule model type, the rules registered for
    /// the context will have to be IDomainRule<T>, restricted to be a class.</typeparam>
    public interface IDomainRuleManager<T>
        where T : class
    {
        /// <summary>
        /// Registers a rule without a context for a model.
        /// </summary>
        /// <param name="rule">The rule to register.</param>
        void RegisterRule(IDomainRule<T> rule);

        /// <summary>
        /// Registers a rule that requires a portion of the domain rule model only.
        /// </summary>
        /// <typeparam name="U">The interface required to be implemented by the rule model</typeparam>
        /// <param name="rule">The rule that operates on the interface portion of the rule model.</param>
        void RegisterPartialRule<U>(IDomainRule<U> rule) where U : class;

        /// <summary>
        /// Registers a rule for a given context, you can register more than one rule for a model to a context.
        /// However you cannot add the same rule for a given model more than once to a context.
        /// </summary>
        /// <param name="rule">The rule to register.</param>
        /// <param name="context">The context to register the rule against.</param>
        /// <typeparam name="V">The context type, usually int or string, restricted to be a value type.</typeparam>
        void RegisterRuleForContext<V>(IDomainRule<T> rule, V context) where V : struct;

        /// <summary>
        /// Executes one or more rules that have been registered for a model.
        /// </summary>
        /// <param name="messages">The system message collection to add rule errors or warning to.</param>
        /// <param name="ruleModel">The model that the rule will operate on.</param>
        void ExecuteRules(ISystemMessageCollection messages, T ruleModel);

        /// <summary>
        /// Executes one or more rules that have been registered against the context.
        /// </summary>
        /// <param name="messages">The system message collection to add rule errors or warning to.</param>
        /// <param name="ruleModel">The model that the rule will operate on.</param>
        /// <param name="context">The context under which the rule will execute. Different rules may execute for different contexts.</param>
        /// <typeparam name="V">The context type, usually int or an enum, restricted to be a value type.</typeparam>
        void ExecuteRulesForContext<V>(ISystemMessageCollection messages, T ruleModel, V context) where V : struct;
    }
}