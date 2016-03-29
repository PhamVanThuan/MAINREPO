using Machine.Fakes;
using Machine.Specifications;
using SAHL.X2Engine2.Communication;

namespace SAHL.X2Engine2.Specs.X2RequestMonitorFactorySpecs
{
    public class when_creating_a_request_monitor_for_existing_thread_waiter : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestMonitorFactory> autoMocker;

        private static IResponseThreadWaiter threadWaiter;
        private static IX2RequestMonitor requestMonitorInitial;
        private static IX2RequestMonitor requestMonitor;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestMonitorFactory>();
            threadWaiter = An<IResponseThreadWaiter>();
            requestMonitorInitial = autoMocker.ClassUnderTest.GetOrCreateRequestMonitor(threadWaiter, Param.IsAny<IX2RequestMonitorCallback>());
        };

        private Because of = () =>
        {
            requestMonitor = autoMocker.ClassUnderTest.GetOrCreateRequestMonitor(threadWaiter, Param.IsAny<IX2RequestMonitorCallback>());
        };

        private It should_return_the_existing_thread_monitor = () =>
        {
            requestMonitor.ShouldBeTheSameAs(requestMonitorInitial);
        };
    }
}