using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class TimeUnitReferenceDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public TimeUnitReferenceDataModel(string timeUnit, int? throughputMetricMessage_id)
        {
            this.TimeUnit = timeUnit;
            this.ThroughputMetricMessage_id = throughputMetricMessage_id;
		
        }

        public TimeUnitReferenceDataModel(int id, string timeUnit, int? throughputMetricMessage_id)
        {
            this.Id = id;
            this.TimeUnit = timeUnit;
            this.ThroughputMetricMessage_id = throughputMetricMessage_id;
		
        }		

        public int Id { get; set; }

        public string TimeUnit { get; set; }

        public int? ThroughputMetricMessage_id { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}