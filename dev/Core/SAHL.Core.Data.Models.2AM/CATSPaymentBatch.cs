using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CATSPaymentBatchDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CATSPaymentBatchDataModel(int cATSPaymentBatchTypeKey, DateTime createdDate, DateTime? processedDate, int cATSPaymentBatchStatusKey, int? cATSFileSequenceNo, string cATSFileName)
        {
            this.CATSPaymentBatchTypeKey = cATSPaymentBatchTypeKey;
            this.CreatedDate = createdDate;
            this.ProcessedDate = processedDate;
            this.CATSPaymentBatchStatusKey = cATSPaymentBatchStatusKey;
            this.CATSFileSequenceNo = cATSFileSequenceNo;
            this.CATSFileName = cATSFileName;
		
        }
		[JsonConstructor]
        public CATSPaymentBatchDataModel(int cATSPaymentBatchKey, int cATSPaymentBatchTypeKey, DateTime createdDate, DateTime? processedDate, int cATSPaymentBatchStatusKey, int? cATSFileSequenceNo, string cATSFileName)
        {
            this.CATSPaymentBatchKey = cATSPaymentBatchKey;
            this.CATSPaymentBatchTypeKey = cATSPaymentBatchTypeKey;
            this.CreatedDate = createdDate;
            this.ProcessedDate = processedDate;
            this.CATSPaymentBatchStatusKey = cATSPaymentBatchStatusKey;
            this.CATSFileSequenceNo = cATSFileSequenceNo;
            this.CATSFileName = cATSFileName;
		
        }		

        public int CATSPaymentBatchKey { get; set; }

        public int CATSPaymentBatchTypeKey { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public int CATSPaymentBatchStatusKey { get; set; }

        public int? CATSFileSequenceNo { get; set; }

        public string CATSFileName { get; set; }

        public void SetKey(int key)
        {
            this.CATSPaymentBatchKey =  key;
        }
    }
}