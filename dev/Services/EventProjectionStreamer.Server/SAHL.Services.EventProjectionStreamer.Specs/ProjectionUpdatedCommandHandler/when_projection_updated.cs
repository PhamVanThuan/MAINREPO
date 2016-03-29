using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Services.EventProjectionStreamer.CommandHandlers;
using SAHL.Services.Interfaces.EventProjectionStreamer;

namespace SAHL.Services.EventProjectionStreamer.Specs
{
    public class when_projection_updated : WithFakes
    {
        private static ProjectionUpdatedCommandHandler _commandHandler;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            _commandHandler = new ProjectionUpdatedCommandHandler();
        };

        Because of = () =>
        {
            var projectionUpdatedCommand = new ProjectionUpdatedCommand("test", "some test data");
            _commandHandler.HandleCommand(projectionUpdatedCommand, metadata);
        };

        It should_send_message_to_clients_subscribed_to_projection = () =>
        {
        };
    }
}