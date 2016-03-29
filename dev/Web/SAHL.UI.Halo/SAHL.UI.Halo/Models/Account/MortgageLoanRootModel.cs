using System;

using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Models.Account
{
    public class MortgageLoanRootModel : IHaloTileModel
    {
        public string AccountNumber { get; set; }

        public OriginationSource OriginationSourceKey { get; set; }

        public string OriginationSource { get; set; }

        public bool IsInArrears { get; set; }

        public bool IsInAdvance { get; set; }

        public bool IsNonPerforming { get; set; }

        public string AccountStatus { get; set; }

        public DateTime OpenDate { get; set; }

        public DateTime CloseDate { get; set; }

        public string Product { get; set; }

        public string CurrentSPV { get; set; }

        public double TotalBondAmount { get; set; }

        public double LoanAgreement { get; set; }

        public string CurrentLTV { get; set; }

        public string CurrentPTI { get; set; }

        public string DebitOrderDay { get; set; }

        public string PaymentType { get; set; }

        public double OriginatingOfferIncomeContributorTotalConfirmedIncome { get; set; }

        public double HouseholdTotalConfirmedIncome { get; set; }

        public DateTime NextResetDate { get; set; }

        public string EffectiveRate { get; set; }

        public string FixedRate { get; set; }

        public int RemainingTerm { get; set; }

        public double Instalment { get; set; }

        public double CurrentBalance { get; set; }

        public double ArrearBalance { get; set; }
    }
}