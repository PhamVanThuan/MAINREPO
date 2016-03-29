using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class MessageParametersDataModel :  IDataModel
    {
        public MessageParametersDataModel(int logMessage_id, string parameterKey, string parameterValue)
        {
            this.LogMessage_id = logMessage_id;
            this.ParameterKey = parameterKey;
            this.ParameterValue = parameterValue;
		
        }		

        public int LogMessage_id { get; set; }

        public string ParameterKey { get; set; }

        public string ParameterValue { get; set; }
    }
}