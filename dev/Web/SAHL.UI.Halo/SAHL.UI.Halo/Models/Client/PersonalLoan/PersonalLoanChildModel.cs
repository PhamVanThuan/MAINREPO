using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Models.Client.PersonalLoan
{
    public class PersonalLoanChildModel : IHaloTileModel
    {
        public string AccountNumber { get; set; }
        public string AccountStatus { get; set; }

        public decimal LoanAmount { get; set; }

        public DateTime OpenDate { get; set; }

        public decimal CurrentBalance { get; set; }

        public decimal ArrearBalance { get; set; }

        public int MonthsInArrears { get; set; }

        public int DebitOrderDay { get; set; }

        public decimal InstalmentAmount { get; set; }

        public int RemainingTerm { get; set; }

        public SAHL.Core.BusinessModel.Enums.OriginationSource OriginationSource { get; set; }
    }
}