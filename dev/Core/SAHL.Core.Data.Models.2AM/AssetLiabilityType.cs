using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AssetLiabilityTypeDataModel :  IDataModel
    {
        public AssetLiabilityTypeDataModel(int assetLiabilityTypeKey, string description)
        {
            this.AssetLiabilityTypeKey = assetLiabilityTypeKey;
            this.Description = description;
		
        }		

        public int AssetLiabilityTypeKey { get; set; }

        public string Description { get; set; }
    }
}