using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_aggregating_messages_across_a_message_collection : WithFakes
    {
        private static MessageCollection messageCollection;
        private static MessageCollection messageCollectionToAggregate;

        Establish context = () =>
        {
            messageCollection = new MessageCollection();
            messageCollection.AddMessage(new SystemMessage("Message", SystemMessageSeverityEnum.Error));
            messageCollectionToAggregate = new MessageCollection();
            messageCollectionToAggregate.AddMessage(new SystemMessage("AggregateMessage", SystemMessageSeverityEnum.Warning));
        };

        Because of = () =>
        {
            messageCollection.Aggregate(messageCollectionToAggregate);
        };

        It should_contain_the_aggregated_messages = () =>
        {
            messageCollection.Last().Message.ToString().ShouldBeTheSameAs("AggregateMessage");
        };

        It should_contain_the_original_message = () =>
        {
            messageCollection.First().Message.ToString().ShouldBeTheSameAs("Message");
        };

        It should_contain_both_messages = () =>
        {
            messageCollection.Count().ShouldEqual(2);
        };
    }
}