using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Communications.CommandHandlers;
using SAHL.Services.Communications.Managers.LiveReplies;
using SAHL.Services.Interfaces.Communications.Commands;
using System;

namespace SAHL.Services.Communications.Specs.CommandHandlers.SendComcorpLiveReply
{
    public class when_told_to_send_a_comcorp_live_reply : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static SendComcorpLiveReplyCommand command;
        private static SendComcorpLiveReplyCommandHandler handler;
        private static ILiveRepliesManager communicationsManager;

        private Establish context = () =>
        {
            communicationsManager = An<ILiveRepliesManager>();

            messages = new SystemMessageCollection();
            metadata = new ServiceRequestMetadata();

            command = new SendComcorpLiveReplyCommand(Guid.NewGuid(), "", "", "", "", DateTime.Now, 1, 450000, 475000, 450000);
            handler = new SendComcorpLiveReplyCommandHandler(communicationsManager);

            communicationsManager.WhenToldTo(x => x.ComcorpLiveRepliesServiceBankId).Return(ProcessBankLiveRepliesRequestServiceHeaderBankId.Item29);
            communicationsManager.WhenToldTo(x => x.ComcorpLiveRepliesServiceVersion).Return("1");
            communicationsManager.WhenToldTo(x => x.GetProcessBankLiveRepliesRequestLiveReplyEventIdFromEventId(Param.IsAny<int>())).Return(ProcessBankLiveRepliesRequestLiveReplyEventId.Item1);
            communicationsManager.WhenToldTo(x => x.ProcessBankLiveReplies(Param.IsAny<string>())).Return("success");
            communicationsManager.WhenToldTo(x => x.GetComcorpLiveRepliesReplyStatus(Param.IsAny<string>())).Return(new Tuple<int, string>(1, "success"));
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_call_GetComcorpLiveRepliesReplyStatus = () =>
        {
            communicationsManager.WasToldTo(x => x.GetComcorpLiveRepliesReplyStatus(Param.IsAny<string>()));
        };
    }
}