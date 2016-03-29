using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Communications.Managers.LiveReplies;
using System;

namespace SAHL.Services.Communications.Specs.Managers.Communications
{
    public class when_told_to_get_comcorp_live_replies_reply_status_with_an_invalid_reply : WithFakes
    {
        private static ILiveRepliesManager communicationsManager;
        private static ICommunicationSettings communicationSettings;
        private static string comcorpReplyXml;
        private static Tuple<int, string> replyStatus;

        private Establish context = () =>
        {
            comcorpReplyXml = @"";
            communicationSettings = An<ICommunicationSettings>();
            communicationsManager = new LiveRepliesManager(communicationSettings);
        };

        private Because of = () =>
        {
            replyStatus = communicationsManager.GetComcorpLiveRepliesReplyStatus(comcorpReplyXml);
        };

        private It should_return_null = () =>
        {
            replyStatus.ShouldBeNull();
        };
    }
}