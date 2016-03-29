using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Models;
using System;

namespace SAHL.Shared.BusinessModel.Rules
{
    public class EffectiveDateCannotBePriorToTheFirstOfTheCurrentMonthRule : IDomainRule<TransactionRuleModel>
    {
        public void ExecuteRule(ISystemMessageCollection messages, TransactionRuleModel ruleModel)
        {
            var validDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (ruleModel.EffectiveDate < validDate)
            {
                var message = "Cannot post a transaction with an effective date prior to the 1st of this month.";
                messages.AddMessage(new SystemMessage(message, SystemMessageSeverityEnum.Error));
            }
        }
    }
}