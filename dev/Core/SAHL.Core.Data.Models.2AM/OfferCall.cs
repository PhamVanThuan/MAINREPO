using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferCallDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferCallDataModel(int offerKey, int callTypeKey, DateTime callDate)
        {
            this.OfferKey = offerKey;
            this.CallTypeKey = callTypeKey;
            this.CallDate = callDate;
		
        }
		[JsonConstructor]
        public OfferCallDataModel(int offerCallKey, int offerKey, int callTypeKey, DateTime callDate)
        {
            this.OfferCallKey = offerCallKey;
            this.OfferKey = offerKey;
            this.CallTypeKey = callTypeKey;
            this.CallDate = callDate;
		
        }		

        public int OfferCallKey { get; set; }

        public int OfferKey { get; set; }

        public int CallTypeKey { get; set; }

        public DateTime CallDate { get; set; }

        public void SetKey(int key)
        {
            this.OfferCallKey =  key;
        }
    }
}