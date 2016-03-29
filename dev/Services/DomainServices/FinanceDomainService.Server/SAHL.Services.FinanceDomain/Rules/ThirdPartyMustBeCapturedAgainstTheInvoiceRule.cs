using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;

namespace SAHL.Services.FinanceDomain.Rules
{
    public class ThirdPartyMustBeCapturedAgainstTheInvoiceRule : IDomainRule<ThirdPartyInvoiceModel>
    {
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;

        public ThirdPartyMustBeCapturedAgainstTheInvoiceRule(IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager)
        {
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, ThirdPartyInvoiceModel ruleModel)
        {
            var thirdPartyInvoice = thirdPartyInvoiceDataManager.GetThirdPartyInvoiceByKey(ruleModel.ThirdPartyInvoiceKey);
            var thirdPartyInvoicerIsSet = thirdPartyInvoice.ThirdPartyId.HasValue && thirdPartyInvoice.ThirdPartyId != Guid.Empty;
            if (!thirdPartyInvoicerIsSet)
            {
                messages.AddMessage(new SystemMessage("The Third Party who sent the invoice has not been captured.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}