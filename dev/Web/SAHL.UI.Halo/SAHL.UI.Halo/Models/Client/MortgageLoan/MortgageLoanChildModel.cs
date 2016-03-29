using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Models.Client.MortgageLoan
{
    public class MortgageLoanChildModel : IHaloTileModel
    {
        public string AccountNumber { get; set; }

        public string PropertyAddress { get; set; }

        public DateTime OpenDate { get; set; }

        public double LoanAgreementAmount { get; set; }

        public double LoanCurrentBalance { get; set; }

        public double LoanArrearBalance { get; set; }

        public int DebitOrderDay { get; set; }

        public double InstalmentAmount { get; set; }

        public int RemainingInstalments { get; set; }

        public SAHL.Core.BusinessModel.Enums.AccountStatus AccountStatus { get; set; }

        public SAHL.Core.BusinessModel.Enums.OriginationSource OriginationSource { get; set; }
    }
}