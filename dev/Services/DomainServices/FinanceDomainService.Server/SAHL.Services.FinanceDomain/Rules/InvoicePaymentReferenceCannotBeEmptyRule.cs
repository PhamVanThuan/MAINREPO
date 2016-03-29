using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Rules
{
    public class InvoicePaymentReferenceCannotBeEmptyRule : IDomainRule<ThirdPartyInvoiceModel>
    {
        public void ExecuteRule(ISystemMessageCollection messages, ThirdPartyInvoiceModel ruleModel)
        {
            if (string.IsNullOrWhiteSpace(ruleModel.PaymentReference))
            {
                messages.AddMessage(new SystemMessage("Invoice payment reference must be captured.", SystemMessageSeverityEnum.Error));
                return;
            }
        }
    }
}
