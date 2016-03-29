using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BulkBatchParameterDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public BulkBatchParameterDataModel(int bulkBatchKey, string parameterName, string parameterValue)
        {
            this.BulkBatchKey = bulkBatchKey;
            this.ParameterName = parameterName;
            this.ParameterValue = parameterValue;
		
        }
		[JsonConstructor]
        public BulkBatchParameterDataModel(int bulkBatchParameterKey, int bulkBatchKey, string parameterName, string parameterValue)
        {
            this.BulkBatchParameterKey = bulkBatchParameterKey;
            this.BulkBatchKey = bulkBatchKey;
            this.ParameterName = parameterName;
            this.ParameterValue = parameterValue;
		
        }		

        public int BulkBatchParameterKey { get; set; }

        public int BulkBatchKey { get; set; }

        public string ParameterName { get; set; }

        public string ParameterValue { get; set; }

        public void SetKey(int key)
        {
            this.BulkBatchParameterKey =  key;
        }
    }
}