using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FLOfferDataModel :  IDataModel
    {
        public FLOfferDataModel(int? offerkey, int? accountKey, int? reservedAccountKey, DateTime? offerStartDate, int? offerTypeKey, int? offerStatusKey, int? originationSourceKey, string reference)
        {
            this.Offerkey = offerkey;
            this.AccountKey = accountKey;
            this.ReservedAccountKey = reservedAccountKey;
            this.OfferStartDate = offerStartDate;
            this.OfferTypeKey = offerTypeKey;
            this.OfferStatusKey = offerStatusKey;
            this.OriginationSourceKey = originationSourceKey;
            this.Reference = reference;
		
        }		

        public int? Offerkey { get; set; }

        public int? AccountKey { get; set; }

        public int? ReservedAccountKey { get; set; }

        public DateTime? OfferStartDate { get; set; }

        public int? OfferTypeKey { get; set; }

        public int? OfferStatusKey { get; set; }

        public int? OriginationSourceKey { get; set; }

        public string Reference { get; set; }
    }
}