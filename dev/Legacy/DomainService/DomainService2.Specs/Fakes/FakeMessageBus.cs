using SAHL.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Specs.Fakes
{
    public class FakeMessageBus : IMessageBus
    {
        public void Publish<T>(T message) where T : class, SAHL.Shared.Messages.IMessage
        {

        }

        public void Subscribe<T>(System.Action<T> handler) where T : class, SAHL.Shared.Messages.IMessage
        {

        }
    }
}
