using Machine.Fakes;
using Machine.Specifications;
using SAHL.Batch.Common;
using SAHL.Batch.Service.Services;
using SAHL.Common.Logging;
using System.Collections.Generic;

namespace SAHL.Batch.Service.Test.BatchServiceManagerSpecs
{
    [Subject(typeof(IBatchServiceManager))]
    public class when_service_starts : WithFakes
    {
        private static IBatchServiceManager batchServiceManager;
        private static IQueueHandlerService queueHandlerService;
        private static ILogger logger;
        private static IStartableQueueHandler startableQueueHandler1;
        private static IStartableQueueHandler startableQueueHandler2;

        private Establish context = () =>
            {
                logger = An<ILogger>();

                startableQueueHandler1 = An<IStartableQueueHandler>();
                startableQueueHandler2 = An<IStartableQueueHandler>();

                queueHandlerService = An<IQueueHandlerService>();
                queueHandlerService.WhenToldTo(x => x.DiscoverStartableQueues()).Return(new List<IStartableQueueHandler> { startableQueueHandler1, startableQueueHandler2 });
                
                batchServiceManager = new BatchServiceManager(queueHandlerService, logger);
            };

        private Because of = () =>
            {
                batchServiceManager.StartQueueHandlers();
            };

        private It should_discover_all_registered_queues_that_can_be_started = () =>
            {
                queueHandlerService.WasToldTo(x => x.DiscoverStartableQueues());
            };

        private It should_start_each_queue = () =>
            {
                startableQueueHandler1.WasToldTo(x => x.Start());
                startableQueueHandler2.WasToldTo(x => x.Start());
            };
    }
}
