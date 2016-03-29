using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Services;

namespace SAHL.X2Engine2.Specs.X2RequestMonitorSpecs
{
    public class when_asked_to_stop_monitoring_a_request_that_is_not_currently_monitored : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestMonitor> autoMocker;
        private static ITimeoutService timeoutService;
        private static IX2Request request;

        Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestMonitor>();
            timeoutService = null;
            request = An<IX2Request>();
            timeoutService = An<ITimeoutService>();
            autoMocker.Get<ITimeoutServiceFactory>().WhenToldTo(x => x.Create(request, Param.IsAny<IX2RequestMonitorCallback>(), Param.IsAny<IResponseThreadWaiter>())).Return(timeoutService);
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.Stop();
        };

        It should_not_try_stop_monitoring_the_request_for_timeouts = () =>
        {
            timeoutService.WasNotToldTo(x => x.Stop());
        };
    }
}