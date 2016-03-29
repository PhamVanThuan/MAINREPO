using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Communications.Managers.LiveReplies;
using System;
using System.Collections.Specialized;

namespace SAHL.Services.Communications.Specs.Managers.Communications
{
    public class when_told_to_get_comcorp_live_replies_reply_status : WithFakes
    {
        private static ILiveRepliesManager communicationsManager;
        private static ICommunicationSettings communicationSettings;
        private static NameValueCollection nameValueCollection;

        private static string comcorpReplyXml;
        private static Tuple<int, string> replyStatus;

        private Establish context = () =>
        {
            comcorpReplyXml = @"
<ProcessBankLiveReplies.Reply>
	<Service.Header>
		<Reply.DateTime>2006-01-02T16:00:00</Reply.DateTime>
		<Service.Result>1</Service.Result>
	</Service.Header>
</ProcessBankLiveReplies.Reply>";

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
        };

        private Because of = () =>
        {
            replyStatus = communicationsManager.GetComcorpLiveRepliesReplyStatus(comcorpReplyXml);
        };

        private It should_return_a_reply_status = () =>
        {
            replyStatus.ShouldNotBeNull();
        };
    }
}