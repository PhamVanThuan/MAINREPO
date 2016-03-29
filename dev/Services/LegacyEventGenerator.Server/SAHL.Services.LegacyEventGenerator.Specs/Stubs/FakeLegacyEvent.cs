using System;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events;

namespace SAHL.Services.LegacyEventGenerator.Specs.Stubs
{
    public class FakeLegacyEvent : LegacyEvent
    {
        public FakeLegacyEvent(Guid id, DateTime date, int adUserKey, string aduserName, int intProperty)
            : base(id,date, adUserKey, aduserName)//, 0, (int)GenericKeyType.RequiredforEventsnotlinkedtoaGenericKey)
        {
            this.IntProperty = intProperty;
        }

        public int IntProperty { get; protected set; }
    }
}