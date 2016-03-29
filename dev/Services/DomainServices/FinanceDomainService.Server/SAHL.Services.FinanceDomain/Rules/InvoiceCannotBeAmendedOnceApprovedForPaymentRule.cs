using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Rules
{
    public class InvoiceCannotBeAmendedOnceApprovedForPaymentRule : IDomainRule<ThirdPartyInvoiceModel>
    {
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;

        public InvoiceCannotBeAmendedOnceApprovedForPaymentRule(IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager)
        {
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, ThirdPartyInvoiceModel ruleModel)
        {
            var thirdPartyInvoice = thirdPartyInvoiceDataManager.GetThirdPartyInvoiceByKey(ruleModel.ThirdPartyInvoiceKey);
            if (thirdPartyInvoice.InvoiceStatusKey == (int)InvoiceStatus.Approved)
            {
                messages.AddMessage(new SystemMessage("An Invoice that has been Approved for Payment cannot be amended.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}