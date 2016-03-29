using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_a_message_collection_contains_error_messages : WithFakes
    {
        private static MessageCollection messageCollection;
        private static bool hasErrorMessages;
        private static bool hasErrors;

        Establish context = () =>
        {
            messageCollection = new MessageCollection();
            messageCollection.AddMessage(new SystemMessage("Message", SystemMessageSeverityEnum.Error));
        };

        Because of = () =>
        {
            hasErrorMessages = messageCollection.HasErrorMessages;
            hasErrors = messageCollection.HasErrors;
        };

        It should_return_true_when_accessing_the_has_error_messages_property = () =>
        {
            hasErrorMessages.ShouldBeTrue();
        };

        It should_return_true_when_accessing_has_errors_property = () =>
        {
            hasErrors.ShouldBeTrue();
        };
    }
}