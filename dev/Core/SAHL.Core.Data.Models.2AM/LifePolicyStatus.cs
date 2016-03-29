using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifePolicyStatusDataModel :  IDataModel
    {
        public LifePolicyStatusDataModel(int policyStatusKey, string description)
        {
            this.PolicyStatusKey = policyStatusKey;
            this.Description = description;
		
        }		

        public int PolicyStatusKey { get; set; }

        public string Description { get; set; }
    }
}