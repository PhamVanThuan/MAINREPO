using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferSubsidyDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferSubsidyDataModel(int offerKey, int subsidyKey)
        {
            this.OfferKey = offerKey;
            this.SubsidyKey = subsidyKey;
		
        }
		[JsonConstructor]
        public OfferSubsidyDataModel(int offerSubsidyKey, int offerKey, int subsidyKey)
        {
            this.OfferSubsidyKey = offerSubsidyKey;
            this.OfferKey = offerKey;
            this.SubsidyKey = subsidyKey;
		
        }		

        public int OfferSubsidyKey { get; set; }

        public int OfferKey { get; set; }

        public int SubsidyKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferSubsidyKey =  key;
        }
    }
}