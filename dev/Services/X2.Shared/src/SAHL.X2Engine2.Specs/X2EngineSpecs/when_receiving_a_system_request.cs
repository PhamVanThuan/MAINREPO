using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Factories;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.X2EngineSpecs
{
    public class when_receiving_a_system_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2Engine> autoMocker;
        private static X2SystemRequestGroup request;
        private static X2Response response;
        private static IServiceRequestMetadata serviceRequestMetadata;

        Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2Engine>();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2SystemRequestGroup(Guid.NewGuid(), serviceRequestMetadata, X2RequestType.SystemRequestGroup, 123467L, new List<string> { "Activity" });
            response = new X2Response(Guid.NewGuid(), String.Empty, request.InstanceId);
            autoMocker.Get<IX2ResponseFactory>().WhenToldTo((responseFactory) => responseFactory.CreateSuccessResponse(request, request.InstanceId, Param.IsAny<ISystemMessageCollection>())).Return(response);
        };

        Because of = () =>
        {
            response = autoMocker.ClassUnderTest.ReceiveSystemRequest(request);
        };

        It should_not_wait_for_the_response_thread_waiter = () =>
        {
            autoMocker.Get<IResponseThreadWaiter>().WasNotToldTo(x => x.Wait());
        };

        It should_return_with_a_success_response = () =>
        {
            response.ShouldNotBeOfType(typeof(X2ErrorResponse));
        };
    }
}