using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BatchServiceTypeDataModel :  IDataModel
    {
        public BatchServiceTypeDataModel(int batchServiceTypeKey, string description)
        {
            this.BatchServiceTypeKey = batchServiceTypeKey;
            this.Description = description;
		
        }		

        public int BatchServiceTypeKey { get; set; }

        public string Description { get; set; }
    }
}