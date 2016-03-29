using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AssetLiabilitySubTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AssetLiabilitySubTypeDataModel(int assetLiabilityTypeKey, string description)
        {
            this.AssetLiabilityTypeKey = assetLiabilityTypeKey;
            this.Description = description;
		
        }
		[JsonConstructor]
        public AssetLiabilitySubTypeDataModel(int assetLiabilitySubTypeKey, int assetLiabilityTypeKey, string description)
        {
            this.AssetLiabilitySubTypeKey = assetLiabilitySubTypeKey;
            this.AssetLiabilityTypeKey = assetLiabilityTypeKey;
            this.Description = description;
		
        }		

        public int AssetLiabilitySubTypeKey { get; set; }

        public int AssetLiabilityTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.AssetLiabilitySubTypeKey =  key;
        }
    }
}