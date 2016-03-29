using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ProposalStatusDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ProposalStatusDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public ProposalStatusDataModel(int proposalStatusKey, string description)
        {
            this.ProposalStatusKey = proposalStatusKey;
            this.Description = description;
		
        }		

        public int ProposalStatusKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ProposalStatusKey =  key;
        }
    }
}