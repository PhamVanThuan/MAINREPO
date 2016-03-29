using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Messaging;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Communication.EasyNetQ;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Specs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.X2ConsumerMonitorSpecs
{
    public class when_asked_to_check_exchange_given_active_consumer_attached : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2ConsumerMonitor> autoMocker;
        private static ITimerFactory timerFactory;
        private static IMessageBusManagementClient managementClient;
        private static X2Workflow workflow;
        private static bool result = false;

        Establish context = () =>
        {
            workflow = new X2Workflow("process", "workflow");
            var declaredWorkflowRoutes = new Dictionary<X2Workflow, List<IX2RouteEndpoint>>();
            var endPoints = new List<IX2RouteEndpoint>();
            endPoints.Add(new X2RouteEndpoint("process.workflow.user","process.workflow.user"));
            declaredWorkflowRoutes.Add(workflow, endPoints);
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2ConsumerMonitor>();
            timerFactory = new FakeTimerFactory();
            autoMocker.Inject<ITimerFactory>(timerFactory);
            autoMocker.Get<IX2QueueManager>().WhenToldTo(x => x.DeclaredWorkflowRoutes).Return(declaredWorkflowRoutes);
            autoMocker.Get<IMessageBusManagementClient>().WhenToldTo(x => x.GetQueuesWithConsumers()).Return(new List<string>(){"process.workflow.user"});
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.Initialise();
            result = autoMocker.ClassUnderTest.IsExchangeActive("process.workflow.user", workflow);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
