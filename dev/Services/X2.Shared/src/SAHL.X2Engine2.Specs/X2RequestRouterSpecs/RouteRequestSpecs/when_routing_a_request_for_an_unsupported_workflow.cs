using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Exceptions;
using SAHL.Core.X2.Messages;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.X2RequestRouterSpecs.RouteRequestSpecs
{
    public class when_routing_a_request_for_an_unsupported_workflow : WithFakes
    {
        static AutoMocker<X2RequestRouter> autoMocker = new NSubstituteAutoMocker<X2RequestRouter>();
        static Exception expected;
        static IX2RouteEndpoint route = null;
        static X2Request request;
        static X2Workflow workflow;
        private static IServiceRequestMetadata serviceRequestMetadata;

        Establish context = () =>
        {
            workflow = new X2Workflow("Process", "Workflow");
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2Request(Guid.NewGuid(), X2RequestType.UserComplete, 1, false, serviceRequestMetadata);
            autoMocker.Get<IX2RequestInterrogator>().WhenToldTo(x => x.GetRequestWorkflow(request)).Return(workflow);
            autoMocker.Get<IX2RoutePlanner>().WhenToldTo(x => x.PlanRoute(Param.IsAny<bool>(), Param.IsAny<X2Workflow>())).Return(route);
        };

        Because of = () =>
        {
            expected = Catch.Exception(() => autoMocker.ClassUnderTest.RouteRequest(request, null));
        };

        It should = () =>
        {
            expected.ShouldBeOfType<NoRoutesAvailableException>();
        };
    }
}
