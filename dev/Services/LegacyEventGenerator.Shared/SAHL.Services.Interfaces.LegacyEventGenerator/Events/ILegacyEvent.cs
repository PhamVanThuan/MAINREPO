using SAHL.Core.Events;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Events
{
    public interface ILegacyEvent : IEvent
    {
        int AdUserKey { get; }

        string AdUserName { get; }
    }
}