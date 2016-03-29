using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.ThreadWaiterManagerSpecs
{
    public class when_no_thread_waiters_for_requests_exist : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<ResponseThreadWaiterManager> autoMocker;

        private static X2CreateInstanceRequest x2Request;
        private static IResponseThreadWaiter threadWaiter;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<ResponseThreadWaiterManager>();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            x2Request = new X2CreateInstanceRequest(Guid.NewGuid(), "activityName", "processName", "workflowName", serviceRequestMetadata, false);
        };

        private Because of = () =>
        {
            threadWaiter = autoMocker.ClassUnderTest.GetThreadWaiter(x2Request.CorrelationId);
        };

        private It should_create_a_request_monitor_with_the_given_waiter = () =>
        {
            threadWaiter.ShouldBeNull();
        };
    }
}