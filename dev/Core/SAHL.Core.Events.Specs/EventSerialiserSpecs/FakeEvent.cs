using System;
using System.Collections.Generic;

namespace SAHL.Core.Events.Specs.EventSerialiserSpecs.Fakes
{
    public class FakeEvent : Event
    {
        public FakeEvent(Guid id, DateTime date, string stringProperty, int intProperty, FakeEnum fakeEnum, int? nullInt, decimal decimalProperty,
            List<string> listProperty, string[] arrayProperty, IEnumerable<Fake> fakeList, Fake fakeProperty)
            : base(id, date)
        {
            this.StringProperty = stringProperty;
            this.IntProperty = intProperty;
            this.FakeEnum = fakeEnum;
            this.NullInt = nullInt;
            this.DecimalProperty = decimalProperty;
            this.ListProperty = listProperty;
            this.ArrayProperty = arrayProperty;
            this.FakeList = fakeList;
            this.FakeProperty = fakeProperty;
        }

        public FakeEnum FakeEnum { get; protected set; }

        public List<string> ListProperty { get; protected set; }

        public IEnumerable<Fake> FakeList { get; protected set; }

        public Fake FakeProperty { get; protected set; }

        public string[] ArrayProperty { get; protected set; }

        public string StringProperty { get; private set; }

        public int IntProperty { get; private set; }

        public int? NullInt { get; private set; }

        public decimal DecimalProperty { get; set; }
    }

    public enum FakeEnum
    {
        abc, def
    }

    public class Fake
    {
        public Fake(int intProperty, string stringProperty)
        {
            this.IntProperty = intProperty;
            this.StringProperty = stringProperty;

        }
        public int IntProperty { get; protected set; }
        public string StringProperty { get; protected set; }
    }
}