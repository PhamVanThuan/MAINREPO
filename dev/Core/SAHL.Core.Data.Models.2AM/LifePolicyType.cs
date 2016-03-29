using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifePolicyTypeDataModel :  IDataModel
    {
        public LifePolicyTypeDataModel(int lifePolicyTypeKey, string description)
        {
            this.LifePolicyTypeKey = lifePolicyTypeKey;
            this.Description = description;
		
        }		

        public int LifePolicyTypeKey { get; set; }

        public string Description { get; set; }
    }
}