using SAHL.Core.Events;
using System;

namespace SAHL.Services.DomainProcessManager.Specs
{
    public class FakeEvent : Event
    {
        public FakeEvent(DateTime date) : base(date)
        {
        }

        public FakeEvent(Guid id, DateTime date) : base(id, date)
        {
        }
    }
}