using System;
using System.Collections.Generic;

namespace SAHL.Shared.Messages.Metrics
{
    [Serializable]
    public abstract class MetricBaseMessage : MessageBase, IMetricMessage
    {
        protected MetricBaseMessage()
        {

        }

		public MetricBaseMessage(string application, string source, string machineName, string user = "", Dictionary<string, object> parameters = null)
			: base(application, source, user : user, parameters : parameters, machineName : machineName)
		{
		}

        public MetricBaseMessage(string application, string source, string user = "", Dictionary<string, object> parameters = null)
            : base(application, source, user, parameters)
        {
        }
    }
}