using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Services.EventPublisher.CommandHandlers;
using SAHL.Services.Interfaces.EventPublisher.Commands;
using System;
using System.Linq;

namespace SAHL.Services.EventPublisher.Specs
{
    public class when_deserialising_event_data_throws_exception : WithEventPublisherFakes
    {
        private static PublishEventCommand command;
        private static PublishEventCommandHandler handler;
        private static int eventKey;
        private static string innerExceptionMessage;
        private static string exceptionMessage;

        private Establish context = () =>
        {
            innerExceptionMessage = "Inner Exception Message";
            exceptionMessage = "Exception from event deserialising";
            eventKey = 1;
            command = new PublishEventCommand(eventKey);
            handler = new PublishEventCommandHandler(eventPublisherDataService, eventSerialiser, messageBus);
            eventSerialiser.WhenToldTo(x => x.Deserialise(Arg.Any<Type>(), eventDataModel.Data))
                .Throw(new Exception(exceptionMessage, new Exception(innerExceptionMessage)));
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_return_the_message_from_the_exception = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual(exceptionMessage);
        };

        private It should_return_the_message_from_the_inner_exception = () =>
        {
            messages.ErrorMessages().Last().Message.ShouldEqual(innerExceptionMessage);
        };
    }
}