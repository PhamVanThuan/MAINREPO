using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferRoleTypeDataModel :  IDataModel
    {
        public OfferRoleTypeDataModel(int offerRoleTypeKey, string description, int offerRoleTypeGroupKey)
        {
            this.OfferRoleTypeKey = offerRoleTypeKey;
            this.Description = description;
            this.OfferRoleTypeGroupKey = offerRoleTypeGroupKey;
		
        }		

        public int OfferRoleTypeKey { get; set; }

        public string Description { get; set; }

        public int OfferRoleTypeGroupKey { get; set; }
    }
}