﻿using SAHL.Core.Messaging.Shared;
using SAHL.Core.SystemMessages;

namespace SAHL.Core.Messaging
{
    public interface IMessageProcessor
    {
    }

    public interface IMessageProcessor<in T> : IMessageProcessor
        where T : IMessage
    {
        ISystemMessageCollection ProcessMessage(T message);
    }
}