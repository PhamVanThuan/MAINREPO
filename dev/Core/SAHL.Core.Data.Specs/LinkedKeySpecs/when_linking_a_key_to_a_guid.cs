using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Affinity;
using System;

namespace SAHL.Core.Data.Specs.LinkedKeySpecs
{
    public class when_linking_a_key_to_a_guid : WithFakes
    {
        private static LinkedKeyManager linkedKeyManager;
        private static ILinkedKeyDataManager linkedKeyDataManager;
        private static int key;
        private static Guid guid;

        private Establish context = () =>
        {
            linkedKeyDataManager = An<ILinkedKeyDataManager>();
            linkedKeyManager = new LinkedKeyManager(linkedKeyDataManager);
            guid = CombGuid.Instance.Generate();
            key = 5;
        };

        private Because of = () =>
        {
            linkedKeyManager.LinkKeyToGuid(key, guid);
        };

        private It should_inserted_the_database = () =>
        {
            linkedKeyDataManager.WasToldTo(x => x.InsertLinkedKey(key, guid));
        };
    }
}