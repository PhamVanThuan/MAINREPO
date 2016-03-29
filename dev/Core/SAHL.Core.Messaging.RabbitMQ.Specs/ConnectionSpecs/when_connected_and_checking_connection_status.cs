using Machine.Fakes;
using Machine.Specifications;
using RabbitMQ.Client;
using SAHL.Core.Logging;

namespace SAHL.Core.Messaging.RabbitMQ.Specs.ConnectionSpecs
{
    internal class when_connected_and_checking_connection_status : WithFakes
    {
        private static IQueueConnection connection;
        private static IRabbitMQConnectionFactoryFactory factoryFactory;
        private static IRabbitMQConnectionFactory factory;
        private static IConnection underlyingConnection;
        private static ILogger logger;
        private static ILoggerSource loggerSource;
        private static bool isConnected;

        private Establish context = () =>
        {
            logger = An<ILogger>();
            loggerSource = An<ILoggerSource>();

            factory = An<IRabbitMQConnectionFactory>();
            factoryFactory = An<IRabbitMQConnectionFactoryFactory>();
            factoryFactory.WhenToldTo(x => x.CreateFactory())
                .Return(factory);

            underlyingConnection = An<IConnection>();
            underlyingConnection.WhenToldTo(x => x.IsOpen)
                .Return(true);

            factory.WhenToldTo(x => x.CreateConnection())
                .Return(underlyingConnection);

            connection = new QueueConnection(logger, loggerSource, factoryFactory);
        };

        private Because of = () =>
        {
            connection.Connect();
            isConnected = connection.IsConnected();
        };

        private It should_indicate_the_connection_is_established= () =>
         {
             isConnected.ShouldBeTrue();
         };
    }
}