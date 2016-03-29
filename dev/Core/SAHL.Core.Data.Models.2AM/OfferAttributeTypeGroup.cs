using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferAttributeTypeGroupDataModel :  IDataModel
    {
        public OfferAttributeTypeGroupDataModel(int offerAttributeTypeGroupKey, string description)
        {
            this.OfferAttributeTypeGroupKey = offerAttributeTypeGroupKey;
            this.Description = description;
		
        }		

        public int OfferAttributeTypeGroupKey { get; set; }

        public string Description { get; set; }
    }
}