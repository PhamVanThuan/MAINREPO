using System;
using System.Diagnostics;

namespace SAHL.Core.Logging
{
    public class MetricTimer : IMetricTimer
    {
        private DateTime startTime;
        private Stopwatch stopWatch = new Stopwatch();

        public void Start()
        {
            this.startTime = DateTime.Now;
            this.stopWatch.Start();
        }

        public IMetricTimerResult Stop()
        {
            this.stopWatch.Stop();
            TimeSpan duration = new TimeSpan(this.stopWatch.Elapsed.Ticks);
            return new MetricTimerResult(startTime, duration);
        }
    }
}