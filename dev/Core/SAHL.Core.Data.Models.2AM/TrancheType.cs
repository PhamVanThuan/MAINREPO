using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class TrancheTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public TrancheTypeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public TrancheTypeDataModel(int trancheTypeKey, string description)
        {
            this.TrancheTypeKey = trancheTypeKey;
            this.Description = description;
		
        }		

        public int TrancheTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.TrancheTypeKey =  key;
        }
    }
}