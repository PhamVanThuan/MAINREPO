using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CapTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CapTypeDataModel(string description, double value)
        {
            this.Description = description;
            this.Value = value;
		
        }
		[JsonConstructor]
        public CapTypeDataModel(int capTypeKey, string description, double value)
        {
            this.CapTypeKey = capTypeKey;
            this.Description = description;
            this.Value = value;
		
        }		

        public int CapTypeKey { get; set; }

        public string Description { get; set; }

        public double Value { get; set; }

        public void SetKey(int key)
        {
            this.CapTypeKey =  key;
        }
    }
}