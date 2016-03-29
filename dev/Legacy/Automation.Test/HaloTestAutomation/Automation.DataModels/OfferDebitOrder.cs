using Common.Enums;

namespace Automation.DataModels
{
    public sealed class OfferDebitOrder
    {
        public int OfferDebitOrderKey { get; set; }

        public int OfferKey { get; set; }

        public int BankAccountKey { get; set; }

        public double Percentage { get; set; }

        public int DebitOrderDay { get; set; }

        public FinancialServicePaymentTypeEnum FinancialServicePaymentTypeKey { get; set; }
    }
}