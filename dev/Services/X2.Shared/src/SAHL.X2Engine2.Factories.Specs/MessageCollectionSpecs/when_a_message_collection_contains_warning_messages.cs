using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_a_message_collection_contains_warning_messages : WithFakes
    {
        private static MessageCollection messageCollection;
        private static bool hasWarningMessages;
        private static bool hasWarnings;

        Establish context = () =>
        {
            hasWarningMessages = false;
            hasWarnings = false;
            messageCollection = new MessageCollection();
            messageCollection.AddMessage(new SystemMessage("Message", SystemMessageSeverityEnum.Warning));
        };

        Because of = () =>
        {
            hasWarningMessages = messageCollection.HasWarningMessages;
            hasWarnings = messageCollection.HasWarnings;
        };

        It should_return_true_when_accessing_the_has_warning_messages_property = () =>
        {
            hasWarningMessages.ShouldBeTrue();
        };

        It should_return_true_when_accessing_the_has_warnings_property = () =>
        {
            hasWarnings.ShouldBeTrue();
        };
    }
}