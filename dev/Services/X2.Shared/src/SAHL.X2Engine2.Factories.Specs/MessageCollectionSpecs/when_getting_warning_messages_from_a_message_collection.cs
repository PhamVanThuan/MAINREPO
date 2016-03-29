using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_getting_warning_messages_from_a_message_collection : WithFakes
    {
        private static MessageCollection messageCollection;
        private static IEnumerable<ISystemMessage> warningMessages;

        Establish context = () =>
        {
            messageCollection = new MessageCollection();
            messageCollection.AddMessage(new SystemMessage("Warning_Message", SystemMessageSeverityEnum.Warning));
            messageCollection.AddMessage(new SystemMessage("Warning_Message1", SystemMessageSeverityEnum.Warning));
            messageCollection.AddMessage(new SystemMessage("Info_Message", SystemMessageSeverityEnum.Info));
            messageCollection.AddMessage(new SystemMessage("Error_Message", SystemMessageSeverityEnum.Error));
        };

        Because of = () =>
        {
            warningMessages = messageCollection.WarningMessages();
        };

        It should_populate_the_collection_with_only_error_messages = () =>
        {
            warningMessages.Where(e => e.Severity == SystemMessageSeverityEnum.Info || e.Severity == SystemMessageSeverityEnum.Error).Count().ShouldEqual(0);
        };

        It should_contain_all_the_error_messages_from_the_original_collection = () =>
        {
            warningMessages.Where(e => e.Severity == SystemMessageSeverityEnum.Warning).Count().ShouldEqual(2);
        };
    }
}