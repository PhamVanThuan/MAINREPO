using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DisbursementTransactionTypeDataModel :  IDataModel
    {
        public DisbursementTransactionTypeDataModel(int disbursementTransactionTypeKey, string description, int? transactionTypeNumber)
        {
            this.DisbursementTransactionTypeKey = disbursementTransactionTypeKey;
            this.Description = description;
            this.TransactionTypeNumber = transactionTypeNumber;
		
        }		

        public int DisbursementTransactionTypeKey { get; set; }

        public string Description { get; set; }

        public int? TransactionTypeNumber { get; set; }
    }
}