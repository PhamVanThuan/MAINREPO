using SAHL.Common.Collections.Interfaces;
using SAHL.X2.Common;

namespace X2DomainService.Interface.IT
{
    public interface IIT : IX2WorkflowService
    {
        void RefreshCacheItem(IDomainMessageCollection messages, object data);

        void RefreshAll(IDomainMessageCollection messages);

        void RefreshUIStatementCache(IDomainMessageCollection messages);

        void RefreshDSCache(IDomainMessageCollection messages);

        void RefreshScheduledEvents(IDomainMessageCollection messages);
    }
}