using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.TextSearch;
using SAHL.Services.CapitecSearch.CommandHandlers;
using SAHL.Services.Interfaces.CapitecSearch.Commands;

namespace SAHL.Services.CapitecSearch.Server.Specs.CommandHandlers.RefreshFailoverLuceneIndexCommandHandlerSpecs
{
    public class when_handling_command : WithFakes
    {
        static ITextSearchProvider textSearchProvider;
        static RefreshFailoverLuceneIndexCommand command; 
        static RefreshFailoverLuceneIndexCommandHandler handler;
        static ISystemMessageCollection messages;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            textSearchProvider = An<ITextSearchProvider>();
            command = new RefreshFailoverLuceneIndexCommand();
            handler = new RefreshFailoverLuceneIndexCommandHandler(textSearchProvider);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command,metadata);
        };

        It should_ask_the_TextSearchProvider_to_refresh_lucene_index_and_clear_staging_data = () =>
        {
            textSearchProvider.WasToldTo(x => x.RefreshIndexAndClearStagingData());
        };
    }
}
