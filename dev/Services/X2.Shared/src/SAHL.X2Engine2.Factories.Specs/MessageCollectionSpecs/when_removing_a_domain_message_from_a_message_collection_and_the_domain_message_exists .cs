using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_removing_a_domain_message_from_a_message_collection_and_the_domain_message_exists : WithFakes
    {
        private static IDomainMessage domainMessage;
        private static MessageCollection messageCollection;
        private static bool removed;

        Establish context = () =>
        {
            domainMessage = new SAHL.Common.DomainMessages.Error("ErrorMessage", "Details");
            messageCollection = new MessageCollection();
            messageCollection.Add(domainMessage);
        };

        Because of = () =>
        {
            removed = messageCollection.Remove(domainMessage);
        };

        It should_remove_the_domain_message_from_the_collection = () =>
        {
            messageCollection.Count.ShouldEqual(0);
        };

        It should_return_true = () =>
        {
            removed.ShouldBeTrue();
        };
    }
}