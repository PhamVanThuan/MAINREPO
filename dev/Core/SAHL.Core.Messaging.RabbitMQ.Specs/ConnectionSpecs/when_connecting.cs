using Machine.Fakes;
using Machine.Specifications;
using System;

namespace SAHL.Core.Messaging.RabbitMQ.Specs.ConnectionSpecs
{
    public class when_connecting : WithFakes
    {
        private static IQueueConnection connection;
        private static IRabbitMQConnectionFactoryFactory factoryFactory;
        private static IRabbitMQConnectionFactory factory;

        private Establish context = () =>
         {
             factory = An<IRabbitMQConnectionFactory>();
             factoryFactory = An<IRabbitMQConnectionFactoryFactory>();
             factoryFactory.WhenToldTo(x => x.CreateFactory())
                 .Return(factory);

             connection = new QueueConnection(null, null, factoryFactory);
         };

        private Because of = () =>
         {
             connection.Connect();
         };

        private It should_connect_to_rabbit_mq = () =>
         {
             factoryFactory.WasToldTo(x => x.CreateFactory());
             factory.WasToldTo(x => x.CreateConnection());
         };

        private It should_set_a_non_empty_guid_for_the_connection = () =>
         {
             connection.ConnectionId.ShouldNotEqual(Guid.Empty);
         };
    }
}