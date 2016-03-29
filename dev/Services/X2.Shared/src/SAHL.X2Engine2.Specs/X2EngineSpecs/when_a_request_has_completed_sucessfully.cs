using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;

namespace SAHL.X2Engine2.Specs.X2EngineSpecs
{
    public class when_a_request_has_completed_sucessfully : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2Engine> autoMocker;

        private static X2Response response;
        private static IResponseThreadWaiter threadWaiterToRelease;
        private static IX2RequestMonitor requestMonitor;

        Establish context = () =>
        {
            requestMonitor = An<IX2RequestMonitor>();
            threadWaiterToRelease = An<IResponseThreadWaiter>();
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2Engine>();
            autoMocker.Get<IResponseThreadWaiterManager>().WhenToldTo(x => x.GetThreadWaiter(Param.IsAny<Guid>())).Return(threadWaiterToRelease);
            autoMocker.Get<IX2RequestMonitorFactory>().WhenToldTo(x => x.GetOrCreateRequestMonitor(Param.IsAny<IResponseThreadWaiter>(), Param.IsAny<IX2RequestMonitorCallback>())).Return(requestMonitor);
            response = new X2Response(Guid.NewGuid(), "message", 0);
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.RequestCompleted(response);
        };

        It should_get_the_response_thread_waiter_to_release_from_the_thread_waiter_manager = () =>
        {
            autoMocker.Get<IResponseThreadWaiterManager>().WasToldTo(x => x.GetThreadWaiter(response.RequestID));
        };

        It should_call_continue_on_the_responses_thread_waiter = () =>
        {
            threadWaiterToRelease.WasToldTo(x => x.Continue(response));
        };

        It should_get_the_request_monitor_for_the_thread_waiter_to_be_released = () =>
        {
            autoMocker.Get<IX2RequestMonitorFactory>().WasToldTo(x => x.GetOrCreateRequestMonitor(threadWaiterToRelease, Param.IsAny<IX2RequestMonitorCallback>()));
        };

        It should_stop_the_request_monitor_for_the_thread_waiter_to_release = () =>
        {
            requestMonitor.WasToldTo(x => x.Stop());
        };

        It should_remove_the_request_monitor_for_the_thread_waiter = () =>
        {
            autoMocker.Get<IX2RequestMonitorFactory>().WasToldTo(x => x.RemoveRequestMonitor(threadWaiterToRelease));
        };

        It should_remove_the_request_monitor = () =>
        {
            autoMocker.Get<IX2RequestMonitorFactory>().WasToldTo(x => x.RemoveRequestMonitor(threadWaiterToRelease));
        };

        It should_release_the_response_from_the_response_thread_waiter = () =>
        {
            autoMocker.Get<IResponseThreadWaiterManager>().WasToldTo(x => x.Release(response.RequestID));
        };

    }
}