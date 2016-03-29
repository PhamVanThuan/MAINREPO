using Machine.Fakes;
using Machine.Specifications;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SAHL.Core.Messaging.RabbitMQ.Specs.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Messaging.RabbitMQ.Specs.ConsumerSpecs
{
    public class when_consuming : WithFakes
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

        Establish context = () =>
        {
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

            consumer = new QueueConsumer(connection, exchangeName, queueName, workAction, isCancelled, "consumerid", null, null);
        };

        Because of = () =>
        {
            consumer.Consume();
        };

        It should_dequeue_a_message = () =>
        {
            innerConsumer.DequeueTimes.ShouldEqual(1);
        };

        It should_have_performed_the_work_action = () =>
        {
            wasWorkPerformed.ShouldBeTrue();
        };

        It should_ack_the_message = () =>
        {
            model.WasToldTo(x => x.BasicAck(Param.IsAny<ulong>(), Param.IsAny<bool>()));
        };
    }
}
