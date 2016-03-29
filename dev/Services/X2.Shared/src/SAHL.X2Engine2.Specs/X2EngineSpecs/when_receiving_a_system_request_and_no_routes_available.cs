using System;
using System.Collections.Generic;
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
    public class when_receiving_a_system_request_and_no_routes_available : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2Engine> autoMocker;
        private static X2SystemRequestGroup request;
        static X2Response response;
        private static IServiceRequestMetadata serviceRequestMetadata;

        Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2Engine>();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2SystemRequestGroup(Guid.NewGuid(), serviceRequestMetadata, X2RequestType.SystemRequestGroup, 123467L, new List<string> { "Activity" });
            autoMocker.Get<IX2RequestRouter>().WhenToldTo(x => x.RouteRequest(request, Param.IsAny<IX2RequestMonitor>())).Throw(new NoRoutesAvailableException("messages"));
            autoMocker.Get<IX2ResponseFactory>().WhenToldTo(x => x.CreateErrorResponse(request, Param.IsAny<string>(), Param.IsAny<long?>(), Param.IsAny<SystemMessageCollection>())).Return(
                new X2ErrorResponse(Guid.NewGuid(), "", 0, new SystemMessageCollection()));
        };

        Because of = () =>
        {
            response = autoMocker.ClassUnderTest.ReceiveSystemRequest(request);
            int i = 0;
        };

        It should_not_wait_for_the_response_thread_waiter = () =>
        {
            autoMocker.Get<IResponseThreadWaiter>().WasNotToldTo(x => x.Wait());
        };

        It should_return_an_error_response = () =>
            {
                response.ShouldBe(typeof(X2ErrorResponse));
            };
    }
}