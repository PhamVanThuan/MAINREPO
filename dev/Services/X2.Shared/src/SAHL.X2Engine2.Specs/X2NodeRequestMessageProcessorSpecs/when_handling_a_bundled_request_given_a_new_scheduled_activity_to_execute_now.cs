using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.X2NodeRequestMessageProcessorSpecs
{
    public class when_handling_a_bundled_request_given_a_new_scheduled_activity_to_execute_now : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2NodeRequestMessageProcessor> autoMocker;
        private static ISystemMessageCollection result;
        private static X2Workflow workflow;
        private static X2RouteEndpoint endpoint;
        private static X2Response response;

        private static X2BundledRequest x2Request;
        private static X2SystemRequestGroup request;

        private static long instanceId = 12;
        private static List<string> actvityNames = new List<string>() { "activity1", "activity2" };

        private Establish context = () =>
        {
            request = new  X2SystemRequestGroup(Guid.NewGuid(), null, X2RequestType.Timer, instanceId, new List<string> { "activityDataModel_Name" }, DateTime.Now);
            x2Request = new X2BundledRequest (new List<IX2Request> { request });

            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2NodeRequestMessageProcessor>();
            autoMocker.Get<IX2RequestInterrogator>().WhenToldTo(x => x.IsRequestMonitored(Arg.Is<IX2Request>(y => y.RequestType == X2RequestType.BundledRequest))).Return(false);
            response = new X2Response(x2Request.Id, "message", x2Request.InstanceId, false);
            autoMocker.Get<IX2WorkflowRequestHandler>().WhenToldTo(x => x.Handle(request)).Return(response);
            workflow = new X2Workflow("process", "workflow");
            autoMocker.Get<IX2RequestInterrogator>().WhenToldTo(x => x.GetRequestWorkflow(request)).Return(workflow);
            endpoint = new X2RouteEndpoint("exchange", "queue");
            autoMocker.Get<IX2QueueNameBuilder>().WhenToldTo(x => x.GetErrorQueue(workflow)).Return(endpoint);
        };

        private Because of = () =>
        {
            result = autoMocker.ClassUnderTest.ProcessMessage(x2Request);
        };

        It should_return_any_errors = () =>
        {
            result.AllMessages.Count().ShouldEqual(0);
        };

        It should_process_the_x2_request = () =>
        {
            autoMocker.Get<IX2WorkflowRequestHandler>().WasToldTo(x => x.Handle(request));
        };

        It should_not_publish_response = () =>
        {
            autoMocker.Get<IX2ResponsePublisher>().WasNotToldTo(x => x.Publish(Arg.Any<IX2RouteEndpoint>(), Arg.Any<X2Response>()));
        };
    }
}
