using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.CATS.Models;
using System.Linq;

namespace SAHL.Services.CATS.Rules
{
    public class APaymentBatchShouldHaveAtleastOnePaymentRule : IDomainRule<PaymentBatch>
    {
        public void ExecuteRule(ISystemMessageCollection messages, PaymentBatch ruleModel)
        {
            if (!ruleModel.Payments.Any())
            {
                messages.AddMessage(new SystemMessage("The payment batch should have at least one payment.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}

