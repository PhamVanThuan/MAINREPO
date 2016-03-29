using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferAccountRelationshipDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferAccountRelationshipDataModel(int accountKey, int offerKey)
        {
            this.AccountKey = accountKey;
            this.OfferKey = offerKey;
		
        }
		[JsonConstructor]
        public OfferAccountRelationshipDataModel(int offerAccountRelationshipKey, int accountKey, int offerKey)
        {
            this.OfferAccountRelationshipKey = offerAccountRelationshipKey;
            this.AccountKey = accountKey;
            this.OfferKey = offerKey;
		
        }		

        public int OfferAccountRelationshipKey { get; set; }

        public int AccountKey { get; set; }

        public int OfferKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferAccountRelationshipKey =  key;
        }
    }
}