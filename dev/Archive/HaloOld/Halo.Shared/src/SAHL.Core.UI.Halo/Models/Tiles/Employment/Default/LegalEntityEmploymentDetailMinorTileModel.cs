using SAHL.Core.UI.Models;
using System;

namespace SAHL.Core.UI.Halo.Tiles.Employment.Default
{
    public class LegalEntityEmploymentDetailMinorTileModel : ITileModel
    {

        public string EmployerName { get; set; }
        public string EmploymentType { get; set; }
        public string Confirmed { get; set; }
        public DateTime ConfirmedDate { get; set; }
        public Decimal IncomeAmount { get; set; }
        public Int16 SalaryPaymentDay { get; set; }

    }
}