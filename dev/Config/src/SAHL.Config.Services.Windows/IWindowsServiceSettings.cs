using SAHL.Config.Services.Core;

namespace SAHL.Config.Services.Windows
{
    public interface IWindowsServiceSettings : IServiceSettings
    {
        bool EnableFirstChanceException { get; }
        bool EnableUnhandledException { get; }
    }
}