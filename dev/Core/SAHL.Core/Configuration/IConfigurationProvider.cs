namespace SAHL.Core.Configuration
{
    public interface IConfigurationProvider
    {
        System.Configuration.Configuration Config { get; }
    }
}