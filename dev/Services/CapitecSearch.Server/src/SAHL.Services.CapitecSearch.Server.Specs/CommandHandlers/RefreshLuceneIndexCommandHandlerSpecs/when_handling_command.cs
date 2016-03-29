using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.TextSearch;
using SAHL.Services.CapitecSearch.CommandHandlers;
using SAHL.Services.Interfaces.CapitecSearch.Commands;

namespace SAHL.Services.CapitecSearch.Server.Specs.CommandHandlers.RefreshLuceneIndexCommandHandlerSpecs
{
    public class when_handling_command : WithFakes
    {
        static ITextSearchProvider textSearchProvider;
        static RefreshLuceneIndexCommand command;
        static RefreshLuceneIndexCommandHandler handler;
        static ISystemMessageCollection messages;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            textSearchProvider = An<ITextSearchProvider>();
            command = new RefreshLuceneIndexCommand();
            handler = new RefreshLuceneIndexCommandHandler(textSearchProvider);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command,metadata);
        };

        It should_ask_the_TextSearchProvider_to_refresh_lucene_index = () =>
        {
            textSearchProvider.WasToldTo(x => x.RefreshIndex());
        };

        It should_ask_the_TextSearchProvider_to_refresh_lucene_index_on_capitec_failover_web_server = () =>
        {
            textSearchProvider.WasToldTo(x => x.RefreshIndexOnCapitecFailoverWebServer());
        };
    }
}
