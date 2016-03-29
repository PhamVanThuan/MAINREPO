using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Services;
using SAHL.X2Engine2.Specs.Mocks;

namespace SAHL.X2Engine2.Specs.X2RequestMonitorSpecs
{
    public class when_a_request_timed_out : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestMonitor> autoMocker;

        private static ITimeoutService timeoutService;
        private static IX2RequestMonitorCallback requestTimeoutCallback;
        private static IResponseThreadWaiter responseThreadWaiter;
        private static X2CreateInstanceRequest request;
        private static IServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestMonitor>();
            metadata = An<IServiceRequestMetadata>();
            request = new X2CreateInstanceRequest(Guid.NewGuid(), "activityName", "processName", "workflowName", metadata, false);
            responseThreadWaiter = new ResponseThreadWaiter(request.CorrelationId);
            requestTimeoutCallback = An<IX2RequestMonitorCallback>();
            timeoutService = new FakeTimeoutService(request, requestTimeoutCallback, 1, responseThreadWaiter);
            autoMocker.Get<ITimeoutServiceFactory>().WhenToldTo(x => x.Create(request, requestTimeoutCallback, Param.IsAny<IResponseThreadWaiter>())).Return(timeoutService);

            autoMocker.ClassUnderTest.Initialise(requestTimeoutCallback);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.MonitorRequest(request);
        };

        private It should_invoke_the_timeout_callback = () =>
        {
            requestTimeoutCallback.WasToldTo(x => x.RequestTimedout(request, responseThreadWaiter));
        };
    }
}