using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Specs.Rules.InvoicePaymentMustBeProcessedRuleSpecs
{
    public class RuleModel : IThirdPartyInvoiceRuleModel
    {
        public RuleModel(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }

        public int ThirdPartyInvoiceKey { get; set; }
    }
}