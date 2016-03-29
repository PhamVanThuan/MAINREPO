namespace SAHL.Communication
{
    public interface IMessageBusConfigurationProvider
    {
        string QueueName { get; }

        string MessageServer { get; }

        string UserName { get; }

        string Password { get; }
    }
}