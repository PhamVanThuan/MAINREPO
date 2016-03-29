namespace SAHL.Services.Cuttlefish.Services
{
    public interface IMessageBusConfigurationProvider
    {
        string HostName { get; }

        string UserName { get; }

        string Password { get; }
    }
}