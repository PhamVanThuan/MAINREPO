using SAHL.Core.Services;
using System;
using System.Collections.Generic;

namespace SAHL.Core.X2.Messages
{
    public interface IX2Request : IX2Message
    {
        X2RequestType RequestType { get; }

        Guid CorrelationId { get; }

        Dictionary<string, string> MapVariables { get; }

        long InstanceId { get; }

        bool IgnoreWarnings { get; }

        IServiceRequestMetadata ServiceRequestMetadata { get; }

        object Data { get; }
    }
}