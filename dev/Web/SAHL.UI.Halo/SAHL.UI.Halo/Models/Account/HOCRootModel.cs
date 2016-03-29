using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Models.Account
{
    public class HOCRootModel : IHaloTileModel
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

        public DateTime CommencementDate { get; set; }

        public DateTime AnniversaryDate { get; set; }

        public string HOCPolicyNumber { get; set; }

        public double HOCMonthlyPremium { get; set; }

        public double HOCTotalSumInsured { get; set; }

        public string RoofDescription { get; set; }

        public string HOCInsurer { get; set; }
    }
}