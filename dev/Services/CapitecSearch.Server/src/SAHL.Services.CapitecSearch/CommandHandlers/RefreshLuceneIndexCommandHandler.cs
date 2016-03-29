using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.TextSearch;
using SAHL.Services.Interfaces.CapitecSearch.Commands;

namespace SAHL.Services.CapitecSearch.CommandHandlers
{
    public class RefreshLuceneIndexCommandHandler : IServiceCommandHandler<RefreshLuceneIndexCommand>
    {
        private ITextSearchProvider textSearchProvider;

        public RefreshLuceneIndexCommandHandler(ITextSearchProvider textSearchProvider)
        {
            this.textSearchProvider = textSearchProvider;
        }

        public ISystemMessageCollection HandleCommand(RefreshLuceneIndexCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            textSearchProvider.RefreshIndex(); // primary index refresh
            textSearchProvider.RefreshIndexOnCapitecFailoverWebServer(); // secondary failover index refresh
            return messages;
        }
    }
}