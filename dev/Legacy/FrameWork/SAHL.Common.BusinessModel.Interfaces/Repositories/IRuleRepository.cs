using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IRuleRepository
    {
        /// <summary>
        /// Takes the name of a rule and returns an IRule that exists in the DB
        /// </summary>
        /// <param name="RuleName">The name of the rule to find.</param>
        /// <returns>The IRule or NULL if it cant be found.</returns>
        IRuleItem FindRuleItemByName(string RuleName);

        /// <summary>
        /// Take the typename of a rule and returns the IRule
        /// </summary>
        /// <param name="RuleName"></param>
        /// <returns></returns>
        IRuleItem FindRuleItemByTypeName(string RuleName);

        /// <summary>
        /// Takes in a partial or complete rule name and returns a list of rules that match the search criteria.
        /// </summary>
        /// <param name="PartialRuleName">The name of the rule to search for</param>
        /// <returns></returns>
        IEventList<IRuleItem> FindRulesByPartialName(string PartialRuleName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="RuleItemKey">The RuleItemKey of the rule you want to lookup.</param>
        /// <returns>An IRuleItem</returns>
        IRuleItem FindRuleByKey(int RuleItemKey);

        /// <summary>
        /// Takes in a RuleItemKey and returns the parametres for that rule.
        /// </summary>
        /// <param name="RuleItemKey">RuleItem Key</param>
        /// <returns></returns>
        IEventList<IRuleParameter> GetRuleParameterByRuleKey(int RuleItemKey);

        /// <summary>
        /// Creates an empty RuleItem to be used when creating a new ruleItem
        /// </summary>
        /// <returns></returns>
        IRuleItem CreateEmptyRuleItem();

        /// <summary>
        /// Saves a RuleItem
        /// </summary>
        /// <param name="RuleToSave"></param>
        void SaveRuleItem(IRuleItem RuleToSave);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IRuleParameter CreateEmptyRuleParameter();

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IRuleParameter FindParameterByKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Param"></param>
        void SaveRuleParameter(IRuleParameter Param);

        IEventList<IRuleItem> GetAllRules();

        IEventList<IAllocationMandateSetGroup> GetAllocationMandatesForOrgStructureKeys(List<int> Keys);

        /// <summary>
        ///
        /// </summary>
        /// <param name="RuleSetKey"></param>
        /// <returns></returns>
        IWorkflowRuleSet GetRuleSetForKey(int RuleSetKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        IWorkflowRuleSet GetRuleSetByName(string Name);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="WFRuleSet"></param>
        void SaveRuleSet(IWorkflowRuleSet WFRuleSet);
    }
}