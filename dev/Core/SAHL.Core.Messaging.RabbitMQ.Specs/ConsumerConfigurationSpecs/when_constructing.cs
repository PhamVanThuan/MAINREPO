using Machine.Fakes;
using Machine.Specifications;
using System;

namespace SAHL.Core.Messaging.RabbitMQ.Specs.ConsumerConfigurationSpecs
{
    public class when_constructing : WithFakes
    {
        private static string exchangeName;
        private static string queueName;
        private static int numInitialConsumers;
        private static Action<string> workerAction;

        private static QueueConsumerConfiguration configuration;

        private Establish context = () =>
         {
             exchangeName = "exchange";
             queueName = "queueName";
             numInitialConsumers = 2;
             workerAction = (message) => { };
         };

        private Because of = () =>
         {
             configuration = new QueueConsumerConfiguration(exchangeName, queueName, numInitialConsumers, workerAction);
         };

        private It should_have_populated_the_exchangename_property = () =>
         {
             configuration.ExchangeName.ShouldEqual(exchangeName);
         };

        private It should_have_populated_the_queuename_property = () =>
         {
             configuration.QueueName.ShouldEqual(queueName);
         };

        private It should_have_populated_the_initialnumberofconsumers_property = () =>
         {
             configuration.InitialNumberOfConsumers.ShouldEqual(numInitialConsumers);
         };

        private It should_have_populated_the_workaction_property = () =>
         {
             configuration.WorkAction.ShouldEqual(workerAction);
         };
    }
}