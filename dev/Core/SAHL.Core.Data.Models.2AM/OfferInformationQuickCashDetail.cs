using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferInformationQuickCashDetailDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferInformationQuickCashDetailDataModel(int offerInformationKey, double? interestRate, double? requestedAmount, int? rateConfigurationKey, bool? disbursed, int? quickCashPaymentTypeKey)
        {
            this.OfferInformationKey = offerInformationKey;
            this.InterestRate = interestRate;
            this.RequestedAmount = requestedAmount;
            this.RateConfigurationKey = rateConfigurationKey;
            this.Disbursed = disbursed;
            this.QuickCashPaymentTypeKey = quickCashPaymentTypeKey;
		
        }
		[JsonConstructor]
        public OfferInformationQuickCashDetailDataModel(int offerInformationQuickCashDetailKey, int offerInformationKey, double? interestRate, double? requestedAmount, int? rateConfigurationKey, bool? disbursed, int? quickCashPaymentTypeKey)
        {
            this.OfferInformationQuickCashDetailKey = offerInformationQuickCashDetailKey;
            this.OfferInformationKey = offerInformationKey;
            this.InterestRate = interestRate;
            this.RequestedAmount = requestedAmount;
            this.RateConfigurationKey = rateConfigurationKey;
            this.Disbursed = disbursed;
            this.QuickCashPaymentTypeKey = quickCashPaymentTypeKey;
		
        }		

        public int OfferInformationQuickCashDetailKey { get; set; }

        public int OfferInformationKey { get; set; }

        public double? InterestRate { get; set; }

        public double? RequestedAmount { get; set; }

        public int? RateConfigurationKey { get; set; }

        public bool? Disbursed { get; set; }

        public int? QuickCashPaymentTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferInformationQuickCashDetailKey =  key;
        }
    }
}