using Newtonsoft.Json;
using System;

namespace SAHL.Shared.Messages.Metrics
{
    public class LatencyMetricMessage : MessageBase
    {
        [JsonProperty]
        public virtual DateTime StartTime { get; set; }

        [JsonProperty]
        public virtual TimeSpan Duration { get; set; }
    }
}