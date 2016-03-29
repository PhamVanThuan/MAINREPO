using Machine.Fakes;
using Machine.Specifications;
using Managers;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Identity;
using System;

namespace SAHL.Core.Data.Specs.Managers.LinkedKeyManagerSpecs
{
    public class when_retrieving_a_linked_key : WithFakes
    {
        private static LinkedKeyManager linkedKeyManager;
        private static ILinkedKeyDataManager linkedKeyDataManager;
        private static Guid guid;
        private static int linkedKey;
        private static int result;

        private Establish context = () =>
            {
                guid = CombGuid.Instance.Generate();
                linkedKey = 1;
                linkedKeyDataManager = An<ILinkedKeyDataManager>();
                linkedKeyManager = new LinkedKeyManager(linkedKeyDataManager);
                linkedKeyDataManager.WhenToldTo(x => x.RetrieveLinkedKey(guid)).Return(linkedKey);
            };

        private Because of = () =>
            {
                result = linkedKeyManager.RetrieveLinkedKey(guid);
            };

        private It should_return_the_key_from_the_data_manager = () =>
            {
                result.ShouldEqual(linkedKey);
            };
    }
}