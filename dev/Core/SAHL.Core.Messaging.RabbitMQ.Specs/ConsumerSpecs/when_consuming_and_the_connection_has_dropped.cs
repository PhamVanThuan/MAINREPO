using Machine.Fakes;
using Machine.Specifications;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SAHL.Core.Logging;
using SAHL.Core.Messaging.RabbitMQ.Specs.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Messaging.RabbitMQ.Specs.ConsumerSpecs
{
    public class when_consuming_and_the_connection_has_dropped : WithFakes
    {
        static IQueueConnection connection;
        static IRabbitMQModel model;
        static FakeRabbitMQConsumer innerConsumer;

        static QueueConsumer consumer;
        static Action<string> workAction;
        static Func<bool> isCancelled;
        static string exchangeName;
        static string queueName;
        static int timesInLoop;
        static bool wasWorkPerformed;
        static ILogger logger;
        static ILoggerSource logSource;

        Establish context = () =>
        {
            logger = An<ILogger>();
            logSource = An<ILoggerSource>();

            workAction = (message) => { wasWorkPerformed = true; };
            isCancelled = () => 
            {
                timesInLoop++;
                if (timesInLoop > 1)
                    return true;
                else
                    return false;
            };

            exchangeName = "exchange";
            queueName = "queue";

            connection = An<IQueueConnection>();
            model = An<IRabbitMQModel>();

            connection.WhenToldTo(x => x.CreateModel())
                .Return(model);

            connection.WhenToldTo(x => x.IsConnected())
                .Return(true);

            string messageBody = "hello world";
            byte[] messageBytes = Encoding.UTF8.GetBytes(messageBody);
            innerConsumer = new FakeRabbitMQConsumer(new BasicDeliverEventArgs("ct", 12345, false, exchangeName, "#",null, messageBytes));

            model.WhenToldTo(x => x.SetupConsumerForQueue(queueName, false))
            .Return(innerConsumer);

            consumer = new QueueConsumer(connection, exchangeName, queueName, workAction, isCancelled, "consumerid", logger, logSource);

            connection.WhenToldTo(x => x.IsConnected())
            .Return(false);
        };

        Because of = () =>
        {

            consumer.Consume();
        };

        It should_try_reconnect = () =>
        {
            connection.WasToldTo(x => x.Reconnect());
        };

        It should_not_dequeue_a_message = () =>
        {
            innerConsumer.DequeueTimes.ShouldEqual(0);
        };

        It should_not_have_performed_the_work_action = () =>
        {
            wasWorkPerformed.ShouldBeFalse();
        };

        It should_not_ack_the_message = () =>
        {
            model.WasNotToldTo(x => x.BasicAck(Param.IsAny<ulong>(), Param.IsAny<bool>()));
        };

        It should_not_nack_the_message = () =>
        {
            model.WasNotToldTo(x => x.BasicNack(Param.IsAny<ulong>(), Param.IsAny<bool>(), Param.IsAny<bool>()));
        };
    }
}
