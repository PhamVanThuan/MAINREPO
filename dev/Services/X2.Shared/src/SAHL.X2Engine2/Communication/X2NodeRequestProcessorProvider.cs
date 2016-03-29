using SAHL.Core;
using SAHL.Core.Messaging;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Communication
{
    public class X2NodeRequestProcessorProvider : IMessageProcessorProvider
    {
        private IIocContainer container;

        public X2NodeRequestProcessorProvider(IIocContainer container)
        {
            this.container = container;
        }

        public dynamic GetMessageProcessor(object message)
        {
            return this.container.GetInstance<IMessageProcessor<IX2Request>>();
        }
    }
}