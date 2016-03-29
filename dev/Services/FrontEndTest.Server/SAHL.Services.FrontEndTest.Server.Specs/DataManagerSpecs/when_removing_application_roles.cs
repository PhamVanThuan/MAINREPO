using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_removing_application_roles : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static int OfferRoleKey;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            OfferRoleKey = 5285892;
        };

        private Because of = () =>
        {
            testDataManager.RemoveApplicationRole(OfferRoleKey);
        };

        private It should_remove_the_application_by_OfferRoleKey = () =>
        {
            fakeDb.FakedDb.InAppContext().
                WasToldTo
                    (x => x.ExecuteNonQuery<int>
                        (Arg.Is<RemoveApplicationRoleStatement>(y => y.OfferRoleKey == OfferRoleKey)));
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}