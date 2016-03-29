using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Managers.Application.Statements;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_checking_for_an_existing_application : WithFakes
    {
        private static IApplicationDataManager applicationDataManager;
        private static FakeDbFactory dbFactory;
        private static int applicationNumber;
        private static bool response;

        private Establish context = () =>
        {
            applicationNumber = 5050;
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne<int>(Param.IsAny<DoesApplicationExistStatement>())).Return(1);
        };

        private Because of = () =>
        {
            response = applicationDataManager.DoesApplicationExist(applicationNumber);
        };

        private It should_check_if_application_exists = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<int>(Arg.Is<DoesApplicationExistStatement>(y => y.ApplicationNumber == applicationNumber)));
        };

        private It should_confirm_that_the_application_exists = () =>
        {
            response.ShouldBeTrue();
        };
    }
}