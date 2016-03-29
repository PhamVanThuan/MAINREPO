using System;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// This is used to update a rule parameter value for testing purposes
        /// </summary>
        /// <param name="RuleName">Rule Name</param>
        /// <param name="RuleParameter">Rule SqlParameter</param>
        /// <param name="NewValue">New SqlParameter Value</param>
        public void UpdateRuleParameter(string RuleName, string RuleParameter, string NewValue)
        {
            string query =
                String.Format(
                            @"update rp
                            set rp.Value = '{0}'
                            from [2am].dbo.ruleItem r with (nolock)
                            join [2am].dbo.ruleParameter rp with (nolock) on r.ruleItemKey=rp.ruleItemKey
                            where r.name = '{1}'
                            and rp.name = '{2}'", NewValue, RuleName, RuleParameter);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteSQLQuery(statement);
        }
    }
}