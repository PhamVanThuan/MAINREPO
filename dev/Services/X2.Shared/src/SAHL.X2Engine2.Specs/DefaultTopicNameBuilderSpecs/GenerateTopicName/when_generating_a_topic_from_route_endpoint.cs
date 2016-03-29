//using Machine.Fakes;
//using Machine.Specifications;
//using SAHL.X2Engine2.Communication;

//namespace SAHL.X2Engine2.Specs.DefaultTopicNameBuilderSpecs.GenerateTopicName
//{
//    public class when_generating_a_topic_from_route_endpoint : WithFakes
//    {
//        private static StructureMap.AutoMocking.AutoMocker<DefaultTopicNameBuilder> autoMocker;
//        private static IX2RouteEndpoint routeEndpoint;
//        private static string expectedTopicName;
//        private static string actualTopicName;

//        private Establish context = () =>
//        {
//            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<DefaultTopicNameBuilder>();

//            routeEndpoint = new X2RouteEndpoint("exchangeName", "queueName");
//            expectedTopicName = routeEndpoint.ExchangeName + "." + routeEndpoint.QueueName;
//        };

//        private Because of = () =>
//        {
//            actualTopicName = autoMocker.ClassUnderTest.GenerateTopicName(routeEndpoint);
//        };

//        private It should_generate_the_expected_topic_name = () =>
//        {
//            actualTopicName.ShouldEqual(expectedTopicName);
//        };
//    }
//}