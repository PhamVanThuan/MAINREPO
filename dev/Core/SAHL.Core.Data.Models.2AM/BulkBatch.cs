using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BulkBatchDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public BulkBatchDataModel(int bulkBatchStatusKey, string description, int bulkBatchTypeKey, int? identifierReferenceKey, DateTime effectiveDate, DateTime? startDateTime, DateTime? completedDateTime, string fileName, string userID, DateTime? changeDate)
        {
            this.BulkBatchStatusKey = bulkBatchStatusKey;
            this.Description = description;
            this.BulkBatchTypeKey = bulkBatchTypeKey;
            this.IdentifierReferenceKey = identifierReferenceKey;
            this.EffectiveDate = effectiveDate;
            this.StartDateTime = startDateTime;
            this.CompletedDateTime = completedDateTime;
            this.FileName = fileName;
            this.UserID = userID;
            this.ChangeDate = changeDate;
		
        }
		[JsonConstructor]
        public BulkBatchDataModel(int bulkBatchKey, int bulkBatchStatusKey, string description, int bulkBatchTypeKey, int? identifierReferenceKey, DateTime effectiveDate, DateTime? startDateTime, DateTime? completedDateTime, string fileName, string userID, DateTime? changeDate)
        {
            this.BulkBatchKey = bulkBatchKey;
            this.BulkBatchStatusKey = bulkBatchStatusKey;
            this.Description = description;
            this.BulkBatchTypeKey = bulkBatchTypeKey;
            this.IdentifierReferenceKey = identifierReferenceKey;
            this.EffectiveDate = effectiveDate;
            this.StartDateTime = startDateTime;
            this.CompletedDateTime = completedDateTime;
            this.FileName = fileName;
            this.UserID = userID;
            this.ChangeDate = changeDate;
		
        }		

        public int BulkBatchKey { get; set; }

        public int BulkBatchStatusKey { get; set; }

        public string Description { get; set; }

        public int BulkBatchTypeKey { get; set; }

        public int? IdentifierReferenceKey { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? CompletedDateTime { get; set; }

        public string FileName { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public void SetKey(int key)
        {
            this.BulkBatchKey =  key;
        }
    }
}