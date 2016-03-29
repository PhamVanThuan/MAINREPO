using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Rules
{
    public class InvoiceNumberCannotExistAgainstAnotherInvoiceForTheSameThirdPartyRule : IDomainRule<ThirdPartyInvoiceModel>
    {
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;

        public InvoiceNumberCannotExistAgainstAnotherInvoiceForTheSameThirdPartyRule(IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager)
        {
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, ThirdPartyInvoiceModel ruleModel)
        {
            IEnumerable<ThirdPartyInvoiceDataModel> invoices = thirdPartyInvoiceDataManager.GetThirdPartyInvoicesByInvoiceNumber(ruleModel.InvoiceNumber);
            if (invoices == null || invoices.Count().Equals(0))
            {
                return;
            }
            var referenceExistsAgainstAnotherInvoice = invoices.Where(x => x.ThirdPartyInvoiceKey != ruleModel.ThirdPartyInvoiceKey
                    && x.ThirdPartyId.Equals(ruleModel.ThirdPartyId)
                    && x.InvoiceNumber.Equals(ruleModel.InvoiceNumber)).Any();
            if (referenceExistsAgainstAnotherInvoice)
            {
                messages.AddMessage(new SystemMessage("The invoice number provided has already been captured against another invoice for the same third party.",
                    SystemMessageSeverityEnum.Error));
            }
        }
    }
}