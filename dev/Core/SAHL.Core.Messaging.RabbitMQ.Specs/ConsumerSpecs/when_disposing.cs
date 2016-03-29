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
    public class when_disposing : WithFakes
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

            consumer = new QueueConsumer(connection, exchangeName, queueName, workAction, isCancelled, "consumerid", null, null);
        };

        Because of = () =>
        {
            consumer.Dispose();
        };

        It should_dispose_of_the_model = () =>
        {
            model.WasToldTo(x => x.Dispose());
        };


    }
}
