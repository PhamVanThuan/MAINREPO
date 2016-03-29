using System;
namespace SAHL.Shared.Messages.Metrics
{
    [Serializable]
    public class TimeUnitReference
    {
        protected TimeUnitReference()
        {
        }

        public TimeUnitReference(TimeUnit timeUnit)
        {
            this.TimeUnit = timeUnit;
        }

        public virtual int Id { get; set; }

        public virtual TimeUnit TimeUnit { get; protected set; }
    }
}