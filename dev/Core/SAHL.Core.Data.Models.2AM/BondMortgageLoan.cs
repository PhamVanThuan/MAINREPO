using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BondMortgageLoanDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public BondMortgageLoanDataModel(int? financialServiceKey, int? bondKey)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.BondKey = bondKey;
		
        }
		[JsonConstructor]
        public BondMortgageLoanDataModel(int bondMortgageLoanKey, int? financialServiceKey, int? bondKey)
        {
            this.BondMortgageLoanKey = bondMortgageLoanKey;
            this.FinancialServiceKey = financialServiceKey;
            this.BondKey = bondKey;
		
        }		

        public int BondMortgageLoanKey { get; set; }

        public int? FinancialServiceKey { get; set; }

        public int? BondKey { get; set; }

        public void SetKey(int key)
        {
            this.BondMortgageLoanKey =  key;
        }
    }
}