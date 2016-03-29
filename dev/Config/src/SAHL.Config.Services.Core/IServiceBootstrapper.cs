using SAHL.Core;

namespace SAHL.Config.Services.Core
{
    public interface IServiceBootstrapper
    {
        IIocContainer Initialise();
    }
}