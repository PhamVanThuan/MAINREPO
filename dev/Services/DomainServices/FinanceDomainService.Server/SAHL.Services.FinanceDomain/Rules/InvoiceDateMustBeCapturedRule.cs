using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Rules
{
    public class InvoiceDateMustBeCapturedRule : IDomainRule<ThirdPartyInvoiceModel>
    {
        private IThirdPartyInvoiceDataManager dataManager;

        public InvoiceDateMustBeCapturedRule(IThirdPartyInvoiceDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, ThirdPartyInvoiceModel ruleModel)
        {
            ThirdPartyInvoiceDataModel invoice = this.dataManager.GetThirdPartyInvoiceByKey(ruleModel.ThirdPartyInvoiceKey);
            if (!invoice.InvoiceDate.HasValue)
            {
                messages.AddMessage(new SystemMessage("The Invoice Date has not been captured.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}