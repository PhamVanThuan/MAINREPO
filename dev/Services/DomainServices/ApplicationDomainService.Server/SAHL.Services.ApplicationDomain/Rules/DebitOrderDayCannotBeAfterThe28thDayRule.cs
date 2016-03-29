using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class DebitOrderDayCannotBeAfterThe28thDayRule : IDomainRule<ApplicationDebitOrderModel>
    {
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ApplicationDebitOrderModel ruleModel)
        {
            if (ruleModel.DebitOrderDay < 1 || ruleModel.DebitOrderDay > 28)
            {
                messages.AddMessage(new SystemMessage("Debit Order Day must be between 1 and 28 days.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}