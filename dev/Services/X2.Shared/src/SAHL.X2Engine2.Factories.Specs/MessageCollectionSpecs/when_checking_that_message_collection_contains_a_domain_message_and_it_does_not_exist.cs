using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_checking_that_message_collection_contains_a_domain_message_and_it_does_not_exist : WithFakes
    {
        private static IDomainMessage domainMessage;
        private static MessageCollection messageCollection;
        private static bool domainMessageExistsInCollection;

        Establish context = () =>
        {
            domainMessageExistsInCollection = true;
            domainMessage = new SAHL.Common.DomainMessages.Error("ErrorMessage", "Details");
            messageCollection = new MessageCollection();
            messageCollection.Add(new Warning("InitialMessage", "Details"));
        };

        Because of = () =>
        {
            domainMessageExistsInCollection = messageCollection.Contains(domainMessage);
        };

        It should_return_false = () =>
        {
            domainMessageExistsInCollection.ShouldBeFalse();
        };
    }
}