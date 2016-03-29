using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.TextSearch;
using SAHL.Services.CapitecSearch.CommandHandlers;
using SAHL.Services.Interfaces.CapitecSearch.Commands;

namespace SAHL.Services.CapitecSearch.Server.Specs.CapitecSearchServiceSpecs
{
    public class when_asked_to_perform_command : WithFakes
    {
        static RefreshLuceneIndexCommand command;
        static CapitecSearchService capitecSearchService;
        static IServiceCommandRouter serviceCommandRouter;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            serviceCommandRouter = An<IServiceCommandRouter>();
            capitecSearchService = new CapitecSearchService(serviceCommandRouter);
        };

        Because of = () =>
        {
            capitecSearchService.PerformCommand(command, metadata);
        };

        It should_ask_the_service_command_router_to_handle_the_command = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(command, metadata));
        };
    }
}
