using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Rules
{
    public class InvoicePaymentShouldBeBeingProcessedRule : IDomainRule<IThirdPartyInvoiceRuleModel>
    {
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;

        public InvoicePaymentShouldBeBeingProcessedRule(IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager)
        {
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, IThirdPartyInvoiceRuleModel ruleModel)
        {
            var thirdPartyInvoice = thirdPartyInvoiceDataManager.GetThirdPartyInvoiceByKey(ruleModel.ThirdPartyInvoiceKey);
            if (thirdPartyInvoice.InvoiceStatusKey != (int)InvoiceStatus.ProcessingPayment)
            {
                messages.AddMessage(new SystemMessage("Invoice payment is not currently being processed.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}