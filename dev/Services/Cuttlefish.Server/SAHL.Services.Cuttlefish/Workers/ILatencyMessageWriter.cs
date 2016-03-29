using SAHL.Core.Data.Models.Cuttlefish;
using System.Collections.Generic;

namespace SAHL.Services.Cuttlefish.Workers
{
    public interface ILatencyMessageWriter
    {
        void WriteMessage(LatencyMetricMessageDataModel latencyMessage, Dictionary<string, string> parameters);
    }
}