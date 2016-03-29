using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Hosts;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.Hosts.InMemoryHostSpecs
{
    public class when_sending_a_create_with_complete_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2EngineHost> autoMocker;

        private static X2CreateInstanceWithCompleteRequest request;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2EngineHost>();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2CreateInstanceWithCompleteRequest(Guid.NewGuid(), "activityName", "processName", "workflowName", serviceRequestMetadata, false);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.SendCreateWorkFlowInstanceWithCompleteRequest(request);
        };

        private It should_forward_the_request_to_the_engine = () =>
        {
            autoMocker.Get<IX2Engine>().WasToldTo(x => x.ReceiveRequest(request));
        };
    }
}