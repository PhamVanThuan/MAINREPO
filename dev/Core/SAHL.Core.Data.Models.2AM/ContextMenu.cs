using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ContextMenuDataModel :  IDataModel
    {
        public ContextMenuDataModel(int contextKey, int? coreBusinessObjectKey, int? parentKey, string description, string uRL, int featureKey, int sequence)
        {
            this.ContextKey = contextKey;
            this.CoreBusinessObjectKey = coreBusinessObjectKey;
            this.ParentKey = parentKey;
            this.Description = description;
            this.URL = uRL;
            this.FeatureKey = featureKey;
            this.Sequence = sequence;
		
        }		

        public int ContextKey { get; set; }

        public int? CoreBusinessObjectKey { get; set; }

        public int? ParentKey { get; set; }

        public string Description { get; set; }

        public string URL { get; set; }

        public int FeatureKey { get; set; }

        public int Sequence { get; set; }
    }
}