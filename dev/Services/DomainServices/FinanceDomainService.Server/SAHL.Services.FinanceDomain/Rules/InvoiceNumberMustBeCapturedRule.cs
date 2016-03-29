using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Rules
{
    public class InvoiceNumberMustBeCapturedRule : IDomainRule<ThirdPartyInvoiceModel>
    {
        private IThirdPartyInvoiceDataManager dataManager;

        public InvoiceNumberMustBeCapturedRule(IThirdPartyInvoiceDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, ThirdPartyInvoiceModel ruleModel)
        {
            ThirdPartyInvoiceDataModel invoice = this.dataManager.GetThirdPartyInvoiceByKey(ruleModel.ThirdPartyInvoiceKey);
            if (string.IsNullOrWhiteSpace(invoice.InvoiceNumber))
            {
                messages.AddMessage(new SystemMessage("The Invoice Number has not been captured.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}