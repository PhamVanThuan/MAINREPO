using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferStatusDataModel :  IDataModel
    {
        public OfferStatusDataModel(int offerStatusKey, string description)
        {
            this.OfferStatusKey = offerStatusKey;
            this.Description = description;
		
        }		

        public int OfferStatusKey { get; set; }

        public string Description { get; set; }
    }
}