using SAHL.Core.Messaging.Shared;
using System;

namespace SAHL.Batch.Common.MessageForwarding
{
    public interface IForwardingMessageBus : IDisposable
    {
		void Publish<T>(T message)
			where T : class;

        void Subscribe<T>(Action<T> subscriptionHandler) 
			where T : class;
    }
}