using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferInformationTypeDataModel :  IDataModel
    {
        public OfferInformationTypeDataModel(int offerInformationTypeKey, string description)
        {
            this.OfferInformationTypeKey = offerInformationTypeKey;
            this.Description = description;
		
        }		

        public int OfferInformationTypeKey { get; set; }

        public string Description { get; set; }
    }
}