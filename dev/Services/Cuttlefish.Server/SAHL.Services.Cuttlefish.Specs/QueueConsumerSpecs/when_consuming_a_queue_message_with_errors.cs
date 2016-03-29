using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using RabbitMQ.Client.Events;
using SAHL.Core.Logging;
using SAHL.Services.Cuttlefish.Managers;
using SAHL.Services.Cuttlefish.Specs.Fakes;
using System;
using System.Text;

namespace SAHL.Services.Cuttlefish.Specs.QueueConsumerSpecs
{
    public class when_consuming_a_queue_message_with_errors : WithFakes
    {
        private static IQueueConsumer consumer;
        private static IQueueConnectionFactory instanceFactory;
        private static IQueueConnection connection;
        private static string serverName;
        private static string exchangeName;
        private static string queueName;
        private static string username;
        private static string password;
        private static Func<bool> cancellationToken;
        private static Action<string> workAction;
        private static BasicDeliverEventArgs queueMessageEvent;
        private static int numLoops = 0;
        private static bool workWasActioned;
        private static string processedMessage;
        private static ILogger logger;
        private static ILoggerSource loggerSource;

        private Establish context = () =>
        {
            serverName = "SomeServer";
            exchangeName = "SomeExchange";
            queueName = "SomeQueue";
            username = "SomeUser";
            password = "SomePassword";

            logger = An<ILogger>();
            loggerSource = An<ILoggerSource>();

            // only do one loop of consuming then exit
            cancellationToken = () =>
            {
                if (numLoops == 0)
                {
                    numLoops++;
                    return false;
                }
                else
                {
                    return true;
                }
            };
            workAction = (message) => { workWasActioned = true; throw new Exception(); };
            queueMessageEvent = new BasicDeliverEventArgs("consumerTag", 1234, false, exchangeName, "#", null, Encoding.UTF8.GetBytes("Some Message Body"));
            connection = new FakeConnection(queueMessageEvent);
            instanceFactory = new FakeConnectionFactory(connection);

            consumer = new QueueConsumer(instanceFactory, serverName, exchangeName, queueName, username, password, workAction, cancellationToken, logger, loggerSource);
        };

        private Because of = () =>
        {
            consumer.Consume();
        };

        private It should_dequeue_a_message = () =>
        {
            ((FakeConnection)connection).DequeueWasCalled.ShouldBeTrue();
        };

        private It should_process_the_message = () =>
        {
            workWasActioned.ShouldBeTrue();
        };

        private It should_send_a_nack_and_requeue = () =>
        {
            ((FakeConnection)connection).NackWasCalled.ShouldBeTrue();
        };
    }
}