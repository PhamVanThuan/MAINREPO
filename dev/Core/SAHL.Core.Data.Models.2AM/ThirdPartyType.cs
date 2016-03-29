using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ThirdPartyTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ThirdPartyTypeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public ThirdPartyTypeDataModel(int thirdPartyTypeKey, string description)
        {
            this.ThirdPartyTypeKey = thirdPartyTypeKey;
            this.Description = description;
		
        }		

        public int ThirdPartyTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ThirdPartyTypeKey =  key;
        }
    }
}