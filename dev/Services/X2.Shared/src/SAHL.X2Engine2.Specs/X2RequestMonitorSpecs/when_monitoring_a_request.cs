using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Services;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.X2RequestMonitorSpecs
{
    public class when_monitoring_a_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestMonitor> autoMocker;

        private static X2CreateInstanceRequest request;
        private static ITimeoutService timeoutService;
        private static IX2RequestMonitorCallback requestTimeoutCallback;
        private static IResponseThreadWaiter responseThreadWaiter;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestMonitor>();

            timeoutService = An<ITimeoutService>();
            responseThreadWaiter = An<IResponseThreadWaiter>();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2CreateInstanceRequest(Guid.NewGuid(), "activityName", "processName", "workflowName", serviceRequestMetadata, false);
            requestTimeoutCallback = An<IX2RequestMonitorCallback>();
            autoMocker.Inject<IResponseThreadWaiter>(responseThreadWaiter);
            autoMocker.Get<ITimeoutServiceFactory>().WhenToldTo(x => x.Create(request, requestTimeoutCallback, responseThreadWaiter)).Return(timeoutService);
            autoMocker.ClassUnderTest.Initialise(requestTimeoutCallback);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.MonitorRequest(request);
        };

        private It should_ask_the_factory_to_create_a_timeout_service = () =>
        {
            autoMocker.Get<ITimeoutServiceFactory>().WasToldTo(x => x.Create(request, requestTimeoutCallback, responseThreadWaiter));
        };

        private It should_start_a_timer = () =>
        {
            timeoutService.WasToldTo(x => x.Start());
        };
    }
}