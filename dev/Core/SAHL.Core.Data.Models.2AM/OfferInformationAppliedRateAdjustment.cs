using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferInformationAppliedRateAdjustmentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferInformationAppliedRateAdjustmentDataModel(string offerElementValue, int rateAdjustmentElementKey, int aDUserKey, int offerInformationFinancialAdjustmentKey, DateTime changeDate)
        {
            this.OfferElementValue = offerElementValue;
            this.RateAdjustmentElementKey = rateAdjustmentElementKey;
            this.ADUserKey = aDUserKey;
            this.OfferInformationFinancialAdjustmentKey = offerInformationFinancialAdjustmentKey;
            this.ChangeDate = changeDate;
		
        }
		[JsonConstructor]
        public OfferInformationAppliedRateAdjustmentDataModel(int offerInformationAppliedRateAdjustmentKey, string offerElementValue, int rateAdjustmentElementKey, int aDUserKey, int offerInformationFinancialAdjustmentKey, DateTime changeDate)
        {
            this.OfferInformationAppliedRateAdjustmentKey = offerInformationAppliedRateAdjustmentKey;
            this.OfferElementValue = offerElementValue;
            this.RateAdjustmentElementKey = rateAdjustmentElementKey;
            this.ADUserKey = aDUserKey;
            this.OfferInformationFinancialAdjustmentKey = offerInformationFinancialAdjustmentKey;
            this.ChangeDate = changeDate;
		
        }		

        public int OfferInformationAppliedRateAdjustmentKey { get; set; }

        public string OfferElementValue { get; set; }

        public int RateAdjustmentElementKey { get; set; }

        public int ADUserKey { get; set; }

        public int OfferInformationFinancialAdjustmentKey { get; set; }

        public DateTime ChangeDate { get; set; }

        public void SetKey(int key)
        {
            this.OfferInformationAppliedRateAdjustmentKey =  key;
        }
    }
}