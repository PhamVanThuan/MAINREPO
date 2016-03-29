using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Models.Account
{
    public class LifeRootModel : IHaloTileModel
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

        public string PolicyType { get; set; }

        public string PolicyNumber { get; set; }

        public string PolicyStatus { get; set; }

        public string Consultant { get; set; }

        public DateTime AcceptedDate { get; set; }

        public DateTime CommencementDate { get; set; }

        public DateTime CancellationDate { get; set; }

        public DateTime CededDate { get; set; }

        public double InitialSumAssured { get; set; }

        public double CurrentSumAssured { get; set; }

        public double DeathBenefitPremium { get; set; }

        public double InstallmentProtectionPremium { get; set; }

        public double AnnualPremium { get; set; }

        public double MonthlyInstalment { get; set; }
    }
}