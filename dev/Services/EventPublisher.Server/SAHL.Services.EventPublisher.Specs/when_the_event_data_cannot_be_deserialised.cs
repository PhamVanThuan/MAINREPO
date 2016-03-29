using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Messaging.Shared;
using SAHL.Services.EventPublisher.CommandHandlers;
using SAHL.Services.Interfaces.EventPublisher.Commands;
using System;
using System.Linq;

namespace SAHL.Services.EventPublisher.Specs
{
    public class when_the_event_data_cannot_be_deserialised : WithEventPublisherFakes
    {
        private static PublishEventCommand command;
        private static PublishEventCommandHandler handler;
        private static int eventKey;

        private Establish context = () =>
        {
            deserialisedEvent = null;
            eventKey = 1;
            command = new PublishEventCommand(eventKey);
            handler = new PublishEventCommandHandler(eventPublisherDataService, eventSerialiser, messageBus);
            eventSerialiser.WhenToldTo(x => x.Deserialise(Arg.Any<Type>(), eventDataModel.Data)).Return(deserialisedEvent);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Could not create event to publish");
        };

        private It should_not_publish_the_event = () =>
        {
            messageBus.DidNotReceive().Publish(Arg.Any<IMessage>());
        };
    }
}