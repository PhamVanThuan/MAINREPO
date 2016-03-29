using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Communications.Managers;
using SAHL.Services.Communications.Managers.LiveReplies;
using System;
using System.Collections.Specialized;

namespace SAHL.Services.Communications.Specs.Managers.Communications
{
    public class when_told_to_start_with_invalid_config_values : WithFakes
    {
        private static ILiveRepliesManager communicationsManager;
        private static ICommunicationSettings communicationSettings;
        private static NameValueCollection nameValueCollection;
        private static Exception exception;
        private static string exceptionMessage;

        private Establish context = () =>
        {
            exceptionMessage = "Missing configuration values for comcorp live replies";

            nameValueCollection = new NameValueCollection(5);
            nameValueCollection.Add("ComcorpLiveRepliesBankId", "0");
            nameValueCollection.Add("ComcorpLiveRepliesWebserviceURL", "");
            nameValueCollection.Add("ComcorpLiveRepliesPublicKeyModulus", "");
            nameValueCollection.Add("ComcorpLiveRepliesPublicKeyExponent", "");
            nameValueCollection.Add("ComcorpLiveRepliesServiceVersion", "-1");
            nameValueCollection.Add("InternalEmailFromAddress", "");
            communicationSettings = new CommunicationSettings(nameValueCollection);
            communicationsManager = new LiveRepliesManager(communicationSettings);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => communicationsManager.Start());
        };

        private It should_throw_an_exception = () =>
        {
            exception.ShouldNotBeNull();
        };

        private It should_have_an_exception_message = () =>
        {
            exception.Message.ShouldEqual(exceptionMessage);
        };
    }
}