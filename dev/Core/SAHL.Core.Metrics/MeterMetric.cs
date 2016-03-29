using System;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Core.Metrics
{
    public class MeterMetric : IMeterMetric, IDisposable
    {
        private EWMA internalEWMA;
        private long value;
        private readonly long _startTime = DateTime.Now.Ticks;
        private static readonly TimeSpan Interval = TimeSpan.FromSeconds(5.0);
        private readonly CancellationTokenSource _token = new CancellationTokenSource();

        public long Count
        {
            get { return Interlocked.Read(ref this.value); }
        }

        public double Rate
        {
            get { return this.internalEWMA.Rate(this.RateUnit); }
        }

        public double MeanRate
        {
            get
            {
                if (this.Count != 0L)
                {
                    long elapsed = (DateTime.Now.Ticks - this._startTime) * 100L;
                    return this.ConvertNanosRate((double)this.Count / (double)elapsed);
                }
                return 0.0;
            }
        }

        public TimeUnit RateUnit
        {
            get;
            protected set;
        }

        public IMetricName Name
        {
            get;
            protected set;
        }

        public static MeterMetric New(IMetricName name, TimeUnit rate)
        {
            MeterMetric meter = new MeterMetric(name, rate.ToMinutes(60), rate);
            Task.Factory.StartNew(delegate
            {
                while (!meter._token.IsCancellationRequested)
                {
                    Thread.Sleep(MeterMetric.Interval);
                    meter.Tick();
                }
            }, meter._token.Token);
            return meter;
        }

        public void Mark()
        {
            this.Mark(1L);
        }

        public void Mark(long value)
        {
            Interlocked.Add(ref this.value, 1L);
            this.internalEWMA.Update(value);
        }

        private MeterMetric(IMetricName name, long alpha, TimeUnit rate)
        {
            double mAlpha = 1.0 - Math.Exp((1d / 12d / alpha));
            this.internalEWMA = new EWMA(mAlpha, 5L, TimeUnit.Seconds);
            Interlocked.Exchange(ref this.value, 0);
            this.RateUnit = rate;
            this.Name = name;
        }

        private void Tick()
        {
            this.internalEWMA.Tick();
        }

        private double ConvertNanosRate(double ratePerNs)
        {
            return ratePerNs * (double)this.RateUnit.ToNanos(1L);
        }

        public void Dispose()
        {
            this._token.Cancel();
        }
    }
}