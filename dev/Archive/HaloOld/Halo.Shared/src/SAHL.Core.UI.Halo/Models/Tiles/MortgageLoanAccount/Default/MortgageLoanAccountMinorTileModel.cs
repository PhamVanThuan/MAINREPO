using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Tiles.MortgageLoanAccount.Default
{
    public class MortgageLoanAccountMinorTileModel : ITileModel
    {
        public string AccountNumber { get; set; }

        public string PropertyAddress { get; set; }

        public double LoanAgreementAmount { get; set; }

        public double LoanCurrentBalance { get; set; }

        public double LoanArrearBalance { get; set; }

        public int DebitOrderDay { get; set; }

        public double InstallmentAmount { get; set; }

        public int RemainingInstalments { get; set; }

        public int MonthsInArrears { get; set; }
    }
}