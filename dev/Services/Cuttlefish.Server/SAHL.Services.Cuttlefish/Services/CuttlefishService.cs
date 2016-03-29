using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Cuttlefish.Managers;
using SAHL.Services.Cuttlefish.Workers;
using System;

namespace SAHL.Services.Cuttlefish.Services
{
    public class CuttlefishService : HostedService
    {
        private ICuttlefishServiceConfigurationProvider cuttlefishServiceConfiguration;
        private IQueueConsumerManager queueConsumerManager;
        private ILogMessageWriter writer;
        private ILogMessageTypeConverter converter;

        public CuttlefishService(ICuttlefishServiceConfigurationProvider cuttlefishServiceConfiguration, IQueueConsumerManager queueConsumerManager,
             ILogMessageWriter writer, ILogMessageTypeConverter converter)
        {
            this.cuttlefishServiceConfiguration = cuttlefishServiceConfiguration;
            this.queueConsumerManager = queueConsumerManager;
            this.writer = writer;
            this.converter = converter;
        }

        public override void Start()
        {
            base.Start();

            Console.WriteLine("Starting Cuttlefish Service.");
            // start the correct number of processes for each type
            // WorkerCountForv2LogMessage
            this.queueConsumerManager.StartConsumer(
                "SAHL.Shared.Messages.LogMessage:SAHL.Shared",
                "SAHL.Shared.Messages.LogMessage:SAHL.Shared_SAHLS47_CuttleFish",
                cuttlefishServiceConfiguration.WorkerCountForv2LogMessage, new LogMessage_v2Worker(this.writer, this.converter));

            //// WorkerCountForv2ThroughputMessage
            //this.queueConsumerManager.StartConsumer(
            //    "SAHL_Shared_Messages_Metrics_ThroughputMetricMessage:SAHL_Shared",
            //    "SAHL_Shared_Messages_Metrics_ThroughputMetricMessage:SAHL_Shared_SAHLS47_CuttleFish",
            //    cuttlefishServiceConfiguration.WorkerCountForv2ThroughputMessage, new ThroughputMessage_v2Worker(dbFactory));

            //// WorkerCountForv2LatencyMessage
            //this.queueConsumerManager.StartConsumer(
            //    "SAHL_Shared_Messages_Metrics_LatencyMetricMessage:SAHL_Shared",
            //    "SAHL_Shared_Messages_Metrics_LatencyMetricMessage:SAHL_Shared_SAHLS47_CuttleFish",
            //    cuttlefishServiceConfiguration.WorkerCountForv2LatencyMessage, new LatencyMessage_v2Worker(dbFactory));

            Console.WriteLine(string.Format("Cuttlefish Service Started.", this.queueConsumerManager.GetNumberOfRunningConsumers()));
        }

        public override void Stop()
        {
            this.queueConsumerManager.StopAllConsumers();

            base.Stop();
        }
    }
}