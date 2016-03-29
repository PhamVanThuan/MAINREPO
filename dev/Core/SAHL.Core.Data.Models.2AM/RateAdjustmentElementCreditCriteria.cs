using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RateAdjustmentElementCreditCriteriaDataModel :  IDataModel
    {
        public RateAdjustmentElementCreditCriteriaDataModel(int rateAdjustmentElementKey, int creditCriteriaKey)
        {
            this.RateAdjustmentElementKey = rateAdjustmentElementKey;
            this.CreditCriteriaKey = creditCriteriaKey;
		
        }		

        public int RateAdjustmentElementKey { get; set; }

        public int CreditCriteriaKey { get; set; }
    }
}