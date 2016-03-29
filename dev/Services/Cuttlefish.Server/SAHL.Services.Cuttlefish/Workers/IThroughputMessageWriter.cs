using SAHL.Core.Data.Models.Cuttlefish;
using System.Collections.Generic;

namespace SAHL.Services.Cuttlefish.Workers
{
    public interface IThroughputMessageWriter
    {
        void WriteMessage(ThroughputMetricMessageDataModel throughputMessage, Dictionary<string, string> parameters);
    }
}