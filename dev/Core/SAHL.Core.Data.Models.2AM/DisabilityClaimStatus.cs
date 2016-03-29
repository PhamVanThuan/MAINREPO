using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DisabilityClaimStatusDataModel :  IDataModel
    {
        public DisabilityClaimStatusDataModel(int disabilityClaimStatusKey, string description)
        {
            this.DisabilityClaimStatusKey = disabilityClaimStatusKey;
            this.Description = description;
		
        }		

        public int DisabilityClaimStatusKey { get; set; }

        public string Description { get; set; }
    }
}