using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ClientOfferStatusDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ClientOfferStatusDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public ClientOfferStatusDataModel(int clientOfferStatusKey, string description)
        {
            this.ClientOfferStatusKey = clientOfferStatusKey;
            this.Description = description;
		
        }		

        public int ClientOfferStatusKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ClientOfferStatusKey =  key;
        }
    }
}