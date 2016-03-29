using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class ThroughputMetricParametersDataModel :  IDataModel
    {
        public ThroughputMetricParametersDataModel(int throughputMetricMessage_id, string parameterKey, string parameterValue)
        {
            this.ThroughputMetricMessage_id = throughputMetricMessage_id;
            this.ParameterKey = parameterKey;
            this.ParameterValue = parameterValue;
		
        }		

        public int ThroughputMetricMessage_id { get; set; }

        public string ParameterKey { get; set; }

        public string ParameterValue { get; set; }
    }
}