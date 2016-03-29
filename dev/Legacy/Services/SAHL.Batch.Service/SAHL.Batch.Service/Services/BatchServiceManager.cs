using SAHL.Batch.Common;
using SAHL.Common.Logging;
using SAHL.Communication;
using SAHL.Core.IoC;
using StructureMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace SAHL.Batch.Service.Services
{
    public class BatchServiceManager : IBatchServiceManager
    {
        private IQueueHandlerService queueHandlerService;
        private ILogger logger;

        public BatchServiceManager(IQueueHandlerService queueHandlerService, ILogger logger)
        {
            this.queueHandlerService = queueHandlerService;
            this.logger = logger;
        }

        public void StartQueueHandlers()
        {
            try
            {
                this.logger.LogFormattedInfo("StartQueueHandlers", this.GetType().ToString());
                IEnumerable<IStartableQueueHandler> startableQueueHandlers = queueHandlerService.DiscoverStartableQueues();
                foreach (var queueHandler in startableQueueHandlers)
                {
                    queueHandler.Start();
                }

                IEnumerable<IStartable> startables = ObjectFactory.GetAllInstances<IStartable>();
                foreach (var startable in startables)
                {
                    startable.Start();
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("BatchServiceManager StartQueueHandlers : {0}", Newtonsoft.Json.JsonConvert.SerializeObject(ex)));
                Trace.Indent();
                throw ex;
            }
        }

        public void StopQueueHandlers()
        {
            try
            {
                this.logger.LogFormattedInfo("StopQueueHandlers", this.GetType().ToString());
                ICancellationNotifier cancellationNotifier = ObjectFactory.GetInstance<ICancellationNotifier>();
                cancellationNotifier.Cancel();

                var messageBus = ObjectFactory.GetInstance<IDiposableMessageBus>();
                messageBus.Dispose();

                IEnumerable<IStoppableQueueHandler> stoppableQueueHandlers = queueHandlerService.DiscoverStoppableQueues();
                foreach (var queueHandler in stoppableQueueHandlers)
                {
                    queueHandler.Stop();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("BatchServiceManager StopQueueHandlers : {0}", Newtonsoft.Json.JsonConvert.SerializeObject(ex)));
                Trace.Indent();
                throw ex;
            }
        }
    }
}