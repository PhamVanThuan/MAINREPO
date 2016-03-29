using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Hosts;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.Hosts.InMemoryHostSpecs
{
    public class when_sending_an_activity_complete_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2EngineHost> autoMocker;

        private static X2RequestForExistingInstance request;
        private static IServiceRequestMetadata serviceRequestMetadata;

        Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2EngineHost>();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2RequestForExistingInstance(Guid.NewGuid(), 1234567L, X2RequestType.UserComplete, serviceRequestMetadata, "ActivityName", false, null);
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.SendActivityCompleteRequest(request);
        };

        It should_forward_the_request_to_the_engine = () =>
        {
            autoMocker.Get<IX2Engine>().WasToldTo(x => x.ReceiveRequest<X2RequestForExistingInstance>(request));
        };
    }
}