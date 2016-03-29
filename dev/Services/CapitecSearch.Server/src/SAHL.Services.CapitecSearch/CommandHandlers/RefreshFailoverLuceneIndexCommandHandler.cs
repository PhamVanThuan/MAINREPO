using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.TextSearch;
using SAHL.Services.Interfaces.CapitecSearch.Commands;

namespace SAHL.Services.CapitecSearch.CommandHandlers
{
    public class RefreshFailoverLuceneIndexCommandHandler : IServiceCommandHandler<RefreshFailoverLuceneIndexCommand>
    {
        private ITextSearchProvider textSearchProvider;

        public RefreshFailoverLuceneIndexCommandHandler(ITextSearchProvider textSearchProvider)
        {
            this.textSearchProvider = textSearchProvider;
        }

        public ISystemMessageCollection HandleCommand(RefreshFailoverLuceneIndexCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            textSearchProvider.RefreshIndexAndClearStagingData();
            return messages;
        }
    }
}