using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FeatureDataModel :  IDataModel
    {
        public FeatureDataModel(int featureKey, string shortName, string longName, bool hasAccess, int? parentKey, int sequence)
        {
            this.FeatureKey = featureKey;
            this.ShortName = shortName;
            this.LongName = longName;
            this.HasAccess = hasAccess;
            this.ParentKey = parentKey;
            this.Sequence = sequence;
		
        }		

        public int FeatureKey { get; set; }

        public string ShortName { get; set; }

        public string LongName { get; set; }

        public bool HasAccess { get; set; }

        public int? ParentKey { get; set; }

        public int Sequence { get; set; }
    }
}