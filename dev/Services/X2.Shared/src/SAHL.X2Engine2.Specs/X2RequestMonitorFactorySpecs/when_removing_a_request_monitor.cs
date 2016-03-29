using Machine.Fakes;
using Machine.Specifications;
using SAHL.X2Engine2.Communication;

namespace SAHL.X2Engine2.Specs.X2RequestMonitorFactorySpecs
{
    public class when_removing_a_request_monitor : WithFakes
    {
        private static IResponseThreadWaiter threadWaiter;
        private static IX2RequestMonitor nullRequestMonitor;
        private static IX2RequestMonitor requestMonitor;
        private static StructureMap.AutoMocking.AutoMocker<X2RequestMonitorFactory> autoMocker;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestMonitorFactory>();
            threadWaiter = An<IResponseThreadWaiter>();
            requestMonitor = autoMocker.ClassUnderTest.GetOrCreateRequestMonitor(threadWaiter, Param.IsAny<IX2RequestMonitorCallback>());
            autoMocker.ClassUnderTest.RemoveRequestMonitor(threadWaiter);
        };

        Because of = () =>
        {
            nullRequestMonitor = autoMocker.ClassUnderTest.GetRequestMonitor(threadWaiter);
        };

        It should_return_null_when_asked_to_get_the_request_monitor = () =>
        {
            nullRequestMonitor.ShouldBeNull();
        };
    }
}