namespace SAHL.Common.Logging
{
    public class MetricsPlugin
    {
        private static IMetrics metrics;

        public static IMetrics Metrics
        {
            get
            {
                if (metrics == null)
                {
                    metrics = new DefaultMetrics();
                }
                return metrics;
            }
            set
            {
                if (value == null)
                {
                    metrics = new DefaultMetrics();
                }
                else
                {
                    metrics = new SafeMetricsWrapper(value);
                }
            }
        }
    }
}