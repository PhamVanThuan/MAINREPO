using SAHL.Core.IoC;
using SAHL.Core.UI.ApplicationState.Models;

namespace SAHL.Core.UI.ApplicationState.Managers
{
    public interface IApplicationStateManager : IStartable
    {
        string ApplicationName { get; }

        ApplicationConfiguration Configuration { get; }
    }
}