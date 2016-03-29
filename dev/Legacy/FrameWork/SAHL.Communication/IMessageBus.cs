using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Shared.Messages;

namespace SAHL.Communication
{
    public interface IMessageBus
    {
        void Publish<T>(T message) where T : class, IMessage;
        void Subscribe<T>(Action<T> handler) where T : class, Shared.Messages.IMessage;
    }
}
