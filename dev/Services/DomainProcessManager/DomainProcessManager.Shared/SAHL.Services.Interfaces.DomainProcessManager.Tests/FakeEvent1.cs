using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.DomainProcessManager.Tests
{
    public class FakeEvent1 : Event
    {
        public FakeEvent1(DateTime date) : base(date)
        {
        }

        public FakeEvent1(Guid id, DateTime date) : base(id, date)
        {
        }
    }
}