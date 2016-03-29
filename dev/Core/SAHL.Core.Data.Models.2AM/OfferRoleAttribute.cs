using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferRoleAttributeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferRoleAttributeDataModel(int offerRoleKey, int offerRoleAttributeTypeKey)
        {
            this.OfferRoleKey = offerRoleKey;
            this.OfferRoleAttributeTypeKey = offerRoleAttributeTypeKey;
		
        }
		[JsonConstructor]
        public OfferRoleAttributeDataModel(int offerRoleAttributeKey, int offerRoleKey, int offerRoleAttributeTypeKey)
        {
            this.OfferRoleAttributeKey = offerRoleAttributeKey;
            this.OfferRoleKey = offerRoleKey;
            this.OfferRoleAttributeTypeKey = offerRoleAttributeTypeKey;
		
        }		

        public int OfferRoleAttributeKey { get; set; }

        public int OfferRoleKey { get; set; }

        public int OfferRoleAttributeTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferRoleAttributeKey =  key;
        }
    }
}