using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;

namespace SAHL.Services.FinanceDomain.Rules
{
    public class InvoiceDateCannotBeInTheFutureRule : IDomainRule<ThirdPartyInvoiceModel>
    {
        public void ExecuteRule(ISystemMessageCollection messages, ThirdPartyInvoiceModel ruleModel)
        {
            if (!ruleModel.InvoiceDate.HasValue)
            {
                messages.AddMessage(new SystemMessage("Invoice date must be captured.", SystemMessageSeverityEnum.Error));
                return;
            }

            var tomorrow = DateTime.Today.AddDays(1);
            if (ruleModel.InvoiceDate.Value >= tomorrow)
            {
                messages.AddMessage(new SystemMessage("Invoice date cannot be a future date.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}
