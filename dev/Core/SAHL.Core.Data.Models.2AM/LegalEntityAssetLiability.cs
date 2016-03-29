using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityAssetLiabilityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LegalEntityAssetLiabilityDataModel(int legalEntityKey, int assetLiabilityKey, int generalStatusKey)
        {
            this.LegalEntityKey = legalEntityKey;
            this.AssetLiabilityKey = assetLiabilityKey;
            this.GeneralStatusKey = generalStatusKey;
		
        }
		[JsonConstructor]
        public LegalEntityAssetLiabilityDataModel(int legalEntityAssetLiabilityKey, int legalEntityKey, int assetLiabilityKey, int generalStatusKey)
        {
            this.LegalEntityAssetLiabilityKey = legalEntityAssetLiabilityKey;
            this.LegalEntityKey = legalEntityKey;
            this.AssetLiabilityKey = assetLiabilityKey;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int LegalEntityAssetLiabilityKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int AssetLiabilityKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.LegalEntityAssetLiabilityKey =  key;
        }
    }
}