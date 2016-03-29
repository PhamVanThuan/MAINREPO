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
    public class when_retrieving_a_linked_key : WithFakes
    {
        private static LinkedKeyDataManager linkedKeyDataManager;
        private static FakeDbFactory fakeDbFactory;
        private static Guid guid;
        private static LinkedKeysDataModel linkedKeysDataModel;
        private static int linkedKey;
        private static int result;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            linkedKeyDataManager = new LinkedKeyDataManager(fakeDbFactory);
            linkedKey = 1234567;
            guid = CombGuid.Instance.Generate();
            linkedKeysDataModel = new LinkedKeysDataModel(linkedKey, guid);
            fakeDbFactory.FakedDb.DbReadOnlyContext
                .WhenToldTo(x => x.SelectOne<LinkedKeysDataModel>(Arg.Is<GetLinkedKeyFromGuidStatement>(y => y.GuidKey == guid)))
                .Return(linkedKeysDataModel);
        };

        private Because of = () =>
        {
            result = linkedKeyDataManager.RetrieveLinkedKey(guid);
        };

        private It should_return_the_key_from_the_model = () =>
        {
            result.ShouldEqual(linkedKeysDataModel.LinkedKey);
        };
    }
}