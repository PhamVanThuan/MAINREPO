using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Services;

namespace SAHL.X2Engine2.Specs.X2RequestMonitorSpecs
{
    public class when_stopping_the_request_monitor_for_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestMonitor> autoMocker;
        private static ITimeoutService timeoutService;
        private static IX2Request request;

        Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestMonitor>();
            timeoutService = An<ITimeoutService>();
            request = An<IX2Request>();
            autoMocker.Get<ITimeoutServiceFactory>().WhenToldTo(x => x.Create(Param.IsAny<IX2Request>(), Param.IsAny<IX2RequestMonitorCallback>(), Param.IsAny<IResponseThreadWaiter>()))
                .Return(timeoutService);
            //this will create the timeout to test the stop()
            autoMocker.ClassUnderTest.MonitorRequest(request);
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.Stop();
        };

        It should_stop_the_timeout_monitor_for_the_request = () =>
        {
            timeoutService.WasToldTo(x => x.Stop());
        };
    }
}