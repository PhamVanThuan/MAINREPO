using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferExternalLifeDataModel :  IDataModel
    {
        public OfferExternalLifeDataModel(int offerKey, int externalLifePolicyKey)
        {
            this.OfferKey = offerKey;
            this.ExternalLifePolicyKey = externalLifePolicyKey;
		
        }		

        public int OfferKey { get; set; }

        public int ExternalLifePolicyKey { get; set; }
    }
}