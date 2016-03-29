using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ProposalTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ProposalTypeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public ProposalTypeDataModel(int proposalTypeKey, string description)
        {
            this.ProposalTypeKey = proposalTypeKey;
            this.Description = description;
		
        }		

        public int ProposalTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ProposalTypeKey =  key;
        }
    }
}