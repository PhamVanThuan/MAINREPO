using System;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Core.Metrics
{
    public class ResetCounter : IResetCounter, IDisposable
    {
        ICounterMetric counter;
        internal TimeSpan Interval;
        private readonly CancellationTokenSource _token = new CancellationTokenSource();

        public long Count
        {
            get { return counter.Count; }
        }

        public void Increment()
        {
            this.counter.Increment();
        }

        public void IncrementBy(long amount)
        {
            this.counter.IncrementBy(amount);
        }

        public void Decrement()
        {
            this.counter.Decrement();
        }

        public void DecrementBy(long amount)
        {
            this.counter.DecrementBy(amount);
        }

        public void Clear()
        {
            this.counter.Clear();
        }

        public IMetricName Name
        {
            get { return this.counter.Name; }
        }

        public TimeUnit RateUnit
        {
            get;
            protected set;
        }

        public static ResetCounter New(IMetricName name, TimeUnit rate)
        {
            ResetCounter counter = new ResetCounter(name, rate);
            Task.Factory.StartNew(delegate
            {
                while (!counter._token.IsCancellationRequested)
                {
                    Thread.Sleep(counter.Interval);
                    counter.Clear();
                }
            }, counter._token.Token);
            return counter;
        }

        private ResetCounter(IMetricName name, TimeUnit rate)
        {
            this.counter = new CounterMetric(name);
            this.RateUnit = rate;
            this.Interval = TimeSpan.FromSeconds(rate.ToSeconds(1));
        }

        public void Dispose()
        {
            this._token.Cancel();
        }
    }
}