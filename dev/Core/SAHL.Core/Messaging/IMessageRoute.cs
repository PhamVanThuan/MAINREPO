namespace SAHL.Core.Messaging
{
    public interface IMessageRoute
    {
        string ExchangeName { get; }

        string QueueName { get; }
    }
}