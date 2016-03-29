using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Metrics
{
    public class HistogramMetric : IHistogramMetric
    {
        private readonly AtomicLong count = new AtomicLong();
        private readonly AtomicLong max = new AtomicLong();
        private readonly AtomicLong min = new AtomicLong();
        private readonly ISample sample;
        private readonly AtomicLong sum = new AtomicLong();
        private readonly AtomicLong varianceM = new AtomicLong();
        private readonly AtomicLong varianceS = new AtomicLong();

        public HistogramMetric(IMetricName name, SampleType sampleType)
            : this(name, sampleType.NewSample())
        {
        }

        public HistogramMetric(IMetricName name, ISample sample)
        {
            this.sample = sample;
            Name = name;
            Clear();
        }

        private double Variance
        {
            get
            {
                if (Count > 1L)
                {
                    return BitConverter.Int64BitsToDouble(varianceS.Get()) / (Count - 1L);
                }
                return 0d;
            }
        }

        public long Count
        {
            get { return count.Get(); }
        }

        public double Max
        {
            get { return max.Get(); }
        }

        public double Min
        {
            get { return min.Get(); }
        }

        public double Mean
        {
            get
            {
                if (Count <= 0L)
                {
                    return 0d;
                }
                return sum.Get() / (double) Count;
            }
        }

        public double StdDeviation
        {
            get
            {
                if (Count <= 0L)
                {
                    return 0d;
                }
                return Math.Sqrt(Variance);
            }
        }

        public ICollection<long> Values
        {
            get { return sample.Values; }
        }

        public long SampleCount
        {
            get { return sample.Count; }
        }

        public double SampleMax
        {
            get { return sample.Count; }
        }

        public double SampleMin
        {
            get
            {
                return ((sample.Count == 0) ? 0L : sample.Values.Max());
            }
        }

        public double SampleMean
        {
            get
            {
                if (sample.Count != 0)
                {
                    return sample.Values.Average();
                }
                return 0d;
            }
        }

        public IMetricName Name { get; protected set; }

        public void Update(int value)
        {
            Update((long) value);
        }

        public void Update(long value)
        {
            count.Increment();
            sample.Update(value);
            SetMax(value);
            SetMin(value);
            sum.Add(value);
            UpdateVariance(value);
        }

        public void Clear()
        {
            sample.Clear();
            count.Set(0L);
            //TODO: is this right? max was set to min value, and min was set to max value?
            max.Set(long.MinValue);
            min.Set(long.MaxValue);
            sum.Set(0L);
            varianceM.Set(-1L);
            varianceS.Set(0L);
        }

        private void SetMax(long value)
        {
            var done = false;
            while (!done)
            {
                var currentMax = max.Get();
                done = (currentMax >= value || max.CompareAndSet(currentMax, value));
            }
        }

        private void SetMin(long value)
        {
            var done = false;
            while (!done)
            {
                var currentMin = min.Get();
                done = (currentMin <= value || min.CompareAndSet(currentMin, value));
            }
        }

        private void UpdateVariance(long value)
        {
            if (varianceM.CompareAndSet(-1L, BitConverter.DoubleToInt64Bits(value)))
            {
                return;
            }
            var done = false;
            while (!done)
            {
                var oldMCas = varianceM.Get();
                var oldM = BitConverter.Int64BitsToDouble(oldMCas);
                var newM = oldM + (value - oldM) / Count;
                var oldSCas = varianceS.Get();
                var oldS = BitConverter.Int64BitsToDouble(oldSCas);
                var newS = oldS + (value - oldM)*(value - newM);
                done = (varianceM.CompareAndSet(oldMCas, BitConverter.DoubleToInt64Bits(newM)) &&
                        varianceS.CompareAndSet(oldSCas, BitConverter.DoubleToInt64Bits(newS)));
            }
        }
    }
}