using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Common.MessageForwarding
{
    public interface IForwardingQueuedHandler<TMessage, TMessageToForward> : IStoppableQueueHandler, IStartableQueueHandler
    {
        void Subscribe(TMessage message);

        void Transform(Func<TMessage, TMessageToForward> transformFunc);
    }
}
