using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_removing_a_domain_message_and_the_domain_message_does_not_exist : WithFakes
    {
        private static IDomainMessage domainMessage;
        private static MessageCollection messageCollection;
        private static bool removed;

        Establish context = () =>
        {
            domainMessage = new SAHL.Common.DomainMessages.Error("Message", "Details");
            messageCollection = new MessageCollection();
            messageCollection.Add(domainMessage);
        };

        Because of = () =>
        {
            removed = messageCollection.Remove(new Warning("Message", "Details"));
        };

        It should_not_remove_any_domain_messages_from_the_collection = () =>
        {
            messageCollection.Count.ShouldEqual(1);
        };

        It should_stil_contain_the_initial_message = () =>
        {
            messageCollection.First().ShouldBeOfType(typeof(Error));
        };

        It should_return_true = () =>
        {
            removed.ShouldBeTrue();
        };
    }
}