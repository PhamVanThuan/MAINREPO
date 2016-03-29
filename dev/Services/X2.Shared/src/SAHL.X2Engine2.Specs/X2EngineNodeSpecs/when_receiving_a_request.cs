//using Machine.Fakes;
//using Machine.Specifications;
//using NSubstitute;
//using SAHL.Core.X2.Messages;
//using SAHL.X2Engine2.Communication;
//using SAHL.X2Engine2.Node;
//using SAHL.X2Engine2.Providers;
//using StructureMap.AutoMocking;
//using StructureMap.AutoMocking.NSubstitute;
//using System;

//namespace SAHL.X2Engine2.Specs.X2EngineNodeSpecs
//{
//    public class when_receiving_a_request : WithFakes
//    {
//        private static AutoMocker<X2EngineNode> autoMocker;
//        private static X2Request request;
//        private static X2Response response;
//        private static IX2Route engineRoute;
//        Establish context = () =>
//        {
//            autoMocker = new NSubstituteAutoMocker<X2EngineNode>();
//            request = new X2Request(Guid.NewGuid(), X2RequestType.UserCreate, null, false, "userName");
//            response = new X2Response(request.CorrelationId, string.Empty, 1234567L);
//            autoMocker.Get<IX2WorkflowRequestHandler>().WhenToldTo(x => x.Handle(Param.IsAny<IX2Request>())).Return(response);
//            autoMocker.Get<IX2EngineConfigurationProvider>().WhenToldTo(x => x.GetEngineRoute()).Return(engineRoute);
//        };

//        Because of = () =>
//        {
//            autoMocker.ClassUnderTest.ReceiveRequest(request);
//        };

//        It should_ask_the_workflow_request_handler_to_handle_the_request = () =>
//        {
//            autoMocker.Get<IX2WorkflowRequestHandler>().WasToldTo(x => x.Handle(request));
//        };

//        It should_get_the_engine_route_from_the_engine_configuration_provider = () =>
//        {
//            autoMocker.Get<IX2EngineConfigurationProvider>().WasToldTo(x => x.GetEngineRoute());
//        };

//        It should_publish_the_response_for_the_engine_using_the_response_publisher = () =>
//        {
//            autoMocker.Get<IX2ResponsePublisher>().WasToldTo(x => x.Publish(engineRoute, response));
//        };
//    }
//}