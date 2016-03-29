using System;
using SAHL.Core.Events;

namespace SAHL.Services.EventPublisher.Specs.EventTypeProviderServiceSpecs
{
    public class FakeEvent : Event
    {
        public FakeEvent(Guid id, DateTime date, string stringProperty, int intProperty, FakeEnum fakeEnum)
     : base(id, date)

        {
            this.StringProperty = stringProperty;
            this.IntProperty = intProperty;
            this.FakeEnum = fakeEnum;
        }

        public string StringProperty { get; protected set; }

        public int IntProperty { get; protected set; }

        public double Property3 { get; protected set; }

        public DateTime Property4 { get; protected set; }

        public FakeEnum FakeEnum { get; protected set; }
    }

    public enum FakeEnum
    {
        abc, def
    }
}