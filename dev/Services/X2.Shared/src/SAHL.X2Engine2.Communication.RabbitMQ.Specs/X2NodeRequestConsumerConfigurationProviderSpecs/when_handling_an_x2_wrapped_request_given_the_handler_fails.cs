using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.X2Engine2.Node.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.X2Engine2.Communication.RabbitMQ.Specs.X2NodeRequestConsumerConfigurationProviderSpecs
{
    public class when_handling_an_x2_wrapped_request_given_the_handler_fails : WithFakes
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
            automocker.ClassUnderTest.GetConsumers().First().WorkAction("{'name'/'bob'}");
        };

        private It should_log_the_error = () =>
        {
            automocker.Get<IRawLogger>().WasToldTo(x => x.LogErrorWithException(Param.IsAny<LogLevel>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<Exception>(), null));
        };
    }
}