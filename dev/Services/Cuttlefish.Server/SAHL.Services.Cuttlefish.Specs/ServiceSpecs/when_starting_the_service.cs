using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Services.Cuttlefish.Managers;
using SAHL.Services.Cuttlefish.Services;
using SAHL.Services.Cuttlefish.Specs.Fakes;
using SAHL.Services.Cuttlefish.Workers;

namespace SAHL.Services.Cuttlefish.Specs.ServiceSpecs
{
    public class when_starting_the_service : WithFakes
    {
        private static CuttlefishService service;
        private static IQueueConsumerManager queueConsumerManager;
        private static ILogMessageWriter writer;
        private static ILogMessageTypeConverter converter;
        private static int numberOfLogMessageWorkers;

        private Establish context = () =>
        {
            queueConsumerManager = An<IQueueConsumerManager>();
            writer = An<ILogMessageWriter>();
            converter = An<ILogMessageTypeConverter>();
            numberOfLogMessageWorkers = 2;
            service = new CuttlefishService(new FakeCuttlefishServiceConfiguration() { WorkerCountForv2LogMessage = numberOfLogMessageWorkers },
                queueConsumerManager, writer, converter);
        };

        private Because of = () =>
        {
            service.Start();
        };

        private It should_start_the_consumers = () =>
            {
                queueConsumerManager.WasToldTo(x => x.StartConsumer(Arg.Any<string>(), Arg.Any<string>(), Arg.Is<int>(numberOfLogMessageWorkers), Arg.Any<LogMessage_v2Worker>()));
            };
    }
}