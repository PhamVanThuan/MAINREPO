using SAHL.Core.Messaging.Shared;
using System;

namespace SAHL.Core.Events
{
    public interface IEvent : IMessage
    {
        string Name { get; }

        DateTime Date { get; }

        string ClassName { get; }
    }
}