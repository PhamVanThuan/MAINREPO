using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Models;
using System;

namespace SAHL.Shared.BusinessModel.Rules
{
    public class EffectiveDateCannotBeInTheFutureRule : IDomainRule<TransactionRuleModel>
    {
        public void ExecuteRule(ISystemMessageCollection messages, TransactionRuleModel ruleModel)
        {
            if (ruleModel.EffectiveDate.Date > DateTime.Now.Date)
            {
                var message = "The Effective Date cannot be in the future";
                messages.AddMessage(new SystemMessage(message, SystemMessageSeverityEnum.Error));
            }
        }
    }
}