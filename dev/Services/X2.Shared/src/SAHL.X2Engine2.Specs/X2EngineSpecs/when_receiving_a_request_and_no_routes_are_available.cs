using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Exceptions;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Factories;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.X2EngineSpecs
{
    public class when_receiving_a_request_and_no_routes_are_available : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2Engine> autoMocker;
        private static X2Request request;
        private static X2Response response;
        private static IServiceRequestMetadata serviceRequestMetadata;

        Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2Engine>();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2RequestForExistingInstance(Guid.NewGuid(), 1234567L, X2RequestType.UserComplete, serviceRequestMetadata, "ActivityName", false, null);
            autoMocker.Get<IX2RequestRouter>().WhenToldTo(x => x.RouteRequest(request, Param.IsAny<IX2RequestMonitor>())).Throw(new NoRoutesAvailableException("no routes available"));
        };

        Because of = () =>
        {
            response = autoMocker.ClassUnderTest.ReceiveRequest(request);
        };

        It should_return_an_error_response = () =>
        {
            autoMocker.Get<IX2ResponseFactory>().WasToldTo(x => x.CreateErrorResponse(request, Param.IsAny<string>(), request.InstanceId, Param.IsAny<SystemMessageCollection>()));
        };
    }
}