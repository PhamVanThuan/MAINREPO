using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.X2.Messages;
using SAHL.Core.X2.Messages.Management;
using SAHL.X2Engine2.Communication;
using System;

namespace SAHL.X2Engine2.Specs.X2EngineSpecs
{
    public class when_receiving_a_node_management_message_given_publisher_error : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2Engine> autoMocker;
        private static IX2NodeManagementMessage message;
        private static X2Response response;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2Engine>();
            message = new X2NodeManagementMessage(X2ManagementType.RefreshCache, "DomainService");
            autoMocker.Get<IX2NodeManagementPublisher>().WhenToldTo(x => x.Publish(Arg.Any<IX2NodeManagementMessage>())).Throw(new Exception("bad stuff happened"));
        };

        private Because of = () =>
        {
            response = autoMocker.ClassUnderTest.ReceiveManagementMessage(message);
        };

        private It should_publish_the_message = () =>
        {
            autoMocker.Get<IX2NodeManagementPublisher>().WasToldTo(x => x.Publish(message));
        };

        private It should_return_with_a_success_response = () =>
        {
            response.ShouldBeOfType(typeof(X2ErrorResponse));
        };
    }
}