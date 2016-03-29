using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class SuperLoRecalcDataModel :  IDataModel
    {
        public SuperLoRecalcDataModel(int financialServiceKey, DateTime? convertedDate, double? accumulatedLoyaltyBenefit, double? recalcAccumLB)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.ConvertedDate = convertedDate;
            this.AccumulatedLoyaltyBenefit = accumulatedLoyaltyBenefit;
            this.RecalcAccumLB = recalcAccumLB;
		
        }		

        public int FinancialServiceKey { get; set; }

        public DateTime? ConvertedDate { get; set; }

        public double? AccumulatedLoyaltyBenefit { get; set; }

        public double? RecalcAccumLB { get; set; }
    }
}