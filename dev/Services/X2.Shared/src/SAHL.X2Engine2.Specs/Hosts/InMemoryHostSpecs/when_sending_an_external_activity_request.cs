using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Hosts;
using System;

namespace SAHL.X2Engine2.Specs.Hosts.InMemoryHostSpecs
{
    public class when_sending_an_external_activity_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2EngineHost> autoMocker;

        private static X2ExternalActivityRequest request;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2EngineHost>();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2ExternalActivityRequest(Guid.NewGuid(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>(), -1, Param.IsAny<int>(), serviceRequestMetadata, new System.Collections.Generic.Dictionary<string, string>());
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.SendExternalActivityRequest(request);
        };

        private It should_forward_the_request_to_the_engine = () =>
        {
            autoMocker.Get<IX2Engine>().WasToldTo(x => x.ReceiveExternalActivityRequest(request));
        };
    }
}