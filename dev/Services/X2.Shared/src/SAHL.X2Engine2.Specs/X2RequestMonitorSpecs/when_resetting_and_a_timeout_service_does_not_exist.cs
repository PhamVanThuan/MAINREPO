using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Services;

namespace SAHL.X2Engine2.Specs.X2RequestMonitorSpecs
{
    public class when_resetting_and_a_timeout_service_does_not_exist : WithFakes
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
            autoMocker.ClassUnderTest.Reset(request);
        };

        It should_not_call_stop_on_the_timeout_service = () =>
        {
            timeoutService.WasNotToldTo(x => x.Stop());
        };

        It should_use_the_timeout_service_factory_to_create_a_new_timeout_service = () =>
        {
            autoMocker.Get<ITimeoutServiceFactory>().WasToldTo(x => x.Create(request, Param.IsAny<IX2RequestMonitorCallback>(), Param.IsAny<IResponseThreadWaiter>())).Times(1);
        };

        It should_start_the_timeout = () =>
        {
            timeoutService.WasToldTo(x => x.Start());
        };
    }
}