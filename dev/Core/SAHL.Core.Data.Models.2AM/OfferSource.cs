using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferSourceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferSourceDataModel(string description, int generalStatusKey)
        {
            this.Description = description;
            this.GeneralStatusKey = generalStatusKey;
		
        }
		[JsonConstructor]
        public OfferSourceDataModel(int offerSourceKey, string description, int generalStatusKey)
        {
            this.OfferSourceKey = offerSourceKey;
            this.Description = description;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int OfferSourceKey { get; set; }

        public string Description { get; set; }

        public int GeneralStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferSourceKey =  key;
        }
    }
}