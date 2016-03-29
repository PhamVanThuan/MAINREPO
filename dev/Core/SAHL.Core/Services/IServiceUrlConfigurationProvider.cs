using SAHL.Core.Configuration;

namespace SAHL.Core.Services
{
    public interface IServiceUrlConfigurationProvider : IConfigurationProvider
    {
        bool IsSelfHostedService { get; }

        string ServiceHostName { get; }

        string ServiceName { get; }
    }
}