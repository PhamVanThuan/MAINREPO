using SAHL.Core.Identity;
using System;

namespace SAHL.Core.Events
{
    public class Event : IEvent
    {
        public Event(DateTime date)
        {
            this.Id = CombGuid.Instance.Generate();
            this.Date = date;
        }

        public Event(Guid id, DateTime date)
            : this(date)
        {
            this.Id = id;
        }

        public string Name
        {
            get { return this.GetType().Name.Replace("LegacyEvent", "").Replace("Event", ""); }
        }

        public DateTime Date { get; private set; }

        public Guid Id { get; private set; }

        public string ClassName
        {
            get { return this.GetType().AssemblyQualifiedName; }
        }
    }
}