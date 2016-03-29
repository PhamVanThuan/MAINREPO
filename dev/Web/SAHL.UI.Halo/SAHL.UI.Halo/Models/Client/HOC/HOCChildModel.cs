using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Models.Client.HOC
{
    public class HOCChildModel : IHaloTileModel
    {
        public string HOCInsurer { get; set; }
        public AccountStatus HocAccountStatus { get; set; }
        public string PolicyNumber { get; set; }
        public string HOCPolicyNumber { get; set; }
        public bool IsSAHLHOC { get; set; }
        public string PropertyAddress { get; set; }
        public double SumInsured { get; set; }
        public DateTime ValuationDate { get; set; }
        public double ActiveValuationHOCValue { get; set; }
        public DateTime CommencementDate { get; set; }
        public DateTime AnniversaryDate { get; set; }
        public bool Ceded { get; set; }
        public double CurrentTotalMonthlyHOCPaymentExposureToSAHL { get; set; }
    }
}