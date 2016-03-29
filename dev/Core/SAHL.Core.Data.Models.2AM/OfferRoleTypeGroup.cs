using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferRoleTypeGroupDataModel :  IDataModel
    {
        public OfferRoleTypeGroupDataModel(int offerRoleTypeGroupKey, string description)
        {
            this.OfferRoleTypeGroupKey = offerRoleTypeGroupKey;
            this.Description = description;
		
        }		

        public int OfferRoleTypeGroupKey { get; set; }

        public string Description { get; set; }
    }
}