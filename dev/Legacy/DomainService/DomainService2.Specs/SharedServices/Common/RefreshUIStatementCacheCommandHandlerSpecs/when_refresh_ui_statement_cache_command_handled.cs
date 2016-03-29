using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Service;

namespace DomainService2.Specs.SharedServices.Common.RefreshUIStatementCacheCommandHandlerSpecs
{
    [Subject(typeof(RefreshUIStatementCacheCommandHandler))]
    internal class when_refresh_ui_statement_cache_command_handled : DomainServiceSpec<RefreshUIStatementCacheCommand, RefreshUIStatementCacheCommandHandler>
    {
        protected static IUIStatementService uiStatementService;

        Establish context = () =>
        {
            uiStatementService = An<IUIStatementService>();

            command = new RefreshUIStatementCacheCommand();
            handler = new RefreshUIStatementCacheCommandHandler(uiStatementService);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_work = () =>
        {
            uiStatementService.WhenToldTo(x => x.ClearCache());
        };
    }
}