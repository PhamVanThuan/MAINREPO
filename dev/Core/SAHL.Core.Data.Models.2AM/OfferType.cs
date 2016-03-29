using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferTypeDataModel :  IDataModel
    {
        public OfferTypeDataModel(int offerTypeKey, string description)
        {
            this.OfferTypeKey = offerTypeKey;
            this.Description = description;
		
        }		

        public int OfferTypeKey { get; set; }

        public string Description { get; set; }
    }
}