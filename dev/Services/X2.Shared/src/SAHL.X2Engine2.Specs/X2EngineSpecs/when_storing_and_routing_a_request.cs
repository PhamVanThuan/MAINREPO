using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.X2EngineSpecs
{
    public class when_storing_and_routing_a_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2Engine> autoMocker;
        private static X2Request request;
        private static IResponseThreadWaiter responseThreadWaiter;
        private static IResponseThreadWaiter responseThreadWaiterReturned;
        private static IX2RequestMonitor requestMonitor;
        private static IServiceRequestMetadata serviceRequestMetadata;

        Establish context = () =>
        {
            responseThreadWaiter = An<IResponseThreadWaiter>();
            requestMonitor = An<IX2RequestMonitor>();
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2Engine>();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2CreateInstanceRequest(Guid.NewGuid(), "activityName", "processName", "workflowName", serviceRequestMetadata, false);
            autoMocker.Get<IResponseThreadWaiterManager>().WhenToldTo(x => x.Create(request)).Return(responseThreadWaiter);
            autoMocker.Get<IX2RequestMonitorFactory>().WhenToldTo(x => x.GetOrCreateRequestMonitor(Param.IsAny<IResponseThreadWaiter>(), Param.IsAny<IX2RequestMonitorCallback>())).Return(requestMonitor);
        };

        Because of = () =>
        {
            responseThreadWaiterReturned = autoMocker.ClassUnderTest.RouteRequest(request);
        };

        It should_create_a_response_thread_waiter = () =>
        {
            autoMocker.Get<IResponseThreadWaiterManager>().WasToldTo(x => x.Create(request));
        };

        It should_create_a_request_monitor = () =>
            {
                autoMocker.Get<IX2RequestMonitorFactory>().WasToldTo(x => x.GetOrCreateRequestMonitor(responseThreadWaiter, autoMocker.ClassUnderTest));
            };

        It should_initialise_the_request_monitor = () =>
            {
                requestMonitor.WasToldTo(x => x.Initialise(autoMocker.ClassUnderTest));
            };

        It should_route_the_request = () =>
            {
                autoMocker.Get<IX2RequestRouter>().WasToldTo(x => x.RouteRequest(request, requestMonitor));
            };

        It should_return_the_thread_waiter_created_for_the_request = () =>
            {
                responseThreadWaiterReturned.ShouldBeTheSameAs(responseThreadWaiter);
            };
    }
}