using SAHL.Core.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Services.Specs.Metrics
{
    public static class FakeMetricsFactory
    {
        private static IDictionary<string, ITimerMetric> metrics;
        public const string dailyMetricDescription = "Daily_Metric_Name";
        public const string hourlyMetricDescription = "Hourly_Metric_Name";
        public const string minuteMetricDescription = "Minute_Metric_Name";

        public static IEnumerable<ITimerMetric> GetTimerMetricsCollections()
        {
            metrics = new Dictionary<string, ITimerMetric>();
            IMetricName dailyMetricName = new FakeDailyMetricName();
            IMetricName hourlyMetricName = new FakeHourlyMetricName();
            IMetricName minuteMetricName = new FakeMinuteMetricName();
            IHistogramMetric histogramMetric = new FakeHistogramMetric();
            IMeterMetric meterMetric = new FakeMeterMetric();
            TimerMetric dailyMetric = new TimerMetric(dailyMetricName, TimeUnit.Days, TimeUnit.Days, histogramMetric, meterMetric);
            TimerMetric hourlyMetric = new TimerMetric(hourlyMetricName, TimeUnit.Hours, TimeUnit.Hours, histogramMetric, meterMetric);
            TimerMetric minuteMetric = new TimerMetric(minuteMetricName, TimeUnit.Minutes, TimeUnit.Minutes, histogramMetric, meterMetric);
            return new List<TimerMetric>() { dailyMetric, hourlyMetric, minuteMetric };
        }

        public class FakeDailyMetricName : IMetricName
        {
            public string Value
            {
                get { return dailyMetricDescription; }
            }
        }
        public class FakeMinuteMetricName : IMetricName
        {
            public string Value
            {
                get { return minuteMetricDescription; }
            }
        }
        public class FakeMeterMetric : IMeterMetric
        {
            public long Count
            {
                get { throw new NotImplementedException(); }
            }

            public double Rate
            {
                get { throw new NotImplementedException(); }
            }

            public double MeanRate
            {
                get { throw new NotImplementedException(); }
            }

            public TimeUnit RateUnit
            {
                get { throw new NotImplementedException(); }
            }

            public void Mark()
            {
                throw new NotImplementedException();
            }

            public void Mark(long value)
            {
                throw new NotImplementedException();
            }

            public IMetricName Name
            {
                get { throw new NotImplementedException(); }
            }
        }

        public class FakeHourlyMetricName : IMetricName
        {
            public string Value
            {
                get { return hourlyMetricDescription; }
            }
        }

        public class FakeHistogramMetric : IHistogramMetric
        {
            public long Count
            {
                get { throw new NotImplementedException(); }
            }

            public double Max
            {
                get { throw new NotImplementedException(); }
            }

            public double Min
            {
                get { throw new NotImplementedException(); }
            }

            public double Mean
            {
                get { throw new NotImplementedException(); }
            }

            public double StdDeviation
            {
                get { throw new NotImplementedException(); }
            }

            public ICollection<long> Values
            {
                get { throw new NotImplementedException(); }
            }

            public long SampleCount
            {
                get { throw new NotImplementedException(); }
            }

            public double SampleMax
            {
                get { throw new NotImplementedException(); }
            }

            public double SampleMin
            {
                get { throw new NotImplementedException(); }
            }

            public double SampleMean
            {
                get { throw new NotImplementedException(); }
            }

            public void Update(int value)
            {
                throw new NotImplementedException();
            }

            public void Update(long value)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public IMetricName Name
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}