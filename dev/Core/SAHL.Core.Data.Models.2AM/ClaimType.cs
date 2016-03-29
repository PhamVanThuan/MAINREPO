using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ClaimTypeDataModel :  IDataModel
    {
        public ClaimTypeDataModel(int claimTypeKey, string description)
        {
            this.ClaimTypeKey = claimTypeKey;
            this.Description = description;
		
        }		

        public int ClaimTypeKey { get; set; }

        public string Description { get; set; }
    }
}