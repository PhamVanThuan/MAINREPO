using System;

namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetClientAssetsAndLiabilitiesQueryResult
    {
        public int LegalEntityKey { get; set; }

        public int AddressKey { get; set; }

        public double AssetValue { get; set; }

        public double LiabilityValue { get; set; }

        public string CompanyName { get; set; }

        public double Cost { get; set; }

        public DateTime Date { get; set; }

        public string AssetLiabilityDescription { get; set; }

        public string AssetTypeDescription { get; set; }

        public int AssetLiabilitySubTypeKey { get; set; }
    }
}