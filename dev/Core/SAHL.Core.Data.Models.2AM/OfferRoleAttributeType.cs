using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferRoleAttributeTypeDataModel :  IDataModel
    {
        public OfferRoleAttributeTypeDataModel(int offerRoleAttributeTypeKey, string description)
        {
            this.OfferRoleAttributeTypeKey = offerRoleAttributeTypeKey;
            this.Description = description;
		
        }		

        public int OfferRoleAttributeTypeKey { get; set; }

        public string Description { get; set; }
    }
}