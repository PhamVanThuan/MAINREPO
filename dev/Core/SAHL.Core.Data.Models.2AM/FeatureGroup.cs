using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FeatureGroupDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public FeatureGroupDataModel(string aDUserGroup, int featureKey)
        {
            this.ADUserGroup = aDUserGroup;
            this.FeatureKey = featureKey;
		
        }
		[JsonConstructor]
        public FeatureGroupDataModel(int featureGroupKey, string aDUserGroup, int featureKey)
        {
            this.FeatureGroupKey = featureGroupKey;
            this.ADUserGroup = aDUserGroup;
            this.FeatureKey = featureKey;
		
        }		

        public int FeatureGroupKey { get; set; }

        public string ADUserGroup { get; set; }

        public int FeatureKey { get; set; }

        public void SetKey(int key)
        {
            this.FeatureGroupKey =  key;
        }
    }
}