using SAHL.Batch.Common;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SAHL.Batch.Service.Services
{
    public class QueueHandlerService : IQueueHandlerService
    {
        private IContainer container;

        public QueueHandlerService()
        {
            container = SAHL.Batch.Service.DependencyResolution.BootStrapper.Initialize();
        }

        public IEnumerable<IStartableQueueHandler> DiscoverStartableQueues()
        {
        var startableQueueHandlers = container.GetAllInstances<IStartableQueueHandler>();
        return startableQueueHandlers;
        }

        public IEnumerable<IStoppableQueueHandler> DiscoverStoppableQueues()
        {
            var stoppableQueueHandlers = container.GetAllInstances<IStoppableQueueHandler>();
            return stoppableQueueHandlers;
        }
    }
}