using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Specs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.X2RequestMonitorFactorySpecs
{
    public class when_handling_an_x2_response_given_no_matching_request_monitor : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestMonitorFactory> autoMocker;
        private static IResponseThreadWaiter threadWaiter;
        private static Guid correlationId = Guid.NewGuid();
        private static Guid diff_correlationId = Guid.NewGuid();
        private static IX2RequestMonitor requestMonitor;
        private static FakeResponseSubscriber responseSubscriber;
        private static IX2RequestMonitorCallback requestMonitorCallback;
        private static X2Response response = new X2Response(diff_correlationId, "message", 1);

        Establish context = () =>
        {
            responseSubscriber = new FakeResponseSubscriber();
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestMonitorFactory>();
            autoMocker.Inject<IX2ResponseSubscriber>(responseSubscriber);
            requestMonitorCallback = autoMocker.Get<IX2RequestMonitorCallback>();
            threadWaiter = new ResponseThreadWaiter(correlationId);
            autoMocker.ClassUnderTest.Initialise();
            
        };

        Because of = () =>
        {
            requestMonitor = autoMocker.ClassUnderTest.GetOrCreateRequestMonitor(threadWaiter, requestMonitorCallback);
            responseSubscriber.InvokeCallback(response);
        };

        It should_not_handle_the_response = () =>
        {
            requestMonitorCallback.WasNotToldTo(x => x.RequestCompleted(Param<X2Response>.Matches(y => y.RequestID == correlationId)));
        };
    }
}
