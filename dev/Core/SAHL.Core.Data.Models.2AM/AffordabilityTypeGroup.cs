using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AffordabilityTypeGroupDataModel :  IDataModel
    {
        public AffordabilityTypeGroupDataModel(int affordabilityTypeGroupKey, string description)
        {
            this.AffordabilityTypeGroupKey = affordabilityTypeGroupKey;
            this.Description = description;
		
        }		

        public int AffordabilityTypeGroupKey { get; set; }

        public string Description { get; set; }
    }
}