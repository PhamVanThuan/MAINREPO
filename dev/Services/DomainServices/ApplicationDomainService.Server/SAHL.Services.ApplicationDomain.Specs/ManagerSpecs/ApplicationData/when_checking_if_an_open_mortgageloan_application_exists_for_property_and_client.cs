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
    public class when_checking_if_an_open_mortgageloan_application_exists_for_property_and_client : WithFakes
    {
        private static IApplicationDataManager applicationDataManager;
        private static FakeDbFactory dbFactory;
        private static bool result;
        private static int propertyKey;
        private static string clientIDNumber;

        private Establish context = () =>
        {
            propertyKey = 695842;
            clientIDNumber = "6812095219087";
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne<int>(Param.IsAny<DoesOpenApplicationExistForPropertyAndClientStatement>())).Return(1);
        };

        private Because of = () =>
        {
            result = applicationDataManager.DoesOpenApplicationExistForPropertyAndClient(propertyKey,clientIDNumber);
        };

        private It should_query_for_an_open_mortgageloan_application_using_propertykey_and_client_id_number = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<int>(Arg.Is<DoesOpenApplicationExistForPropertyAndClientStatement>(y => y.PropertyKey == propertyKey 
                && y.ClientIDNumber == clientIDNumber)));
        };

        private It should_return_true = () =>
       {
           result.ShouldBeTrue();
       };
    }
}