using System;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Tiles.PersonalLoanAccount.Default
{
    public class PersonalLoanAccountMinorTileModel : ITileModel
    {
        public string AccountNumber { get; set; }

        public decimal LoanAmount { get; set; }

        public DateTime OpenDate { get; set; }

        public decimal CurrentBalance { get; set; }

        public decimal ArrearBalance { get; set; }

        public int MonthsInArrears { get; set; }

        public int DebitOrderDay { get; set; }

        public decimal InstallmentAmount { get; set; }

        public int RemainingTerm { get; set; }
    }
}