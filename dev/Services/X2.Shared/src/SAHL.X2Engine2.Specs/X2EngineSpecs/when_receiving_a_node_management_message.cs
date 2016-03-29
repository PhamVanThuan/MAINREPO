using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.Core.X2.Messages.Management;
using SAHL.X2Engine2.Communication;

namespace SAHL.X2Engine2.Specs.X2EngineSpecs
{
    public class when_receiving_a_node_management_message : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2Engine> autoMocker;
        private static IX2NodeManagementMessage message;
        private static X2Response response;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2Engine>();
            message = new X2NodeManagementMessage(X2ManagementType.RefreshCache, "DomainService");
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
            response.IsErrorResponse.ShouldBeFalse();
        };
    }
}