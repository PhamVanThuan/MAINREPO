using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BulkBatchTypeDataModel :  IDataModel
    {
        public BulkBatchTypeDataModel(int bulkBatchTypeKey, string description, string filePath)
        {
            this.BulkBatchTypeKey = bulkBatchTypeKey;
            this.Description = description;
            this.FilePath = filePath;
		
        }		

        public int BulkBatchTypeKey { get; set; }

        public string Description { get; set; }

        public string FilePath { get; set; }
    }
}