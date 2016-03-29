using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HOCTransactionsDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public HOCTransactionsDataModel(decimal accountKey, string action, DateTime? uploadDate, string reason, DateTime? insertDate)
        {
            this.AccountKey = accountKey;
            this.Action = action;
            this.UploadDate = uploadDate;
            this.Reason = reason;
            this.InsertDate = insertDate;
		
        }
		[JsonConstructor]
        public HOCTransactionsDataModel(decimal transactionKey, decimal accountKey, string action, DateTime? uploadDate, string reason, DateTime? insertDate)
        {
            this.TransactionKey = transactionKey;
            this.AccountKey = accountKey;
            this.Action = action;
            this.UploadDate = uploadDate;
            this.Reason = reason;
            this.InsertDate = insertDate;
		
        }		

        public decimal TransactionKey { get; set; }

        public decimal AccountKey { get; set; }

        public string Action { get; set; }

        public DateTime? UploadDate { get; set; }

        public string Reason { get; set; }

        public DateTime? InsertDate { get; set; }

        public void SetKey(decimal key)
        {
            this.TransactionKey =  key;
        }
    }
}