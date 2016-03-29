using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_adding_a_message_to_a_message_collection : WithFakes
    {
        private static MessageCollection messageCollection;

        Establish context = () =>
        {
            messageCollection = new MessageCollection();
        };

        Because of = () =>
        {
            messageCollection.AddMessage(new SystemMessage("Message", SystemMessageSeverityEnum.Info));
        };

        It should_add_the_message_to_the_collection = () =>
        {
            messageCollection.Count.ShouldEqual(1);
        };
    }
}