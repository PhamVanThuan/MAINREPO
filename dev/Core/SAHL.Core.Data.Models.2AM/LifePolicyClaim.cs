using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifePolicyClaimDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LifePolicyClaimDataModel(int financialServiceKey, int claimStatusKey, int claimTypeKey, DateTime claimDate)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.ClaimStatusKey = claimStatusKey;
            this.ClaimTypeKey = claimTypeKey;
            this.ClaimDate = claimDate;
		
        }
		[JsonConstructor]
        public LifePolicyClaimDataModel(int lifePolicyClaimKey, int financialServiceKey, int claimStatusKey, int claimTypeKey, DateTime claimDate)
        {
            this.LifePolicyClaimKey = lifePolicyClaimKey;
            this.FinancialServiceKey = financialServiceKey;
            this.ClaimStatusKey = claimStatusKey;
            this.ClaimTypeKey = claimTypeKey;
            this.ClaimDate = claimDate;
		
        }		

        public int LifePolicyClaimKey { get; set; }

        public int FinancialServiceKey { get; set; }

        public int ClaimStatusKey { get; set; }

        public int ClaimTypeKey { get; set; }

        public DateTime ClaimDate { get; set; }

        public void SetKey(int key)
        {
            this.LifePolicyClaimKey =  key;
        }
    }
}