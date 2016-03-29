using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Affinity;
using System;

namespace SAHL.Core.Data.Specs.LinkedKeySpecs
{
    public class when_retrieving_a_linked_key : WithFakes
    {
        private static LinkedKeyManager linkedKeyManager;
        private static ILinkedKeyDataManager linkedKeyDataManager;
        private static int key;
        private static Guid guid;

        private Establish context = () =>
        {
            guid = CombGuid.Instance.Generate();
            linkedKeyDataManager = An<ILinkedKeyDataManager>();
            linkedKeyManager = new LinkedKeyManager(linkedKeyDataManager);
            linkedKeyDataManager.WhenToldTo(x => x.RetrieveLinkedKey(guid)).Return(77);
        };

        private Because of = () =>
        {
            key = linkedKeyManager.RetrieveLinkedKey(guid);
        };

        private It should_retrieve_the_linkedkey = () =>
        {
            linkedKeyDataManager.WasToldTo(x => x.RetrieveLinkedKey(guid));
        };

        It should_return_the_rerieved_key = () =>
        {
            key.ShouldEqual(77);
        };
    }
}