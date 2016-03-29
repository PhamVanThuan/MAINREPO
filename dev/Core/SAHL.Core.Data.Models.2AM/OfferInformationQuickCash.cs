using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferInformationQuickCashDataModel :  IDataModel
    {
        public OfferInformationQuickCashDataModel(int offerInformationKey, double? creditApprovedAmount, int? term, double? creditUpfrontApprovedAmount)
        {
            this.OfferInformationKey = offerInformationKey;
            this.CreditApprovedAmount = creditApprovedAmount;
            this.Term = term;
            this.CreditUpfrontApprovedAmount = creditUpfrontApprovedAmount;
		
        }		

        public int OfferInformationKey { get; set; }

        public double? CreditApprovedAmount { get; set; }

        public int? Term { get; set; }

        public double? CreditUpfrontApprovedAmount { get; set; }
    }
}