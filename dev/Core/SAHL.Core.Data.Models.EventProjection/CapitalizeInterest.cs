using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.EventProjection
{
    [Serializable]
    public partial class CapitalizeInterestDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CapitalizeInterestDataModel(int financialServiceKey, decimal amount, DateTime eventEffectiveDate)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.Amount = amount;
            this.EventEffectiveDate = eventEffectiveDate;
		
        }
		[JsonConstructor]
        public CapitalizeInterestDataModel(int pK, int financialServiceKey, decimal amount, DateTime eventEffectiveDate)
        {
            this.PK = pK;
            this.FinancialServiceKey = financialServiceKey;
            this.Amount = amount;
            this.EventEffectiveDate = eventEffectiveDate;
		
        }		

        public int PK { get; set; }

        public int FinancialServiceKey { get; set; }

        public decimal Amount { get; set; }

        public DateTime EventEffectiveDate { get; set; }

        public void SetKey(int key)
        {
            this.PK =  key;
        }
    }
}