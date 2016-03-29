using System;

namespace SAHL.Core.Metrics
{
    public class GaugeMetric<T> : IGaugeMetric<T>
    {
        #region variables

        private Func<T> evaluator;

        #endregion variables

        #region properties

        public T Value
        {
            get { return evaluator(); }
        }

        public string ValueAsString
        {
            get { return Value.ToString(); }
        }

        public IMetricName Name
        {
            get;
            protected set;
        }

        #endregion properties

        #region constructor

        public GaugeMetric(IMetricName name, Func<T> evaluator)
        {
            this.Name = name;
            this.evaluator = evaluator;
        }

        #endregion constructor
    }
}