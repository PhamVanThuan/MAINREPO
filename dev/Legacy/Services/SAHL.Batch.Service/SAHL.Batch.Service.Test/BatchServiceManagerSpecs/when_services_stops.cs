namespace SAHL.Batch.Service.Test.BatchServiceManagerSpecs
{
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Batch.Common;
    using SAHL.Batch.Service.Services;
    using SAHL.Common.Logging;
    using StructureMap;

    [Subject(typeof(IBatchServiceManager))]
    public class when_services_stops : WithFakes
    {
        private static IBatchServiceManager batchServiceManager;
        private static IQueueHandlerService queueHandlerService;
        private static ILogger logger;

        private Establish context = () =>
            {
                ObjectFactory.Initialize(x =>
                {
                    x.For<ICancellationNotifier>().AlwaysUnique().Use(() => { return An<ICancellationNotifier>(); });
                    x.For<IDiposableMessageBus>().AlwaysUnique().Use(() => { return An<IDiposableMessageBus>(); });
                });
                logger = An<ILogger>();
                queueHandlerService = An<IQueueHandlerService>();
                batchServiceManager = new BatchServiceManager(queueHandlerService,logger);
            };

        private Because of = () =>
            {
                batchServiceManager.StopQueueHandlers();
            };

        private It should_discover_all_registered_queues_that_can_be_stopped = () =>
        {
            queueHandlerService.WasToldTo(x => x.DiscoverStoppableQueues());
        };
    }
}