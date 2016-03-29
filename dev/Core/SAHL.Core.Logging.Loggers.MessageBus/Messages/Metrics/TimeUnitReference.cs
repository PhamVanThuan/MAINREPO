using System;

namespace SAHL.Core.Logging.Messages.Metrics
{
    [Serializable]
    public class TimeUnitReference
    {
        public TimeUnitReference(TimeUnit timeUnit)
        {
            this.TimeUnit = timeUnit;
        }

        public virtual int Id { get; set; }

        public virtual TimeUnit TimeUnit { get; protected set; }
    }
}