using System;
using System.Linq;

namespace SAHL.Services.Interfaces.DomainQuery.Model
{
    public class GetMortgageLoanDetailsQueryResult
    {
        public int AccountKey { get; set; }

        public int FinancialServiceTypeKey { get; set; }

        public int AccountStatusKey { get; set; }

        public DateTime? OpenDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public int CreditMatrixKey { get; set; }

        public int? PropertyKey { get; set; }

        public int MortgageLoanPurposeKey { get; set; }

        public int Term { get; set; }

        public decimal InitialBalance { get; set; }

        public int RemainingInstalments { get; set; }

        public decimal InterestRate { get; set; }

        public int RateConfigurationKey { get; set; }

        public int ResetConfigurationKey { get; set; }

        public decimal RateAdjustment { get; set; }

        public decimal ActiveMarketRate { get; set; }

        public decimal MTDInterest { get; set; }
    }
}