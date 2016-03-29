using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MarginDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public MarginDataModel(double value, string description)
        {
            this.Value = value;
            this.Description = description;
		
        }
		[JsonConstructor]
        public MarginDataModel(int marginKey, double value, string description)
        {
            this.MarginKey = marginKey;
            this.Value = value;
            this.Description = description;
		
        }		

        public int MarginKey { get; set; }

        public double Value { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.MarginKey =  key;
        }
    }
}