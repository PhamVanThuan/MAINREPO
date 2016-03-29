using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_a_message_collection_contains_info_messages : WithFakes
    {
        private static MessageCollection messageCollection;
        private static bool hasInfoMessages;

        Establish context = () =>
        {
            messageCollection = new MessageCollection();
            messageCollection.AddMessage(new SystemMessage("Message", SystemMessageSeverityEnum.Info));
        };

        Because of = () =>
        {
            hasInfoMessages = messageCollection.HasInfoMessages;
        };

        It should_return_true_when_accessing_the_has_info_messages_property = () =>
        {
            hasInfoMessages.ShouldBeTrue();
        };
    }
}