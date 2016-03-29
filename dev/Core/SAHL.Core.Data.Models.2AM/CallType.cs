using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CallTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CallTypeDataModel(string description, bool inBound)
        {
            this.Description = description;
            this.InBound = inBound;
		
        }
		[JsonConstructor]
        public CallTypeDataModel(int callTypeKey, string description, bool inBound)
        {
            this.CallTypeKey = callTypeKey;
            this.Description = description;
            this.InBound = inBound;
		
        }		

        public int CallTypeKey { get; set; }

        public string Description { get; set; }

        public bool InBound { get; set; }

        public void SetKey(int key)
        {
            this.CallTypeKey =  key;
        }
    }
}