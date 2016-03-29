using Machine.Fakes;
using Machine.Specifications;
using Managers;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Identity;
using System;

namespace SAHL.Core.Data.Specs.Managers.LinkedKeyManagerSpecs
{
    public class when_deleting_a_linked_key : WithFakes
    {
        private static LinkedKeyManager linkedKeyManager;
        private static ILinkedKeyDataManager linkedKeyDataManager;
        private static Guid guid;

        private Establish context = () =>
        {
            guid = CombGuid.Instance.Generate();
            linkedKeyDataManager = An<ILinkedKeyDataManager>();
            linkedKeyManager = new LinkedKeyManager(linkedKeyDataManager);
        };

        private Because of = () =>
        {
            linkedKeyManager.DeleteLinkedKey(guid);
        };

        private It should_use_the_data_manager_to_delete_the_linked_key = () =>
        {
            linkedKeyDataManager.WasToldTo(x => x.DeleteLinkedKey(guid));
        };
    }
}