using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.Specs.X2EngineSpecs
{
    public class when_receiving_a_timeout_and_maximum_retries_reached : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2Engine> autoMocker;

        private static IX2RequestMonitor requestMonitor;
        private static IResponseThreadWaiter responseThreadWaiter;
        private static X2CreateInstanceRequest request;
        private static X2Response response;
        private static int currentNumberOfRetries;
        private static string errorMessage = "Request timed out and not servicable";

        Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2Engine>();

            request = new X2CreateInstanceRequest(Guid.NewGuid(), "activityName", "processName", "workflowName", null, false);
            responseThreadWaiter = An<IResponseThreadWaiter>();
            requestMonitor = An<IX2RequestMonitor>();
            response = new X2Response(Guid.NewGuid(), errorMessage, 0);
            currentNumberOfRetries = 1;

            autoMocker.Get<IX2RequestMonitorFactory>().WhenToldTo(x => x.GetOrCreateRequestMonitor(responseThreadWaiter, Param.IsAny<IX2RequestMonitorCallback>())).Return(requestMonitor);
            autoMocker.Get<IX2ResponseFactory>().WhenToldTo(x => x.CreateErrorResponse(request, errorMessage, 0, Param.IsAny<SystemMessageCollection>())).Return(response);
            autoMocker.Get<IX2RequestStore>().WhenToldTo(x => x.GetNumberOfTimeouts(request)).Return(currentNumberOfRetries);
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.RequestTimedout(request, responseThreadWaiter);
        };

        It should_get_the_request_monitor_for_the_request = () =>
        {
            autoMocker.Get<IX2RequestMonitorFactory>().WasToldTo(x => x.GetOrCreateRequestMonitor(responseThreadWaiter, Param.IsAny<IX2RequestMonitorCallback>()));
        };

        It should_stop_the_request_monitor_for_the_request = () =>
        {
            requestMonitor.WasToldTo(x => x.Stop());
        };

        It should_create_error_response = () =>
        {
            autoMocker.Get<IX2ResponseFactory>().WasToldTo(x => x.CreateErrorResponse(request, errorMessage, request.InstanceId, Param.IsAny<SystemMessageCollection>()));
        };

        It should_tell_thread_waiter_to_continue = () =>
        {
            responseThreadWaiter.WasToldTo(x => x.Continue(response));
        };

        It should_remove_the_request_monitor_for_the_request = () =>
        {
            autoMocker.Get<IX2RequestMonitorFactory>().WasToldTo(x => x.RemoveRequestMonitor(responseThreadWaiter));
        };
    }
}