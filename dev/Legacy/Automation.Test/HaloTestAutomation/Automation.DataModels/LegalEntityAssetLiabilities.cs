using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class LegalEntityAssetLiabilities
    {
        public LegalEntityAssetLiabilities()
        {
        }

        #region Contructors

        public LegalEntityAssetLiabilities(LegalEntityAssetLiabilities legalEntityAssetLiabilityModel)
        {
            var thisInstance = this;
            Helpers.SetProperties<LegalEntityAssetLiabilities, LegalEntityAssetLiabilities>(ref thisInstance, legalEntityAssetLiabilityModel);
        }

        #endregion Contructors

        public int LegalEntityKey { get; set; }

        public int LegalEntityAssetLiabilityKey { get; set; }

        public GeneralStatusEnum GeneralStatusKey { get; set; }

        public int AssetLiabilityKey { get; set; }

        public Common.Enums.AssetLiabilityTypeEnum AssetLiabilityTypeKey { get; set; }

        public Common.Enums.AssetLiabilitySubTypeEnum AssetLiabilitySubTypeKey { get; set; }

        public int AddressKey { get; set; }

        public DateTime DateAcquired { get; set; }

        public DateTime DateRepayable { get; set; }

        public DateTime? Date { get; set; }

        public string CompanyName { get; set; }

        public Automation.DataModels.Address Address { get; set; }

        public double AssetValue { get; set; }

        public double LiabilityValue { get; set; }

        public string OtherDescription { get; set; }

        public double Cost { get; set; }

        public string AssetLiabilityTypeDescription { get; set; }

        public string AssetLiabilitySubTypeDescription { get; set; }

        #region Other

        public Automation.DataModels.LegalEntity LegalEntityModel { get; set; }

        #endregion Other
    }
}