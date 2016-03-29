using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RateOverrideTypeDataModel :  IDataModel
    {
        public RateOverrideTypeDataModel(int rateOverrideTypeKey, string description, int? rateOverrideTypeGroupKey)
        {
            this.RateOverrideTypeKey = rateOverrideTypeKey;
            this.Description = description;
            this.RateOverrideTypeGroupKey = rateOverrideTypeGroupKey;
		
        }		

        public int RateOverrideTypeKey { get; set; }

        public string Description { get; set; }

        public int? RateOverrideTypeGroupKey { get; set; }
    }
}