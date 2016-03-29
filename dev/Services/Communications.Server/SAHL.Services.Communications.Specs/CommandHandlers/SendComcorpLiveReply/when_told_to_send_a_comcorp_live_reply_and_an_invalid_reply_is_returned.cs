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
    public class when_told_to_send_a_comcorp_live_reply_and_an_invalid_reply_is_returned : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static SendComcorpLiveReplyCommand command;
        private static SendComcorpLiveReplyCommandHandler handler;
        private static ILiveRepliesManager communicationsManager;
        private static Tuple<int, string> replyStatus;

        private Establish context = () =>
        {
            replyStatus = null;
            messages = new SystemMessageCollection();
            metadata = new ServiceRequestMetadata();

            communicationsManager = An<ILiveRepliesManager>();

            command = new SendComcorpLiveReplyCommand(Guid.NewGuid(), "", "", "", "", DateTime.Now, 1, 450000, 475000, 450000);
            handler = new SendComcorpLiveReplyCommandHandler(communicationsManager);

            communicationsManager.WhenToldTo(x => x.ComcorpLiveRepliesServiceBankId).Return(ProcessBankLiveRepliesRequestServiceHeaderBankId.Item29);
            communicationsManager.WhenToldTo(x => x.ComcorpLiveRepliesServiceVersion).Return("1");
            communicationsManager.WhenToldTo(x => x.GetProcessBankLiveRepliesRequestLiveReplyEventIdFromEventId(Param.IsAny<int>())).Return(ProcessBankLiveRepliesRequestLiveReplyEventId.Item1);
            communicationsManager.WhenToldTo(x => x.ProcessBankLiveReplies(Param.IsAny<string>())).Return("success");
            communicationsManager.WhenToldTo(x => x.GetComcorpLiveRepliesReplyStatus(Param.IsAny<string>())).Return(replyStatus);
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
            messages.ErrorMessages().First().Message.ShouldStartWith("Invalid reply status.");
        };
    }
}