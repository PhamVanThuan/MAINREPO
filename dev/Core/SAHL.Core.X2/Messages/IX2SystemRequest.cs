using System;

namespace SAHL.Core.X2.Messages
{
    public interface IX2SystemRequest : IX2Request, IX2RequestForExistingInstance
    {
        DateTime ActivityTime { get; }
    }

    public interface IX2ScheduledActivityRequest
    {
    }
}