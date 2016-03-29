using SAHL.Core.Messaging.Shared;
using System;

namespace SAHL.Core.Logging.Messages
{
    public interface IBaseMessage : IMessage
    {
        string Application { get; }

        string MachineName { get; }

        DateTime MessageDate { get; }

        string Source { get; }

        string User { get; }
    }
}