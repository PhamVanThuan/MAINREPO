using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ReportGroupDataModel :  IDataModel
    {
        public ReportGroupDataModel(int reportGroupKey, string description, int featureKey)
        {
            this.ReportGroupKey = reportGroupKey;
            this.Description = description;
            this.FeatureKey = featureKey;
		
        }		

        public int ReportGroupKey { get; set; }

        public string Description { get; set; }

        public int FeatureKey { get; set; }
    }
}