using Machine.Fakes;
using Machine.Specifications;
using RabbitMQ.Client;
using SAHL.Core.Logging;

namespace SAHL.Core.Messaging.RabbitMQ.Specs.ConnectionSpecs
{
    internal class when_not_connected_and_creating_a_model : WithFakes
    {
        private static IQueueConnection connection;
        private static IRabbitMQConnectionFactoryFactory factoryFactory;
        private static IRabbitMQConnectionFactory factory;
        private static IConnection underlyingConnection;
        private static ILogger logger;
        private static ILoggerSource loggerSource;
        private static IRabbitMQModel model;

        private Establish context = () =>
        {
            logger = An<ILogger>();
            loggerSource = An<ILoggerSource>();

            factory = An<IRabbitMQConnectionFactory>();
            factoryFactory = An<IRabbitMQConnectionFactoryFactory>();
            factoryFactory.WhenToldTo(x => x.CreateFactory())
                .Return(factory);

            underlyingConnection = null;
            factory.WhenToldTo(x => x.CreateConnection())
                .Return(underlyingConnection);

            connection = new QueueConnection(logger, loggerSource, factoryFactory);
        };

        private Because of = () =>
        {
            model = connection.CreateModel();
        };

        private It should_not_create_a_model = () =>
         {
             model.ShouldBeNull();
         };
    }
}