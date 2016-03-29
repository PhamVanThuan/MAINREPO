using EasyNetQ;
using EasyNetQ.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Communication
{
    public class EasyNetQMessageBus : MarshalByRefObject, IMessageBus
    {
        private IMessageBusConfigurationProvider config;
        private IBus serviceBus;

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpointName">Every consumer must have a unique EP ie rabbitMQ://MachineName/ApplicationName</param>
        public EasyNetQMessageBus(IMessageBusConfigurationProvider config)
        {
            this.config = config;
            var connectionString = String.Format("host={0};username={1};password={2}", this.config.MessageServer, this.config.UserName, this.config.Password);
            serviceBus = RabbitHutch.CreateBus(connectionString, x =>
                {
                    x.Register<ISerializer>(y => { return new JsonSerializer(new TypeNameSerializer()); });
                    x.Register<IEasyNetQLogger>( y => { return new ConsoleLogger();});

                });
        }

        public void Publish<T>(T message) where T : class, SAHL.Shared.Messages.IMessage
        {
            serviceBus.Publish<T>(message);
        }

        public void Subscribe<T>(Action<T> handler) where T : class, Shared.Messages.IMessage
        {
            serviceBus.Subscribe(this.config.QueueName, handler);
        }
    }
}