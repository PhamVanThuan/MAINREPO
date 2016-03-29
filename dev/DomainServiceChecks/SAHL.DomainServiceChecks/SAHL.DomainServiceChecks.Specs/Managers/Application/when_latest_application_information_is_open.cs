using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainServiceChecks.Managers.ApplicationDataManager;
using SAHL.DomainServiceChecks.Managers.ApplicationDataManager.Statements;

namespace SAHL.DomainServiceCheck.Specs.Managers.Application
{
    public class when_latest_application_information_is_open : WithFakes
    {
        private static ApplicationDataManager applicationDataManager;
        private static int applicationNumber;
        private static bool isLatestApplicationInformationOpen;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            applicationNumber = 1;
            applicationDataManager = new ApplicationDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne<int>(Param.IsAny<IsLatestApplicationInformationOpenStatement>())).Return(1);
        };

        private Because of = () =>
        {
            isLatestApplicationInformationOpen = applicationDataManager.IsLatestApplicationInformationOpen(applicationNumber);
        };

        private It should_check_for_the_latest_application_information_in_our_system = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Arg.Is<IsLatestApplicationInformationOpenStatement>(y => y.ApplicationNumber == applicationNumber)));
        };

        private It should_be_open = () =>
        {
            isLatestApplicationInformationOpen.ShouldEqual(true);
        };
    }
}