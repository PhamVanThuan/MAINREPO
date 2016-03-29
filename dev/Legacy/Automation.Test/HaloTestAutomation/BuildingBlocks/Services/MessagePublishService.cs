using BuildingBlocks.Services.Contracts;
using SAHL.Core.Identity;
using SAHL.Core.Messaging;
using SAHL.Core.Messaging.EasyNetQ;
using SAHL.Services.Capitec.Models.Shared.Capitec;
using System;
namespace BuildingBlocks.Services
{
    public sealed class MessagePublishService : IMessagePublishService
    {
        private IMessageBus bus;
        public MessagePublishService()
        {
            try {
                var messageRoute = typeof(IMessageRoute);
                var defaultRoutName = new DefaultRouteNameBuilder<MessageRoute>();
                this.bus = new MessageBus(new MessageBusConfigurationProvider()
                                            , new IMessageRouteNameBuilder[] { defaultRoutName }
                                            , new DefaultEasyNetQMessageBusSettings());
            }
            catch { }
        }
        public void Publish<T>(T message)
        {
            bus.Publish((dynamic)message);
        }
    }
}