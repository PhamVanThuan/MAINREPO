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
    public class when_consuming_and_the_connection_has_changed : WithFakes
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

            connection.WhenToldTo(x => x.ConnectionId)
            .Return(Guid.NewGuid());
        };

        Because of = () =>
        {

            consumer.Consume();
        };

        It should_dispose_the_model = () =>
        {
            model.WasToldTo(x => x.Dispose());
        };

        It should_create_a_rabbitmq_model = () =>
        {
            connection.WasToldTo(x => x.CreateModel());
        };

        It should_declare_a_direct_durable_exchange = () =>
        {
            model.WasToldTo(x => x.ExchangeDeclare(exchangeName, ExchangeType.Direct, true));
        };

        It should_declare_a_durable_queue = () =>
        {
            model.WasToldTo(x => x.QueueDeclare(queueName, true, false, false));
        };

        It should_bind_the_queue_to_the_exchange_with_the_default_routing_key = () =>
        {
            model.WasToldTo(x => x.QueueBind(queueName, exchangeName, "#"));
        };

        It should_set_the_exchange_to_round_robin_for_this_consumer_by_using_a_prefetch_of_one = () =>
        {
            model.WasToldTo(x => x.SetQos(0, 1, false));
        };

        It should_create_and_bind_a_consumer_for_the_queue_that_acks_and_nacks_manually = () =>
        {
            model.WasToldTo(x => x.SetupConsumerForQueue(queueName, false));
        };
    }
}
