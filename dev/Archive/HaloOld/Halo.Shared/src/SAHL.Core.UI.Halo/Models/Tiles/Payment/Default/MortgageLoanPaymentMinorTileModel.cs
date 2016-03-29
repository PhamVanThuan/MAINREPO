using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Tiles.Payment.Default
{
    public class MortgageLoanPaymentMinorTileModel : ITileModel
    {
        public string EffectiveDate { get; set; }

        public string PaymentMethod { get; set; }

        public string PaymentDay { get; set; }

        public string UserName { get; set; }

        public string AccountHolderName { get; set; }

        public string AccountType { get; set; }

        public string AccountNumber { get; set; }

        public string BankingInstitution { get; set; }

        public string BranchCode { get; set; }

        public decimal FixedDebitOrderAmount { get; set; }

        public decimal SubsidyStopOrderAmount { get; set; }

        public string SubsidyProvider { get; set; }

        public bool FutureDatedChangeExists { get; set; }
    }
}