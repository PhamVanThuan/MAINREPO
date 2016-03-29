using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class SANTAMPolicyStatusDataModel :  IDataModel
    {
        public SANTAMPolicyStatusDataModel(int sANTAMPolicyStatusKey, string description)
        {
            this.SANTAMPolicyStatusKey = sANTAMPolicyStatusKey;
            this.Description = description;
		
        }		

        public int SANTAMPolicyStatusKey { get; set; }

        public string Description { get; set; }
    }
}