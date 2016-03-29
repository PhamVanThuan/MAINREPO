using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BatchLoanTransactionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public BatchLoanTransactionDataModel(int batchTransactionKey, int loanTransactionNumber)
        {
            this.BatchTransactionKey = batchTransactionKey;
            this.LoanTransactionNumber = loanTransactionNumber;
		
        }
		[JsonConstructor]
        public BatchLoanTransactionDataModel(int batchLoanTransactionKey, int batchTransactionKey, int loanTransactionNumber)
        {
            this.BatchLoanTransactionKey = batchLoanTransactionKey;
            this.BatchTransactionKey = batchTransactionKey;
            this.LoanTransactionNumber = loanTransactionNumber;
		
        }		

        public int BatchLoanTransactionKey { get; set; }

        public int BatchTransactionKey { get; set; }

        public int LoanTransactionNumber { get; set; }

        public void SetKey(int key)
        {
            this.BatchLoanTransactionKey =  key;
        }
    }
}