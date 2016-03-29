using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BulkBatchLogDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public BulkBatchLogDataModel(int bulkBatchKey, string description, int messageTypeKey, string messageReference, string messageReferenceKey)
        {
            this.BulkBatchKey = bulkBatchKey;
            this.Description = description;
            this.MessageTypeKey = messageTypeKey;
            this.MessageReference = messageReference;
            this.MessageReferenceKey = messageReferenceKey;
		
        }
		[JsonConstructor]
        public BulkBatchLogDataModel(int bulkBatchLogKey, int bulkBatchKey, string description, int messageTypeKey, string messageReference, string messageReferenceKey)
        {
            this.BulkBatchLogKey = bulkBatchLogKey;
            this.BulkBatchKey = bulkBatchKey;
            this.Description = description;
            this.MessageTypeKey = messageTypeKey;
            this.MessageReference = messageReference;
            this.MessageReferenceKey = messageReferenceKey;
		
        }		

        public int BulkBatchLogKey { get; set; }

        public int BulkBatchKey { get; set; }

        public string Description { get; set; }

        public int MessageTypeKey { get; set; }

        public string MessageReference { get; set; }

        public string MessageReferenceKey { get; set; }

        public void SetKey(int key)
        {
            this.BulkBatchLogKey =  key;
        }
    }
}