using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_getting_all_messages_from_a_message_collection : WithFakes
    {
        private static MessageCollection messageCollectionToGet;
        private static IEnumerable<ISystemMessage> messageCollection;

        Establish context = () =>
        {
            messageCollectionToGet = new MessageCollection();
            messageCollectionToGet.AddMessage(new SystemMessage("ErrorMessage", SystemMessageSeverityEnum.Error));
            messageCollectionToGet.AddMessage(new SystemMessage("WarningMessage", SystemMessageSeverityEnum.Warning));
            messageCollectionToGet.AddMessage(new SystemMessage("InfoMessage", SystemMessageSeverityEnum.Info));
        };

        Because of = () =>
        {
            messageCollection = messageCollectionToGet.AllMessages;
        };

        It should_get_all_messages_from_the_collection = () =>
        {
            messageCollection.Count().ShouldEqual(messageCollectionToGet.Count());
        };
    }
}