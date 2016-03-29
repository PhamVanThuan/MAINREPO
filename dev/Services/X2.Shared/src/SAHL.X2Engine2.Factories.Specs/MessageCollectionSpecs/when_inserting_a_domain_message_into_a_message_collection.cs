using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_inserting_a_domain_message_into_a_message_collection : WithFakes
    {
        private static IDomainMessage domainMessage;
        private static MessageCollection messageCollection;

        Establish context = () =>
        {
            domainMessage = new SAHL.Common.DomainMessages.Error("ErrorMessage", "Details");
            messageCollection = new MessageCollection();
            messageCollection.Add(new Warning("InitialMessage", "Details"));
        };

        Because of = () =>
        {
            messageCollection.Insert(1, domainMessage);
        };

        It should_insert_the_domain_message_at_the_specified_index = () =>
        {
            messageCollection.Last().Message.ShouldEqual("ErrorMessage");
        };

        It should_still_contain_the_first_message_in_the_collection = () =>
        {
            messageCollection.First().Message.ShouldEqual("InitialMessage");
        };
    }
}