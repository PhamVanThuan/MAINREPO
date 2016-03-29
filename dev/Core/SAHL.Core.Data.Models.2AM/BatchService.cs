using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BatchServiceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public BatchServiceDataModel(int batchServiceTypeKey, DateTime requestedDate, string requestedBy, int batchCount, byte[] fileContent, string fileName)
        {
            this.BatchServiceTypeKey = batchServiceTypeKey;
            this.RequestedDate = requestedDate;
            this.RequestedBy = requestedBy;
            this.BatchCount = batchCount;
            this.FileContent = fileContent;
            this.FileName = fileName;
		
        }
		[JsonConstructor]
        public BatchServiceDataModel(int batchServiceKey, int batchServiceTypeKey, DateTime requestedDate, string requestedBy, int batchCount, byte[] fileContent, string fileName)
        {
            this.BatchServiceKey = batchServiceKey;
            this.BatchServiceTypeKey = batchServiceTypeKey;
            this.RequestedDate = requestedDate;
            this.RequestedBy = requestedBy;
            this.BatchCount = batchCount;
            this.FileContent = fileContent;
            this.FileName = fileName;
		
        }		

        public int BatchServiceKey { get; set; }

        public int BatchServiceTypeKey { get; set; }

        public DateTime RequestedDate { get; set; }

        public string RequestedBy { get; set; }

        public int BatchCount { get; set; }

        public byte[] FileContent { get; set; }

        public string FileName { get; set; }

        public void SetKey(int key)
        {
            this.BatchServiceKey =  key;
        }
    }
}