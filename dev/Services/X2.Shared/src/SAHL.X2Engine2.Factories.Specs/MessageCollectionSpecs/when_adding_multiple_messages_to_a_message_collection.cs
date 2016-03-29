using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_adding_multiple_messages_to_a_message_collection : WithFakes
    {
        private static IEnumerable<ISystemMessage> messages;
        private static MessageCollection collection;

        Establish context = () =>
        {
            collection = new MessageCollection();
            SystemMessage message1 = new SystemMessage("Message1", SystemMessageSeverityEnum.Error);
            SystemMessage message2 = new SystemMessage("Message2", SystemMessageSeverityEnum.Info);
            SystemMessage message3 = new SystemMessage("Message3", SystemMessageSeverityEnum.Warning);
            messages = new[] { message1, message2, message3 };
        };

        Because of = () =>
        {
            collection.AddMessages(messages);
        };

        It should_add_all_the_messages_to_the_collection = () =>
        {
            collection.Count.ShouldEqual(3);
        };
    }
}