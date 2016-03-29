using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Communications.Managers.LiveReplies;
using System;
using System.Globalization;

namespace SAHL.Services.Communications.Specs.Managers.Communications
{
    public class when_told_to_generate_an_xml_string_from_a_live_reply : WithFakes
    {
        private static ILiveRepliesManager communicationsManager;
        private static ICommunicationSettings communicationSettings;
        private static string comcorpReplyXml;

        private static ProcessBankLiveRepliesRequest liveReplyRequest;
        private static ProcessBankLiveRepliesRequestServiceHeader ServiceHeader;
        private static ProcessBankLiveRepliesRequestLiveReply[] LiveReplies;
        private static ProcessBankLiveRepliesRequestLiveReply LiveReply;

        private Establish context = () =>
        {
            communicationSettings = An<ICommunicationSettings>();
            communicationsManager = new LiveRepliesManager(communicationSettings);

            LiveReply = new ProcessBankLiveRepliesRequestLiveReply();
            LiveReply.BankReference = "1111111";
            LiveReply.BondAccountNo = "2222222";
            LiveReply.ComcorpReference = "3333333";
            LiveReply.EventComment = "test event";
            LiveReply.EventDate = DateTime.Now;
            LiveReply.EventId = ProcessBankLiveRepliesRequestLiveReplyEventId.Item1;
            LiveReply.OfferedAmount = "450000";
            LiveReply.RegisteredAmount = "475000";
            LiveReply.RequestedAmount = "450000";

            LiveReplies = new ProcessBankLiveRepliesRequestLiveReply[1];
            LiveReplies[0] = LiveReply;

            ServiceHeader = new ProcessBankLiveRepliesRequestServiceHeader();
            ServiceHeader.ApplicationMac = "";
            ServiceHeader.BankId = ProcessBankLiveRepliesRequestServiceHeaderBankId.Item29;
            ServiceHeader.RequestDateTime = DateTime.Now.ToString(@"yyyy/MM/dd hh:mm:ss tt", CultureInfo.InvariantCulture);
            ServiceHeader.ServiceVersion = "1";

            liveReplyRequest = new ProcessBankLiveRepliesRequest();
            liveReplyRequest.LiveReplies = LiveReplies;
            liveReplyRequest.ServiceHeader = ServiceHeader;
        };

        private Because of = () =>
        {
            comcorpReplyXml = communicationsManager.GenerateXmlStringFromObject(liveReplyRequest);
        };

        private It should_return_an_xml_string_that_is_not_null = () =>
        {
            comcorpReplyXml.ShouldNotBeNull();
        };
    }
}