using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.X2EngineSpecs
{
    public class when_receiving_a_create_instance_complete_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2Engine> autoMocker;

        private static X2Request request;
        private static IResponseThreadWaiter threadWaiter;
        private static IX2RequestMonitor requestMonitor;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2Engine>();

            threadWaiter = An<IResponseThreadWaiter>();
            requestMonitor = An<IX2RequestMonitor>();
            autoMocker.Get<IX2RequestMonitorFactory>().WhenToldTo(x => x.GetOrCreateRequestMonitor(threadWaiter, Param.IsAny<IX2RequestMonitorCallback>())).Return(requestMonitor);
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2CreateInstanceWithCompleteRequest(Guid.NewGuid(), "activityName", "processName", "workflowName", serviceRequestMetadata, false);
            autoMocker.Get<IResponseThreadWaiterManager>().WhenToldTo(x => x.Create(request)).Return(threadWaiter);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.ReceiveRequest(request);
        };

        private It should_route_the_request = () =>
        {
            autoMocker.Get<IX2RequestRouter>().WasToldTo(x => x.RouteRequest(request, requestMonitor));
        };

        private It should_wait_for_the_response = () =>
        {
            threadWaiter.WasToldTo(x => x.Wait());
        };
    }
}