using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RateAdjustmentGroupDataModel :  IDataModel
    {
        public RateAdjustmentGroupDataModel(int rateAdjustmentGroupKey, string description)
        {
            this.RateAdjustmentGroupKey = rateAdjustmentGroupKey;
            this.Description = description;
		
        }		

        public int RateAdjustmentGroupKey { get; set; }

        public string Description { get; set; }
    }
}