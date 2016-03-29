using System;

namespace SAHL.Core.Messaging.Shared
{
    public interface IMessage
    {
        Guid Id { get; }
    }
}