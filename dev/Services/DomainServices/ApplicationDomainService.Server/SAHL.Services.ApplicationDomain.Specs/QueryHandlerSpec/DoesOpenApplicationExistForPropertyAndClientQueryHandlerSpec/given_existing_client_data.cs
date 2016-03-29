using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.QueryHandlers;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.QueryHandlerSpec.DoesOpenApplicationExistForPropertyAndClientQueryHandlerSpec
{
    public class given_existing_client_data : WithFakes
    {
        private static DoesOpenApplicationExistForPropertyAndClientQueryHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static DoesOpenApplicationExistForPropertyAndClientQuery query;
        private static int propertyKey;
        private static string clientIDNumber;

        private Establish context = () =>
        {
            propertyKey = 1234;
            clientIDNumber = "8804155399089";

            applicationDataManager = An<IApplicationDataManager>();
            query = new DoesOpenApplicationExistForPropertyAndClientQuery(propertyKey, clientIDNumber);
            handler = new DoesOpenApplicationExistForPropertyAndClientQueryHandler(applicationDataManager);
            applicationDataManager.WhenToldTo(x => x.DoesOpenApplicationExistForPropertyAndClient(Param.IsAny<int>(), Param.IsAny<string>())).Return(true);
        };

        private Because of = () =>
        {
            handler.HandleQuery(query);
        };

        private It should_check_if_an_open_application_exists = () =>
        {
            applicationDataManager.WasToldTo(x => x.DoesOpenApplicationExistForPropertyAndClient(propertyKey, clientIDNumber));
        };

        private It should_set_the_result_to_true = () =>
        {
            query.Result.Results.First().ShouldBeTrue();
        };
    }
}