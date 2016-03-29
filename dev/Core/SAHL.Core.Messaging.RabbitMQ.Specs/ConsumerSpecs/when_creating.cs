using Machine.Fakes;
using Machine.Specifications;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Messaging.RabbitMQ.Specs.ConsumerSpecs
{
    public class when_creating : WithFakes
    {
        static IQueueConnection connection;
        static IRabbitMQModel model;

        static QueueConsumer consumer;
        static Action<string> workAction;
        static Func<bool> isCancelled;
        static string exchangeName;
        static string queueName;

        Establish context = () =>
        {
            workAction = (message) => { };
            isCancelled = () => { return false; };
            connection = An<IQueueConnection>();
            model = An<IRabbitMQModel>();

            connection.WhenToldTo(x => x.CreateModel())
                .Return(model);

            exchangeName = "exchange";
            queueName = "queue";
        };

        Because of = () =>
        {
            consumer = new QueueConsumer(connection, exchangeName, queueName, workAction, isCancelled, "consumerid", null, null);
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
