using System.Threading;

namespace SAHL.Core.Metrics
{
    internal struct VolatileLong
    {
        private long _value;

        public static VolatileLong operator +(VolatileLong left, VolatileLong right)
        {
            return VolatileLong.Add(left, right);
        }

        private static VolatileLong Add(VolatileLong left, VolatileLong right)
        {
            left.Set(left.Get() + right.Get());
            return left.Get();
        }

        public static VolatileLong operator -(VolatileLong left, VolatileLong right)
        {
            left.Set(left.Get() - right.Get());
            return left.Get();
        }

        public static VolatileLong operator *(VolatileLong left, VolatileLong right)
        {
            left.Set(left.Get() * right.Get());
            return left.Get();
        }

        public static VolatileLong operator /(VolatileLong left, VolatileLong right)
        {
            left.Set(left.Get() / right.Get());
            return left.Get();
        }

        private VolatileLong(VolatileLong value)
        {
            this = default(VolatileLong);
            this.Set(value);
        }

        public void Set(long value)
        {
            Thread.VolatileWrite(ref this._value, value);
        }

        public long Get()
        {
            return Thread.VolatileRead(ref this._value);
        }

        public static implicit operator VolatileLong(long value)
        {
            VolatileLong result = default(VolatileLong);
            result.Set(value);
            return result;
        }

        public static implicit operator long(VolatileLong value)
        {
            return value.Get();
        }

        public override string ToString()
        {
            return this.Get().ToString();
        }
    }
}