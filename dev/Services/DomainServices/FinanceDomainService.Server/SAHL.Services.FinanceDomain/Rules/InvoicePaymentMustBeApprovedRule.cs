using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Rules
{
    public class InvoicePaymentMustBeApprovedRule : IDomainRule<IThirdPartyInvoiceRuleModel>
    {
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;

        public InvoicePaymentMustBeApprovedRule(IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager)
        {
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, IThirdPartyInvoiceRuleModel ruleModel)
        {
            var thirdPartyInvoice = thirdPartyInvoiceDataManager.GetThirdPartyInvoiceByKey(ruleModel.ThirdPartyInvoiceKey);
            if (thirdPartyInvoice.InvoiceStatusKey != (int)InvoiceStatus.Approved)
            {
                messages.AddMessage(new SystemMessage("An Approved Invoice is required.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}