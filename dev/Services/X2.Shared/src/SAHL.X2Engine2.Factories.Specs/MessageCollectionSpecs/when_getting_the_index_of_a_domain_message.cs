using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_getting_the_index_of_a_domain_message : WithFakes
    {
        private static IDomainMessage domainMessage1;
        private static IDomainMessage domainMessage2;
        private static MessageCollection messageCollection;
        private static int index;

        Establish context = () =>
        {
            domainMessage1 = new SAHL.Common.DomainMessages.Warning("ErrorMessage", "Details");
            domainMessage2 = new SAHL.Common.DomainMessages.Warning("ErrorMessage1", "Details1");
            //add to the collection
            messageCollection = new MessageCollection();
            messageCollection.Add(domainMessage1);
            messageCollection.Add(domainMessage2);
        };

        Because of = () =>
        {
            index = messageCollection.IndexOf(domainMessage2);
        };

        It should_return_the_index_of_the_domain_message_in_the_collection = () =>
        {
            index.ShouldEqual(1);
        };
    }
}