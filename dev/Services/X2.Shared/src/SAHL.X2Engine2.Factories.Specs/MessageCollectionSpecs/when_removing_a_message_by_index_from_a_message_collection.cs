using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_removing_a_message_by_index_from_a_message_collection : WithFakes
    {
        private static IDomainMessage domainMessage;
        private static MessageCollection messageCollection;

        Establish context = () =>
        {
            domainMessage = new SAHL.Common.DomainMessages.Error("ErrorMessage", "Details");
            messageCollection = new MessageCollection();
            messageCollection.Add(domainMessage);
        };

        Because of = () =>
        {
            messageCollection.RemoveAt(0);
        };

        It should_remove_the_message_at_the_index_provided = () =>
        {
            messageCollection.Count.ShouldEqual(0);
        };
    }
}