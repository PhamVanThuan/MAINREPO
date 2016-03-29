using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CapabilityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CapabilityDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public CapabilityDataModel(int capabilityKey, string description)
        {
            this.CapabilityKey = capabilityKey;
            this.Description = description;
		
        }		

        public int CapabilityKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.CapabilityKey =  key;
        }
    }
}