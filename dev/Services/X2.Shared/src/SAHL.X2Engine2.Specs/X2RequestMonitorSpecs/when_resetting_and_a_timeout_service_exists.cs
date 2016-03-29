using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Services;

namespace SAHL.X2Engine2.Specs.X2RequestMonitorSpecs
{
    public class when_resetting_and_a_timeout_service_exists : WithFakes
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
            autoMocker.ClassUnderTest.Reset(request);
        };

        It should_call_stop_on_the_existing_timeout_service = () =>
        {
            timeoutService.WasToldTo(x => x.Stop());
        };

        It should_call_start_on_the_timeout_service = () =>
        {
            timeoutService.WasToldTo(x => x.Start()).Times(2);
        };

        It should_create_a_new_timeout_from_the_factory = () =>
        {
            autoMocker.Get<ITimeoutServiceFactory>().WasToldTo(x => x.Create(request, Param.IsAny<IX2RequestMonitorCallback>(), Param.IsAny<IResponseThreadWaiter>()))
                .Times(2);
        };
    }
}