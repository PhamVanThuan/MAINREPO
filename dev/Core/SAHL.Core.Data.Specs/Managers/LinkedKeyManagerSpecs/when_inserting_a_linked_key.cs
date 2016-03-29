using Machine.Fakes;
using Machine.Specifications;
using Managers;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Identity;
using System;

namespace SAHL.Core.Data.Specs.Managers.LinkedKeyManagerSpecs
{
    public class when_inserting_a_linked_key : WithFakes
    {
        private static LinkedKeyManager linkedKeyManager;
        private static ILinkedKeyDataManager linkedKeyDataManager;
        private static Guid guid;
        private static int linkedKey;

        private Establish context = () =>
        {
            linkedKey = 1;
            guid = CombGuid.Instance.Generate();
            linkedKeyDataManager = An<ILinkedKeyDataManager>();
            linkedKeyManager = new LinkedKeyManager(linkedKeyDataManager);
        };

        private Because of = () =>
        {
            linkedKeyManager.LinkKeyToGuid(linkedKey, guid);
        };

        private It should_use_data_manager_to_link_the_guid = () =>
        {
            linkedKeyDataManager.WasToldTo(x => x.InsertLinkedKey(linkedKey, guid));
        };
    }
}