using Machine.Fakes;
using Machine.Specifications;
using SAHL.X2Engine2.Node.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System.Collections.Generic;

namespace SAHL.X2Engine2.Communication.RabbitMQ.Specs.X2NodeRequestConsumerConfigurationProviderSpecs
{
    public class when_initialising : WithFakes
    {
        private static AutoMocker<X2NodeRequestConsumerConfigurationProvider> automocker = new NSubstituteAutoMocker<X2NodeRequestConsumerConfigurationProvider>();
        private static List<X2WorkflowConfiguration> workflowConfigurations = new List<X2WorkflowConfiguration>();

        private Establish context = () =>
        {
            workflowConfigurations.Add(new X2WorkflowConfiguration("process", "workflow", 1, 1));
            automocker.Get<IX2NodeConfigurationProvider>().WhenToldTo(x => x.GetWorkflowConfigurations()).Return(workflowConfigurations);
        };

        private Because of = () =>
        {
            automocker.ClassUnderTest.Initialise();
        };

        private It should_get_the_supported_workflows = () =>
        {
            automocker.Get<IX2NodeConfigurationProvider>().WasToldTo(x => x.GetWorkflowConfigurations());
        };

        private It should_init_the_queue_consumers = () =>
        {
            automocker.ClassUnderTest.GetConsumers().Count.ShouldEqual(2);
        };
    }
}