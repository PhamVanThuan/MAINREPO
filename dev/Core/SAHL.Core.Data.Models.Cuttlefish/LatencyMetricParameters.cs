using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class LatencyMetricParametersDataModel :  IDataModel
    {
        public LatencyMetricParametersDataModel(int latencyMetricMessage_id, string parameterKey, string parameterValue)
        {
            this.LatencyMetricMessage_id = latencyMetricMessage_id;
            this.ParameterKey = parameterKey;
            this.ParameterValue = parameterValue;
		
        }		

        public int LatencyMetricMessage_id { get; set; }

        public string ParameterKey { get; set; }

        public string ParameterValue { get; set; }
    }
}