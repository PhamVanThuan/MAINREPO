namespace SAHL.Core.Configuration
{
    public interface IApplicationConfigurationProvider : IConfigurationProvider
    {
        string ApplicationName { get; }
    }
}