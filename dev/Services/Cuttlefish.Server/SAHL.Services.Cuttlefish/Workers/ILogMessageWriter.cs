using SAHL.Core.Data.Models.Cuttlefish;
using System.Collections.Generic;

namespace SAHL.Services.Cuttlefish.Workers
{
    public interface ILogMessageWriter
    {
        void WriteMessage(LogMessageDataModel logMessage, Dictionary<string, string> parameters);
    }
}