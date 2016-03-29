using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainServiceChecks.Managers.ApplicationDataManager;
using SAHL.DomainServiceChecks.Managers.ApplicationDataManager.Statements;

namespace SAHL.DomainService.Check.Specs.Managers.Application
{
    public class when_open_application_exists : WithFakes
    {
        private static ApplicationDataManager applicationDataManager;
        private static int applicationNumber;
        private static bool isOpenApplication;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            applicationNumber = 1;
            applicationDataManager = new ApplicationDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne<int>(Param.IsAny<IsOpenApplicationStatement>())).Return(1);
        };

        private Because of = () =>
        {
            isOpenApplication = applicationDataManager.IsApplicationOpen(applicationNumber);
        };

        private It should_check_for_an_open_application_in_our_system = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Arg.Is<IsOpenApplicationStatement>(y => y.ApplicationNumber == applicationNumber)));
        };

        private It should_confirm_that_the_application_does_not_exist = () =>
        {
            isOpenApplication.ShouldBeTrue();
        };
    }
}