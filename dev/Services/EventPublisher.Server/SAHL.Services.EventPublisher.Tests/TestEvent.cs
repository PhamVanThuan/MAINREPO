using System;
using SAHL.Core.Events;

namespace SAHL.Services.EventPublisher.Tests
{
    internal class TestEvent : Event
    {
        public TestEvent(DateTime date, string message, Guid someId)
            : base(date)
        {
            this.Message = message;
            this.SomeId  = someId;
        }

        public TestEvent(Guid id, DateTime date, string message, Guid someId)
            : base(id, date)
        {
            this.Message = message;
            this.SomeId = someId;
        }

        public string Message { get; private set; }
        public Guid SomeId { get; private set; }
    }
}