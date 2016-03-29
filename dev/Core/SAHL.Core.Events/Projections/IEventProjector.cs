namespace SAHL.Core.Events.Projections
{
    public interface IEventProjector { }

    public interface IEventProjector<TEvent> : IEventProjector where TEvent : IEvent
    {
    }
}