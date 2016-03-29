using SAHL.Core.SystemMessages;

namespace SAHL.Core.Events
{
    public interface IEventHandler<T> where T : IEvent
    {
        ISystemMessageCollection HandleEvent(T @event);
    }
}