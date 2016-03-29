namespace SAHL.Core.Events.Projections
{
    public interface IProjectionHandleWrapper
    {
    }

    public interface IProjectionHandleWrapper<TEvent, TProjection> : IProjectionHandleWrapper
        where TEvent : class,IEvent
        where TProjection : IEventProjector
    {
        void Handle(WrappedEvent<TEvent> @event);
    }
}