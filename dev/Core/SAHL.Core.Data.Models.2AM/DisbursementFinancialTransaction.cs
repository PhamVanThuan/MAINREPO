using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DisbursementFinancialTransactionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DisbursementFinancialTransactionDataModel(int disbursementKey, int financialTransactionKey)
        {
            this.DisbursementKey = disbursementKey;
            this.FinancialTransactionKey = financialTransactionKey;
		
        }
		[JsonConstructor]
        public DisbursementFinancialTransactionDataModel(int disbursementFinancialTransactionKey, int disbursementKey, int financialTransactionKey)
        {
            this.DisbursementFinancialTransactionKey = disbursementFinancialTransactionKey;
            this.DisbursementKey = disbursementKey;
            this.FinancialTransactionKey = financialTransactionKey;
		
        }		

        public int DisbursementFinancialTransactionKey { get; set; }

        public int DisbursementKey { get; set; }

        public int FinancialTransactionKey { get; set; }

        public void SetKey(int key)
        {
            this.DisbursementFinancialTransactionKey =  key;
        }
    }
}