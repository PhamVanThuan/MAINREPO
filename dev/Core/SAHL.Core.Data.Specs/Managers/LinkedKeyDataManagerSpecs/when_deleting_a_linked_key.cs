using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Data.Models._2AM.Managers.LinkedKeyManager.Statements;
using SAHL.Core.Identity;
using SAHL.Core.Testing.Fakes;
using System;

namespace SAHL.Core.Data.Specs.Managers.LinkedKeyDataManagerSpecs
{
    public class when_deleting_a_linked_key : WithFakes
    {
        private static LinkedKeyDataManager linkedKeyDataManager;
        private static FakeDbFactory fakeDbFactory;
        private static Guid guid;

        private Establish context = () =>
        {
            guid = CombGuid.Instance.Generate();
            fakeDbFactory = new FakeDbFactory();
            linkedKeyDataManager = new LinkedKeyDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            linkedKeyDataManager.DeleteLinkedKey(guid);
        };

        private It should_delete_the_linked_key_using_params_provided = () =>
        {
            fakeDbFactory.FakedDb.DbContext.WasToldTo(x => x.Delete<LinkedKeysDataModel>(Arg.Is<RemoveLinkedKeysForGuidStatement>(y => y.GuidKey == guid)));
        };

        private It should_call_complete_on_the_db_context = () =>
        {
            fakeDbFactory.FakedDb.DbContext.WasToldTo(x => x.Complete());
        };
    }
}