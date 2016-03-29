using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Rules
{
    public class InvoiceMustHaveAtLeastOneLineItemRule : IDomainRule<ThirdPartyInvoiceModel>
    {
        public void ExecuteRule(ISystemMessageCollection messages, ThirdPartyInvoiceModel ruleModel)
        {
            if (!ruleModel.LineItems.Any())
            {
                messages.AddMessage(new SystemMessage("The Invoice does not contain any line items.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}