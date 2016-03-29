using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class PriorityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public PriorityDataModel(int originationSourceProductKey, string description)
        {
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.Description = description;
		
        }
		[JsonConstructor]
        public PriorityDataModel(int priorityKey, int originationSourceProductKey, string description)
        {
            this.PriorityKey = priorityKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.Description = description;
		
        }		

        public int PriorityKey { get; set; }

        public int OriginationSourceProductKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.PriorityKey =  key;
        }
    }
}