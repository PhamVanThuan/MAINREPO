using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BulkBatchStatusDataModel :  IDataModel
    {
        public BulkBatchStatusDataModel(int bulkBatchStatusKey, string description)
        {
            this.BulkBatchStatusKey = bulkBatchStatusKey;
            this.Description = description;
		
        }		

        public int BulkBatchStatusKey { get; set; }

        public string Description { get; set; }
    }
}