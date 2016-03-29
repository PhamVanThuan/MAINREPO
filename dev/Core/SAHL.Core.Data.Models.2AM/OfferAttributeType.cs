using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferAttributeTypeDataModel :  IDataModel
    {
        public OfferAttributeTypeDataModel(int offerAttributeTypeKey, string description, bool iSGeneric, int? offerAttributeTypeGroupKey, bool userEditable)
        {
            this.OfferAttributeTypeKey = offerAttributeTypeKey;
            this.Description = description;
            this.ISGeneric = iSGeneric;
            this.OfferAttributeTypeGroupKey = offerAttributeTypeGroupKey;
            this.UserEditable = userEditable;
		
        }		

        public int OfferAttributeTypeKey { get; set; }

        public string Description { get; set; }

        public bool ISGeneric { get; set; }

        public int? OfferAttributeTypeGroupKey { get; set; }

        public bool UserEditable { get; set; }
    }
}