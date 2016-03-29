using SAHL.Config.Services.Core;
using SAHL.Core.Services;

namespace SAHL.Config.Services.Windows.Web
{
    public interface IWebSelfHostedServiceSettings : IServiceSettings
    {
        string WebApiBaseAddress { get; }
    }
}