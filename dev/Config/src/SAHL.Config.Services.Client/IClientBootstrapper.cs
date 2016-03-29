using SAHL.Core;

namespace SAHL.Config.Services.Client
{
    public interface IClientBootstrapper
    {
        IIocContainer Initialise();
    }
}