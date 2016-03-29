using System;

namespace SAHL.Core.Metrics
{
    public static class TimeUnitExtensions
    {
        private static readonly long[][] _conversionMatrix = TimeUnitExtensions.BuildConversionMatrix();

        private static long[][] BuildConversionMatrix()
        {
            int unitsCount = Enum.GetValues(typeof(TimeUnit)).Length;
            long[] timingFactors = new long[]
            {
                1000L,
                1000L,
                1000L,
                60L,
                60L,
                24L
            };
            long[][] matrix = new long[unitsCount][];
            for (int source = 0; source < unitsCount; source++)
            {
                matrix[source] = new long[source];
                long cumulativeFactor = 1L;
                for (int target = source - 1; target >= 0; target--)
                {
                    cumulativeFactor *= timingFactors[target];
                    matrix[source][target] = cumulativeFactor;
                }
            }
            return matrix;
        }

        public static long Convert(this TimeUnit source, long duration, TimeUnit target)
        {
            if (source == target)
            {
                return duration;
            }
            return (source > target) ? (duration * TimeUnitExtensions._conversionMatrix[(int)source][(int)target]) : (duration / TimeUnitExtensions._conversionMatrix[(int)target][(int)source]);
        }

        public static long ToNanos(this TimeUnit source, long interval)
        {
            return source.Convert(interval, TimeUnit.Nanoseconds);
        }

        public static long ToMicros(this TimeUnit source, long interval)
        {
            return source.Convert(interval, TimeUnit.Microseconds);
        }

        public static long ToMillis(this TimeUnit source, long interval)
        {
            return source.Convert(interval, TimeUnit.Milliseconds);
        }

        public static long ToSeconds(this TimeUnit source, long interval)
        {
            return source.Convert(interval, TimeUnit.Seconds);
        }

        public static long ToMinutes(this TimeUnit source, long interval)
        {
            return source.Convert(interval, TimeUnit.Minutes);
        }

        public static long ToHours(this TimeUnit source, long interval)
        {
            return source.Convert(interval, TimeUnit.Hours);
        }

        public static long ToDays(this TimeUnit source, long interval)
        {
            return source.Convert(interval, TimeUnit.Days);
        }
    }
}