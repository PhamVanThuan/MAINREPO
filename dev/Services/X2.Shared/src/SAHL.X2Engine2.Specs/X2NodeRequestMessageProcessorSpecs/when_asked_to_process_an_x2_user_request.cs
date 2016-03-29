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
    public class when_asked_to_process_an_x2_user_request : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2NodeRequestMessageProcessor> autoMocker;
        private static IX2Request x2Request;
        private static ISystemMessageCollection result;
        private static X2Workflow workflow;
        private static X2RouteEndpoint endpoint;
        private static X2Response response;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2NodeRequestMessageProcessor>();
            autoMocker.Get<IX2RequestInterrogator>().WhenToldTo(x => x.IsRequestMonitored(Arg.Is<IX2Request>(y => y.RequestType == X2RequestType.UserCreate))).Return(true);
            x2Request = new SAHL.Core.X2.Messages.X2CreateInstanceRequest(Guid.NewGuid(), "activityName", "processName", "workFlowName", null, true, null, null, new Dictionary<string, string>(), null);
            response = new X2Response(x2Request.Id, "message", x2Request.InstanceId, false);
            autoMocker.Get<IX2WorkflowRequestHandler>().WhenToldTo(x => x.Handle(x2Request)).Return(response);
            workflow = new X2Workflow("process", "workflow");
            autoMocker.Get<IX2RequestInterrogator>().WhenToldTo(x => x.GetRequestWorkflow(x2Request)).Return(workflow);
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
            autoMocker.Get<IX2WorkflowRequestHandler>().WasToldTo(x => x.Handle(x2Request));
        };

        It should_publish_response = () =>
        {
            autoMocker.Get<IX2ResponsePublisher>().WasToldTo(x => x.Publish(Arg.Is<IX2RouteEndpoint>(y => y.ExchangeName == X2QueueManager.X2EngineResponseExchange 
                && y.QueueName == X2QueueManager.X2EngineResponseQueue), response));
        };
    }
}
