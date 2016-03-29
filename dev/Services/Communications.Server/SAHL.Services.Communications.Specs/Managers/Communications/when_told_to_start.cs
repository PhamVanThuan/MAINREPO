using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Communications.Managers.LiveReplies;
using System.Collections.Specialized;

namespace SAHL.Services.Communications.Specs.Managers.Communications
{
    public class when_told_to_start : WithFakes
    {
        private static ILiveRepliesManager communicationsManager;
        private static ICommunicationSettings communicationSettings;
        private static NameValueCollection nameValueCollection;
        private static string expectedComcorpLiveRepliesServiceVersion;
        private static ProcessBankLiveRepliesRequestServiceHeaderBankId expectedComcorpLiveRepliesServiceBankId;

        private Establish context = () =>
        {
            expectedComcorpLiveRepliesServiceVersion = "1";
            expectedComcorpLiveRepliesServiceBankId = ProcessBankLiveRepliesRequestServiceHeaderBankId.Item29;

            nameValueCollection = new NameValueCollection(5);
            nameValueCollection.Add("ComcorpLiveRepliesBankId", "29");
            nameValueCollection.Add("ComcorpLiveRepliesWebserviceURL", "https://preprod.comcorponline.co.za//Comcorp.LiveReplies/LiveRepliesService.asmx");
            nameValueCollection.Add("ComcorpLiveRepliesPublicKeyModulus",
                "t6DLHL2Im2lsMU9HDXoht1AA1LocgEIEJbm+F5pqJg/qzY/Lc6RtRejwcv7X5NQP1mefimdRbhnzUvbHUIdEMJ9wV0hcamU24ituOf+Gq/piLQiF32X0HtozznIyU2EvH7mgTiBRey2g64fkgpsRlNCC5yQjuXM+qPTyEICCQWU=");
            nameValueCollection.Add("ComcorpLiveRepliesPublicKeyExponent", "AQAB");
            nameValueCollection.Add("ComcorpLiveRepliesServiceVersion", "1");
            nameValueCollection.Add("InternalEmailFromAddress", "halo@sahomeloans.com");
            communicationSettings = new CommunicationSettings(nameValueCollection);
            communicationsManager = new LiveRepliesManager(communicationSettings);
        };

        private Because of = () =>
        {
            communicationsManager.Start();
        };

        private It should_set_ComcorpLiveRepliesServiceVersion = () =>
        {
            communicationsManager.ComcorpLiveRepliesServiceVersion.ShouldEqual(expectedComcorpLiveRepliesServiceVersion);
        };

        private It should_set_ComcorpLiveRepliesServiceBankId = () =>
        {
            communicationsManager.ComcorpLiveRepliesServiceBankId.ShouldEqual(expectedComcorpLiveRepliesServiceBankId);
        };
    }
}