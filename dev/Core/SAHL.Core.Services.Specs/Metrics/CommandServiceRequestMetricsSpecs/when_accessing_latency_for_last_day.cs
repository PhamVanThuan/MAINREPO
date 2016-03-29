using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Metrics;
using SAHL.Core.Services.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Services.Specs.Metrics.CommandServiceRequestMetricsSpecs
{
    public class when_accessing_latency_for_last_day : WithFakes
    {
        private static CommandServiceRequestMetrics commandServiceRequestMetrics;
        private static string serviceName;
        private static IMetricsCollector collector;
        private static IHostedService hostedService;
        private static IDictionary<string, ITimerMetric> metrics;
        private static IEnumerable<ITimerMetric> timerMetrics;

        private Establish context = () =>
        {
            metrics = new Dictionary<string, ITimerMetric>();
            timerMetrics = FakeMetricsFactory.GetTimerMetricsCollections();
            collector = An<IMetricsCollector>();
            hostedService = An<IHostedService>();
            serviceName = "testServiceName";
            collector.WhenToldTo(x => x.GetMetricsNameForSet<ITimerMetric>(serviceName)).Return(timerMetrics);
            commandServiceRequestMetrics = new CommandServiceRequestMetrics(serviceName, collector, hostedService);
        };

        private Because of = () =>
        {
            metrics = commandServiceRequestMetrics.CommandLatencyForLastDay;
        };

        private It should_contain_only_the_daily_metric = () =>
        {
            metrics.First().Key.ShouldEqual(FakeMetricsFactory.dailyMetricDescription);
        };

        private It should_only_contain_a_single_metric = () =>
        {
            metrics.Count.ShouldEqual(1);
        };

        private It should_use_the_collector_to_retrieve_metrics_for_the_service_provided = () =>
        {
            collector.WasToldTo(x => x.GetMetricsNameForSet<ITimerMetric>(serviceName));
        };
    }
}