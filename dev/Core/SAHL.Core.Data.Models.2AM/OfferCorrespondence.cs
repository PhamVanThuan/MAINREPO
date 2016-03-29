using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferCorrespondenceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferCorrespondenceDataModel(int offerKey, int correspondenceKey)
        {
            this.OfferKey = offerKey;
            this.CorrespondenceKey = correspondenceKey;
		
        }
		[JsonConstructor]
        public OfferCorrespondenceDataModel(int offerCorrespondenceKey, int offerKey, int correspondenceKey)
        {
            this.OfferCorrespondenceKey = offerCorrespondenceKey;
            this.OfferKey = offerKey;
            this.CorrespondenceKey = correspondenceKey;
		
        }		

        public int OfferCorrespondenceKey { get; set; }

        public int OfferKey { get; set; }

        public int CorrespondenceKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferCorrespondenceKey =  key;
        }
    }
}