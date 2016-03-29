using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Logging;
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
    public class when_handling_a_bundled_request_given_database_storage_failure : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2NodeRequestMessageProcessor> autoMocker;
        private static ISystemMessageCollection result;
        private static X2Workflow workflow;
        private static X2RouteEndpoint endpoint;
        private static X2ErrorResponse response;

        private static X2BundledRequest x2Request;
        private static X2RequestForAutoForward request;

        private static long instanceId = 12;
        private static List<string> actvityNames = new List<string>() { "activity1", "activity2" };

        private Establish context = () =>
        {
            request = new X2RequestForAutoForward(Guid.NewGuid(), null, X2RequestType.AutoForward, instanceId);
            x2Request = new X2BundledRequest(new List<IX2Request> { request });

            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2NodeRequestMessageProcessor>();
            autoMocker.Get<IX2RequestInterrogator>().WhenToldTo(x => x.IsRequestMonitored(Arg.Is<IX2Request>(y => y.RequestType == X2RequestType.BundledRequest))).Return(false);

            autoMocker.Get<IX2RequestStore>().WhenToldTo(x => x.StoreReceivedRequest(Arg.Any<IX2Request>())).Throw(new Exception("something bad happened"));
            response = new X2ErrorResponse(x2Request.Id, "message", x2Request.InstanceId, new SystemMessageCollection());
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
            autoMocker.Get<IX2ResponsePublisher>().WasNotToldTo(x => x.Publish(Arg.Is<IX2RouteEndpoint>(y => y.ExchangeName == X2QueueManager.X2EngineResponseExchange && y.QueueName == X2QueueManager.X2EngineResponseQueue), Arg.Any<X2Response>()));
        };
        
        It should_publish_request_to_the_error_queue = () =>
        {
            autoMocker.Get<IX2RequestPublisher>().WasToldTo(x => x.Publish(endpoint, Arg.Is<X2BundledRequest>(y => y.Requests.First() == request)));
        };

        It should_try_to_store_the_request = () =>
        {
            autoMocker.Get<IX2RequestStore>().WasToldTo(x => x.StoreReceivedRequest(Arg.Is<IX2Request>(y => ((X2BundledRequest)((X2WrappedRequest)y).X2Request).Requests.First() == request)));
        };

        It should_not_log_the_error = () =>
        {
            autoMocker.Get<IRawLogger>().WasNotToldTo(x => x.LogErrorWithException(Arg.Any<LogLevel>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Exception>(), Arg.Any<IDictionary<string, object>>()));
        };

        It should_publish_error_response_to_x2_error_queue = () =>
        {
            autoMocker.Get<IX2ResponsePublisher>().WasToldTo(x => x.Publish(Arg.Is<IX2RouteEndpoint>(y => y.ExchangeName == X2QueueManager.X2EngineErrorExchange && y.QueueName == X2QueueManager.X2EngineErrorQueue), Arg.Any<X2Response>()));
        };


    }
}
