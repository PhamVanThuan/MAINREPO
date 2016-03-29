using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferInformationInterestOnlyDataModel :  IDataModel
    {
        public OfferInformationInterestOnlyDataModel(int offerInformationKey, double? installment, DateTime? maturityDate)
        {
            this.OfferInformationKey = offerInformationKey;
            this.Installment = installment;
            this.MaturityDate = maturityDate;
		
        }		

        public int OfferInformationKey { get; set; }

        public double? Installment { get; set; }

        public DateTime? MaturityDate { get; set; }
    }
}