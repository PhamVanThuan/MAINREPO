using System.Collections.Generic;

namespace SAHL.Batch.Common
{
    public interface IQueueHandlerService
    {
        IEnumerable<IStartableQueueHandler> DiscoverStartableQueues();

        IEnumerable<IStoppableQueueHandler> DiscoverStoppableQueues();
    }
}