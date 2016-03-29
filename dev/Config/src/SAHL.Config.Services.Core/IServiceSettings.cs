namespace SAHL.Config.Services.Core
{
    public interface IServiceSettings
    {
        string DisplayName { get; }

        string Description { get; }

        string ServiceName { get; }
    }
}