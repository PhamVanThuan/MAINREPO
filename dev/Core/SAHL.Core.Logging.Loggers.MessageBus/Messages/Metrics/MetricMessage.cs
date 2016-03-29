using System;
using System.Collections.Generic;

namespace SAHL.Core.Logging.Messages.Metrics
{
    [Serializable]
    public abstract class MetricMessage : BaseMessage, IMetricMessage
    {
        public MetricMessage(string source, string user = "", IDictionary<string, object> parameters = null)
            : base(source, user, parameters)
        {
        }
    }
}