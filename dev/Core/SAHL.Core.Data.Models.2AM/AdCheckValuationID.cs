using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AdCheckValuationIDDataModel :  IDataModel
    {
        public AdCheckValuationIDDataModel(int adCheckValuationIDKey, int adCheckValuationIDStatusKey, int offerKey, int? valuationKey, DateTime changeDate)
        {
            this.AdCheckValuationIDKey = adCheckValuationIDKey;
            this.AdCheckValuationIDStatusKey = adCheckValuationIDStatusKey;
            this.OfferKey = offerKey;
            this.ValuationKey = valuationKey;
            this.ChangeDate = changeDate;
		
        }		

        public int AdCheckValuationIDKey { get; set; }

        public int AdCheckValuationIDStatusKey { get; set; }

        public int OfferKey { get; set; }

        public int? ValuationKey { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}