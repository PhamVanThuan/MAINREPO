namespace SAHL.Core.Messaging
{
    public interface IMessageRouteNameBuilder
    {
        string BuildRouteName(IMessageRoute route);
    }

    public interface IMessageRouteNameBuilder<T> : IMessageRouteNameBuilder
        where T : IMessageRoute
    {
    }
}