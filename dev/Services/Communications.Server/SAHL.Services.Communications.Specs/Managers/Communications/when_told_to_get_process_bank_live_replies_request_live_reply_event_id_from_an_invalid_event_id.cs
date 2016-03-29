using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Communications.Managers.LiveReplies;


namespace SAHL.Services.Communications.Specs.Managers.Communications
{
    public class when_told_to_get_process_bank_live_replies_request_live_reply_event_id_from_an_invalid_event_id : WithFakes
    {
        private static ILiveRepliesManager communicationsManager;
        private static ICommunicationSettings communicationSettings;
        private static ProcessBankLiveRepliesRequestLiveReplyEventId? processBankLiveRepliesRequestLiveReplyEventId;
        private static int eventId;

        private Establish context = () =>
        {
            eventId = -1;
            communicationSettings = An<ICommunicationSettings>();
            communicationsManager = new LiveRepliesManager(communicationSettings);
        };

        private Because of = () =>
        {
            processBankLiveRepliesRequestLiveReplyEventId = communicationsManager.GetProcessBankLiveRepliesRequestLiveReplyEventIdFromEventId(eventId);
        };

        private It should_return_a_process_bank_live_replies_request_live_reply_event_id = () =>
        {
            processBankLiveRepliesRequestLiveReplyEventId.ShouldBeNull();
        };
    }
}