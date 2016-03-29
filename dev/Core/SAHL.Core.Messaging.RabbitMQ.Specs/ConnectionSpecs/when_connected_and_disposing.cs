using Machine.Fakes;
using Machine.Specifications;
using RabbitMQ.Client;
using SAHL.Core.Logging;

namespace SAHL.Core.Messaging.RabbitMQ.Specs.ConnectionSpecs
{
    internal class when_connected_and_disposing : WithFakes
    {
        private static IQueueConnection connection;
        private static IRabbitMQConnectionFactoryFactory factoryFactory;
        private static IRabbitMQConnectionFactory factory;
        private static IConnection underlyingConnection;
        private static ILogger logger;
        private static ILoggerSource loggerSource;
        private static IModel model;

        private Establish context = () =>
        {
            logger = An<ILogger>();
            loggerSource = An<ILoggerSource>();

            factory = An<IRabbitMQConnectionFactory>();
            factoryFactory = An<IRabbitMQConnectionFactoryFactory>();
            factoryFactory.WhenToldTo(x => x.CreateFactory())
                .Return(factory);

            underlyingConnection = An<IConnection>();
            factory.WhenToldTo(x => x.CreateConnection())
                .Return(underlyingConnection);

            connection = new QueueConnection(logger, loggerSource, factoryFactory);
        };

        private Because of = () =>
        {
            connection.Connect();
            connection.Dispose();
        };

        private It should_close_the_underlying_connection = () =>
         {
             underlyingConnection.WasToldTo(x => x.Close());
         };

        private It should_dispose_the_underlying_connection = () =>
        {
            underlyingConnection.WasToldTo(x => x.Dispose());
        };
    }
}