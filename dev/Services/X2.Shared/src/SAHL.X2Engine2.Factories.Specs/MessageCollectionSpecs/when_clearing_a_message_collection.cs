using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_clearing_a_message_collection : WithFakes
    {
        private static MessageCollection messageCollection;

        Establish context = () =>
        {
            messageCollection = new MessageCollection();
            messageCollection.AddMessage(new SystemMessage("ErrorMessage", SystemMessageSeverityEnum.Error));
            messageCollection.AddMessage(new SystemMessage("WarningMessage", SystemMessageSeverityEnum.Warning));
            messageCollection.AddMessage(new SystemMessage("InfoMessage", SystemMessageSeverityEnum.Info));
        };

        Because of = () =>
        {
            messageCollection.Clear();
        };

        It should_clear_all_messages_from_the_collection = () =>
        {
            messageCollection.Count().ShouldEqual(0);
        };
    }
}