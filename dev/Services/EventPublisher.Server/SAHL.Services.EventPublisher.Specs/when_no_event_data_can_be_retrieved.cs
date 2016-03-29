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
    public class when_no_event_data_can_be_retrieved : WithEventPublisherFakes
    {
        private static PublishEventCommand command;
        private static PublishEventCommandHandler handler;
        private static int eventKey;

        private Establish context = () =>
        {
            eventKey = 1;
            command = new PublishEventCommand(eventKey);
            //set to NULL
            eventDataModel = null;
            eventPublisherDataService.WhenToldTo(x => x.GetEventDataModelByEventKey(Arg.Any<int>())).Return(eventDataModel);
            handler = new PublishEventCommandHandler(eventPublisherDataService, eventSerialiser, messageBus);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_return_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Could not get event data");
        };

        private It should_not_publish_the_event = () =>
        {
            messageBus.DidNotReceive().Publish(Arg.Any<IMessage>());
        };
    }
}