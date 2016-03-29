using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Communications.Managers.LiveReplies;
using System;
using System.Collections.Specialized;
using System.Globalization;

namespace SAHL.Services.Communications.Specs.Managers.Communications
{
    public class when_told_to_create_comcorp_mac_from_xml : WithFakes
    {
        private static ILiveRepliesManager communicationsManager;
        private static ICommunicationSettings communicationSettings;
        private static NameValueCollection nameValueCollection;

        private static ProcessBankLiveRepliesRequest liveReplyRequest;
        private static ProcessBankLiveRepliesRequestServiceHeader ServiceHeader;
        private static ProcessBankLiveRepliesRequestLiveReply[] LiveReplies;
        private static ProcessBankLiveRepliesRequestLiveReply LiveReply;

        private static string comcorpReplyXml;
        private static string mac;

        private Establish context = () =>
        {
            nameValueCollection = new NameValueCollection(2);
            nameValueCollection.Add("ComcorpLiveRepliesBankId", "29");
            nameValueCollection.Add("ComcorpLiveRepliesWebserviceURL", "https://preprod.comcorponline.co.za//Comcorp.LiveReplies/LiveRepliesService.asmx");
            nameValueCollection.Add("ComcorpLiveRepliesPublicKeyModulus",
                "t6DLHL2Im2lsMU9HDXoht1AA1LocgEIEJbm+F5pqJg/qzY/Lc6RtRejwcv7X5NQP1mefimdRbhnzUvbHUIdEMJ9wV0hcamU24ituOf+Gq/piLQiF32X0HtozznIyU2EvH7mgTiBRey2g64fkgpsRlNCC5yQjuXM+qPTyEICCQWU=");
            nameValueCollection.Add("ComcorpLiveRepliesPublicKeyExponent", "AQAB");
            nameValueCollection.Add("ComcorpLiveRepliesServiceVersion", "1");
            nameValueCollection.Add("InternalEmailFromAddress", "halo@sahomeloans.com");

            communicationSettings = new CommunicationSettings(nameValueCollection);
            communicationsManager = new LiveRepliesManager(communicationSettings);
            communicationsManager.Start();

            LiveReply = new ProcessBankLiveRepliesRequestLiveReply();
            LiveReply.BankReference = "SAHL123";
            LiveReply.BondAccountNo = "1234567";
            LiveReply.ComcorpReference = "654321";
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

            comcorpReplyXml = communicationsManager.GenerateXmlStringFromObject(liveReplyRequest);
        };

        private Because of = () =>
        {
            mac = communicationsManager.CreateComcorpMessageAuthenticationCodeFromXml(comcorpReplyXml);
        };

        private It should_return_a_mac_string_that_is_not_null = () =>
        {
            mac.ShouldNotBeNull();
        };
    }
}