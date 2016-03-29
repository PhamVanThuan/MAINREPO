using System;

namespace SAHL.Core.Metrics
{
    public class UpTime : IUpTime
    {
        public DateTime StartDateTime
        {
            get;
            protected set;
        }

        TimeSpan IUpTime.UpTime
        {
            get
            {
                return DateTime.Now.Subtract(this.StartDateTime);
            }
        }

        public IMetricName Name
        {
            get;
            protected set;
        }

        public UpTime(IMetricName name)
            : this(name, DateTime.Now)
        {
        }

        public UpTime(IMetricName name, DateTime dateTime)
        {
            this.Name = name;
            this.StartDateTime = dateTime;
        }

        public void Update(DateTime startDateTime)
        {
            this.StartDateTime = startDateTime;
        }
    }
}