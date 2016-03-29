using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Communications.CommandHandlers;
using SAHL.Services.Communications.Managers.LiveReplies;
using SAHL.Services.Interfaces.Communications.Commands;
using System;
using System.Linq;

namespace SAHL.Services.Communications.Specs.CommandHandlers.SendComcorpLiveReply
{
    public class when_told_to_send_a_comcorp_live_reply_with_an_invalid_event_id : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static SendComcorpLiveReplyCommand command;
        private static SendComcorpLiveReplyCommandHandler handler;
        private static ILiveRepliesManager communicationsManager;

        private Establish context = () =>
        {
            messages = new SystemMessageCollection();
            metadata = new ServiceRequestMetadata();

            communicationsManager = An<ILiveRepliesManager>();

            command = new SendComcorpLiveReplyCommand(Guid.NewGuid(), "", "", "", "", DateTime.Now, -1, 450000, 475000, 450000);
            handler = new SendComcorpLiveReplyCommandHandler(communicationsManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };

        private It should_have_the_expected_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldStartWith("Invalid event id provided: ");
        };
    }
}