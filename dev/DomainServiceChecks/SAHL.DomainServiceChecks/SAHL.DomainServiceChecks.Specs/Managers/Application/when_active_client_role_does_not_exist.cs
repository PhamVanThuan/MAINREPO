using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainServiceChecks.Managers.ApplicationDataManager;
using SAHL.DomainServiceChecks.Managers.ApplicationDataManager.Statements;

namespace SAHL.DomainServiceCheck.Specs.Managers.Application
{
    public class when_active_client_role_does_not_exist : WithFakes
    {
        private static ApplicationDataManager applicationDataManager;
        private static int applicationRoleKey;
        private static bool isActiveClientRole;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            applicationRoleKey = 1;
            applicationDataManager = new ApplicationDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne<int>(Param.IsAny<IsActiveClientRoleStatement>())).Return(0);
        };

        private Because of = () =>
        {
            isActiveClientRole = applicationDataManager.IsActiveClientRole(applicationRoleKey);
        };

        private It should_check_for_an_active_client_role_in_our_system = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Arg.Is<IsActiveClientRoleStatement>(y => y.ApplicationRoleKey == applicationRoleKey)));
        };

        private It should_not_be_find_an_active_client_role = () =>
        {
            isActiveClientRole.ShouldEqual(false);
        };
    }
}