using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Affinity;
using System;

namespace SAHL.Core.Data.Specs.LinkedKeySpecs
{
    public class when_deleting_a_linked_key : WithFakes
    {
        private static LinkedKeyManager linkedKeyManager;
        private static ILinkedKeyDataManager linkedKeyDataManager;

        private static Guid guid;

        private Establish context = () =>
        {
            linkedKeyDataManager = An<ILinkedKeyDataManager>();
            linkedKeyManager = new LinkedKeyManager(linkedKeyDataManager);
            guid = CombGuid.Instance.Generate();
        };

        private Because of = () =>
        {
            linkedKeyManager.DeleteLinkedKey(guid);
        };

        private It should_delete_the_linkedkey = () =>
        {
            linkedKeyDataManager.WasToldTo(x => x.DeleteLinkedKey(guid));
        };
    }
}