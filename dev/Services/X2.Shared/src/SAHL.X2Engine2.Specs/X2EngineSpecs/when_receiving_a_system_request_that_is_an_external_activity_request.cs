using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using System;

namespace SAHL.X2Engine2.Specs.X2EngineSpecs
{
    public class when_receiving_a_system_request_that_is_an_external_activity_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2Engine> autoMocker;
        private static X2ExternalActivityRequest request;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2Engine>();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";

            request = new X2ExternalActivityRequest(Guid.NewGuid(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>(), -1, Param.IsAny<int>(), serviceRequestMetadata, new System.Collections.Generic.Dictionary<string, string>());
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.ReceiveExternalActivityRequest(request);
        };

        It should_not_wait_for_the_response_thread_waiter = () =>
        {
            autoMocker.Get<IResponseThreadWaiter>().WasNotToldTo(x => x.Wait());
        };
    }
}