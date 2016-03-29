using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferExpenseOfferInformationQuickCashDetailDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferExpenseOfferInformationQuickCashDetailDataModel(int offerExpenseKey, int offerInformationQuickCashDetailKey)
        {
            this.OfferExpenseKey = offerExpenseKey;
            this.OfferInformationQuickCashDetailKey = offerInformationQuickCashDetailKey;
		
        }
		[JsonConstructor]
        public OfferExpenseOfferInformationQuickCashDetailDataModel(int offerExpenseOfferInformationQuickCashDetailKey, int offerExpenseKey, int offerInformationQuickCashDetailKey)
        {
            this.OfferExpenseOfferInformationQuickCashDetailKey = offerExpenseOfferInformationQuickCashDetailKey;
            this.OfferExpenseKey = offerExpenseKey;
            this.OfferInformationQuickCashDetailKey = offerInformationQuickCashDetailKey;
		
        }		

        public int OfferExpenseOfferInformationQuickCashDetailKey { get; set; }

        public int OfferExpenseKey { get; set; }

        public int OfferInformationQuickCashDetailKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferExpenseOfferInformationQuickCashDetailKey =  key;
        }
    }
}