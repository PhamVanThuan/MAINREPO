using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Specs.Mocks;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.X2RequestMonitorSpecs
{
    public class when_receiving_a_response : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestMonitor> autoMocker;

        private static IX2RequestMonitorCallback requestCallback;
        private static FakeResponseSubscriber responseSubscriber;
        private static X2Response response;
        private static X2Request request;
        private static Guid requestID;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestMonitor>();

            requestID = Guid.NewGuid();
            response = new X2Response(requestID, "test message", 0);
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2CreateInstanceRequest(requestID, "activityName", "processName", "workflowName", serviceRequestMetadata, false);
            requestCallback = An<IX2RequestMonitorCallback>();

            responseSubscriber = new FakeResponseSubscriber();
            Action<X2Response> action = (x) => 
            {
                requestCallback.RequestCompleted(x);
            };
            responseSubscriber.Subscribe(action);
            autoMocker.Inject<IX2ResponseSubscriber>(responseSubscriber);
            autoMocker.ClassUnderTest.Initialise(requestCallback);
            autoMocker.ClassUnderTest.MonitorRequest(request);
        };

        private Because of = () =>
        {
            responseSubscriber.InvokeCallback(response);
        };

        private It should_complete_the_request = () =>
        {
            requestCallback.WasToldTo(x => x.RequestCompleted(Param<X2Response>.Matches( y => y.RequestID == request.CorrelationId)));
        };
    }
}