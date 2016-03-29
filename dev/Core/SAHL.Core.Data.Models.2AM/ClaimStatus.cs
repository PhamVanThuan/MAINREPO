using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ClaimStatusDataModel :  IDataModel
    {
        public ClaimStatusDataModel(int claimStatusKey, string description)
        {
            this.ClaimStatusKey = claimStatusKey;
            this.Description = description;
		
        }		

        public int ClaimStatusKey { get; set; }

        public string Description { get; set; }
    }
}