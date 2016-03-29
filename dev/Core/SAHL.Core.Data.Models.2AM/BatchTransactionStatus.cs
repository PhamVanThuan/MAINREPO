using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BatchTransactionStatusDataModel :  IDataModel
    {
        public BatchTransactionStatusDataModel(int batchTransactionStatusKey, string description)
        {
            this.BatchTransactionStatusKey = batchTransactionStatusKey;
            this.Description = description;
		
        }		

        public int BatchTransactionStatusKey { get; set; }

        public string Description { get; set; }
    }
}