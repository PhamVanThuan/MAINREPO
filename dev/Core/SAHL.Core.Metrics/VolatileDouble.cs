using System.Threading;

namespace SAHL.Core.Metrics
{
    internal struct VolatileDouble
    {
        private double _value;

        public static VolatileDouble operator +(VolatileDouble left, VolatileDouble right)
        {
            return VolatileDouble.Add(left, right);
        }

        private static VolatileDouble Add(VolatileDouble left, VolatileDouble right)
        {
            left.Set(left.Get() + right.Get());
            return left.Get();
        }

        public static VolatileDouble operator -(VolatileDouble left, VolatileDouble right)
        {
            left.Set(left.Get() - right.Get());
            return left.Get();
        }

        public static VolatileDouble operator *(VolatileDouble left, VolatileDouble right)
        {
            left.Set(left.Get() * right.Get());
            return left.Get();
        }

        public static VolatileDouble operator /(VolatileDouble left, VolatileDouble right)
        {
            left.Set(left.Get() / right.Get());
            return left.Get();
        }

        private VolatileDouble(double value)
        {
            this = default(VolatileDouble);
            this.Set(value);
        }

        public void Set(double value)
        {
            Thread.VolatileWrite(ref this._value, value);
        }

        public double Get()
        {
            return Thread.VolatileRead(ref this._value);
        }

        public static implicit operator VolatileDouble(double value)
        {
            return new VolatileDouble(value);
        }

        public static implicit operator double(VolatileDouble value)
        {
            return value.Get();
        }

        public override string ToString()
        {
            return this.Get().ToString();
        }
    }
}