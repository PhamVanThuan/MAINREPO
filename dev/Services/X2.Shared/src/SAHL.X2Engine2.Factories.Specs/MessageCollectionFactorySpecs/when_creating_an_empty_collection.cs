using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionFactorySpecs
{
    public class when_creating_an_empty_collection : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<MessageCollectionFactory> autoMocker;
        private static ISystemMessageCollection messageCollection;

        Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<MessageCollectionFactory>();
        };

        Because of = () =>
        {
            messageCollection = autoMocker.ClassUnderTest.CreateEmptyCollection();
        };

        It should_return_an_empty_message_collection = () =>
        {
            messageCollection.ShouldNotBeNull();
        };
    }
}