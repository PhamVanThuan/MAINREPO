using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Events;
using SAHL.Core.Messaging.Shared;
using SAHL.Services.EventPublisher.CommandHandlers;
using SAHL.Services.EventPublisher.Specs.EventTypeProviderServiceSpecs;
using SAHL.Services.Interfaces.EventPublisher.Commands;
using System;

namespace SAHL.Services.EventPublisher.Specs
{
    public class when_told_to_publish_a_valid_event : WithEventPublisherFakes
    {
        private static PublishEventCommand command;
        private static PublishEventCommandHandler handler;
        private static int eventKey;

        private Establish context = () =>
        {
            eventSerialiser.WhenToldTo(x => x.Deserialise(Arg.Any<Type>(), Arg.Any<string>()))
                .Return(eventModel);
            eventKey = 1;
            command = new PublishEventCommand(eventKey);
            handler = new PublishEventCommandHandler(eventPublisherDataService, eventSerialiser, messageBus);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_publish_the_event = () =>
        {
            messageBus.Received().Publish(Arg.Any<IMessage>());
        };

        private It should_publish_the_wrapped_event_with_event_guid = () =>
        {
            messageBus.Received().Publish(Arg.Is<IWrappedEvent<FakeEvent>>(
                y => y.Id == eventGuid  && y.InternalEvent.Id == eventGuid));
        };

        private It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}